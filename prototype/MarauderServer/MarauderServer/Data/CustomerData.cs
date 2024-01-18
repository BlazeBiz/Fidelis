using MarauderServer.Models;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Diagnostics;

namespace MarauderServer.Data
{
    public partial class CustomerData
    {
        private CustomerAddressData? customerAddressData;

        public CustomerData(string connectionString, CustomerAddressData customerAddressData) : base(connectionString)
        {
            this.customerAddressData = customerAddressData;
        }

        internal override List<Customer> GetListFromReader(SqlDataReader reader)
        {
            Debug.Assert(customerAddressData != null);
            // First get the Customer(s)
            List<Customer> customers = base.GetListFromReader(reader);

            // Build a dictionary for temporary use
            Dictionary<int, Customer> dictionary = new Dictionary<int, Customer>(
                customers.Select<Customer, KeyValuePair<int, Customer>>(c => new KeyValuePair<int, Customer>(c.CustomerId, c))
            );

            // Get the Addresses, and assign each to a Customer
            if (reader.NextResult())
            {
                // Now the CustomerAddressData object will read what it needs to from the reader.
                List<CustomerAddress> addresses = customerAddressData.GetListFromReader(reader);
                foreach (CustomerAddress address in addresses)
                {
                    // Find the customer and add it to the addresses collection
                    Customer customer = dictionary[address.CustomerId];
                    if (customer.CustomerAddresses == null) customer.CustomerAddresses = new List<CustomerAddress>();
                    customer.CustomerAddresses.Add(address);
                }
            }

            return customers;
        }

        /// <summary>
        /// Returns all customers
        /// </summary>
        /// <returns>List of Customer objects</returns>
        public IEnumerable<Customer> ListCustomers(string searchField, string searchType, string searchValue)
        {
            string proc;
            searchField = searchField.ToLower();
            if (searchField == "customername")
            {
                proc = "viking.CustomerSearchByName";
            } else if (searchField == "customernumber")
            {
                proc = "viking.CustomerSearchByNbr";
            }
            else
            {
                throw new Exception($"ListCustomers: Invalid search field: '{searchField}'");
            }
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

            cmd.Parameters.AddWithValue("@searchValue", searchValue);
            return GetList(cmd, "ListCustomers");
        }

        /// <summary>
        /// Returns one customer by ID
        /// </summary>
        /// <returns>A Customer object, NULL if not found</returns>
        public Customer? GetCustomer(int customerId)
        {
            SqlCommand cmd = new SqlCommand("viking.CustomerGet");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@customerId", customerId);
            return this.GetObject(cmd, "GetCustomer");
        }

        public Customer? UpdateCustomer(Customer customer)
        {
            SqlCommand cmd = new SqlCommand("viking.CustomerUpdate") { CommandType = CommandType.StoredProcedure };
            string json = System.Text.Json.JsonSerializer.Serialize<Customer>(customer);
            cmd.Parameters.AddWithValue("@customerJSON", json);

            return this.GetObject(cmd, "UpdateCustomer");
        }

        public Customer InsertCustomer(Customer customer)
        {
            SqlCommand cmd = new SqlCommand("viking.CustomerAdd") { CommandType = CommandType.StoredProcedure };
            string json = System.Text.Json.JsonSerializer.Serialize<Customer>(customer);
            cmd.Parameters.AddWithValue("@customerJSON", json);

            Customer? newCustomer = this.GetObject(cmd, "InsertCustomer");

            if (newCustomer == null)
            {
                // Something went wrong
                throw new Exception("Insert customer failed.");
            }
            return newCustomer;
        }
    }
}
