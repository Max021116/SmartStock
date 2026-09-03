IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [Categories] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(200) NOT NULL,
    [Description] nvarchar(1000) NULL,
    [ParentCategoryId] int NULL,
    CONSTRAINT [PK_Categories] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Categories_Categories_ParentCategoryId] FOREIGN KEY ([ParentCategoryId]) REFERENCES [Categories] ([Id]) ON DELETE NO ACTION
);
GO

CREATE INDEX [IX_Categories_ParentCategoryId] ON [Categories] ([ParentCategoryId]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260818064113_InitialCreate', N'8.0.11');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [Products] (
    [Id] int NOT NULL IDENTITY,
    [SKU] nvarchar(100) NOT NULL,
    [Name] nvarchar(200) NOT NULL,
    [Description] nvarchar(1000) NULL,
    [Price] decimal(18,2) NOT NULL,
    [CategoryId] int NOT NULL,
    [IsActive] bit NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    CONSTRAINT [PK_Products] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Products_Categories_CategoryId] FOREIGN KEY ([CategoryId]) REFERENCES [Categories] ([Id]) ON DELETE NO ACTION
);
GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Description', N'Name', N'ParentCategoryId') AND [object_id] = OBJECT_ID(N'[Categories]'))
    SET IDENTITY_INSERT [Categories] ON;
INSERT INTO [Categories] ([Id], [Description], [Name], [ParentCategoryId])
VALUES (1, N'...', N'Electronics', NULL),
(2, N'...', N'Laptops', 1),
(3, N'...', N'Desktops', 1),
(4, N'...', N'Smartphones', 1),
(5, N'...', N'Tablets', 1),
(6, N'...', N'Wearables', 1),
(7, N'...', N'Audio', 1),
(8, N'...', N'Video', 1),
(9, N'...', N'Gaming', 1),
(10, N'...', N'Office', 1);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Description', N'Name', N'ParentCategoryId') AND [object_id] = OBJECT_ID(N'[Categories]'))
    SET IDENTITY_INSERT [Categories] OFF;
GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'CategoryId', N'CreatedAt', N'Description', N'IsActive', N'Name', N'Price', N'SKU') AND [object_id] = OBJECT_ID(N'[Products]'))
    SET IDENTITY_INSERT [Products] ON;
INSERT INTO [Products] ([Id], [CategoryId], [CreatedAt], [Description], [IsActive], [Name], [Price], [SKU])
VALUES (1, 2, '2026-01-01T00:00:00.0000000', N'...', CAST(1 AS bit), N'Product 1', 999.99, N'SKU-001'),
(2, 2, '2026-01-01T00:00:00.0000000', N'...', CAST(1 AS bit), N'Product 2', 899.99, N'SKU-002'),
(3, 2, '2026-01-01T00:00:00.0000000', N'...', CAST(1 AS bit), N'Product 3', 799.99, N'SKU-003'),
(4, 2, '2026-01-01T00:00:00.0000000', N'...', CAST(1 AS bit), N'Product 4', 699.99, N'SKU-004'),
(5, 2, '2026-01-01T00:00:00.0000000', N'...', CAST(1 AS bit), N'Product 5', 599.99, N'SKU-005'),
(6, 2, '2026-01-01T00:00:00.0000000', N'...', CAST(1 AS bit), N'Product 6', 499.99, N'SKU-006'),
(7, 2, '2026-01-01T00:00:00.0000000', N'...', CAST(1 AS bit), N'Product 7', 399.99, N'SKU-007'),
(8, 2, '2026-01-01T00:00:00.0000000', N'...', CAST(1 AS bit), N'Product 8', 299.99, N'SKU-008'),
(9, 2, '2026-01-01T00:00:00.0000000', N'...', CAST(1 AS bit), N'Product 9', 199.99, N'SKU-009'),
(10, 2, '2026-01-01T00:00:00.0000000', N'...', CAST(1 AS bit), N'Product 10', 99.99, N'SKU-010');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'CategoryId', N'CreatedAt', N'Description', N'IsActive', N'Name', N'Price', N'SKU') AND [object_id] = OBJECT_ID(N'[Products]'))
    SET IDENTITY_INSERT [Products] OFF;
