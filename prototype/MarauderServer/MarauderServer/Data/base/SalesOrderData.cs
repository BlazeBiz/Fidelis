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
            obj.SalesOrderID = Int32PropertyFromRow(row, "SalesOrderID");
            obj.SalesOrderNbr = StringNullablePropertyFromRow(row, "SalesOrderNbr");
            obj.CustomerID = Int32PropertyFromRow(row, "CustomerID");
            obj.CustomerPurchaseOrderNbr = StringNullablePropertyFromRow(row, "CustomerPurchaseOrderNbr");
            obj.OrderDate = DateTimePropertyFromRow(row, "OrderDate");
            obj.SalesOrderStatusCd = Int32PropertyFromRow(row, "SalesOrderStatusCd");
            obj.PaymentTermsCd = Int32NullablePropertyFromRow(row, "PaymentTermsCd");
            obj.ShipToAddressID = Int32NullablePropertyFromRow(row, "ShipToAddressID");
            obj.BillToAddressID = Int32NullablePropertyFromRow(row, "BillToAddressID");
            obj.Created = DateTimePropertyFromRow(row, "Created");
            obj.CreatedBy = Int32PropertyFromRow(row, "CreatedBy");
            obj.Modified = DateTimePropertyFromRow(row, "Modified");
            obj.ModifiedBy = Int32PropertyFromRow(row, "ModifiedBy");

        }
        
        override protected void BindParameters(SqlParameterCollection sqlParameters, SalesOrder obj, IEnumerable<string>? parametersToBind = null)
        {
            List<string>? bindNames = null;
            if (parametersToBind != null) bindNames = new List<string>(parametersToBind);
            if (bindNames == null || bindNames.Contains("SalesOrderID")) sqlParameters.AddWithValue("@SalesOrderID", obj.SalesOrderID);
            if (bindNames == null || bindNames.Contains("SalesOrderNbr")) sqlParameters.AddWithValue("@SalesOrderNbr", obj.SalesOrderNbr);
            if (bindNames == null || bindNames.Contains("CustomerID")) sqlParameters.AddWithValue("@CustomerID", obj.CustomerID);
            if (bindNames == null || bindNames.Contains("CustomerPurchaseOrderNbr")) sqlParameters.AddWithValue("@CustomerPurchaseOrderNbr", obj.CustomerPurchaseOrderNbr);
            if (bindNames == null || bindNames.Contains("OrderDate")) sqlParameters.AddWithValue("@OrderDate", obj.OrderDate);
            if (bindNames == null || bindNames.Contains("SalesOrderStatusCd")) sqlParameters.AddWithValue("@SalesOrderStatusCd", obj.SalesOrderStatusCd);
            if (bindNames == null || bindNames.Contains("PaymentTermsCd")) sqlParameters.AddWithValue("@PaymentTermsCd", obj.PaymentTermsCd);
            if (bindNames == null || bindNames.Contains("ShipToAddressID")) sqlParameters.AddWithValue("@ShipToAddressID", obj.ShipToAddressID);
            if (bindNames == null || bindNames.Contains("BillToAddressID")) sqlParameters.AddWithValue("@BillToAddressID", obj.BillToAddressID);
            if (bindNames == null || bindNames.Contains("Created")) sqlParameters.AddWithValue("@Created", obj.Created);
            if (bindNames == null || bindNames.Contains("CreatedBy")) sqlParameters.AddWithValue("@CreatedBy", obj.CreatedBy);
            if (bindNames == null || bindNames.Contains("Modified")) sqlParameters.AddWithValue("@Modified", obj.Modified);
            if (bindNames == null || bindNames.Contains("ModifiedBy")) sqlParameters.AddWithValue("@ModifiedBy", obj.ModifiedBy);

        }

    }
}
