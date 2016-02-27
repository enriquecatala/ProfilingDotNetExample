CREATE TABLE [dbo].[Cities] (
    [ID]    INT          IDENTITY (1, 1) NOT NULL,
    [Name]  VARCHAR (40) COLLATE Latin1_General_CI_AS NOT NULL,
    [State] CHAR (2)     COLLATE Latin1_General_CI_AS NOT NULL,
    CONSTRAINT [XPKCidade] PRIMARY KEY CLUSTERED ([ID] ASC)
);

