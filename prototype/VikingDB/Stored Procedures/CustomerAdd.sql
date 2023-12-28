﻿CREATE PROCEDURE [dbo].[CustomerAdd]
	@customerJSON nvarchar(max)
AS
    Declare @userId INT = 1   -- Stub
Begin

    /**********************************************************************************************
    ***                         Prepare parameters and variables                                ***
    **********************************************************************************************/
    BEGIN -- Begin Declare variables and Parse JSON region
        declare @CustomerId int;
        declare @CustomerName nvarchar(50) = TRIM(JSON_VALUE(@customerJSON, '$.CustomerName'));
        declare @CustomerNbr nvarchar(20) = TRIM(JSON_VALUE(@customerJSON, '$.CustomerNbr'));
        declare @PaymentTerms nvarchar(20) = TRIM(JSON_VALUE(@customerJSON, '$.PaymentTerms'));
        declare @GLLink nvarchar(20) = TRIM(JSON_VALUE(@customerJSON, '$.GLLink'));

        declare @now datetime = getutcdate();
    End -- End Declare variables and Parse JSON region

    Begin Transaction

    /**********************************************************************************************
    ***                               Customer                                                 ***
    **********************************************************************************************/
    BEGIN -- Insert Customer region
        INSERT INTO Customer (            
            CustomerName,
            CustomerNbr,
            PaymentTerms,
            GLLink,
            Created,
            CreatedBy,
            Modified,
            ModifiedBy
        ) VALUES (
            @CustomerName,
            @CustomerNbr,
            @PaymentTerms,
            @GLLink,
            @now,
            @userId,
            @now,
            @userId
        );
    End -- End Update Customer region

    SELECT @CustomerId = @@IDENTITY;

    /**********************************************************************************************
    ***                               Projects                                                ***
    **********************************************************************************************/
    --BEGIN -- Begin Update Projects region

    --END -- End Update Projects region

    Commit Transaction

    -- Return the modified object
    Exec dbo.CustomerGet @CustomerId

    RETURN 0
End
