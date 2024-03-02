using MarauderServer.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace MarauderServer.Data
{
    public partial class StatisticsData : BaseData<Statistics>
    {
        public StatisticsData(string connectionString) : base(connectionString) { }

        internal override void RowToObject(SqlDataReader row, Statistics obj)
        {
            obj.NumberCustomers = Int32PropertyFromRow(row, "NumberCustomers");
            obj.NumberSalesOrders = Int32PropertyFromRow(row, "NumberSalesOrders");
        }

        override protected void BindParameters(SqlParameterCollection sqlParameters, Statistics obj, IEnumerable<string>? parametersToBind = null)
        {
            List<string>? bindNames = null;
            if (parametersToBind != null) bindNames = new List<string>(parametersToBind);
            if (bindNames == null || bindNames.Contains("NumberCustomers")) sqlParameters.AddWithValue("@NumberCustomers", obj.NumberCustomers);
            if (bindNames == null || bindNames.Contains("NumberSalesOrders")) sqlParameters.AddWithValue("@NumberSalesOrders", obj.NumberSalesOrders);
        }

        /// <summary>
        /// Returns one statistics object (db row counts)
        /// </summary>
        /// <returns>A Statistics object, NULL if not available</returns>
        public Statistics? GetStatistics()
        {
            SqlCommand cmd = new SqlCommand("viking.StatisticsGet");
            cmd.CommandType = CommandType.StoredProcedure;
            return this.GetObject(cmd, "GetStatistics");
        }

    }
}