GO

CREATE INDEX [IX_Products_CategoryId] ON [Products] ([CategoryId]);
GO

CREATE UNIQUE INDEX [IX_Products_SKU] ON [Products] ([SKU]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260826063144_AddProducts', N'8.0.11');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [AspNetRoles] (
    [Id] nvarchar(450) NOT NULL,
    [Name] nvarchar(256) NULL,
    [NormalizedName] nvarchar(256) NULL,
    [ConcurrencyStamp] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetRoles] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [AspNetUsers] (
    [Id] nvarchar(450) NOT NULL,
    [FullName] nvarchar(max) NULL,
    [UserName] nvarchar(256) NULL,
    [NormalizedUserName] nvarchar(256) NULL,
    [Email] nvarchar(256) NULL,
    [NormalizedEmail] nvarchar(256) NULL,
    [EmailConfirmed] bit NOT NULL,
    [PasswordHash] nvarchar(max) NULL,
    [SecurityStamp] nvarchar(max) NULL,
    [ConcurrencyStamp] nvarchar(max) NULL,
    [PhoneNumber] nvarchar(max) NULL,
    [PhoneNumberConfirmed] bit NOT NULL,
    [TwoFactorEnabled] bit NOT NULL,
    [LockoutEnd] datetimeoffset NULL,
    [LockoutEnabled] bit NOT NULL,
    [AccessFailedCount] int NOT NULL,
    CONSTRAINT [PK_AspNetUsers] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [Customers] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(200) NOT NULL,
    [Email] nvarchar(100) NULL,
    [Phone] nvarchar(20) NULL,
    [Address] nvarchar(200) NULL,
    CONSTRAINT [PK_Customers] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [Suppliers] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(200) NOT NULL,
    [ContactEmail] nvarchar(100) NULL,
    [Phone] nvarchar(20) NULL,
    [Address] nvarchar(200) NULL,
    CONSTRAINT [PK_Suppliers] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [AspNetRoleClaims] (
    [Id] int NOT NULL IDENTITY,
    [RoleId] nvarchar(450) NOT NULL,
    [ClaimType] nvarchar(max) NULL,
    [ClaimValue] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetRoleClaims] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [AspNetUserClaims] (
    [Id] int NOT NULL IDENTITY,
    [UserId] nvarchar(450) NOT NULL,
    [ClaimType] nvarchar(max) NULL,
    [ClaimValue] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [AspNetUserLogins] (
    [LoginProvider] nvarchar(450) NOT NULL,
    [ProviderKey] nvarchar(450) NOT NULL,
    [ProviderDisplayName] nvarchar(max) NULL,
    [UserId] nvarchar(450) NOT NULL,
    CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY ([LoginProvider], [ProviderKey]),
    CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [AspNetUserRoles] (
    [UserId] nvarchar(450) NOT NULL,
    [RoleId] nvarchar(450) NOT NULL,
    CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY ([UserId], [RoleId]),
    CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [AspNetUserTokens] (
    [UserId] nvarchar(450) NOT NULL,
    [LoginProvider] nvarchar(450) NOT NULL,
    [Name] nvarchar(450) NOT NULL,
    [Value] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY ([UserId], [LoginProvider], [Name]),
    CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);
GO

CREATE INDEX [IX_AspNetRoleClaims_RoleId] ON [AspNetRoleClaims] ([RoleId]);
GO

CREATE UNIQUE INDEX [RoleNameIndex] ON [AspNetRoles] ([NormalizedName]) WHERE [NormalizedName] IS NOT NULL;
GO

CREATE INDEX [IX_AspNetUserClaims_UserId] ON [AspNetUserClaims] ([UserId]);
GO

CREATE INDEX [IX_AspNetUserLogins_UserId] ON [AspNetUserLogins] ([UserId]);
GO

CREATE INDEX [IX_AspNetUserRoles_RoleId] ON [AspNetUserRoles] ([RoleId]);
GO

CREATE INDEX [EmailIndex] ON [AspNetUsers] ([NormalizedEmail]);
GO

CREATE UNIQUE INDEX [UserNameIndex] ON [AspNetUsers] ([NormalizedUserName]) WHERE [NormalizedUserName] IS NOT NULL;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260826085845_AddPartnersAndIdentity', N'8.0.11');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [PurchaseOrders] (
    [Id] int NOT NULL IDENTITY,
    [SupplierId] int NOT NULL,
    [OrderDate] datetime2 NOT NULL,
    [Status] nvarchar(50) NOT NULL,
    [TotalAmount] decimal(18,2) NOT NULL,
    CONSTRAINT [PK_PurchaseOrders] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_PurchaseOrders_Suppliers_SupplierId] FOREIGN KEY ([SupplierId]) REFERENCES [Suppliers] ([Id]) ON DELETE NO ACTION
);
GO

CREATE TABLE [PurchaseOrderItems] (
    [Id] int NOT NULL IDENTITY,
    [PurchaseOrderId] int NOT NULL,
    [ProductId] int NOT NULL,
    [Quantity] int NOT NULL,
    [UnitCost] decimal(18,2) NOT NULL,
    CONSTRAINT [PK_PurchaseOrderItems] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_PurchaseOrderItems_Products_ProductId] FOREIGN KEY ([ProductId]) REFERENCES [Products] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_PurchaseOrderItems_PurchaseOrders_PurchaseOrderId] FOREIGN KEY ([PurchaseOrderId]) REFERENCES [PurchaseOrders] ([Id]) ON DELETE CASCADE
);
GO

