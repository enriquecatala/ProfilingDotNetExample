CREATE PROCEDURE [dbo].[GetCustomersPaged]
	@pagenumber int = 0,
	@pagesize int
AS
begin
	SELECT [ID_Customer], 
			[Name], 
			[Col1], 
			[Col2]
    FROM [dbo].[CustomersBig] 
    ORDER BY [ID_Customer] ASC
    OFFSET (@pagenumber*@pagesize) ROWS FETCH NEXT @pagesize ROWS ONLY 
end 