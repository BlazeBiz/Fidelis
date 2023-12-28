CREATE PROCEDURE [dbo].[CustomerUpdate]
	@customerJSON nvarchar(max)
AS
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
    ***                               Projects                                                ***
    **********************************************************************************************/
    --BEGIN -- Begin Update Projects region

    --    Declare @projects table (
    --        [ProjectId] INT NOT NULL,
    --        [Sequence] INT NOT NULL
    --    );

    --    Insert @projects
    --        SELECT *   
    --            FROM OPENJSON(@portfolioJSON, N'$.Projects')  
    --            WITH ( 
    --                ProjectId INT,
    --                Sequence INT
    --            )

    --    /*** UPDATE ***/
    --    Update p
    --        Set Sequence = up.Sequence
    --        From Project p
    --        Join @projects up on p.ProjectId = up.ProjectId and p.PortfolioId = @PortfolioId
    --        -- Only change the sequence of Active projects
    --        where p.Status = 1 And p.Sequence != up.Sequence

    --END -- End Update Projects region

    Commit Transaction

    -- Return the modified object
    Exec dbo.CustomerGet @CustomerId

    RETURN 0
End
