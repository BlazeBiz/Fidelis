CREATE PROCEDURE [dbo].[CustomerSearchByNbr]
	@searchValue varchar(100)
AS
	SELECT * 
	FROM Customer
	WHERE CustomerNbr LIKE @searchValue
RETURN 0
