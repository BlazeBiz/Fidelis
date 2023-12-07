using System;
using MarauderServer.Models;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;

namespace MarauderServer.Data
{
    public partial class CustomerData : BaseData<Customer>
    {
        public CustomerData(string connectionString) : base(connectionString) {}

        internal override void RowToObject(SqlDataReader row, Customer obj)
        {
            obj.CustomerID = Int32PropertyFromRow(row, "CustomerID");
            obj.CustomerName = StringPropertyFromRow(row, "CustomerName");
            obj.CustomerNbr = StringNullablePropertyFromRow(row, "CustomerNbr");
            obj.PaymentTerms = StringNullablePropertyFromRow(row, "PaymentTerms");
            obj.GLLink = StringNullablePropertyFromRow(row, "GLLink");
            obj.Created = DateTimePropertyFromRow(row, "Created");
            obj.CreatedBy = Int32PropertyFromRow(row, "CreatedBy");
            obj.Modified = DateTimePropertyFromRow(row, "Modified");
            obj.ModifiedBy = Int32PropertyFromRow(row, "ModifiedBy");

        }
        
        override protected void BindParameters(SqlParameterCollection sqlParameters, Customer obj, IEnumerable<string>? parametersToBind = null)
        {
            List<string>? bindNames = null;
            if (parametersToBind != null) bindNames = new List<string>(parametersToBind);
            if (bindNames == null || bindNames.Contains("CustomerID")) sqlParameters.AddWithValue("@CustomerID", obj.CustomerID);
            if (bindNames == null || bindNames.Contains("CustomerName")) sqlParameters.AddWithValue("@CustomerName", obj.CustomerName);
            if (bindNames == null || bindNames.Contains("CustomerNbr")) sqlParameters.AddWithValue("@CustomerNbr", obj.CustomerNbr);
            if (bindNames == null || bindNames.Contains("PaymentTerms")) sqlParameters.AddWithValue("@PaymentTerms", obj.PaymentTerms);
            if (bindNames == null || bindNames.Contains("GLLink")) sqlParameters.AddWithValue("@GLLink", obj.GLLink);
            if (bindNames == null || bindNames.Contains("Created")) sqlParameters.AddWithValue("@Created", obj.Created);
            if (bindNames == null || bindNames.Contains("CreatedBy")) sqlParameters.AddWithValue("@CreatedBy", obj.CreatedBy);
            if (bindNames == null || bindNames.Contains("Modified")) sqlParameters.AddWithValue("@Modified", obj.Modified);
            if (bindNames == null || bindNames.Contains("ModifiedBy")) sqlParameters.AddWithValue("@ModifiedBy", obj.ModifiedBy);

        }

    }
}
