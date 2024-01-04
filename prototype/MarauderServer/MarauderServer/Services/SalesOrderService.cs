using MarauderServer.Data;
using MarauderServer.Models;

namespace MarauderServer.Services
{
    public class SalesOrderService
    {
        private SalesOrderData salesOrderData;

        public SalesOrderService(SalesOrderData SalesOrderData)
        {
            this.salesOrderData = SalesOrderData;
        }

        public SalesOrder? GetSalesOrder(int SalesOrderId)
        {
            return salesOrderData.GetSalesOrder(SalesOrderId);
        }

        public IEnumerable<SalesOrder> ListSalesOrders(string searchField, string searchType, string searchValue)
        {
            return salesOrderData.ListSalesOrders(searchField, searchType, searchValue);
        }

        public IEnumerable<SalesOrder> ListSalesOrdersForCustomer(int customerId)
        {
            return salesOrderData.ListSalesOrdersForCustomer(customerId);
        }
        public SalesOrder? UpdateSalesOrder(SalesOrder SalesOrder)
        {
            return salesOrderData.UpdateSalesOrder(SalesOrder);
        }

        public SalesOrder AddSalesOrder(SalesOrder SalesOrder)
        {
            return salesOrderData.InsertSalesOrder(SalesOrder);
        }
    }
}
