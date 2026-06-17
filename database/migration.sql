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
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260612152940_InitialCreate'
)
BEGIN
    CREATE TABLE [User] (
        [Id] uniqueidentifier NOT NULL,
        [FullName] nvarchar(max) NOT NULL,
        [Email] nvarchar(max) NOT NULL,
        [PasswordHash] nvarchar(max) NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        [UpdatedAt] datetime2 NULL,
        CONSTRAINT [PK_User] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260612152940_InitialCreate'
)
BEGIN
    CREATE TABLE [AuditLog] (
        [Id] uniqueidentifier NOT NULL,
        [UserId] uniqueidentifier NULL,
        [Action] nvarchar(max) NOT NULL,
        [TableName] nvarchar(max) NOT NULL,
        [Details] nvarchar(max) NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        [UpdatedAt] datetime2 NULL,
        CONSTRAINT [PK_AuditLog] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_AuditLog_User_UserId] FOREIGN KEY ([UserId]) REFERENCES [User] ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260612152940_InitialCreate'
)
BEGIN
    CREATE TABLE [Patient] (
        [Id] uniqueidentifier NOT NULL,
        [FullName] nvarchar(max) NOT NULL,
        [PhoneNumber] nvarchar(max) NOT NULL,
        [DateOfBirth] date NOT NULL,
        [ReasonForVisitc] nvarchar(max) NOT NULL,
        [CreatedByUserId] uniqueidentifier NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        [UpdatedAt] datetime2 NULL,
        CONSTRAINT [PK_Patient] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Patient_User_CreatedByUserId] FOREIGN KEY ([CreatedByUserId]) REFERENCES [User] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260612152940_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_AuditLog_UserId] ON [AuditLog] ([UserId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260612152940_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Patient_CreatedByUserId] ON [Patient] ([CreatedByUserId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260612152940_InitialCreate'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260612152940_InitialCreate', N'10.0.9');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260613125900_emailUniqe'
)
BEGIN
    DECLARE @var nvarchar(max);
    SELECT @var = QUOTENAME([d].[name])
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[User]') AND [c].[name] = N'Email');
    IF @var IS NOT NULL EXEC(N'ALTER TABLE [User] DROP CONSTRAINT ' + @var + ';');
    ALTER TABLE [User] ALTER COLUMN [Email] nvarchar(450) NOT NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260613125900_emailUniqe'
)
BEGIN
    CREATE UNIQUE INDEX [IX_User_Email] ON [User] ([Email]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260613125900_emailUniqe'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260613125900_emailUniqe', N'10.0.9');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260613194900_FixedRewasonForVisit'
)
BEGIN
    EXEC sp_rename N'[Patient].[ReasonForVisitc]', N'ReasonForVisit', 'COLUMN';
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260613194900_FixedRewasonForVisit'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260613194900_FixedRewasonForVisit', N'10.0.9');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260613220509_AddWebhookTables'
)
BEGIN
    CREATE TABLE [WebhookEndpoints] (
        [Id] uniqueidentifier NOT NULL,
        [Name] nvarchar(max) NOT NULL,
        [Url] nvarchar(max) NOT NULL,
        [EventType] nvarchar(max) NOT NULL,
        [IsActive] bit NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        [UpdatedAt] datetime2 NULL,
        CONSTRAINT [PK_WebhookEndpoints] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260613220509_AddWebhookTables'
)
BEGIN
    CREATE TABLE [WebhookDeliveryLogs] (
        [Id] uniqueidentifier NOT NULL,
        [WebhookEndpointId] uniqueidentifier NOT NULL,
        [PatientId] uniqueidentifier NOT NULL,
        [EventType] nvarchar(max) NOT NULL,
        [RequestPayload] nvarchar(max) NOT NULL,
        [ResponseStatusCode] int NULL,
        [ResponseBody] nvarchar(max) NOT NULL,
        [IsSuccess] bit NOT NULL,
        [ErrorMessage] nvarchar(max) NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        [UpdatedAt] datetime2 NULL,
        CONSTRAINT [PK_WebhookDeliveryLogs] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_WebhookDeliveryLogs_WebhookEndpoints_WebhookEndpointId] FOREIGN KEY ([WebhookEndpointId]) REFERENCES [WebhookEndpoints] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260613220509_AddWebhookTables'
)
BEGIN
    CREATE INDEX [IX_WebhookDeliveryLogs_WebhookEndpointId] ON [WebhookDeliveryLogs] ([WebhookEndpointId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260613220509_AddWebhookTables'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260613220509_AddWebhookTables', N'10.0.9');
END;

COMMIT;
GO

