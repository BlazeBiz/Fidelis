CREATE PROCEDURE [dbo].[SalesOrderUpdate]
	@salesOrderJSON nvarchar(max)
AS
    Declare @userId INT = 1   -- Stub
Begin

    /**********************************************************************************************
    ***                         Prepare parameters and variables                                ***
    **********************************************************************************************/
    BEGIN -- Begin Declare variables and Parse JSON region
        declare @SalesOrderId int = Convert(int, JSON_VALUE(@salesOrderJSON, '$.SalesOrderId'));
        declare @SalesOrderNumber nvarchar(30) = TRIM(JSON_VALUE(@salesOrderJSON, '$.SalesOrderNumber'));
        --declare @CustomerId int = CONVERT(int, JSON_VALUE(@salesOrderJSON, '$.CustomerId'));
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
    BEGIN -- Update SalesOrder region
        Update SalesOrder Set
            SalesOrderNumber = @SalesOrderNumber, 
            -- can't change CustomerId
            CustomerPONumber = @CustomerPONumber, 
            -- can't change OrderDate, 
            SalesOrderStatusCode = @SalesOrderStatusCode, 
            PaymentTermsCode = @PaymentTermsCode, 
            ShipToAddressId = @ShipToAddressId, 
            BillToAddressId = @BillToAddressId, 
            Modified = @now, 
            ModifiedBy = @userId
        Where SalesOrderId = @SalesOrderId
    End -- End Update SalesOrder region

    /*********************************************************************************************
    ***                            Other tables                                                ***
    *********************************************************************************************/
    --BEGIN -- Begin Update Projects region
    --END -- End Update Projects region

    Commit Transaction

    -- Return the modified object
    Exec dbo.SalesOrderGet @SalesOrderId

    RETURN 0
End
