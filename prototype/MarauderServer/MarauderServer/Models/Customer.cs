using System;

namespace MarauderServer.Models
{
    public partial class Customer
    {
        public List<CustomerAddress>? CustomerAddresses { get; set; }
    }
}
