-- Get a single SalesOrder. Get the entire object graph
CREATE PROCEDURE [dbo].[SalesOrderGet]
	@salesOrderID int
AS

	-- SalesOrder
	SELECT * FROM SalesOrder WHERE SalesOrderID = @salesOrderID;

	if @@ROWCOUNT = 1
	BEGIN
		-- There was an order. Get the customer information
		DECLARE @customerID int 
		SELECT @customerID = CustomerID FROM SalesOrder WHERE SalesOrderID = @salesOrderID;
		-- Get Customer and CustomerAddress
		exec CustomerGet @customerID

	END
RETURN 0
