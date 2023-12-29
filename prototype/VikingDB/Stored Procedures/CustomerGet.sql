-- Get a single Customer. Get the entire object graph
CREATE PROCEDURE [dbo].[CustomerGet]
	@customerId int
AS

	-- Customer
	SELECT * FROM Customer WHERE CustomerId = @customerId

	-- CustomerAddress
	SELECT * 
		FROM CustomerAddress 
		WHERE CustomerId = @customerId
		  AND IsDeleted = 0;

	-- Could do SalesOrder, but won't do that right now, as that could be a large graph. 
	-- Plus, GetSalesOrder will probably call this procedure, so we don't want to recurse.
RETURN 0
