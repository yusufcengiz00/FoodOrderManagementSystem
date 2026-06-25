/*
    FoodOrderDB - Tam kurulum scripti
    Veritabani: FoodOrderDB
    Sunucu   : (localdb)\MSSQLLocalDB  (Context.cs ile uyumlu)

    Model eslesmeleri:
      Users       -> User.cs        (UserId, FullName, Email, Password)
      Products    -> Product.cs     (ProductId, ProductName, Price)
      Orders      -> Order.cs        (OrderId, UserId, OrderDate, TotalAmount)
      OrderDetails-> OrderDetail.cs  (OrderDetailId, OrderId, ProductId, Quantity, Price, ProductName*)
      * ProductName JOIN ile dondurulur; tabloda kolon yoktur.
*/

USE master;
GO

IF DB_ID(N'FoodOrderDB') IS NOT NULL
BEGIN
    ALTER DATABASE FoodOrderDB SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE FoodOrderDB;
END
GO

CREATE DATABASE FoodOrderDB;
GO

USE FoodOrderDB;
GO

/* ============================================================
   TABLOLAR
   ============================================================ */

CREATE TABLE dbo.Users
(
    UserId   INT            IDENTITY(1,1) NOT NULL,
    FullName NVARCHAR(100)  NOT NULL,
    Email    NVARCHAR(150)  NOT NULL,
    Password NVARCHAR(100)  NOT NULL,
    CONSTRAINT PK_Users PRIMARY KEY (UserId),
    CONSTRAINT UQ_Users_Email UNIQUE (Email)
);
GO

CREATE TABLE dbo.Products
(
    ProductId   INT            IDENTITY(1,1) NOT NULL,
    ProductName NVARCHAR(150)  NOT NULL,
    Price       DECIMAL(18,2)  NOT NULL,
    CONSTRAINT PK_Products PRIMARY KEY (ProductId),
    CONSTRAINT CK_Products_Price CHECK (Price >= 0)
);
GO

CREATE TABLE dbo.Orders
(
    OrderId     INT            IDENTITY(1,1) NOT NULL,
    UserId      INT            NOT NULL,
    OrderDate   DATETIME2(0)   NOT NULL CONSTRAINT DF_Orders_OrderDate DEFAULT (SYSDATETIME()),
    TotalAmount DECIMAL(18,2)  NOT NULL CONSTRAINT DF_Orders_TotalAmount DEFAULT (0),
    CONSTRAINT PK_Orders PRIMARY KEY (OrderId),
    CONSTRAINT FK_Orders_Users FOREIGN KEY (UserId) REFERENCES dbo.Users (UserId)
);
GO

CREATE TABLE dbo.OrderDetails
(
    OrderDetailId INT           IDENTITY(1,1) NOT NULL,
    OrderId       INT           NOT NULL,
    ProductId     INT           NOT NULL,
    Quantity      INT           NOT NULL,
    Price         DECIMAL(18,2) NOT NULL,
    CONSTRAINT PK_OrderDetails PRIMARY KEY (OrderDetailId),
    CONSTRAINT FK_OrderDetails_Orders FOREIGN KEY (OrderId) REFERENCES dbo.Orders (OrderId) ON DELETE CASCADE,
    CONSTRAINT FK_OrderDetails_Products FOREIGN KEY (ProductId) REFERENCES dbo.Products (ProductId),
    CONSTRAINT CK_OrderDetails_Quantity CHECK (Quantity > 0),
    CONSTRAINT CK_OrderDetails_Price CHECK (Price >= 0),
    CONSTRAINT UQ_OrderDetails_Order_Product UNIQUE (OrderId, ProductId)
);
GO

CREATE INDEX IX_Orders_UserId ON dbo.Orders (UserId);
CREATE INDEX IX_OrderDetails_OrderId ON dbo.OrderDetails (OrderId);
CREATE INDEX IX_OrderDetails_ProductId ON dbo.OrderDetails (ProductId);
GO

/* ============================================================
   YARDIMCI: Siparis toplamini guncelle
   ============================================================ */

