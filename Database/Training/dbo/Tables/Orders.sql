CREATE TABLE [dbo].[Orders] (
    [ID_Order]    INT             IDENTITY (1, 1) NOT NULL,
    [ID_Customer] INT             NOT NULL,
    [Order_Date]  DATETIME        NOT NULL,
    [Value]       NUMERIC (18, 2) NOT NULL,
    CONSTRAINT [xpk_Orders] PRIMARY KEY CLUSTERED ([ID_Order] ASC),
    CONSTRAINT [fk_Orders_Customers] FOREIGN KEY ([ID_Customer]) REFERENCES [dbo].[Customers] ([ID_Customer])
);

