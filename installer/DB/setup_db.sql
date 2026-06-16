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

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260504204447_InitialCreate'
)
BEGIN
    CREATE TABLE [Categories] (
        [Id] int NOT NULL IDENTITY,
        [Name] nvarchar(100) NOT NULL,
        [Description] nvarchar(500) NULL,
        CONSTRAINT [PK_Categories] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260504204447_InitialCreate'
)
BEGIN
    CREATE TABLE [Users] (
        [Id] int NOT NULL IDENTITY,
        [FirstName] nvarchar(100) NOT NULL,
        [LastName] nvarchar(100) NOT NULL,
        [MiddleName] nvarchar(100) NULL,
        [Email] nvarchar(256) NOT NULL,
        [Phone] nvarchar(20) NULL,
        [PasswordHash] nvarchar(max) NOT NULL,
        [Role] nvarchar(max) NOT NULL,
        [IsBlocked] bit NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        CONSTRAINT [PK_Users] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260504204447_InitialCreate'
)
BEGIN
    CREATE TABLE [RealEstateObjects] (
        [Id] int NOT NULL IDENTITY,
        [CategoryId] int NOT NULL,
        [Title] nvarchar(200) NOT NULL,
        [Description] nvarchar(max) NULL,
        [Price] decimal(18,2) NOT NULL,
        [Area] decimal(18,2) NOT NULL,
        [Floor] int NULL,
        [TotalFloors] int NULL,
        [Rooms] int NULL,
        [Address] nvarchar(300) NOT NULL,
        [Status] nvarchar(max) NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        CONSTRAINT [PK_RealEstateObjects] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_RealEstateObjects_Categories_CategoryId] FOREIGN KEY ([CategoryId]) REFERENCES [Categories] ([Id]) ON DELETE NO ACTION
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260504204447_InitialCreate'
)
BEGIN
    CREATE TABLE [Bookings] (
        [Id] int NOT NULL IDENTITY,
        [UserId] int NOT NULL,
        [ObjectId] int NOT NULL,
        [BookingType] nvarchar(max) NOT NULL,
        [BookingStatus] nvarchar(max) NOT NULL,
        [Comment] nvarchar(500) NULL,
        [PreferredDate] datetime2 NULL,
        [CreatedAt] datetime2 NOT NULL,
        [UpdatedAt] datetime2 NULL,
        CONSTRAINT [PK_Bookings] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Bookings_RealEstateObjects_ObjectId] FOREIGN KEY ([ObjectId]) REFERENCES [RealEstateObjects] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_Bookings_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]) ON DELETE NO ACTION
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260504204447_InitialCreate'
)
BEGIN
    CREATE TABLE [Photos] (
        [Id] int NOT NULL IDENTITY,
        [ObjectId] int NOT NULL,
        [FilePath] nvarchar(500) NOT NULL,
        [AltText] nvarchar(200) NULL,
        [IsMain] bit NOT NULL,
        CONSTRAINT [PK_Photos] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Photos_RealEstateObjects_ObjectId] FOREIGN KEY ([ObjectId]) REFERENCES [RealEstateObjects] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260504204447_InitialCreate'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Description', N'Name') AND [object_id] = OBJECT_ID(N'[Categories]'))
        SET IDENTITY_INSERT [Categories] ON;
    EXEC(N'INSERT INTO [Categories] ([Id], [Description], [Name])
    VALUES (1, N''Квартири у нових житлових комплексах'', N''Квартири''),
    (2, N''Приватні будинки та таунхауси'', N''Будинки''),
    (3, N''Офісні та торгові приміщення'', N''Комерційна нерухомість'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Description', N'Name') AND [object_id] = OBJECT_ID(N'[Categories]'))
        SET IDENTITY_INSERT [Categories] OFF;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260504204447_InitialCreate'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'CreatedAt', N'Email', N'FirstName', N'IsBlocked', N'LastName', N'MiddleName', N'PasswordHash', N'Phone', N'Role') AND [object_id] = OBJECT_ID(N'[Users]'))
        SET IDENTITY_INSERT [Users] ON;
    EXEC(N'INSERT INTO [Users] ([Id], [CreatedAt], [Email], [FirstName], [IsBlocked], [LastName], [MiddleName], [PasswordHash], [Phone], [Role])
    VALUES (1, ''2026-01-01T00:00:00.0000000'', N''admin@construction.ua'', N''Адмін'', CAST(0 AS bit), N''Системний'', NULL, N''$2a$11$miFVcItBmOH25gYFdHy82upbRi12BbkeRtNo9JSZ.OKpC982H5KJ.'', NULL, N''Admin'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'CreatedAt', N'Email', N'FirstName', N'IsBlocked', N'LastName', N'MiddleName', N'PasswordHash', N'Phone', N'Role') AND [object_id] = OBJECT_ID(N'[Users]'))
        SET IDENTITY_INSERT [Users] OFF;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260504204447_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Bookings_ObjectId] ON [Bookings] ([ObjectId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260504204447_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Bookings_UserId] ON [Bookings] ([UserId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260504204447_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Photos_ObjectId] ON [Photos] ([ObjectId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260504204447_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_RealEstateObjects_CategoryId] ON [RealEstateObjects] ([CategoryId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260504204447_InitialCreate'
)
BEGIN
    CREATE UNIQUE INDEX [IX_Users_Email] ON [Users] ([Email]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260504204447_InitialCreate'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260504204447_InitialCreate', N'8.0.0');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260505183615_AddNotifications'
)
BEGIN
    CREATE TABLE [Notifications] (
        [Id] int NOT NULL IDENTITY,
        [UserId] int NOT NULL,
        [Message] nvarchar(max) NOT NULL,
        [IsRead] bit NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        CONSTRAINT [PK_Notifications] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Notifications_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260505183615_AddNotifications'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''$2a$11$jaCJXi6Qc/ppAuDn/dV7xuyqNOAT0K0j68RWeKRVGoAC/mQvMMPDi''
    WHERE [Id] = 1;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260505183615_AddNotifications'
)
BEGIN
    CREATE INDEX [IX_Notifications_UserId] ON [Notifications] ([UserId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260505183615_AddNotifications'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260505183615_AddNotifications', N'8.0.0');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260505232917_AddPriceTypeAndProjectFields'
)
BEGIN
    ALTER TABLE [RealEstateObjects] ADD [PriceType] nvarchar(max) NOT NULL DEFAULT N'';
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260505232917_AddPriceTypeAndProjectFields'
)
BEGIN
    ALTER TABLE [RealEstateObjects] ADD [ProjectQuarter] int NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260505232917_AddPriceTypeAndProjectFields'
)
BEGIN
    ALTER TABLE [RealEstateObjects] ADD [ProjectYear] int NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260505232917_AddPriceTypeAndProjectFields'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''$2a$11$G.TJHArHwENP7L0HSOACxe1zqHRubzSTA3sNAPe1e7vpJ8NmdQ6wO''
    WHERE [Id] = 1;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260505232917_AddPriceTypeAndProjectFields'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260505232917_AddPriceTypeAndProjectFields', N'8.0.0');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260506061531_SplitStatusFields'
)
BEGIN
    EXEC sp_rename N'[RealEstateObjects].[Status]', N'SaleStatus', N'COLUMN';
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260506061531_SplitStatusFields'
)
BEGIN
    ALTER TABLE [RealEstateObjects] ADD [BuildingStatus] nvarchar(max) NOT NULL DEFAULT N'';
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260506061531_SplitStatusFields'
)
BEGIN
    ALTER TABLE [RealEstateObjects] ADD [IsHidden] bit NOT NULL DEFAULT CAST(0 AS bit);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260506061531_SplitStatusFields'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''$2a$11$iRzSE7zQZAz75XGrduHbveFvjQbQ3QOayB9Ti80k1KyJ57yLJwbby''
    WHERE [Id] = 1;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260506061531_SplitStatusFields'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260506061531_SplitStatusFields', N'8.0.0');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260506071331_AddIsFeatured'
)
BEGIN
    ALTER TABLE [RealEstateObjects] ADD [IsFeatured] bit NOT NULL DEFAULT CAST(0 AS bit);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260506071331_AddIsFeatured'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''$2a$11$Vg2P2.T4Ar7Aa9b.GqEGLON37/uZLtX0AvrPYEsqGh5Cn1SZRMqzm''
    WHERE [Id] = 1;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260506071331_AddIsFeatured'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260506071331_AddIsFeatured', N'8.0.0');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260515191206_AddIsProject'
)
BEGIN
    ALTER TABLE [RealEstateObjects] ADD [IsProject] bit NOT NULL DEFAULT CAST(0 AS bit);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260515191206_AddIsProject'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''$2a$11$OheeGbRsvv/DcaJzFnlrh.wxflEr3dYz1taVvw1L/gMZD.X8pZqBu''
    WHERE [Id] = 1;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260515191206_AddIsProject'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260515191206_AddIsProject', N'8.0.0');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260515192233_AddCommercialFields'
)
BEGIN
    ALTER TABLE [RealEstateObjects] ADD [CeilingHeight] decimal(18,2) NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260515192233_AddCommercialFields'
)
BEGIN
    ALTER TABLE [RealEstateObjects] ADD [CommercialType] nvarchar(max) NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260515192233_AddCommercialFields'
)
BEGIN
    ALTER TABLE [RealEstateObjects] ADD [HasParking] bit NOT NULL DEFAULT CAST(0 AS bit);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260515192233_AddCommercialFields'
)
BEGIN
    ALTER TABLE [RealEstateObjects] ADD [HasSeparateEntrance] bit NOT NULL DEFAULT CAST(0 AS bit);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260515192233_AddCommercialFields'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''$2a$11$ABDm9mk9zQdB8SLuF.bkYuRbVSqQ6SGq0SeHA73FfYej6F7MgQTP.''
    WHERE [Id] = 1;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260515192233_AddCommercialFields'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260515192233_AddCommercialFields', N'8.0.0');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260616171900_FixCeilingHeightPrecision'
)
BEGIN
    DECLARE @var0 sysname;
    SELECT @var0 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[RealEstateObjects]') AND [c].[name] = N'CeilingHeight');
    IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [RealEstateObjects] DROP CONSTRAINT [' + @var0 + '];');
    ALTER TABLE [RealEstateObjects] ALTER COLUMN [CeilingHeight] decimal(5,2) NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260616171900_FixCeilingHeightPrecision'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''$2a$11$VjTZreMMXxWJudYqzeZm2.eg6cIWhtJcNro8ddBHprz8jzYIstg2q''
    WHERE [Id] = 1;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260616171900_FixCeilingHeightPrecision'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260616171900_FixCeilingHeightPrecision', N'8.0.0');
END;
GO

COMMIT;
GO

