
CREATE PROCEDURE dbo.InsertarFilaAFila( @id BIGINT,@texto VARCHAR(512))
AS
BEGIN
INSERT INTO dbo.InsercionesBatch
        ( Id, texto )
VALUES(@id,@texto)

END