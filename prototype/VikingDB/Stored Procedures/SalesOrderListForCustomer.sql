-- Get a single SalesOrder. Get the entire object graph
CREATE PROCEDURE [dbo].[SalesOrderListForCustomer]
	@customerID int
AS

	-- SalesOrder
	SELECT * FROM SalesOrder WHERE CustomerID = @customerID;

	if @@ROWCOUNT > 0
	BEGIN
		-- There was at least one order. Get the customer information
		-- Get Customer and CustomerAddress
		exec CustomerGet @customerID
	END

RETURN 0
