using MarauderServer.Models;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Diagnostics;
using System.Linq;

namespace MarauderServer.Data
{
    public partial class SalesOrderData
    {
        private CustomerData? customerData;

        public SalesOrderData(string connectionString, CustomerData customerData) : base(connectionString)
        {
            this.customerData = customerData;
        }

        internal override List<SalesOrder> GetListFromReader(SqlDataReader reader)
        {
            Debug.Assert(customerData != null);
            // First get the SalesOrder(s)
            List<SalesOrder> salesOrders = base.GetListFromReader(reader);

            // Get the Customer information for each salesOrder
            if (reader.NextResult())
            {
                // Now the CustomerData object will read what it needs to from the reader.
                List<Customer> customers = customerData.GetListFromReader(reader);

                // Build a dictionary for temporary use
                Dictionary<int, Customer> dictionary = new Dictionary<int, Customer>(
                    customers.Select<Customer, KeyValuePair<int, Customer>>(c => new KeyValuePair<int, Customer>(c.CustomerId, c))
                );

                // Go through each Sales Order and assign the salesOrder
                foreach (SalesOrder salesOrder in salesOrders)
                {
                    if (dictionary.ContainsKey(salesOrder.CustomerId))
                    {
                        // Find the associated salesOrder object
                        salesOrder.Customer = dictionary[salesOrder.CustomerId];

                        // Find the addresses in the salesOrder address array, and set shipto and billto
                        if (salesOrder.Customer.CustomerAddresses != null)
                        {
                            salesOrder.ShipToAddress =
                                salesOrder.Customer.CustomerAddresses.FirstOrDefault<CustomerAddress>(ca => ca.CustomerAddressId == salesOrder.ShipToAddressId);
                            salesOrder.BillToAddress =
                                salesOrder.Customer.CustomerAddresses.FirstOrDefault<CustomerAddress>(ca => ca.CustomerAddressId == salesOrder.BillToAddressId);
                        }
                    }
                }
            }

            return salesOrders;
        }

        /// <summary>
        /// Returns all sales order based on search criteria
        /// </summary>
        /// <returns>List of SalesOrder objects</returns>
        public IEnumerable<SalesOrder> ListSalesOrders(string searchField, string searchType, string searchValue)
        {
            searchField = searchField.ToLower();
            if (searchField != "customername" && searchField != "customernumber" && 
                searchField != "customerponumber" && searchField != "salesordernumber")
            {
                throw new Exception($"ListCustomers: Invalid search field: '{searchField}'");
            }

            string proc = "SalesOrderSearch";
            SqlCommand cmd = new SqlCommand(proc);
            cmd.CommandType = CommandType.StoredProcedure;

            if (searchType == "contains")
            {
                searchValue = "%" + searchValue + "%";
            }
            else if (searchType == "startswith")
            {
                searchValue = searchValue + "%";
            }
            else if (searchType != "equals")
            {
                throw new Exception($"ListCustomers: Invalid search type: '{searchType}'");
            }

            cmd.Parameters.AddWithValue("@searchField", searchField);
            cmd.Parameters.AddWithValue("@searchValue", searchValue);
            return GetList(cmd, "ListSalesOrders");
        }


        /// <summary>
        /// Returns all salesOrders for a salesOrder
        /// </summary>
        /// <returns>List of salesOrder objects for a salesOrder</returns>
        public IEnumerable<SalesOrder> ListSalesOrdersForCustomer(int customerId)
        {
            SqlCommand cmd = new SqlCommand("SalesOrderListForCustomer");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@customerId", customerId);
            return GetList(cmd, "ListSalesOrdersForCustomer");
        }

        /// <summary>
        /// Returns one salesOrder by ID
        /// </summary>
        /// <returns>A salesOrder object, NULL if not found</returns>
        public SalesOrder? GetSalesOrder(int salesOrderId)
        {
            SqlCommand cmd = new SqlCommand("SalesOrderGet");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@salesOrderId", salesOrderId);
            return this.GetObject(cmd, "GetSalesOrder");
        }

        public SalesOrder? UpdateSalesOrder(SalesOrder salesOrder)
        {
            SqlCommand cmd = new SqlCommand("SalesOrderUpdate") { CommandType = CommandType.StoredProcedure };
            string json = System.Text.Json.JsonSerializer.Serialize<SalesOrder>(salesOrder);
            cmd.Parameters.AddWithValue("@salesOrderJSON", json);

            return this.GetObject(cmd, "UpdateSalesOrder");
        }

        public SalesOrder InsertSalesOrder(SalesOrder salesOrder)
        {
            SqlCommand cmd = new SqlCommand("SalesOrderAdd") { CommandType = CommandType.StoredProcedure };
            string json = System.Text.Json.JsonSerializer.Serialize<SalesOrder>(salesOrder);
            cmd.Parameters.AddWithValue("@salesOrderJSON", json);

            SalesOrder? newSalesOrder = this.GetObject(cmd, "InsertSalesOrder");

            if (newSalesOrder == null)
            {
                // Something went wrong
                throw new Exception("Insert salesOrder failed.");
            }
            return newSalesOrder;
        }
    }
}
