

/*
 ECB: Tarda unos 2 minutos
*/

IF OBJECT_ID('OrdersBig') IS NULL
BEGIN
	SELECT  IDENTITY(Int, 1,1) AS ID_Order, A.ID_Customer, GETDATE() - (CheckSUM(NEWID()) / 1000000) AS Order_Date, A.Value
	INTO OrdersBig
	FROM Orders A
		
	UPDATE OrdersBig SET Order_Date = CONVERT(Date, Order_Date),
						 Value = ABS(CONVERT(Numeric(18,2), (CheckSUM(NEWID()) / 1000000.5)));

	ALTER TABLE OrdersBig ADD CONSTRAINT xpk_OrdersBig PRIMARY KEY(ID_Order);
END 


IF OBJECT_ID('CustomersBig') IS NULL
BEGIN 
	SELECT IDENTITY(Int, 1,1) AS ID_Customer, A.Name + ' ' + SubString(CONVERT(VarChar(250),NEWID()),1,8) AS Name, A.Col1, A.Col2
	  INTO CustomersBig
	  FROM Customers A
	

	ALTER TABLE CustomersBig ADD CONSTRAINT xpk_CustomersBig PRIMARY KEY(ID_Customer);
END 


IF OBJECT_ID('ProductsBig') IS NULL
BEGIN
 
	SELECT IDENTITY(Int, 1,1) AS ID_Product, A.Product_Name + ' ' + SubString(CONVERT(VarChar(250),NEWID()),1,8) AS Product_Name, a.Col1
	  INTO ProductsBig
	  FROM Products A
	

	ALTER TABLE ProductsBig ADD CONSTRAINT xpk_ProductsBig PRIMARY KEY(ID_Product);
END 


IF OBJECT_ID('ItemsBig') IS NULL
BEGIN
 
	SELECT OrdersBig.ID_Order,
		   ISNULL(CONVERT(Integer, CONVERT(Integer, ABS(Checksum(NEWID())) / 1000000)),0) AS ID_Product,
		   GetDate() -  ABS(Checksum(NEWID())) / 1000000 AS Delivery_Date,
		   CONVERT(Integer, ABS(Checksum(NEWID())) / 1000000) AS Quantity
	  INTO ItemsBig
	  FROM OrdersBig;

	ALTER TABLE ItemsBig ADD CONSTRAINT [xpk_ItemsBig] PRIMARY KEY([ID_Order], [ID_Product]);

	ALTER TABLE [dbo].[OrdersBig]  WITH CHECK ADD  CONSTRAINT [fk_OrdersBig_CustomersBig] FOREIGN KEY([ID_Customer])
	REFERENCES [dbo].[CustomersBig] ([ID_Customer]);
END 