CREATE INDEX [IX_PurchaseOrderItems_ProductId] ON [PurchaseOrderItems] ([ProductId]);
GO

CREATE INDEX [IX_PurchaseOrderItems_PurchaseOrderId] ON [PurchaseOrderItems] ([PurchaseOrderId]);
GO

CREATE INDEX [IX_PurchaseOrders_SupplierId] ON [PurchaseOrders] ([SupplierId]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260902060224_AddPurchaseOrders', N'8.0.11');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [SalesOrders] (
    [Id] int NOT NULL IDENTITY,
    [CustomerId] int NOT NULL,
    [OrderDate] datetime2 NOT NULL,
    [Status] nvarchar(50) NOT NULL,
    [TotalAmount] decimal(18,2) NOT NULL,
    CONSTRAINT [PK_SalesOrders] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_SalesOrders_Customers_CustomerId] FOREIGN KEY ([CustomerId]) REFERENCES [Customers] ([Id]) ON DELETE NO ACTION
);
GO

CREATE TABLE [Invoices] (
    [Id] int NOT NULL IDENTITY,
    [SalesOrderId] int NOT NULL,
    [InvoiceNumber] nvarchar(50) NOT NULL,
    [IssueDate] datetime2 NOT NULL,
    [DueDate] datetime2 NOT NULL,
    [IsPaid] bit NOT NULL,
    CONSTRAINT [PK_Invoices] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Invoices_SalesOrders_SalesOrderId] FOREIGN KEY ([SalesOrderId]) REFERENCES [SalesOrders] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [SalesOrderItems] (
    [Id] int NOT NULL IDENTITY,
    [SalesOrderId] int NOT NULL,
    [ProductId] int NOT NULL,
    [Quantity] int NOT NULL,
    [UnitPrice] decimal(18,2) NOT NULL,
    CONSTRAINT [PK_SalesOrderItems] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_SalesOrderItems_Products_ProductId] FOREIGN KEY ([ProductId]) REFERENCES [Products] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_SalesOrderItems_SalesOrders_SalesOrderId] FOREIGN KEY ([SalesOrderId]) REFERENCES [SalesOrders] ([Id]) ON DELETE CASCADE
);
GO

