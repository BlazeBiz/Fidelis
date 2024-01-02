using System;
using MarauderServer.Models;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;

namespace MarauderServer.Data
{
    public partial class SalesOrderData : BaseData<SalesOrder>
    {
        public SalesOrderData(string connectionString) : base(connectionString) {}

        internal override void RowToObject(SqlDataReader row, SalesOrder obj)
        {
            obj.SalesOrderId = Int32PropertyFromRow(row, "SalesOrderId");
            obj.SalesOrderNumber = StringNullablePropertyFromRow(row, "SalesOrderNumber");
            obj.CustomerId = Int32PropertyFromRow(row, "CustomerId");
            obj.CustomerPONumber = StringNullablePropertyFromRow(row, "CustomerPONumber");
            obj.OrderDate = DateTimePropertyFromRow(row, "OrderDate");
            obj.SalesOrderStatusCode = Int32PropertyFromRow(row, "SalesOrderStatusCode");
            obj.PaymentTermsCode = Int32NullablePropertyFromRow(row, "PaymentTermsCode");
            obj.ShipToAddressId = Int32NullablePropertyFromRow(row, "ShipToAddressId");
            obj.BillToAddressId = Int32NullablePropertyFromRow(row, "BillToAddressId");
            obj.Created = DateTimePropertyFromRow(row, "Created");
            obj.CreatedBy = Int32PropertyFromRow(row, "CreatedBy");
            obj.Modified = DateTimePropertyFromRow(row, "Modified");
            obj.ModifiedBy = Int32PropertyFromRow(row, "ModifiedBy");

        }
        
        override protected void BindParameters(SqlParameterCollection sqlParameters, SalesOrder obj, IEnumerable<string>? parametersToBind = null)
        {
            List<string>? bindNames = null;
            if (parametersToBind != null) bindNames = new List<string>(parametersToBind);
            if (bindNames == null || bindNames.Contains("SalesOrderId")) sqlParameters.AddWithValue("@SalesOrderId", obj.SalesOrderId);
            if (bindNames == null || bindNames.Contains("SalesOrderNumber")) sqlParameters.AddWithValue("@SalesOrderNumber", obj.SalesOrderNumber);
            if (bindNames == null || bindNames.Contains("CustomerId")) sqlParameters.AddWithValue("@CustomerId", obj.CustomerId);
            if (bindNames == null || bindNames.Contains("CustomerPONumber")) sqlParameters.AddWithValue("@CustomerPONumber", obj.CustomerPONumber);
            if (bindNames == null || bindNames.Contains("OrderDate")) sqlParameters.AddWithValue("@OrderDate", obj.OrderDate);
            if (bindNames == null || bindNames.Contains("SalesOrderStatusCode")) sqlParameters.AddWithValue("@SalesOrderStatusCode", obj.SalesOrderStatusCode);
            if (bindNames == null || bindNames.Contains("PaymentTermsCode")) sqlParameters.AddWithValue("@PaymentTermsCode", obj.PaymentTermsCode);
            if (bindNames == null || bindNames.Contains("ShipToAddressId")) sqlParameters.AddWithValue("@ShipToAddressId", obj.ShipToAddressId);
            if (bindNames == null || bindNames.Contains("BillToAddressId")) sqlParameters.AddWithValue("@BillToAddressId", obj.BillToAddressId);
            if (bindNames == null || bindNames.Contains("Created")) sqlParameters.AddWithValue("@Created", obj.Created);
            if (bindNames == null || bindNames.Contains("CreatedBy")) sqlParameters.AddWithValue("@CreatedBy", obj.CreatedBy);
            if (bindNames == null || bindNames.Contains("Modified")) sqlParameters.AddWithValue("@Modified", obj.Modified);
            if (bindNames == null || bindNames.Contains("ModifiedBy")) sqlParameters.AddWithValue("@ModifiedBy", obj.ModifiedBy);

        }

    }
}
