-- Get a single SalesOrder. Get the entire object graph
CREATE PROCEDURE [viking].[SalesOrderGet]
	@salesOrderId int
AS

	-- SalesOrder
	SELECT * FROM SalesOrder WHERE SalesOrderId = @salesOrderId;

	if @@ROWCOUNT = 1
	BEGIN
		-- There was an order. Get the customer information
		DECLARE @customerId int 
		SELECT @customerId = CustomerId FROM SalesOrder WHERE SalesOrderId = @salesOrderId;
		-- Get Customer and CustomerAddress
		exec viking.CustomerGet @customerId

	END
RETURN 0
