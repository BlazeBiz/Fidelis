CREATE PROCEDURE [viking].[_MergeConfigData]
	@environment varchar(12) = ''
AS
--If @environment = ''
--BEGIN
--	Print 'Please specify @environment parameter: Dev | UAT | Prod | Training'
--	return -1
--END
--Print 'Merging data for environment: ' + @environment
Print '================='
Print 'Merging Config data'
Print '================='

	-- Clear the results table
		--Delete from _DataMergeResult
		Declare @returnValue int

	---- ------------------------------------------------------------------------------------------------------------
	---- Utility Tables
	---- ------------------------------------------------------------------------------------------------------------
	----	
	---- PowersOfTwo
	--Exec @returnValue = BuildPowersOfTwo
	--if @returnValue <> 0 return @returnValue

	---- ------------------------------------------------------------------------------------------------------------
	---- Supporting Tables
	---- ------------------------------------------------------------------------------------------------------------
	---- 
	---- ------------------------------------------------------------------------------------------------------------
	---- SubscriptionPlans
	---- ------------------------------------------------------------------------------------------------------------
	--Exec @returnValue = MergeSubscriptionPlan
	--if @returnValue <> 0 return @returnValue

	----	
	---- HealthCategory
	--Exec @returnValue = MergeHealthCategory
	--if @returnValue <> 0 return @returnValue

	---- ------------------------------------------------------------------------------------------------------------
	---- Permissions
	---- ------------------------------------------------------------------------------------------------------------
	----	
	---- Permission
	--Exec @returnValue = MergePermission
	--if @returnValue <> 0 return @returnValue
	----	
	---- Role
	--Exec @returnValue = MergeRole
	--if @returnValue <> 0 return @returnValue
	----	
	---- Theme
	--Exec @returnValue = MergeTheme
	--if @returnValue <> 0 return @returnValue
	----	
	---- ------------------------------------------------------------------------------------------------------------
	---- Templates
	---- ------------------------------------------------------------------------------------------------------------
	--Exec @returnValue = MergeTemplates
	--if @returnValue <> 0 return @returnValue


	------ Show the results
	----Select * from _DataMergeResult

RETURN 0
