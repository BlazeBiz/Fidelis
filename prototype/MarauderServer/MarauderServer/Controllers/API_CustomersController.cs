using MarauderServer.Models;
using MarauderServer.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MarauderServer.Controllers
{
    [Route("api/customers")]
    [ApiController]
    public class API_CustomersController : ControllerBase
    {
        private CustomerService customerService;

        public API_CustomersController(CustomerService customerService)
        {
            this.customerService = customerService;
        }

        // GET: api/Customers
        [HttpGet]
        public ActionResult<IEnumerable<Customer>> Get([FromQuery] string searchField="", [FromQuery] string searchType="", [FromQuery] string searchValue="")
        {
            if (String.IsNullOrEmpty(searchField) || String.IsNullOrEmpty(searchType) || String.IsNullOrEmpty(searchValue))
            {
                return BadRequest("Please specify a value for searchField, searchType, and searchValue");
            }
            searchField = searchField.Trim().ToLower();
            if (searchField != "customername" && searchField != "customernumber")
            {
                return BadRequest("searchField must be 'customerName' or 'customerNumber'");
            }

            searchType = searchType.Trim().ToLower();
            if (searchType != "contains" && searchType != "equals" && searchType != "startswith")
            {
                return BadRequest("searchType must be 'contains' or 'equals' or 'startsWith'");
            }

            return Ok(customerService.ListCustomers(searchField, searchType, searchValue));
        }

        // GET api/Customers/5
        [HttpGet("{id}")]
        public ActionResult<Customer> Get(int id)
        {
            Customer? obj = customerService.GetCustomer(id);
            if (obj == null)
            {
                return NotFound();
            }
            return Ok(obj);
        }

        // PUT api/Customers/5
        [HttpPut("{id}")]
        public ActionResult<Customer> UpdateCustomer(int id, Customer customer)
        {
            // Make sure id matches what is in the interaction
            if (id != customer.CustomerId)
            {
                return BadRequest("Id in URL does not match Id in Body");
            }

            try
            {
                Customer? obj = customerService.UpdateCustomer(customer);
                if (obj == null)
                {
                    return NotFound();
                }
                else
                {
                    return Ok(obj);
                }
            }
            //catch (PermissionException)
            //{
            //    return Forbid();
            //}
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        // POST api/Customers
        [HttpPost()]
        public ActionResult<Customer> CreateCustomer(Customer customer)
        {
            return customerService.AddCustomer(customer);
        }

    }
}