CREATE UNIQUE INDEX [IX_Invoices_InvoiceNumber] ON [Invoices] ([InvoiceNumber]);
GO

CREATE UNIQUE INDEX [IX_Invoices_SalesOrderId] ON [Invoices] ([SalesOrderId]);
GO

CREATE INDEX [IX_SalesOrderItems_ProductId] ON [SalesOrderItems] ([ProductId]);
GO

CREATE INDEX [IX_SalesOrderItems_SalesOrderId] ON [SalesOrderItems] ([SalesOrderId]);
GO

CREATE INDEX [IX_SalesOrders_CustomerId] ON [SalesOrders] ([CustomerId]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260902061113_AddSalesOrdersAndInvoices', N'8.0.11');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [StockMovements] (
    [Id] int NOT NULL IDENTITY,
    [ProductId] int NOT NULL,
    [MovementType] nvarchar(50) NOT NULL,
    [Quantity] int NOT NULL,
    [MovementDate] datetime2 NOT NULL,
    [ReferenceId] int NULL,
    [Notes] nvarchar(500) NULL,
    CONSTRAINT [PK_StockMovements] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_StockMovements_Products_ProductId] FOREIGN KEY ([ProductId]) REFERENCES [Products] ([Id]) ON DELETE NO ACTION
);
GO

