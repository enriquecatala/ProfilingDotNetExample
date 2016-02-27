CREATE TABLE [dbo].[Items] (
    [ID_Order]      INT      NOT NULL,
    [ID_Product]    INT      NOT NULL,
    [Delivery_Date] DATETIME NOT NULL,
    [Quantity]      INT      NULL,
    CONSTRAINT [xpk_Items] PRIMARY KEY CLUSTERED ([ID_Order] ASC, [ID_Product] ASC),
    CONSTRAINT [fk_Items_Orders] FOREIGN KEY ([ID_Order]) REFERENCES [dbo].[Orders] ([ID_Order]),
    CONSTRAINT [fk_Items_Products] FOREIGN KEY ([ID_Product]) REFERENCES [dbo].[Products] ([ID_Product])
);

