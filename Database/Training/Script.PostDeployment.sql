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
*/
/*
 WORKAROUND PARA DESPLEGAR EL ESQUEMA CUSTOM QUE QUERAMOS
*/

if object_id('aggregates.CONCAT_AGG') is NOT NULL BEGIN
	drop aggregate Aggregates.CONCAT_AGG
END

if object_id('aggregates.CONCAT_AGG_Optimized') is not NULL BEGIN
	DROP AGGREGATE aggregates.CONCAT_AGG_Optimized
END

if object_id('dbo.CONCAT_AGG') is not NULL begin
    alter schema Aggregates transfer dbo.CONCAT_AGG
END

if object_id('dbo.CONCAT_AGG_Optimized') is not NULL begin
    alter schema Aggregates transfer dbo.CONCAT_AGG_Optimized
END


PRINT '******************************* Creating objects for DeployType = $(DeployType) ************************************'
--:on error ignore
USE [$(DatabaseName)]
GO

IF OBJECT_ID('ItemsBig') IS NOT NULL
BEGIN 
	DROP TABLE DBO.ItemsBig
END 

IF OBJECT_ID('OrdersBig') IS NOT NULL
BEGIN
	DROP TABLE dbo.OrdersBig
END 


IF OBJECT_ID('CustomersBig') IS NOT NULL
BEGIN 
	DROP TABLE dbo.CustomersBig
END 


IF OBJECT_ID('ProductsBig') IS NOT NULL
BEGIN
	DROP TABLE dbo.ProductsBig
END 





IF ( '$(DeployType)' = 'Debug')
BEGIN
 :r .\POST-DEPLOYMENT\CreateBigTables-debug-environment.sql
END
ELSE
BEGIN
 :r .\POST-DEPLOYMENT\CreateBigTables-release-environment.sql
END
