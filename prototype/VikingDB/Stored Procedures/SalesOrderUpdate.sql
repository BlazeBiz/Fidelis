CREATE PROCEDURE [dbo].[SalesOrderUpdate]
	@salesOrderJSON nvarchar(max)
AS
    Declare @userID INT = 1   -- Stub
Begin

    /**********************************************************************************************
    ***                         Prepare parameters and variables                                ***
    **********************************************************************************************/
    BEGIN -- Begin Declare variables and Parse JSON region
        declare @SalesOrderID int = Convert(int, JSON_VALUE(@salesOrderJSON, '$.SalesOrderID'));
        declare @SalesOrderNbr nvarchar(30) = TRIM(JSON_VALUE(@salesOrderJSON, '$.SalesOrderNbr'));
        --declare @CustomerID int = CONVERT(int, JSON_VALUE(@salesOrderJSON, '$.CustomerID'));
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
    BEGIN -- Update SalesOrder region
        Update SalesOrder Set
            SalesOrderNbr = @SalesOrderNbr, 
            -- can't change CustomerID
            CustomerPurchaseOrderNbr = @CustomerPurchaseOrderNbr, 
            -- can't change OrderDate, 
            SalesOrderStatusCd = @SalesOrderStatusCd, 
            PaymentTermsCd = @PaymentTermsCd, 
            ShipToAddressID = @ShipToAddressID, 
            BillToAddressID = @BillToAddressID, 
            Modified = @now, 
            ModifiedBy = @userID
        Where SalesOrderID = @SalesOrderID
    End -- End Update SalesOrder region

    /*********************************************************************************************
    ***                            Other tables                                                ***
    *********************************************************************************************/
    --BEGIN -- Begin Update Projects region
    --END -- End Update Projects region

    Commit Transaction

    -- Return the modified object
    Exec dbo.SalesOrderGet @SalesOrderID

    RETURN 0
End
