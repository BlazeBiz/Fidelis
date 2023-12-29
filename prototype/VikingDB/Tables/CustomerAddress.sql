CREATE TABLE [dbo].[CustomerAddress]
(
    [CustomerAddressId] INT NOT NULL PRIMARY KEY IDENTITY(1001, 1), 
    [CustomerId] INT NOT NULL, 
    [ShipToFlag] BIT NOT NULL, 
    [BillToFlag] BIT NOT NULL,
    ShipToName varchar(40) NULL,
    AddressLine1 varchar(40) NULL,
    AddressLine2 varchar(40) NULL,
    AddressLine3 varchar(40) NULL,
    City varchar(40) NULL,
    StateCode char(2) NULL,
    ZipCode varchar(10) NULL, 
    [Created] DATETIME NOT NULL DEFAULT getutcdate(), 
    [CreatedBy] INT NOT NULL, 
    [Modified] DATETIME NOT NULL DEFAULT getutcdate(), 
    [ModifiedBy] INT NOT NULL,
    [IsDeleted] BIT NOT NULL DEFAULT 0,
    [Deleted] DATETIME NULL,
    [DeletedBy] INT NULL, 
    CONSTRAINT [FK_CustomerAddress_Customer] FOREIGN KEY (CustomerId) REFERENCES Customer(CustomerId)
)
