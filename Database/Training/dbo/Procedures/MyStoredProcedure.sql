 CREATE PROCEDURE dbo.MyStoredProcedure @producto varchar(255)
 AS
        BEGIN
              SELECT    Col1
              FROM      dbo.ProductsBig
              WHERE     Product_Name = @producto
        END