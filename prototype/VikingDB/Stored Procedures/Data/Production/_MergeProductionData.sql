CREATE PROCEDURE [viking].[_MergeProductionData]
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
	Exec @returnValue = viking.MergeCustomer_Production
	if @returnValue <> 0 return @returnValue

	-- ------------------------------------------------------------------------------------------------------------
	-- MergeSalesOrder
	-- ------------------------------------------------------------------------------------------------------------
	--	
	-- MergeSalesOrder
	Exec @returnValue = viking.MergeSalesOrder_Production
	if @returnValue <> 0 return @returnValue

	---- Show the results
	--Select * from _DataMergeResult

RETURN 0