CREATE OR ALTER PROCEDURE dbo.sp_OrderRecalculateTotal
    @OrderId INT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE dbo.Orders
    SET TotalAmount = ISNULL((
        SELECT SUM(CAST(d.Price AS DECIMAL(18,2)) * d.Quantity)
        FROM dbo.OrderDetails d
        WHERE d.OrderId = @OrderId
    ), 0)
    WHERE OrderId = @OrderId;
END
GO

/* ============================================================
   USER PROCEDURE'LERI
   ============================================================ */

CREATE OR ALTER PROCEDURE dbo.sp_UserGetAll
AS
BEGIN
    SET NOCOUNT ON;

    SELECT UserId, FullName, Email, Password
    FROM dbo.Users
    ORDER BY FullName;
END
GO

CREATE OR ALTER PROCEDURE dbo.sp_UserGetById
    @UserId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT UserId, FullName, Email, Password
    FROM dbo.Users
    WHERE UserId = @UserId;
END
GO

CREATE OR ALTER PROCEDURE dbo.sp_UserKaydet
    @UserId   INT,
    @FullName NVARCHAR(100),
    @Email    NVARCHAR(150),
    @Password NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    IF @UserId = 0
    BEGIN
        INSERT INTO dbo.Users (FullName, Email, Password)
        VALUES (@FullName, @Email, @Password);
    END
    ELSE
    BEGIN
        UPDATE dbo.Users
        SET FullName = @FullName,
            Email    = @Email,
            Password = CASE
                           WHEN @Password IS NULL OR LTRIM(RTRIM(@Password)) = N'' THEN Password
                           ELSE @Password
                       END
        WHERE UserId = @UserId;
    END
END
GO

CREATE OR ALTER PROCEDURE dbo.sp_UserDelete
    @UserId INT
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (SELECT 1 FROM dbo.Orders WHERE UserId = @UserId)
    BEGIN
        RAISERROR(N'Bu kullaniciya ait siparisler var. Once siparisleri silin.', 16, 1);
        RETURN;
    END

    DELETE FROM dbo.Users WHERE UserId = @UserId;
END
GO

/* ============================================================
   PRODUCT PROCEDURE'LERI
   ============================================================ */

CREATE OR ALTER PROCEDURE dbo.sp_ProductGetAll
AS
BEGIN
    SET NOCOUNT ON;

    SELECT ProductId, ProductName, Price
    FROM dbo.Products
    ORDER BY ProductName;
END
GO

CREATE OR ALTER PROCEDURE dbo.sp_ProductGetById
    @ProductId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT ProductId, ProductName, Price
    FROM dbo.Products
    WHERE ProductId = @ProductId;
END
GO

CREATE OR ALTER PROCEDURE dbo.sp_ProductKaydet
    @ProductId   INT,
    @ProductName NVARCHAR(150),
    @Price       DECIMAL(18,2)
AS
BEGIN
    SET NOCOUNT ON;

    IF @ProductId = 0
    BEGIN
        INSERT INTO dbo.Products (ProductName, Price)
        VALUES (@ProductName, @Price);
    END
    ELSE
    BEGIN
        UPDATE dbo.Products
        SET ProductName = @ProductName,
            Price       = @Price
        WHERE ProductId = @ProductId;
    END
END
GO

CREATE OR ALTER PROCEDURE dbo.sp_ProductDelete
    @ProductId INT
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (SELECT 1 FROM dbo.OrderDetails WHERE ProductId = @ProductId)
    BEGIN
        RAISERROR(N'Bu urun siparislerde kullaniliyor. Silinemez.', 16, 1);
        RETURN;
    END

    DELETE FROM dbo.Products WHERE ProductId = @ProductId;
END
GO

/* ============================================================
   ORDER PROCEDURE'LERI
   ============================================================ */

CREATE OR ALTER PROCEDURE dbo.sp_OrderGetAll
AS
BEGIN
    SET NOCOUNT ON;

    SELECT OrderId, UserId, OrderDate, TotalAmount
    FROM dbo.Orders
    ORDER BY OrderDate DESC, OrderId DESC;
END
GO

CREATE OR ALTER PROCEDURE dbo.sp_OrderKaydet
    @OrderId INT = 0,
    @UserId  INT
