CREATE PROCEDURE [dbo].[_MergeTestData]
AS
Print '================='
Print 'Merging TEST data'
Print '================='

	-- Clear the results table
		--Delete from _DataMergeResult
		Declare @returnValue int

	-- ------------------------------------------------------------------------------------------------------------
	-- MergeCustomer
	-- ------------------------------------------------------------------------------------------------------------
	--	
	-- MergeCustomer
	Exec @returnValue = MergeCustomer_Test
	if @returnValue <> 0 return @returnValue

	-- ------------------------------------------------------------------------------------------------------------
	-- MergeSalesOrder
	-- ------------------------------------------------------------------------------------------------------------
	--	
	-- MergeSalesOrder
	Exec @returnValue = MergeSalesOrder_Test
	if @returnValue <> 0 return @returnValue

	---- Show the results
	--Select * from _DataMergeResult

RETURN 0
