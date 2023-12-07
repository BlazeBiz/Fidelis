using MarauderServer.Data;
using MarauderServer.Models;

namespace MarauderServer.Services
{
    public class CustomerService
    {
        private CustomerData customerData;

        public CustomerService(CustomerData customerData)
        {
            this.customerData = customerData;
        }

        public Customer? GetCustomer(int customerID)
        {
            return customerData.GetCustomer(customerID);
        }

        public IEnumerable<Customer> ListCustomers(string searchField, string searchType, string searchValue)
        {
            return customerData.ListCustomers(searchField, searchType, searchValue);
        }
        public Customer? UpdateCustomer(Customer customer)
        {
            return customerData.UpdateCustomer(customer);
        }
        public Customer AddCustomer(Customer customer)
        {
            return customerData.InsertCustomer(customer);
        }
    }
}