AS
BEGIN
    SET NOCOUNT ON;

    IF @OrderId = 0
    BEGIN
        INSERT INTO dbo.Orders (UserId, OrderDate, TotalAmount)
        VALUES (@UserId, SYSDATETIME(), 0);

        SELECT CAST(SCOPE_IDENTITY() AS INT);
    END
    ELSE
    BEGIN
        UPDATE dbo.Orders
        SET UserId = @UserId
        WHERE OrderId = @OrderId;
    END
END
GO

CREATE OR ALTER PROCEDURE dbo.sp_OrderDelete
    @OrderId INT
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM dbo.Orders WHERE OrderId = @OrderId;
    -- OrderDetails: ON DELETE CASCADE ile otomatik silinir
END
GO

/* ============================================================
   ORDER DETAIL PROCEDURE'LERI
   ============================================================ */

CREATE OR ALTER PROCEDURE dbo.sp_OrderDetailGetByOrderId
    @OrderId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        d.OrderDetailId,
        d.OrderId,
        d.ProductId,
        d.Quantity,
        d.Price,
        p.ProductName
    FROM dbo.OrderDetails d
    INNER JOIN dbo.Products p ON p.ProductId = d.ProductId
    WHERE d.OrderId = @OrderId
    ORDER BY d.OrderDetailId;
END
GO

CREATE OR ALTER PROCEDURE dbo.sp_OrderDetailKaydet
    @OrderId   INT,
    @ProductId INT,
    @Quantity  INT,
    @Price     DECIMAL(18,2)
AS
BEGIN
    SET NOCOUNT ON;

    IF @Quantity <= 0
    BEGIN
        RAISERROR(N'Adet sifirdan buyuk olmalidir.', 16, 1);
        RETURN;
    END

    IF EXISTS (SELECT 1 FROM dbo.OrderDetails WHERE OrderId = @OrderId AND ProductId = @ProductId)
    BEGIN
        UPDATE dbo.OrderDetails
        SET Quantity = Quantity + @Quantity,
            Price    = @Price
        WHERE OrderId = @OrderId AND ProductId = @ProductId;
    END
    ELSE
    BEGIN
        INSERT INTO dbo.OrderDetails (OrderId, ProductId, Quantity, Price)
        VALUES (@OrderId, @ProductId, @Quantity, @Price);
    END

    EXEC dbo.sp_OrderRecalculateTotal @OrderId;
END
GO

CREATE OR ALTER PROCEDURE dbo.sp_OrderDetailDelete
    @OrderDetailId INT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @OrderId INT;

    SELECT @OrderId = OrderId
    FROM dbo.OrderDetails
    WHERE OrderDetailId = @OrderDetailId;

    DELETE FROM dbo.OrderDetails WHERE OrderDetailId = @OrderDetailId;

    IF @OrderId IS NOT NULL
        EXEC dbo.sp_OrderRecalculateTotal @OrderId;
END
GO

/* ============================================================
   ORNEK VERILER (istege bagli test)
   ============================================================ */

INSERT INTO dbo.Users (FullName, Email, Password) VALUES
(N'Ahmet Yilmaz', N'ahmet@mail.com', N'123456'),
(N'Ayse Demir',   N'ayse@mail.com',  N'123456'),
(N'Mehmet Kaya',  N'mehmet@mail.com', N'123456');

INSERT INTO dbo.Products (ProductName, Price) VALUES
(N'Margherita Pizza', 180.00),
(N'Karışik Pizza',    220.00),
(N'Hamburger',        150.00),
(N'Cola',              35.00),
(N'Tiramisu',          90.00);

INSERT INTO dbo.Orders (UserId, OrderDate, TotalAmount) VALUES
(1, SYSDATETIME(), 0);

DECLARE @SampleOrderId INT = SCOPE_IDENTITY();

INSERT INTO dbo.OrderDetails (OrderId, ProductId, Quantity, Price) VALUES
(@SampleOrderId, 1, 2, 180.00),
(@SampleOrderId, 4, 2,  35.00);

EXEC dbo.sp_OrderRecalculateTotal @SampleOrderId;
GO

PRINT N'FoodOrderDB basariyla olusturuldu.';
