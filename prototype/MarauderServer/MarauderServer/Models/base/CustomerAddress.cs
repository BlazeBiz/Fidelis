using System;

namespace MarauderServer.Models
{
    public partial class CustomerAddress
    {
        public int CustomerAddressId { get; set; }
        public int CustomerID { get; set; }
        public bool ShipToFlag { get; set; }
        public bool BillToFlag { get; set; }
        public string? ShipToName { get; set; }
        public string? AddressLine1 { get; set; }
        public string? AddressLine2 { get; set; }
        public string? AddressLine3 { get; set; }
        public string? City { get; set; }
        public string? StateCD { get; set; }
        public string? ZipCode { get; set; }
        public DateTime Created { get; set; }
        public int CreatedBy { get; set; }
        public DateTime Modified { get; set; }
        public int ModifiedBy { get; set; }

    }
}
