CREATE TABLE [dbo].[Users] (
    [Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    [UserName] NVARCHAR(50) NOT NULL,
    [PasswordHash] NVARCHAR(MAX) NOT NULL,
    [Email] NVARCHAR(100) NOT NULL,
    [LastLoginTime] DATETIME2 NULL
);

-- Unique Constraints
CREATE UNIQUE INDEX IX_Users_UserName ON [dbo].[Users]([UserName]);
CREATE UNIQUE INDEX IX_Users_Email ON [dbo].[Users]([Email]);


CREATE TABLE [dbo].[Products] (
    [Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    [Category] NVARCHAR(50) NOT NULL,
    [ProductCode] NVARCHAR(10) NOT NULL,
    [Name] NVARCHAR(100) NOT NULL,
    [ImagePath] NVARCHAR(MAX) NOT NULL,
    [Price] DECIMAL(18,2) NOT NULL,
    [MinimumQuantity] INT NOT NULL,
    [DiscountRate] FLOAT NULL
);

-- Unique Constraint
CREATE UNIQUE INDEX IX_Products_ProductCode ON [dbo].[Products]([ProductCode]);


CREATE TABLE [dbo].[RefreshTokens] (
    [Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    [Token] NVARCHAR(MAX) NOT NULL,
    [Expires] DATETIME2 NOT NULL,
    [Created] DATETIME2 NOT NULL,
    [Revoked] DATETIME2 NULL,
    [UserId] UNIQUEIDENTIFIER NOT NULL
);

-- Foreign Key to Users
ALTER TABLE [dbo].[RefreshTokens]
ADD CONSTRAINT FK_RefreshTokens_Users
FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users]([Id])
ON DELETE CASCADE;
