using System;

namespace MarauderServer.Models
{
    public partial class SalesOrder
    {
        public Customer? Customer { get; set; }
        public CustomerAddress? ShipToAddress { get; set; }
        public CustomerAddress? BillToAddress { get; set; }

    }
}
