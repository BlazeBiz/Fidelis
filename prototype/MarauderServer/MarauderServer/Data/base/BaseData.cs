using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;

namespace MarauderServer.Data
{
    public abstract partial class BaseData<T> where T : new()
    {
        private static readonly Dictionary<Type, object> cache = new();
        protected List<T>? GetFromCache()
        {
            if (cache.ContainsKey(typeof(T))) return (List<T>)cache[typeof(T)];
            return null;
        }
        protected void AddToCache(List<T> values)
        {
            cache[typeof(T)] = values;
        }
        protected void RemoveFromCache()
        {
            if (cache.ContainsKey(typeof(T))) cache.Remove(typeof(T));
        }

        protected string connectionString;

        public BaseData(string connectionString)
        {
            this.connectionString = connectionString;
        }

        /// <summary>
        /// After a query has been executed and a SqlReader returned, loop through the result set, create objects and add them to a List
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        virtual internal List<T> GetListFromReader(SqlDataReader reader)
        {
            List<T> list = new();
            while (reader.Read())
            {
                list.Add(NewFromRow(reader));
            }
            return list;
        }

        // Create and open a new connection using retry logic. Since the Azure SQL Free tier goes into auto-pause,
        // the connection times out if it hasn't been used for an hour. This logic tries up to maxTries times, with a delay of
        // secondsBetweenTries seconds between each try.
        private SqlConnection GetNewConnection()
        {
            // Establish defaults for retries and wait times
            int tryNumber = 1;
            const int maxTries = 6;
            const int secondsBetweenTries = 5;
            while (true)
            {
                // Create a new connection
                SqlConnection conn = new(connectionString);

                // Try to establish a connection
                try
                {
                    conn.Open();
                    // If the open succeeds, exit the loop by returning from the method
                    return conn;
                }
                catch (SqlException ex)
                {
                    // Failed to connect (probably a timeout while SQL Server is resuming). Wait and then retry.
                    Console.WriteLine($"Failed to connect to database on try # {tryNumber}. Message: {ex.Message}");
                    if (tryNumber < maxTries)
                    {
                        conn.Dispose();
                        tryNumber++;
                        System.Threading.Thread.Sleep(secondsBetweenTries * 1000);
                    }
                    else
                    {
                        // maxTries reached - don't retry any more, throw the error
                        throw;
                    }
                }
            }
        }

        /// <summary>
        /// Given a command, executes the sql and gets a data reader. The reader is then passed to the readerProcesFunction, which defaults to just reading rows and creating objects.
        /// </summary>
        /// <param name="cmd">SQLCommand object to execute (Select)</param>
        /// <param name="methodName">Method calling this method, used when logging errors.</param>
        /// <param name="readerProcessFunction">Method to execute once the SqlDataReader is retrieved.</param>
        /// <returns></returns>
        protected List<T> ExecuteReader(SqlCommand cmd, string methodName, Func<SqlDataReader, List<T>>? readerProcessFunction = null)
        {
            if (readerProcessFunction == null) readerProcessFunction = GetListFromReader;
            List<T> list;
            try
            {
                using SqlConnection conn = GetNewConnection();
                cmd.Connection = conn;
                SqlDataReader rdr = cmd.ExecuteReader();
                list = readerProcessFunction(rdr);
            }
            catch (Exception ex)
            {
                // TODO: Log

                Console.Error.WriteLine($"Error in {methodName}: {ex.Message}\r\n{ex.StackTrace}");
                throw;
            }
            return list;
        }

