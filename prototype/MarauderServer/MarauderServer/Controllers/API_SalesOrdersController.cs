using MarauderServer.Models;
using MarauderServer.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MarauderServer.Controllers
{
    [Route("api/salesorders")]
    [ApiController]
    public class API_SalesOrdersController : ControllerBase
    {
        private SalesOrderService salesOrderService;

        public API_SalesOrdersController(SalesOrderService salesOrderService)
        {
            this.salesOrderService = salesOrderService;
        }

        // GET: api/salesOrders
        [HttpGet]
        public ActionResult<IEnumerable<Customer>> Get([FromQuery] string searchField = "", [FromQuery] string searchType = "", [FromQuery] string searchValue = "")
        {
            if (String.IsNullOrEmpty(searchField) || String.IsNullOrEmpty(searchType) || String.IsNullOrEmpty(searchValue))
            {
                return BadRequest("Please specify a value for searchField, searchType, and searchValue");
            }
            searchField = searchField.Trim().ToLower();
            if (searchField != "salesordernumber" && searchField != "customerponumber" &&
                 searchField != "customername" && searchField != "customernumber")
            {
                return BadRequest("searchField must be 'salesOrderNumber' or 'customerPONumber' or 'customerName' or 'customerNumber'");
            }

            searchType = searchType.Trim().ToLower();
            if (searchType != "contains" && searchType != "equals" && searchType != "startswith")
            {
                return BadRequest("searchType must be 'contains' or 'equals' or 'startsWith'");
            }

            return Ok(salesOrderService.ListSalesOrders(searchField, searchType, searchValue));
        }

        // GET: api/Customers/{n}/salesorders
        [HttpGet("/api/customers/{customerId}/salesorders")]
        public IEnumerable<SalesOrder> ListForCustomer(int customerId)
        {
            return salesOrderService.ListSalesOrdersForCustomer(customerId);
        }

        // GET api/SalesOrders/5
        [HttpGet("{id}")]
        public ActionResult<SalesOrder> Get(int id)
        {
            SalesOrder? obj = salesOrderService.GetSalesOrder(id);
            if (obj == null)
            {
                return NotFound();
            }
            return Ok(obj);
        }

        // PUT api/SalesOrders/5
        [HttpPut("{id}")]
        public ActionResult<SalesOrder> UpdateSalesOrder(int id, SalesOrder salesOrder)
        {
            // Make sure id matches what is in the interaction
            if (id != salesOrder.SalesOrderId)
            {
                return BadRequest("Id in URL does not match Id in Body");
            }

            try
            {
                SalesOrder? obj = salesOrderService.UpdateSalesOrder(salesOrder);
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

        // POST api/SalesOrders
        [HttpPost()]
        public ActionResult<SalesOrder> CreateSalesOrder(SalesOrder salesOrder)
        {
            return salesOrderService.AddSalesOrder(salesOrder);
        }

    }
}
