CREATE PROCEDURE [dbo].[PerformanceAccesoDatos] @idProductMin int, @idProductMax int
AS
BEGIN 
	select [Product_Name] from dbo.ProductsBig where ID_Product BETWEEN @idProductMin AND @idProductMax
END 
