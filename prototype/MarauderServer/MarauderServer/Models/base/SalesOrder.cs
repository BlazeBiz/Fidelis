using System;

namespace MarauderServer.Models
{
    public partial class SalesOrder
    {
        public int SalesOrderId { get; set; }
        public string? SalesOrderNumber { get; set; }
        public int CustomerId { get; set; }
        public string? CustomerPONumber { get; set; }
        public DateTime OrderDate { get; set; }
        public int SalesOrderStatusCode { get; set; }
        public int? PaymentTermsCode { get; set; }
        public int? ShipToAddressId { get; set; }
        public int? BillToAddressId { get; set; }
        public DateTime Created { get; set; }
        public int CreatedBy { get; set; }
        public DateTime Modified { get; set; }
        public int ModifiedBy { get; set; }

    }
}
