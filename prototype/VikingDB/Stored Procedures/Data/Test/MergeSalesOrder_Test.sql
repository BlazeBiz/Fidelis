CREATE PROCEDURE [dbo].[MergeSalesOrder_Test]
AS
SET NOCOUNT ON

Declare @table TABLE
(
	[SalesOrderId] INT NOT NULL PRIMARY KEY IDENTITY(1, 1), 
    [SalesOrderNbr] VARCHAR(30) NULL, 
    [CustomerId] INT NOT NULL, 
    [CustomerPurchaseOrderNbr] VARCHAR(30) NULL, 
    [OrderDate] DATE NOT NULL DEFAULT getdate(), 
    [SalesOrderStatusCd] INT NOT NULL DEFAULT 1, -- 1 open, 2 complete, 3 cancelled
    [PaymentTermsCd] INT NULL, 
    [ShipToAddressId] INT NULL, 
    [BillToAddressId] INT NULL
)

Insert @table Values
	('Nbr1', 1, 'PO number', DateAdd(day, -45, getdate()), 2, 1, 01, 02),
	('Nbr2', 1, 'PO number', DateAdd(day, -40, getdate()), 1, 1, 03, 03),
	('Nbr3', 2, 'PO number', DateAdd(day, -35, getdate()), 1, 1, 04, 05),
	('Nbr4', 2, 'PO number', DateAdd(day, -32, getdate()), 1, 1, 06, 06),
	('Nbr5', 3, 'PO number', DateAdd(day, -31, getdate()), 2, 1, 07, 08),
	('Nbr6', 3, 'PO number', DateAdd(day, -27, getdate()), 3, 1, 07, 09),
	('Nbr7', 3, 'PO number', DateAdd(day, -21, getdate()), 1, 1, 09, 09),
	('Nbr8', 4, 'PO number', DateAdd(day, -15, getdate()), 1, 1, 10, 11),
	('Nbr9', 4, 'PO number', DateAdd(day, -04, getdate()), 1, 1, 12, 12);


-- All work should be transacted
BEGIN TRANSACTION

------------------------------------------------------------------------------------------------------------
-- Merge SalesOrder
------------------------------------------------------------------------------------------------------------
SET IDENTITY_INSERT SalesOrder ON
MERGE INTO SalesOrder AS [Target]
USING
 (SELECT * from @table) AS [Source]
ON (Target.SalesOrderId = Source.SalesOrderId)
WHEN MATCHED Then
 UPDATE SET
	SalesOrderNbr = Source.SalesOrderNbr,
	CustomerId    = Source.CustomerId,
	CustomerPurchaseOrderNbr = Source.CustomerPurchaseOrderNbr,
	OrderDate     = Source.OrderDate,
	SalesOrderStatusCd = Source.SalesOrderStatusCd,
	PaymentTermsCd = Source.PaymentTermsCd,
	ShipToAddressId = Source.ShipToAddressId,
	BillToAddressId = Source.BillToAddressId,
	Modified     = getutcdate()

WHEN NOT MATCHED BY TARGET THEN
 INSERT(SalesOrderId, SalesOrderNbr, CustomerId, CustomerPurchaseOrderNbr, OrderDate, SalesOrderStatusCd, PaymentTermsCd, ShipToAddressId, BillToAddressId, Created, CreatedBy, Modified, ModifiedBy)
 VALUES(Source.SalesOrderId, Source.SalesOrderNbr, Source.CustomerId, Source.CustomerPurchaseOrderNbr, Source.OrderDate, Source.SalesOrderStatusCd, Source.PaymentTermsCd, Source.ShipToAddressId, Source.BillToAddressId, getutcdate(), 1, getutcdate(), 1)
WHEN NOT MATCHED BY SOURCE AND Target.SalesOrderId <= 1000 THEN 
 DELETE
;
SET IDENTITY_INSERT SalesOrder OFF


DECLARE @mergeError int, @mergeCount int, @tableName varchar(50)
SELECT @mergeError = @@ERROR, @mergeCount = @@ROWCOUNT, @tableName = '[SalesOrder]'

IF @mergeError != 0
 BEGIN
	PRINT 'ERROR OCCURRED IN MERGE FOR ' + @tableName + '. Rows affected: ' + CAST(@mergeCount AS VARCHAR(100)); -- SQL should always return zero rows affected
 END
ELSE
 BEGIN
	PRINT @tableName + ' rows affected by MERGE: ' + CAST(@mergeCount AS VARCHAR(100));
 END

COMMIT TRANSACTION
RETURN @mergeError