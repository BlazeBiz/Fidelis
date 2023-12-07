CREATE PROCEDURE [dbo].[_MergeProductionData]
AS
Print '======================='
Print 'Merging PRODUCTION data'
Print '======================='

	-- Clear the results table
		--Delete from _DataMergeResult
		Declare @returnValue int

	-- ------------------------------------------------------------------------------------------------------------
	-- MergeCustomer
	-- ------------------------------------------------------------------------------------------------------------
	--	
	-- MergeCustomer
	--Exec @returnValue = MergeCustomer_Test
	--if @returnValue <> 0 return @returnValue

	---- Show the results
	--Select * from _DataMergeResult

RETURN 0
