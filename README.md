# DemoEfWebApi

#DB script
-- Products table
CREATE TABLE [dbo].[Products] (
    [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [Name] NVARCHAR(200) NOT NULL,
    [Price] DECIMAL(18,2) NOT NULL,
    [Description] NVARCHAR(MAX) NULL
);

-- Users table (for simple username/password)
CREATE TABLE [dbo].[Users] (
    [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [Username] NVARCHAR(100) NOT NULL UNIQUE,
    [PasswordHash] NVARCHAR(200) NOT NULL -- store hashed password in real app
);

-- Tokens table
CREATE TABLE [dbo].[Tokens] (
    [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [UserId] INT NOT NULL REFERENCES dbo.Users(Id),
    [TokenValue] NVARCHAR(200) NOT NULL UNIQUE,
    [ExpiresAt] DATETIME NOT NULL
);

-- sample data
INSERT INTO dbo.Users ([Username],[PasswordHash]) VALUES ('admin','password'); -- for demo only; treat as plain in demo
INSERT INTO dbo.Products ([Name],[Price],[Description]) VALUES
('Sample Product 1', 99.99, 'Demo product 1'),
('Sample Product 2', 49.50, 'Demo product 2');
