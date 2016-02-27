CREATE PROCEDURE [dbo].[ContainsCustomersByIds]
	@ids AS dbo.IDsTableType READONLY
AS
BEGIN
	SELECT COUNT(*)
	FROM @ids i
	INNER JOIN dbo.CustomersBig cb ON cb.ID_Customer = i.Id
END 