        protected int ExecuteScalar(SqlCommand cmd, string methodName)
        {
            try
            {
                using SqlConnection conn = GetNewConnection();
                cmd.Connection = conn;
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch (Exception ex)
            {
                // TODO: Log
                Console.Error.WriteLine($"Error in {methodName}: {ex.Message}\r\n{ex.StackTrace}");
                throw;
            }
        }

        protected int ExecuteNonQuery(SqlCommand cmd, string methodName)
        {
            int returnValue = ExecuteNonQuery(cmd, methodName, out string printOutput);
            // TODO: Log printOutput

            return returnValue;
        }

        protected int ExecuteNonQuery(SqlCommand cmd, string methodName, out string output)
        {
            string printOutput = "";
            int returnValue = 0;
            try
            {
                using SqlConnection conn = GetNewConnection();
                conn.InfoMessage += (object sender, SqlInfoMessageEventArgs e) =>
                {
                    printOutput += e.Message;
                };
                cmd.Connection = conn;
                returnValue = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                // TODO: Log
                Console.Error.WriteLine($"Error in {methodName}: {ex.Message}\r\n{ex.StackTrace}");
                throw;
            }
            output = printOutput;
            return returnValue;
        }

        /// <summary>
        /// Given a SqlCommand object which returns one result set, execute and return a list of items.
        /// </summary>
        /// <param name="cmd">A SqlCommand object with its cmdText and Parameters setup.</param>
        /// <param name="methodName">Method calling this method, used when logging errors.</param>
        /// <returns>An IList of type T</returns>
        protected List<T> GetList(SqlCommand cmd, string methodName)
        {
            // Default functionality for GetList is to loop through and call NewFromRow and add the item to the list
            return ExecuteReader(cmd, methodName);
        }

        /// <summary>
        /// Given a string query which returns one result set, execute and return a list of items.
        /// </summary>
        /// <param name="sql">SQL to execute.</param>
        /// <param name="methodName">Method calling this method, used when logging errors.</param>
        /// <returns>An IList of type T</returns>
        protected IList<T> GetList(string sql, string methodName)
        {
            // Create a command object
            SqlCommand cmd = new(sql);
            return GetList(cmd, methodName);
        }

        protected T? GetObject(SqlCommand cmd, string methodName)
        {
            IList<T> list = GetList(cmd, methodName);
            if (list.Count > 0) return list[0];
            return default; // default(T) is null
        }

        virtual internal T NewFromRow(SqlDataReader row)
        {
            T obj = new();
            RowToObject(row, obj);
            return obj;
        }

        abstract internal void RowToObject(SqlDataReader row, T obj);


        abstract protected void BindParameters(SqlParameterCollection sqlParameters,
                                        T obj, IEnumerable<string>? parametersToBind = null);

        /// <summary>
        /// Helper method to build a sql command object to execute a stored procedure in one line. 
        /// </summary>
        /// <param name="sql">The name of the stored procedure</param>
        /// <param name="parameters">Any number of SqlParameters used when executing the proc. Use the `Param` helper method to build a parameter with a value.</param>
        /// <returns>A SqlCommand object. Note that it must be connected to a SqlConnection before executing.</returns>
        static protected SqlCommand StoredProc(string sql, params SqlParameter[] parameters)
        {
            SqlCommand cmd = new(sql) { CommandType = System.Data.CommandType.StoredProcedure };
            cmd.Parameters.AddRange(parameters);
            return cmd;
        }

        /// <summary>
        /// Shortcut method to create a new SqlParameter with a value.
        /// </summary>
        /// <param name="name">Name of the sql parameter variable, like `@id`</param>
        /// <param name="val">The value to bind into the sql parameter</param>
        /// <returns>A SqlParameter object to be used in a statement or procedure</returns>
        static protected SqlParameter Param(string name, object val) => new(name, val);

        /// <summary>
        /// Determines whether a result set contains a column with the specified name.
        /// </summary>
        /// <param name="rdr">SqlDataReader with its cursor pointing to a result set</param>
        /// <param name="columnName">Name of the column in question</param>
        /// <returns>True if the result set contains the column, False otherwise.</returns>
        static protected bool ContainsColumn(SqlDataReader rdr, string columnName)
        {
            columnName = columnName.ToLower();
            for (int i = 0; i < rdr.FieldCount; i++)
            {
                if (rdr.GetName(i).ToLower() == columnName) return true;
            }
            return false;
        }

        #region Property mapping helpers
        //********************************************************************
        // Helper to replace null with DBNull for parameters
        //********************************************************************
        static protected object DbNullable(object val)
        {
            return val ?? DBNull.Value;
        }

        //********************************************************************
        // Helpers to move data from the Reader to the business object
        //********************************************************************
        static protected bool BoolPropertyFromRow(SqlDataReader row, string propName)
        {
            return Convert.ToBoolean(row[propName]);
        }
        static protected bool? BoolNullablePropertyFromRow(SqlDataReader row, string propName)
        {
            return row.IsDBNull(row.GetOrdinal(propName)) ? (bool?)null : Convert.ToBoolean(row[propName]);
        }
        static protected string StringPropertyFromRow(SqlDataReader row, string propName)
        {
            if (row.IsDBNull(row.GetOrdinal(propName))) return "";
            string? converted = Convert.ToString(row[propName]);
            return (converted == null) ? "" : (string)converted;
        }
        static protected string? StringNullablePropertyFromRow(SqlDataReader row, string propName)
        {
            return row.IsDBNull(row.GetOrdinal(propName)) ? (string?)null : Convert.ToString(row[propName]);
        }
        static protected SByte SBytePropertyFromRow(SqlDataReader row, string propName)
        {
            return Convert.ToSByte(row[propName]);
        }
        static protected SByte? SByteNullablePropertyFromRow(SqlDataReader row, string propName)
        {
            return row.IsDBNull(row.GetOrdinal(propName)) ? (SByte?)null : Convert.ToSByte(row[propName]);
        }
        static protected Byte BytePropertyFromRow(SqlDataReader row, string propName)
        {
            return Convert.ToByte(row[propName]);
        }
        static protected Byte? ByteNullablePropertyFromRow(SqlDataReader row, string propName)
        {
            return row.IsDBNull(row.GetOrdinal(propName)) ? (Byte?)null : Convert.ToByte(row[propName]);
        }
        static protected Int16 Int16PropertyFromRow(SqlDataReader row, string propName)
        {
            return Convert.ToInt16(row[propName]);
        }
        static protected Int16? Int16NullablePropertyFromRow(SqlDataReader row, string propName)
        {
            return row.IsDBNull(row.GetOrdinal(propName)) ? (Int16?)null : Convert.ToInt16(row[propName]);
        }
        static protected Int32 Int32PropertyFromRow(SqlDataReader row, string propName)
        {
            return Convert.ToInt32(row[propName]);
        }
        static protected Int32? Int32NullablePropertyFromRow(SqlDataReader row, string propName)
        {
            return row.IsDBNull(row.GetOrdinal(propName)) ? (Int32?)null : Convert.ToInt32(row[propName]);
        }
        static protected Int64 Int64PropertyFromRow(SqlDataReader row, string propName)
        {
            return Convert.ToInt64(row[propName]);
        }
        static protected Int64? Int64NullablePropertyFromRow(SqlDataReader row, string propName)
        {
            return row.IsDBNull(row.GetOrdinal(propName)) ? (Int64?)null : Convert.ToInt64(row[propName]);
        }
        static protected DateTime DateTimePropertyFromRow(SqlDataReader row, string propName)
        {
            return Convert.ToDateTime(row[propName]);
        }
        static protected DateTime? DateTimeNullablePropertyFromRow(SqlDataReader row, string propName)
        {
            return row.IsDBNull(row.GetOrdinal(propName)) ? (DateTime?)null : Convert.ToDateTime(row[propName]);
        }
        static protected Decimal DecimalPropertyFromRow(SqlDataReader row, string propName)
        {
            return Convert.ToDecimal(row[propName]);
        }
        static protected Decimal? DecimalNullablePropertyFromRow(SqlDataReader row, string propName)
        {
            return row.IsDBNull(row.GetOrdinal(propName)) ? (Decimal?)null : Convert.ToDecimal(row[propName]);
        }
        static protected double SinglePropertyFromRow(SqlDataReader row, string propName)
        {
            return Convert.ToSingle(row[propName]);
        }
        static protected double? SingleNullablePropertyFromRow(SqlDataReader row, string propName)
        {
            return row.IsDBNull(row.GetOrdinal(propName)) ? (Single?)null : Convert.ToSingle(row[propName]);
        }
        static protected double DoublePropertyFromRow(SqlDataReader row, string propName)
        {
            return Convert.ToDouble(row[propName]);
        }
        static protected double? DoubleNullablePropertyFromRow(SqlDataReader row, string propName)
        {
            return row.IsDBNull(row.GetOrdinal(propName)) ? (Double?)null : Convert.ToDouble(row[propName]);
        }
        #endregion

    }
}
