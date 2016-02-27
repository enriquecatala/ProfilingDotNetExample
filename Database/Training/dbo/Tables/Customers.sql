CREATE TABLE [dbo].[Customers] (
    [ID_Customer] INT           IDENTITY (1, 1) NOT NULL,
    [Name]        VARCHAR (255) COLLATE Latin1_General_CI_AS NOT NULL,
    [Col1]        VARCHAR (255) COLLATE Latin1_General_CI_AS CONSTRAINT [DF__Customers__Col1__7F60ED59] DEFAULT (newid()) NOT NULL,
    [Col2]        VARCHAR (250) COLLATE Latin1_General_CI_AS CONSTRAINT [DF__Customers__Col2__00551192] DEFAULT (newid()) NOT NULL,
    [ID_City]     INT           NULL,
    CONSTRAINT [xpk_Customers] PRIMARY KEY CLUSTERED ([ID_Customer] ASC)
);

