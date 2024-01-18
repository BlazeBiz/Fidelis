CREATE PROCEDURE [viking].[SalesOrderSearch]
	@searchField varchar(30),
	@searchValue varchar(100)
AS
	-- A list of all the Customer ids of the Sales Orders
	DECLARE @customerIds TABLE (
		CustomerId int NOT NULL
	)
	-- A list of all the SalesOrder ids that satisfy the search
	DECLARE @salesOrderIds TABLE (
		SalesOrderId int NOT NULL
	)

	/*****************************************************************
	* Execute queries to find all ids based on the searchField
	*****************************************************************/
	IF @searchField = 'customername'
	BEGIN
		INSERT @customerIds
			SELECT CustomerId 
			FROM Customer 
			WHERE CustomerName LIKE @searchValue
		INSERT @salesOrderIds
			SELECT SalesOrderId 
			FROM SalesOrder 
			WHERE CustomerId IN (SELECT CustomerId FROM @customerIds)
	END
	ELSE IF @searchField = 'customernumber'
	BEGIN
		INSERT @customerIds
			SELECT CustomerId 
			FROM Customer 
			WHERE CustomerNumber LIKE @searchValue
		INSERT @salesOrderIds
			SELECT SalesOrderId 
			FROM SalesOrder 
			WHERE CustomerId IN (SELECT CustomerId FROM @customerIds)
	END
	ELSE IF @searchField = 'salesordernumber'
	BEGIN
		INSERT @salesOrderIds
			SELECT SalesOrderId 
			FROM SalesOrder 
			WHERE SalesOrderNumber LIKE @searchValue
		INSERT @customerIds
			SELECT DISTINCT CustomerId 
			FROM SalesOrder 
			WHERE SalesOrderId IN (SELECT SalesOrderId FROM @salesOrderIds)
	END
	ELSE IF @searchField = 'customerponumber'
	BEGIN
		INSERT @salesOrderIds
			SELECT SalesOrderId 
			FROM SalesOrder 
			WHERE CustomerPONumber LIKE @searchValue
		INSERT @customerIds
			SELECT DISTINCT CustomerId 
			FROM SalesOrder 
			WHERE SalesOrderId IN (SELECT SalesOrderId FROM @salesOrderIds)
	END
	--ELSE
	--BEGIN
	--	-- TODO: Raise Error here, bad searchField
	--END

	-- Select the matching Sales Orders
	SELECT * 
		FROM SalesOrder
		WHERE SalesOrderId IN (SELECT SalesOrderId FROM @salesOrderIds)

	if @@ROWCOUNT > 0
	BEGIN
		-- Get the customer information
		SELECT * 
			FROM Customer
			WHERE CustomerId IN (SELECT CustomerId FROM @customerIds)

			-- Select the CustomerAddress information
		SELECT * 
			FROM CustomerAddress 
			WHERE CustomerId IN (SELECT CustomerId FROM @customerIds)
			AND IsDeleted = 0;
	END
RETURN 0
