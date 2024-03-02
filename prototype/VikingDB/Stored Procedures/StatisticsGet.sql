CREATE PROCEDURE [viking].[StatisticsGet]
AS
Select (Select COUNT(*) from viking.Customer) AS NumberCustomers,
	   (Select COUNT(*) from viking.SalesOrder) AS NumberSalesOrders
RETURN 0
