-- Get a single SalesOrder. Get the entire object graph
CREATE PROCEDURE [viking].[SalesOrderListForCustomer]
	@customerId int
AS

	-- SalesOrder
	SELECT * FROM SalesOrder WHERE CustomerId = @customerId;

	if @@ROWCOUNT > 0
	BEGIN
		-- There was at least one order. Get the customer information
		-- Get Customer and CustomerAddress
		exec viking.CustomerGet @customerId
	END

RETURN 0
