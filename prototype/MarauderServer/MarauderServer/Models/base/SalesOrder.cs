using System;

namespace MarauderServer.Models
{
    public partial class SalesOrder
    {
        public int SalesOrderID { get; set; }
        public string? SalesOrderNbr { get; set; }
        public int CustomerID { get; set; }
        public string? CustomerPurchaseOrderNbr { get; set; }
        public DateTime OrderDate { get; set; }
        public int SalesOrderStatusCd { get; set; }
        public int? PaymentTermsCd { get; set; }
        public int? ShipToAddressID { get; set; }
        public int? BillToAddressID { get; set; }
        public DateTime Created { get; set; }
        public int CreatedBy { get; set; }
        public DateTime Modified { get; set; }
        public int ModifiedBy { get; set; }

    }
}