CREATE INDEX [IX_StockMovements_ProductId_MovementDate] ON [StockMovements] ([ProductId], [MovementDate]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260902070514_AddStockMovements', N'8.0.11');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [Suppliers] ADD [CreatedAt] datetime2 NOT NULL DEFAULT '0001-01-01T00:00:00.0000000';
GO

ALTER TABLE [Suppliers] ADD [CreatedBy] nvarchar(max) NULL;
GO

ALTER TABLE [Suppliers] ADD [IsDeleted] bit NOT NULL DEFAULT CAST(0 AS bit);
GO

ALTER TABLE [Suppliers] ADD [RowVersion] varbinary(max) NOT NULL DEFAULT 0x;
GO

ALTER TABLE [Suppliers] ADD [UpdatedAt] datetime2 NULL;
GO

ALTER TABLE [Suppliers] ADD [UpdatedBy] nvarchar(max) NULL;
GO

ALTER TABLE [SalesOrders] ADD [RowVersion] rowversion NOT NULL;
GO

ALTER TABLE [PurchaseOrders] ADD [RowVersion] rowversion NOT NULL;
GO

ALTER TABLE [Products] ADD [CreatedBy] nvarchar(max) NULL;
GO

ALTER TABLE [Products] ADD [IsDeleted] bit NOT NULL DEFAULT CAST(0 AS bit);
GO

ALTER TABLE [Products] ADD [RowVersion] rowversion NOT NULL;
GO

ALTER TABLE [Products] ADD [UpdatedAt] datetime2 NULL;
GO

ALTER TABLE [Products] ADD [UpdatedBy] nvarchar(max) NULL;
GO

ALTER TABLE [Customers] ADD [CreatedAt] datetime2 NOT NULL DEFAULT '0001-01-01T00:00:00.0000000';
GO

ALTER TABLE [Customers] ADD [CreatedBy] nvarchar(max) NULL;
GO

ALTER TABLE [Customers] ADD [IsDeleted] bit NOT NULL DEFAULT CAST(0 AS bit);
GO

ALTER TABLE [Customers] ADD [RowVersion] varbinary(max) NOT NULL DEFAULT 0x;
GO

ALTER TABLE [Customers] ADD [UpdatedAt] datetime2 NULL;
GO

ALTER TABLE [Customers] ADD [UpdatedBy] nvarchar(max) NULL;
GO

UPDATE [Products] SET [CreatedBy] = NULL, [IsDeleted] = CAST(0 AS bit), [UpdatedAt] = NULL, [UpdatedBy] = NULL
WHERE [Id] = 1;
SELECT @@ROWCOUNT;

GO

UPDATE [Products] SET [CreatedBy] = NULL, [IsDeleted] = CAST(0 AS bit), [UpdatedAt] = NULL, [UpdatedBy] = NULL
WHERE [Id] = 2;
SELECT @@ROWCOUNT;

GO

UPDATE [Products] SET [CreatedBy] = NULL, [IsDeleted] = CAST(0 AS bit), [UpdatedAt] = NULL, [UpdatedBy] = NULL
WHERE [Id] = 3;
SELECT @@ROWCOUNT;

GO

UPDATE [Products] SET [CreatedBy] = NULL, [IsDeleted] = CAST(0 AS bit), [UpdatedAt] = NULL, [UpdatedBy] = NULL
WHERE [Id] = 4;
SELECT @@ROWCOUNT;

GO

UPDATE [Products] SET [CreatedBy] = NULL, [IsDeleted] = CAST(0 AS bit), [UpdatedAt] = NULL, [UpdatedBy] = NULL
WHERE [Id] = 5;
SELECT @@ROWCOUNT;

GO

UPDATE [Products] SET [CreatedBy] = NULL, [IsDeleted] = CAST(0 AS bit), [UpdatedAt] = NULL, [UpdatedBy] = NULL
WHERE [Id] = 6;
SELECT @@ROWCOUNT;

GO

UPDATE [Products] SET [CreatedBy] = NULL, [IsDeleted] = CAST(0 AS bit), [UpdatedAt] = NULL, [UpdatedBy] = NULL
WHERE [Id] = 7;
SELECT @@ROWCOUNT;

GO

UPDATE [Products] SET [CreatedBy] = NULL, [IsDeleted] = CAST(0 AS bit), [UpdatedAt] = NULL, [UpdatedBy] = NULL
WHERE [Id] = 8;
SELECT @@ROWCOUNT;

GO

UPDATE [Products] SET [CreatedBy] = NULL, [IsDeleted] = CAST(0 AS bit), [UpdatedAt] = NULL, [UpdatedBy] = NULL
WHERE [Id] = 9;
SELECT @@ROWCOUNT;

GO

UPDATE [Products] SET [CreatedBy] = NULL, [IsDeleted] = CAST(0 AS bit), [UpdatedAt] = NULL, [UpdatedBy] = NULL
WHERE [Id] = 10;
SELECT @@ROWCOUNT;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260902090757_AddConcurrencyAndAudit', N'8.0.11');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [SalesOrderItems] ADD [LineTotal] AS [Quantity] * [UnitPrice] PERSISTED;
GO

ALTER TABLE [PurchaseOrderItems] ADD [LineTotal] AS [Quantity] * [UnitCost] PERSISTED;
GO

CREATE INDEX [IX_Suppliers_ContactEmail] ON [Suppliers] ([ContactEmail]);
GO

ALTER TABLE [StockMovements] ADD CONSTRAINT [CK_StockMovements_Quantity] CHECK ([Quantity] >= 0);
GO

ALTER TABLE [SalesOrderItems] ADD CONSTRAINT [CK_SalesOrderItems_Quantity] CHECK ([Quantity] >= 0);
GO

ALTER TABLE [SalesOrderItems] ADD CONSTRAINT [CK_SalesOrderItems_UnitPrice] CHECK ([UnitPrice] >= 0);
GO

ALTER TABLE [PurchaseOrderItems] ADD CONSTRAINT [CK_PurchaseOrderItems_Quantity] CHECK ([Quantity] >= 0);
GO

ALTER TABLE [PurchaseOrderItems] ADD CONSTRAINT [CK_PurchaseOrderItems_UnitCost] CHECK ([UnitCost] >= 0);
GO

ALTER TABLE [Products] ADD CONSTRAINT [CK_Products_Price] CHECK ([Price] >= 0);
GO

CREATE INDEX [IX_Customers_Email] ON [Customers] ([Email]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260903024325_AddConstraints', N'8.0.11');
GO

COMMIT;
GO

