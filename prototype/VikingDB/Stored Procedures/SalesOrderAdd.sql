CREATE PROCEDURE [dbo].[SalesOrderAdd]
	@salesOrderJSON nvarchar(max)
AS
    Declare @userId INT = 1   -- Stub
Begin

    /**********************************************************************************************
    ***                         Prepare parameters and variables                                ***
    **********************************************************************************************/
    BEGIN -- Begin Declare variables and Parse JSON region
        declare @SalesOrderId int;
        declare @SalesOrderNbr nvarchar(30) = TRIM(JSON_VALUE(@salesOrderJSON, '$.SalesOrderNbr'));
        declare @CustomerId int = CONVERT(int, JSON_VALUE(@salesOrderJSON, '$.CustomerId'));
        declare @CustomerPurchaseOrderNbr nvarchar(30) = TRIM(JSON_VALUE(@salesOrderJSON, '$.CustomerPurchaseOrderNbr'));
        declare @SalesOrderStatusCd int =  1; -- (Open)  CONVERT(int, JSON_VALUE(@salesOrderJSON, '$.SalesOrderStatusCd'));
        declare @PaymentTermsCd int = CONVERT(int, JSON_VALUE(@salesOrderJSON, '$.PaymentTermsCd'));
        declare @ShipToAddressId int = CONVERT(int, JSON_VALUE(@salesOrderJSON, '$.ShipToAddressId'));
        declare @BillToAddressId int = CONVERT(int, JSON_VALUE(@salesOrderJSON, '$.BillToAddressId'));

        declare @now datetime = getutcdate();
    End -- End Declare variables and Parse JSON region

    Begin Transaction

    /*********************************************************************************************
    ***                               SalesOrder                                               ***
    **********************************************************************************************/

    BEGIN -- Insert Customer region
        INSERT INTO SalesOrder (
            SalesOrderNbr, 
            CustomerId, 
            CustomerPurchaseOrderNbr, 
            OrderDate, 
            SalesOrderStatusCd, 
            PaymentTermsCd, 
            ShipToAddressId, 
            BillToAddressId, 
            Created, 
            CreatedBy,
            Modified, 
            ModifiedBy
        ) VALUES (
            @SalesOrderNbr, 
            @CustomerId, 
            @CustomerPurchaseOrderNbr, 
            @now, 
            @SalesOrderStatusCd, 
            @PaymentTermsCd, 
            @ShipToAddressId, 
            @BillToAddressId, 
            @now, 
            @userId, 
            @now, 
            @userId
        );
    End -- End Update Customer region

    SELECT @SalesOrderId = @@IDENTITY;

    /**********************************************************************************************
    ***                          Other associate tables...                                      ***
    **********************************************************************************************/
    --BEGIN -- Begin Update Projects region

    --END -- End Update Projects region

    Commit Transaction

    -- Return the modified object
    Exec dbo.SalesOrderGet @SalesOrderId

    RETURN 0
End
