CREATE TABLE [dbo].[Orders_LazySpool] (
    [ID]         INT             IDENTITY (1, 1) NOT NULL,
    [Customer]   INT             NOT NULL,
    [Vendedor]   VARCHAR (30)    NOT NULL,
    [Quantidade] SMALLINT        NOT NULL,
    [Value]      NUMERIC (18, 2) NOT NULL,
    [Data]       DATETIME        NOT NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC)
);

