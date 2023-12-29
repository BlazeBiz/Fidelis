CREATE PROCEDURE [dbo].[CustomerUpdate]
	@customerJSON nvarchar(max)
AS
    -- TODO
    Declare @userId INT = 1   -- Stub
Begin

    /**********************************************************************************************
    ***                         Prepare parameters and variables                                ***
    **********************************************************************************************/
    BEGIN -- Begin Declare variables and Parse JSON region
        declare @CustomerId int = Convert(int, JSON_VALUE(@customerJSON, '$.CustomerId'));
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
    BEGIN -- Update Customer region
        Update Customer Set
            CustomerName = @CustomerName,
            CustomerNbr = @CustomerNbr,
            PaymentTerms = @PaymentTerms,
            GLLink = @GLLink,
            Modified = @now,
            ModifiedBy = @userId
        Where CustomerId = @CustomerId
    End -- End Update Customer region

    /**********************************************************************************************
    ***                            CustomerAddress                                              ***
    **********************************************************************************************/
    BEGIN -- Begin Update CustomerAddress region

        Declare @customerAddress table (
            CustomerAddressId INT,
            CustomerId INT NOT NULL,
            ShipToFlag BIT NOT NULL,
            BillToFlag BIT NOT NULL,
            ShipToName varchar(40) NULL,
            AddressLine1 varchar(40) NULL,
            AddressLine2 varchar(40) NULL,
            AddressLine3 varchar(40) NULL,
            City varchar(40) NULL,
            StateCode char(2) NULL,
            ZipCode varchar(10) NULL,
            IsDeleted BIT NOT NULL DEFAULT 0
        );

        Insert @customerAddress 
            SELECT *   
                FROM OPENJSON(@customerJSON, N'$.CustomerAddresses')  
                WITH ( 
                    CustomerAddressId INT, 
                    CustomerId int,
                    ShipToFlag BIT,
                    BillToFlag BIT,
                    ShipToName varchar(40),
                    AddressLine1 varchar(40),
                    AddressLine2 varchar(40),
                    AddressLine3 varchar(40),
                    City varchar(40),
                    StateCode char(2),
                    ZipCode varchar(10),
                    IsDeleted BIT
                )  

        /*** DELETE ***/
        -- Mark any customerAddresses marked as deleted
        UPDATE ca
            SET IsDeleted = 1,
                DeletedBy = @userId
            FROM CustomerAddress ca
            JOIN @customerAddress uca on ca.CustomerAddressId = uca.CustomerAddressId
            WHERE uca.IsDeleted = 1


        /*** UPDATE ***/
        -- Make updates to the changed addresses
        UPDATE ca
            Set CustomerId = @CustomerId,
                ShipToFlag = uca.ShipToFlag,
                BillToFlag = uca.BillToFlag,
                ShipToName = uca.ShipToName,
                AddressLine1 = uca.AddressLine1,
                AddressLine2 = uca.AddressLine2,
                AddressLine3 = uca.AddressLine3,
                City = uca.City,
                StateCode = uca.StateCode,
                ZipCode = uca.ZipCode,
                Modified = @now,
                ModifiedBy = @userId
            FROM CustomerAddress ca
            JOIN @customerAddress uca on ca.CustomerAddressId = uca.CustomerAddressId
            WHERE uca.IsDeleted = 0
              AND uca.CustomerAddressId > 0

        /*** INSERT ***/
        -- There's no reference to new IDs in the Customer row, so we can simply add addresses to the table
        INSERT CustomerAddress 
            (CustomerId, ShipToFlag, BillToFlag, ShipToName, 
             AddressLine1, AddressLine2, AddressLine3, 
             City, StateCode, ZipCode, Created, CreatedBy, Modified, ModifiedBy)

             SELECT @CustomerId, ShipToFlag, BillToFlag, ShipToName, 
                AddressLine1, AddressLine2, AddressLine3, 
                City, StateCode, ZipCode, @now, @userId, @now, @userId
             FROM @customerAddress
             WHERE ISNULL(CustomerAddressId, 0) = 0
        
    END -- End Update CustomerAddress region

    Commit Transaction

    -- Return the modified object
    Exec dbo.CustomerGet @CustomerId

    RETURN 0
End
