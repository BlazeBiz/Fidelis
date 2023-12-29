CREATE PROCEDURE [dbo].[MergeCustomer_Test]
AS
SET NOCOUNT ON

Declare @table TABLE
(
	[CustomerId] INT NOT NULL PRIMARY KEY IDENTITY(1, 1),
    [CustomerName] VARCHAR(50) NOT NULL, 
    [CustomerNbr] VARCHAR(20) NULL, 
    [PaymentTerms] VARCHAR(20) NULL, 
    [GLLink] VARCHAR(20) NULL
)

Insert @table Values
	('Customer 1', 'AAA', 'Net 10', NULL),
	('Customer 2', 'BBB', 'Net 10', NULL),
	('Customer 3', 'CCC', 'Net 10', NULL),
	('Customer 4', 'DDD', 'Net 10', NULL),
	('Customer 5', 'EEE', 'Net 10', NULL);

Declare @addressTable TABLE
(
    [CustomerAddressId] INT NOT NULL PRIMARY KEY IDENTITY(1, 1), 
    [CustomerId] INT NOT NULL, 
    [ShipToFlag] BIT NOT NULL, 
    [BillToFlag] BIT NOT NULL,
    ShipToName varchar(40) NULL,
    AddressLine1 varchar(40) NULL,
    AddressLine2 varchar(40) NULL,
    AddressLine3 varchar(40) NULL,
    City varchar(40) NULL,
    StateCode char(2) NULL,
    ZipCode varchar(10) NULL
)

Insert @addressTable Values
(1, 1, 0, 'ST Name 1.1', '11 Address line one', 'Address line two', null, 'Onetown', 'OH', 44441),
(1, 0, 1, null         , '12 Address line one', 'Address line two', null, 'Twotown', 'OH', 44442),
(1, 1, 1, 'ST Name 1.3', '13 Address line one', 'Address line two', null, 'Threetown', 'OH', 44443),
(2, 1, 0, 'ST Name 2.1', '21 Address line one', 'Address line two', null, 'Onetown', 'OH', 44441),
(2, 0, 1, null         , '22 Address line one', 'Address line two', null, 'Twotown', 'OH', 44442),
(2, 1, 1, 'ST Name 2.3', '23 Address line one', 'Address line two', null, 'Threetown', 'OH', 44443),
(3, 1, 0, 'ST Name 3.1', '31 Address line one', 'Address line two', null, 'Onetown', 'OH', 44441),
(3, 0, 1, null         , '32 Address line one', 'Address line two', null, 'Twotown', 'OH', 44442),
(3, 1, 1, 'ST Name 3.3', '33 Address line one', 'Address line two', null, 'Threetown', 'OH', 44443),
(4, 1, 0, 'ST Name 4.1', '41 Address line one', 'Address line two', null, 'Onetown', 'OH', 44441),
(4, 0, 1, null         , '42 Address line one', 'Address line two', null, 'Twotown', 'OH', 44442),
(4, 1, 1, 'ST Name 4.3', '43 Address line one', 'Address line two', null, 'Threetown', 'OH', 44443),
(5, 1, 0, 'ST Name 5.1', '51 Address line one', 'Address line two', null, 'Onetown', 'OH', 44441),
(5, 0, 1, null         , '52 Address line one', 'Address line two', null, 'Twotown', 'OH', 44442),
(5, 1, 1, 'ST Name 5.3', '53 Address line one', 'Address line two', null, 'Threetown', 'OH', 44443);

-- All work should be transacted
BEGIN TRANSACTION
-- Prevent RI conflicts on a delete of customers by removing Addresses for those to be deleted
DELETE FROM CustomerAddress
    WHERE CustomerId <= 1000
      AND CustomerId NOT IN (SELECT DISTINCT CustomerId from @table)

------------------------------------------------------------------------------------------------------------
-- Merge Customer
------------------------------------------------------------------------------------------------------------
SET IDENTITY_INSERT Customer ON
MERGE INTO Customer AS [Target]
USING
 (SELECT * from @table) AS [Source]
ON (Target.CustomerId = Source.CustomerId)
WHEN MATCHED Then
 UPDATE SET
	CustomerName = Source.CustomerName,
	CustomerNbr  = Source.CustomerNbr,
	PaymentTerms = Source.PaymentTerms,
	GLLink       = Source.GLLink,
	Modified     = getutcdate()

WHEN NOT MATCHED BY TARGET THEN
 INSERT(CustomerId, CustomerName, CustomerNbr, PaymentTerms, GLLink, Created, CreatedBy, Modified, ModifiedBy)
 VALUES(Source.CustomerId, Source.CustomerName, Source.CustomerNbr, Source.PaymentTerms, Source.GLLink, getutcdate(), 1, getutcdate(), 1)
WHEN NOT MATCHED BY SOURCE AND Target.CustomerId <= 1000 THEN 
 DELETE
;
SET IDENTITY_INSERT Customer OFF


DECLARE @mergeError int, @mergeCount int, @tableName varchar(50)
SELECT @mergeError = @@ERROR, @mergeCount = @@ROWCOUNT, @tableName = '[Customer]'

IF @mergeError != 0
 BEGIN
	PRINT 'ERROR OCCURRED IN MERGE FOR ' + @tableName + '. Rows affected: ' + CAST(@mergeCount AS VARCHAR(100)); -- SQL should always return zero rows affected
 END
ELSE
 BEGIN
	PRINT @tableName + ' rows affected by MERGE: ' + CAST(@mergeCount AS VARCHAR(100));
 END

------------------------------------------------------------------------------------------------------------
-- Merge CustomerAddress
------------------------------------------------------------------------------------------------------------
SET IDENTITY_INSERT CustomerAddress ON
MERGE INTO CustomerAddress AS [Target]
USING
 (SELECT * from @addressTable) AS [Source]
ON (Target.CustomerAddressId = Source.CustomerAddressId)
WHEN MATCHED Then
 UPDATE SET
	CustomerId  = Source.CustomerId,
	ShipToFlag  = Source.ShipToFlag,
	BillToFlag  = Source.BillToFlag,
	ShipToName  = Source.ShipToName,
	AddressLine1 = Source.AddressLine1,
	AddressLine2 = Source.AddressLine2,
	AddressLine3 = Source.AddressLine3,
	City = Source.City,
	StateCode = Source.StateCode,
	ZipCode = Source.ZipCode,
	Modified    = getutcdate()


WHEN NOT MATCHED BY TARGET THEN
 INSERT(CustomerAddressId, CustomerId, ShipToFlag, BillToFlag, ShipToName, AddressLine1, AddressLine2, AddressLine3, City, StateCode, ZipCode, Created, CreatedBy, Modified, ModifiedBy)
 VALUES(Source.CustomerAddressId, Source.CustomerId, Source.ShipToFlag, Source.BillToFlag, Source.ShipToName, 
	Source.AddressLine1, Source.AddressLine2, Source.AddressLine3, Source.City, Source.StateCode, Source.ZipCode, getutcdate(), 1, getutcdate(), 1)
WHEN NOT MATCHED BY SOURCE AND Target.CustomerAddressId <= 1000 THEN 
 DELETE
;
SET IDENTITY_INSERT CustomerAddress OFF


SELECT @mergeError = @@ERROR, @mergeCount = @@ROWCOUNT, @tableName = '[CustomerAddress]'

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