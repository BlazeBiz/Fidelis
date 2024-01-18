CREATE PROCEDURE [viking].[SalesOrderAdd]
	@salesOrderJSON nvarchar(max)
AS
    Declare @userId INT = 1   -- Stub
Begin

    /**********************************************************************************************
    ***                         Prepare parameters and variables                                ***
    **********************************************************************************************/
    BEGIN -- Begin Declare variables and Parse JSON region
        declare @SalesOrderId int;
        declare @SalesOrderNumber nvarchar(30) = TRIM(JSON_VALUE(@salesOrderJSON, '$.SalesOrderNumber'));
        declare @CustomerId int = CONVERT(int, JSON_VALUE(@salesOrderJSON, '$.CustomerId'));
        declare @CustomerPONumber nvarchar(30) = TRIM(JSON_VALUE(@salesOrderJSON, '$.CustomerPONumber'));
        declare @SalesOrderStatusCode int =  1; -- (Open)  CONVERT(int, JSON_VALUE(@salesOrderJSON, '$.SalesOrderStatusCode'));
        declare @PaymentTermsCode int = CONVERT(int, JSON_VALUE(@salesOrderJSON, '$.PaymentTermsCode'));
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
            SalesOrderNumber, 
            CustomerId, 
            CustomerPONumber, 
            OrderDate, 
            SalesOrderStatusCode, 
            PaymentTermsCode, 
            ShipToAddressId, 
            BillToAddressId, 
            Created, 
            CreatedBy,
            Modified, 
            ModifiedBy
        ) VALUES (
            @SalesOrderNumber, 
            @CustomerId, 
            @CustomerPONumber, 
            @now, 
            @SalesOrderStatusCode, 
            @PaymentTermsCode, 
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
    Exec viking.SalesOrderGet @SalesOrderId

    RETURN 0
End
