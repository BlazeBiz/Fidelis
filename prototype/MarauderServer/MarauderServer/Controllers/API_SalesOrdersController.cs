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

        // GET: api/Customers
        [HttpGet("/api/customers/{customerID}/salesorders")]
        public IEnumerable<SalesOrder> ListForCustomer(int customerID)
        {
            return salesOrderService.ListSalesOrdersForCustomer(customerID);
        }

        // GET api/Customers/5
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
            if (id != salesOrder.SalesOrderID)
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
