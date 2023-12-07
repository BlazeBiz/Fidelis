CREATE PROCEDURE [dbo].[SalesOrderAdd]
	@salesOrderJSON nvarchar(max)
AS
    Declare @userID INT = 1   -- Stub
Begin

    /**********************************************************************************************
    ***                         Prepare parameters and variables                                ***
    **********************************************************************************************/
    BEGIN -- Begin Declare variables and Parse JSON region
        declare @SalesOrderID int;
        declare @SalesOrderNbr nvarchar(30) = TRIM(JSON_VALUE(@salesOrderJSON, '$.SalesOrderNbr'));
        declare @CustomerID int = CONVERT(int, JSON_VALUE(@salesOrderJSON, '$.CustomerID'));
        declare @CustomerPurchaseOrderNbr nvarchar(30) = TRIM(JSON_VALUE(@salesOrderJSON, '$.CustomerPurchaseOrderNbr'));
        declare @SalesOrderStatusCd int =  1; -- (Open)  CONVERT(int, JSON_VALUE(@salesOrderJSON, '$.SalesOrderStatusCd'));
        declare @PaymentTermsCd int = CONVERT(int, JSON_VALUE(@salesOrderJSON, '$.PaymentTermsCd'));
        declare @ShipToAddressID int = CONVERT(int, JSON_VALUE(@salesOrderJSON, '$.ShipToAddressID'));
        declare @BillToAddressID int = CONVERT(int, JSON_VALUE(@salesOrderJSON, '$.BillToAddressID'));

        declare @now datetime = getutcdate();
    End -- End Declare variables and Parse JSON region

    Begin Transaction

    /*********************************************************************************************
    ***                               SalesOrder                                               ***
    **********************************************************************************************/

    BEGIN -- Insert Customer region
        INSERT INTO SalesOrder (
            SalesOrderNbr, 
            CustomerID, 
            CustomerPurchaseOrderNbr, 
            OrderDate, 
            SalesOrderStatusCd, 
            PaymentTermsCd, 
            ShipToAddressID, 
            BillToAddressID, 
            Created, 
            CreatedBy,
            Modified, 
            ModifiedBy
        ) VALUES (
            @SalesOrderNbr, 
            @CustomerID, 
            @CustomerPurchaseOrderNbr, 
            @now, 
            @SalesOrderStatusCd, 
            @PaymentTermsCd, 
            @ShipToAddressID, 
            @BillToAddressID, 
            @now, 
            @userID, 
            @now, 
            @userID
        );
    End -- End Update Customer region

    SELECT @SalesOrderID = @@IDENTITY;

    /**********************************************************************************************
    ***                          Other associate tables...                                      ***
    **********************************************************************************************/
    --BEGIN -- Begin Update Projects region

    --END -- End Update Projects region

    Commit Transaction

    -- Return the modified object
    Exec dbo.SalesOrderGet @SalesOrderID

    RETURN 0
End
