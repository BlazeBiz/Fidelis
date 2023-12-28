using MarauderServer.Data;
using MarauderServer.Models;

namespace MarauderServer.Services
{
    public class SalesOrderService
    {
        private SalesOrderData SalesOrderData;

        public SalesOrderService(SalesOrderData SalesOrderData)
        {
            this.SalesOrderData = SalesOrderData;
        }

        public SalesOrder? GetSalesOrder(int SalesOrderId)
        {
            return SalesOrderData.GetSalesOrder(SalesOrderId);
        }

        public IEnumerable<SalesOrder> ListSalesOrdersForCustomer(int customerId)
        {
            return SalesOrderData.ListSalesOrdersForCustomer(customerId);
        }
        public SalesOrder? UpdateSalesOrder(SalesOrder SalesOrder)
        {
            return SalesOrderData.UpdateSalesOrder(SalesOrder);
        }

        public SalesOrder AddSalesOrder(SalesOrder SalesOrder)
        {
            return SalesOrderData.InsertSalesOrder(SalesOrder);
        }
    }
}
