-- =============================================================
-- Product Management System - Database Script
-- SQL Server
-- Run this entire script in SQL Server Management Studio (SSMS)
-- or Azure Data Studio to create the database and stored procedures.
-- =============================================================

-- 1. Create database (only if it doesn't already exist)
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'ProductManagement')
BEGIN
    CREATE DATABASE ProductManagement;
END
GO

USE ProductManagement;
GO

-- 2. Create Products table
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Products' AND xtype='U')
BEGIN
    CREATE TABLE Products (
        Id           INT IDENTITY(1,1) PRIMARY KEY,
        Name         NVARCHAR(100)  NOT NULL,
        Description  NVARCHAR(500)  NULL,
        Price        DECIMAL(18, 2) NOT NULL,
        CreatedDate  DATETIME       NOT NULL DEFAULT GETDATE()
    );
END
GO

-- =============================================================
-- STORED PROCEDURES
-- =============================================================

-- Drop if they exist, so script can be re-run safely
IF OBJECT_ID('sp_Product_Create', 'P')    IS NOT NULL DROP PROCEDURE sp_Product_Create;
IF OBJECT_ID('sp_Product_GetAll', 'P')    IS NOT NULL DROP PROCEDURE sp_Product_GetAll;
IF OBJECT_ID('sp_Product_GetById', 'P')   IS NOT NULL DROP PROCEDURE sp_Product_GetById;
IF OBJECT_ID('sp_Product_Update', 'P')    IS NOT NULL DROP PROCEDURE sp_Product_Update;
IF OBJECT_ID('sp_Product_Delete', 'P')    IS NOT NULL DROP PROCEDURE sp_Product_Delete;
GO

-- 1) CREATE a new product
CREATE PROCEDURE sp_Product_Create
    @Name        NVARCHAR(100),
    @Description NVARCHAR(500),
    @Price       DECIMAL(18, 2)
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Products (Name, Description, Price, CreatedDate)
    VALUES (@Name, @Description, @Price, GETDATE());

    -- Return the newly created product (with its generated Id)
    SELECT Id, Name, Description, Price, CreatedDate
    FROM Products
    WHERE Id = SCOPE_IDENTITY();
END
GO

-- 2) GET ALL products
CREATE PROCEDURE sp_Product_GetAll
AS
BEGIN
    SET NOCOUNT ON;

    SELECT Id, Name, Description, Price, CreatedDate
    FROM Products
    ORDER BY CreatedDate DESC;
END
GO

-- 3) GET product by Id
CREATE PROCEDURE sp_Product_GetById
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT Id, Name, Description, Price, CreatedDate
    FROM Products
    WHERE Id = @Id;
END
GO

-- 4) UPDATE an existing product
CREATE PROCEDURE sp_Product_Update
    @Id          INT,
    @Name        NVARCHAR(100),
    @Description NVARCHAR(500),
    @Price       DECIMAL(18, 2)
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Products
    SET Name = @Name,
        Description = @Description,
        Price = @Price
    WHERE Id = @Id;

    -- Return number of rows affected (1 = success, 0 = not found)
    SELECT @@ROWCOUNT AS RowsAffected;
END
GO

-- 5) DELETE a product
CREATE PROCEDURE sp_Product_Delete
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM Products WHERE Id = @Id;

    SELECT @@ROWCOUNT AS RowsAffected;
END
GO

-- =============================================================
-- OPTIONAL: Seed some sample data
-- =============================================================
IF NOT EXISTS (SELECT 1 FROM Products)
BEGIN
    EXEC sp_Product_Create @Name='Laptop Dell XPS 13', @Description='13-inch ultrabook, 16GB RAM', @Price=1299.99;
    EXEC sp_Product_Create @Name='Mechanical Keyboard', @Description='RGB backlit, blue switches',  @Price=89.50;
    EXEC sp_Product_Create @Name='Wireless Mouse',      @Description='Ergonomic, rechargeable',     @Price=29.99;
END
GO

PRINT 'Database, table, and stored procedures created successfully!';