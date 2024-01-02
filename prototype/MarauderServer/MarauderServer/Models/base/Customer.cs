using System;

namespace MarauderServer.Models
{
    public partial class Customer
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; } = "";
        public string? CustomerNumber { get; set; }
        public string? PaymentTerms { get; set; }
        public string? GLLink { get; set; }
        public DateTime Created { get; set; }
        public int CreatedBy { get; set; }
        public DateTime Modified { get; set; }
        public int ModifiedBy { get; set; }

    }
}
