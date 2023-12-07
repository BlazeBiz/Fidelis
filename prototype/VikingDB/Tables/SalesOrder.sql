CREATE TABLE [dbo].[SalesOrder]
(
	[SalesOrderID] INT NOT NULL PRIMARY KEY IDENTITY(1001, 1), 
    [SalesOrderNbr] VARCHAR(30) NULL, 
    [CustomerID] INT NOT NULL, 
    [CustomerPurchaseOrderNbr] VARCHAR(30) NULL, 
    [OrderDate] DATE NOT NULL DEFAULT getdate(),
    [SalesOrderStatusCd] INT NOT NULL DEFAULT 1, -- 1 open, 2 complete, 3 cancelled
    [PaymentTermsCd] INT NULL, 
    [ShipToAddressID] INT NULL, 
    [BillToAddressID] INT NULL, 
    [Created] DATETIME NOT NULL DEFAULT getutcdate(), 
    [CreatedBy] INT NOT NULL, 
    [Modified] DATETIME NOT NULL DEFAULT getutcdate(), 
    [ModifiedBy] INT NOT NULL, 
    CONSTRAINT [FK_SalesOrder_Customer] FOREIGN KEY (CustomerID) REFERENCES Customer(CustomerID)
)
