/*
 Pre-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be executed before the build script.	
 Use SQLCMD syntax to include a file in the pre-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the pre-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/
-- Solo si no es Azure
IF(EXISTS ( SELECT 1 FROM (SELECT SERVERPROPERTY('Edition') AS v) x WHERE v <> 'SQL Azure'))
begin
EXEC sp_configure 'clr enabled',1;
RECONFIGURE WITH OVERRIDE;
end


IF EXISTS ( SELECT  *
            FROM    sys.assemblies
            WHERE   name = 'CustomFunctions' )
   BEGIN
         DROP FUNCTION dbo.BuscarTexto
        -- DROP FUNCTION dbo.BuscarTextoCI
         DROP ASSEMBLY CustomFunctions
   END