CREATE PROCEDURE [viking].[CustomerSearchByName]
	@searchValue varchar(100)
AS
	SELECT * 
	FROM Customer
	WHERE CustomerName LIKE @searchValue
RETURN 0
