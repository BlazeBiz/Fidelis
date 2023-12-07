﻿CREATE TABLE [dbo].[CustomerAddress]
(
    [CustomerAddressID] INT NOT NULL PRIMARY KEY IDENTITY(1001, 1), 
    [CustomerID] INT NOT NULL, 
    [ShipToFlag] BIT NOT NULL, 
    [BillToFlag] BIT NOT NULL,
    ShipToName varchar(40) NULL,
    AddressLine1 varchar(40) NULL,
    AddressLine2 varchar(40) NULL,
    AddressLine3 varchar(40) NULL,
    City varchar(40) NULL,
    StateCD char(2) NULL,
    ZipCode varchar(10) NULL, 
    [Created] DATETIME NOT NULL DEFAULT getutcdate(), 
    [CreatedBy] INT NOT NULL, 
    [Modified] DATETIME NOT NULL DEFAULT getutcdate(), 
    [ModifiedBy] INT NOT NULL, 
    CONSTRAINT [FK_CustomerAddress_Customer] FOREIGN KEY (CustomerID) REFERENCES Customer(CustomerID)
)