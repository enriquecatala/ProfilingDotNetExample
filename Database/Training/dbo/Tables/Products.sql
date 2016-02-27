CREATE TABLE [dbo].[Products] (
    [ID_Product]   INT           IDENTITY (1, 1) NOT NULL,
    [Product_Name] VARCHAR (250) COLLATE Latin1_General_CI_AS NULL,
    [Col1]         VARCHAR (250) COLLATE Latin1_General_CI_AS CONSTRAINT [DF__Products__Col1__03317E3D] DEFAULT (newid()) NOT NULL,
    CONSTRAINT [xpk_Products] PRIMARY KEY CLUSTERED ([ID_Product] ASC)
);

