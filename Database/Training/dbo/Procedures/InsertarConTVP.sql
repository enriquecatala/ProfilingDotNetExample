
CREATE PROCEDURE dbo.InsertarConTVP( @tbl dbo.InsercionesBatchType READONLY)
AS
BEGIN
INSERT INTO dbo.InsercionesBatch
        ( Id, texto )
SELECT id,texto FROM @tbl

END