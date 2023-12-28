﻿CREATE TABLE [dbo].[SalesOrder]
(
	[SalesOrderId] INT NOT NULL PRIMARY KEY IDENTITY(1001, 1), 
    [SalesOrderNbr] VARCHAR(30) NULL, 
    [CustomerId] INT NOT NULL, 
    [CustomerPurchaseOrderNbr] VARCHAR(30) NULL, 
    [OrderDate] DATE NOT NULL DEFAULT getdate(),
    [SalesOrderStatusCd] INT NOT NULL DEFAULT 1, -- 1 open, 2 complete, 3 cancelled
    [PaymentTermsCd] INT NULL, 
    [ShipToAddressId] INT NULL, 
    [BillToAddressId] INT NULL, 
    [Created] DATETIME NOT NULL DEFAULT getutcdate(), 
    [CreatedBy] INT NOT NULL, 
    [Modified] DATETIME NOT NULL DEFAULT getutcdate(), 
    [ModifiedBy] INT NOT NULL, 
    CONSTRAINT [FK_SalesOrder_Customer] FOREIGN KEY (CustomerId) REFERENCES Customer(CustomerId)
)
