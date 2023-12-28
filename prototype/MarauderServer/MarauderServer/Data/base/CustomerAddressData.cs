using System;
using MarauderServer.Models;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;

namespace MarauderServer.Data
{
    public partial class CustomerAddressData : BaseData<CustomerAddress>
    {
        public CustomerAddressData(string connectionString) : base(connectionString) {}

        internal override void RowToObject(SqlDataReader row, CustomerAddress obj)
        {
            obj.CustomerAddressId = Int32PropertyFromRow(row, "CustomerAddressId");
            obj.CustomerId = Int32PropertyFromRow(row, "CustomerId");
            obj.ShipToFlag = BoolPropertyFromRow(row, "ShipToFlag");
            obj.BillToFlag = BoolPropertyFromRow(row, "BillToFlag");
            obj.ShipToName = StringNullablePropertyFromRow(row, "ShipToName");
            obj.AddressLine1 = StringNullablePropertyFromRow(row, "AddressLine1");
            obj.AddressLine2 = StringNullablePropertyFromRow(row, "AddressLine2");
            obj.AddressLine3 = StringNullablePropertyFromRow(row, "AddressLine3");
            obj.City = StringNullablePropertyFromRow(row, "City");
            obj.StateCD = StringNullablePropertyFromRow(row, "StateCD");
            obj.ZipCode = StringNullablePropertyFromRow(row, "ZipCode");
            obj.Created = DateTimePropertyFromRow(row, "Created");
            obj.CreatedBy = Int32PropertyFromRow(row, "CreatedBy");
            obj.Modified = DateTimePropertyFromRow(row, "Modified");
            obj.ModifiedBy = Int32PropertyFromRow(row, "ModifiedBy");

        }
        
        override protected void BindParameters(SqlParameterCollection sqlParameters, CustomerAddress obj, IEnumerable<string>? parametersToBind = null)
        {
            List<string>? bindNames = null;
            if (parametersToBind != null) bindNames = new List<string>(parametersToBind);
            if (bindNames == null || bindNames.Contains("CustomerAddressId")) sqlParameters.AddWithValue("@CustomerAddressId", obj.CustomerAddressId);
            if (bindNames == null || bindNames.Contains("CustomerId")) sqlParameters.AddWithValue("@CustomerId", obj.CustomerId);
            if (bindNames == null || bindNames.Contains("ShipToFlag")) sqlParameters.AddWithValue("@ShipToFlag", obj.ShipToFlag);
            if (bindNames == null || bindNames.Contains("BillToFlag")) sqlParameters.AddWithValue("@BillToFlag", obj.BillToFlag);
            if (bindNames == null || bindNames.Contains("ShipToName")) sqlParameters.AddWithValue("@ShipToName", obj.ShipToName);
            if (bindNames == null || bindNames.Contains("AddressLine1")) sqlParameters.AddWithValue("@AddressLine1", obj.AddressLine1);
            if (bindNames == null || bindNames.Contains("AddressLine2")) sqlParameters.AddWithValue("@AddressLine2", obj.AddressLine2);
            if (bindNames == null || bindNames.Contains("AddressLine3")) sqlParameters.AddWithValue("@AddressLine3", obj.AddressLine3);
            if (bindNames == null || bindNames.Contains("City")) sqlParameters.AddWithValue("@City", obj.City);
            if (bindNames == null || bindNames.Contains("StateCD")) sqlParameters.AddWithValue("@StateCD", obj.StateCD);
            if (bindNames == null || bindNames.Contains("ZipCode")) sqlParameters.AddWithValue("@ZipCode", obj.ZipCode);
            if (bindNames == null || bindNames.Contains("Created")) sqlParameters.AddWithValue("@Created", obj.Created);
            if (bindNames == null || bindNames.Contains("CreatedBy")) sqlParameters.AddWithValue("@CreatedBy", obj.CreatedBy);
            if (bindNames == null || bindNames.Contains("Modified")) sqlParameters.AddWithValue("@Modified", obj.Modified);
            if (bindNames == null || bindNames.Contains("ModifiedBy")) sqlParameters.AddWithValue("@ModifiedBy", obj.ModifiedBy);

        }

    }
}
