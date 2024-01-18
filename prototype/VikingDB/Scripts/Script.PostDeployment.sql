/*
Post-Deployment Script Template                            
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.        
 Use SQLCMD syntax to include a file in the post-deployment script.            
 Example:      :r .\myfile.sql                                
 Use SQLCMD syntax to reference a variable in the post-deployment script.        
 Example:      :setvar TableName MyTable                            
               SELECT * FROM [$(TableName)]                    
--------------------------------------------------------------------------------------
--Select 'Data Path:' + [$(DefaultDataPath)]
--Select 'File Prefix:' + [$(DefaultFilePrefix)]
--Select 'Database Name:' + [$(DatabaseName)]
--Select 'Log Path:' + [$(DefaultLogPath)]
*/

PRINT 'Server: ' + @@servername
PRINT 'Version: ' + @@version
Print 'Environment: $(Environment)' 

--Declare @edition varchar(100) = Cast( SERVERPROPERTY('Edition') As varchar(100))
--Declare @isTest bit = Case WHEN CHARINDEX('Standard Edition', @edition) > 0 Or CHARINDEX('Express Edition', @edition) > 0 THEN 1 ELSE 0 END;

Exec viking._MergeConfigData

--if @isTest = 1
if '$(Environment)' = 'Test'
BEGIN
    Exec viking._MergeTestData
END
if '$(Environment)' = 'Production'
BEGIN
    Exec viking._MergeProductionData
END
