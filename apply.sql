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
    WHERE [MigrationId] = N'20260629225044_AddNewMemberEntity'
)
BEGIN
    EXEC(N'DELETE FROM [RolePermissions]
    WHERE [Id] = ''03eb6362-d558-4c52-abcf-d56fc0f6955c'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260629225044_AddNewMemberEntity'
)
BEGIN
    EXEC(N'DELETE FROM [RolePermissions]
    WHERE [Id] = ''04ccbdc5-1aeb-4e81-ac19-ab620c85e9b6'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260629225044_AddNewMemberEntity'
)
BEGIN
    EXEC(N'DELETE FROM [RolePermissions]
    WHERE [Id] = ''057302f5-3758-4d01-8e38-c991b59f2298'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260629225044_AddNewMemberEntity'
)
BEGIN
    EXEC(N'DELETE FROM [RolePermissions]
    WHERE [Id] = ''06b4b0d4-b1b1-41eb-ad43-9541ddefa59b'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260629225044_AddNewMemberEntity'
)
BEGIN
    EXEC(N'DELETE FROM [RolePermissions]
    WHERE [Id] = ''0af92da4-9a07-4290-a522-78dfbab5c81f'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260629225044_AddNewMemberEntity'
)
BEGIN
    EXEC(N'DELETE FROM [RolePermissions]
    WHERE [Id] = ''127002d8-cf6a-4035-bfb9-84cb3f51038c'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260629225044_AddNewMemberEntity'
)
BEGIN
    EXEC(N'DELETE FROM [RolePermissions]
    WHERE [Id] = ''1893a9d6-ba62-4eb4-a070-92fc3951892f'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260629225044_AddNewMemberEntity'
)
BEGIN
    EXEC(N'DELETE FROM [RolePermissions]
    WHERE [Id] = ''1ca3ec1a-826e-48e5-b5aa-e1a916ae1909'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260629225044_AddNewMemberEntity'
)
BEGIN
    EXEC(N'DELETE FROM [RolePermissions]
    WHERE [Id] = ''3dbc6a23-8010-4fde-add2-68769b0cbbf4'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260629225044_AddNewMemberEntity'
)
BEGIN
    EXEC(N'DELETE FROM [RolePermissions]
    WHERE [Id] = ''44c7afa1-a567-4652-b083-05ae13bfcecd'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260629225044_AddNewMemberEntity'
)
BEGIN
    EXEC(N'DELETE FROM [RolePermissions]
    WHERE [Id] = ''5a22b771-294e-402f-8996-544c71a2bf35'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260629225044_AddNewMemberEntity'
)
BEGIN
    EXEC(N'DELETE FROM [RolePermissions]
    WHERE [Id] = ''64bb391f-27d3-49ea-8a1c-5a9f1cff948f'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260629225044_AddNewMemberEntity'
)
BEGIN
    EXEC(N'DELETE FROM [RolePermissions]
    WHERE [Id] = ''746cc333-2a27-419a-96db-41967d2f8324'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260629225044_AddNewMemberEntity'
)
BEGIN
    EXEC(N'DELETE FROM [RolePermissions]
    WHERE [Id] = ''764dca3f-41de-45db-a89d-5c20fc8b22e9'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260629225044_AddNewMemberEntity'
)
BEGIN
    EXEC(N'DELETE FROM [RolePermissions]
    WHERE [Id] = ''9d95fc93-62b9-43e4-85df-d48bae9db131'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260629225044_AddNewMemberEntity'
)
BEGIN
    EXEC(N'DELETE FROM [RolePermissions]
    WHERE [Id] = ''b8c16606-294c-4be3-be61-bc3f63ed11e0'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260629225044_AddNewMemberEntity'
)
BEGIN
    EXEC(N'DELETE FROM [RolePermissions]
    WHERE [Id] = ''bd48abee-a0f4-46e4-b0ba-33d161c638af'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260629225044_AddNewMemberEntity'
)
BEGIN
    EXEC(N'DELETE FROM [RolePermissions]
    WHERE [Id] = ''bd8b45a6-05d7-457a-9017-fb87524b1cf5'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260629225044_AddNewMemberEntity'
)
BEGIN
    EXEC(N'DELETE FROM [RolePermissions]
    WHERE [Id] = ''d77a3621-8dc1-4df1-959d-adf1ddc798de'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260629225044_AddNewMemberEntity'
)
BEGIN
    EXEC(N'DELETE FROM [RolePermissions]
    WHERE [Id] = ''ea0b55ff-58a4-4a91-a4c7-f7219ab5f975'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260629225044_AddNewMemberEntity'
)
BEGIN
    EXEC(N'DELETE FROM [RolePermissions]
    WHERE [Id] = ''ee3e2099-ec64-48e8-9afd-f461f035e6f5'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260629225044_AddNewMemberEntity'
)
BEGIN
    EXEC(N'DELETE FROM [RolePermissions]
    WHERE [Id] = ''f30ecd20-1572-4377-a981-47bf992fe5ac'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260629225044_AddNewMemberEntity'
)
BEGIN
    EXEC(N'DELETE FROM [RolePermissions]
    WHERE [Id] = ''f8f4bfff-728b-45b5-ae2c-3b19387a27c0'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260629225044_AddNewMemberEntity'
)
BEGIN
    EXEC(N'DELETE FROM [RolePermissions]
    WHERE [Id] = ''fec91eab-19ad-4654-8117-f2ab2473f88a'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260629225044_AddNewMemberEntity'
)
BEGIN
    DECLARE @var nvarchar(max);
    SELECT @var = QUOTENAME([d].[name])
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Members]') AND [c].[name] = N'DateOfBirth');
    IF @var IS NOT NULL EXEC(N'ALTER TABLE [Members] DROP CONSTRAINT ' + @var + ';');
    ALTER TABLE [Members] DROP COLUMN [DateOfBirth];
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260629225044_AddNewMemberEntity'
)
BEGIN
    DECLARE @var1 nvarchar(max);
    SELECT @var1 = QUOTENAME([d].[name])
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Members]') AND [c].[name] = N'DeviceUserId');
    IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [Members] DROP CONSTRAINT ' + @var1 + ';');
    ALTER TABLE [Members] DROP COLUMN [DeviceUserId];
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260629225044_AddNewMemberEntity'
)
BEGIN
    DECLARE @var2 nvarchar(max);
    SELECT @var2 = QUOTENAME([d].[name])
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Members]') AND [c].[name] = N'EmergencyContactPhone');
    IF @var2 IS NOT NULL EXEC(N'ALTER TABLE [Members] DROP CONSTRAINT ' + @var2 + ';');
    ALTER TABLE [Members] DROP COLUMN [EmergencyContactPhone];
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260629225044_AddNewMemberEntity'
)
BEGIN
    DECLARE @var3 nvarchar(max);
    SELECT @var3 = QUOTENAME([d].[name])
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Members]') AND [c].[name] = N'FaceId');
    IF @var3 IS NOT NULL EXEC(N'ALTER TABLE [Members] DROP CONSTRAINT ' + @var3 + ';');
    ALTER TABLE [Members] DROP COLUMN [FaceId];
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260629225044_AddNewMemberEntity'
)
BEGIN
    DECLARE @var4 nvarchar(max);
    SELECT @var4 = QUOTENAME([d].[name])
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Members]') AND [c].[name] = N'Gender');
    IF @var4 IS NOT NULL EXEC(N'ALTER TABLE [Members] DROP CONSTRAINT ' + @var4 + ';');
    ALTER TABLE [Members] DROP COLUMN [Gender];
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260629225044_AddNewMemberEntity'
)
BEGIN
    DECLARE @var5 nvarchar(max);
    SELECT @var5 = QUOTENAME([d].[name])
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Members]') AND [c].[name] = N'Notes');
    IF @var5 IS NOT NULL EXEC(N'ALTER TABLE [Members] DROP CONSTRAINT ' + @var5 + ';');
    ALTER TABLE [Members] DROP COLUMN [Notes];
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260629225044_AddNewMemberEntity'
)
BEGIN
    DECLARE @var6 nvarchar(max);
    SELECT @var6 = QUOTENAME([d].[name])
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Members]') AND [c].[name] = N'Phone');
    IF @var6 IS NOT NULL EXEC(N'ALTER TABLE [Members] DROP CONSTRAINT ' + @var6 + ';');
    ALTER TABLE [Members] DROP COLUMN [Phone];
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260629225044_AddNewMemberEntity'
)
BEGIN
    DECLARE @var7 nvarchar(max);
    SELECT @var7 = QUOTENAME([d].[name])
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Members]') AND [c].[name] = N'WhatsAppNumber');
    IF @var7 IS NOT NULL EXEC(N'ALTER TABLE [Members] DROP CONSTRAINT ' + @var7 + ';');
    ALTER TABLE [Members] DROP COLUMN [WhatsAppNumber];
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260629225044_AddNewMemberEntity'
)
BEGIN
    EXEC sp_rename N'[Members].[Status]', N'PaymentMethod', 'COLUMN';
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260629225044_AddNewMemberEntity'
)
BEGIN
    EXEC sp_rename N'[Members].[PhotoPath]', N'DiseaseType', 'COLUMN';
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260629225044_AddNewMemberEntity'
)
BEGIN
    EXEC sp_rename N'[Members].[JoinDate]', N'SubscriptionStartDate', 'COLUMN';
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260629225044_AddNewMemberEntity'
)
BEGIN
    EXEC sp_rename N'[Members].[IsActive]', N'IsDeleted', 'COLUMN';
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260629225044_AddNewMemberEntity'
)
BEGIN
    EXEC sp_rename N'[Members].[FingerprintId]', N'FingerprintDeviceId', 'COLUMN';
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260629225044_AddNewMemberEntity'
)
BEGIN
    EXEC sp_rename N'[Members].[EmergencyContactName]', N'ReferralSource', 'COLUMN';
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260629225044_AddNewMemberEntity'
)
BEGIN
    EXEC sp_rename N'[Members].[Email]', N'Company', 'COLUMN';
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260629225044_AddNewMemberEntity'
)
BEGIN
    EXEC sp_rename N'[Members].[Code]', N'ReceiptNumber', 'COLUMN';
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260629225044_AddNewMemberEntity'
)
BEGIN
    EXEC sp_rename N'[Members].[IX_Members_Code]', N'IX_Members_ReceiptNumber', 'INDEX';
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260629225044_AddNewMemberEntity'
)
BEGIN
    ALTER TABLE [Members] ADD [AdminSignature] nvarchar(max) NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260629225044_AddNewMemberEntity'
)
BEGIN
    ALTER TABLE [Members] ADD [FreeMonths] int NOT NULL DEFAULT 0;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260629225044_AddNewMemberEntity'
)
BEGIN
    ALTER TABLE [Members] ADD [FreezeDays] int NOT NULL DEFAULT 0;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260629225044_AddNewMemberEntity'
)
BEGIN
    ALTER TABLE [Members] ADD [HasDisease] bit NOT NULL DEFAULT CAST(0 AS bit);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260629225044_AddNewMemberEntity'
)
BEGIN
    ALTER TABLE [Members] ADD [MemberSignature] nvarchar(max) NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260629225044_AddNewMemberEntity'
)
BEGIN
    ALTER TABLE [Members] ADD [NationalId] nvarchar(14) NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260629225044_AddNewMemberEntity'
)
BEGIN
    ALTER TABLE [Members] ADD [Nationality] nvarchar(100) NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260629225044_AddNewMemberEntity'
)
BEGIN
    ALTER TABLE [Members] ADD [PackageId] uniqueidentifier NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260629225044_AddNewMemberEntity'
)
BEGIN
    ALTER TABLE [Members] ADD [PaidAmount] decimal(18,2) NOT NULL DEFAULT 0.0;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260629225044_AddNewMemberEntity'
)
BEGIN
    ALTER TABLE [Members] ADD [PhoneNumber] nvarchar(11) NOT NULL DEFAULT N'';
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260629225044_AddNewMemberEntity'
)
BEGIN
    ALTER TABLE [Members] ADD [RegistrationDate] datetime2 NOT NULL DEFAULT '0001-01-01T00:00:00.0000000';
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260629225044_AddNewMemberEntity'
)
BEGIN
    ALTER TABLE [Members] ADD [RemainingAmount] decimal(18,2) NOT NULL DEFAULT 0.0;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260629225044_AddNewMemberEntity'
)
BEGIN
    ALTER TABLE [Members] ADD [SubscriptionDurationMonths] int NOT NULL DEFAULT 0;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260629225044_AddNewMemberEntity'
)
BEGIN
    ALTER TABLE [Members] ADD [SubscriptionEndDate] datetime2 NOT NULL DEFAULT '0001-01-01T00:00:00.0000000';
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260629225044_AddNewMemberEntity'
)
BEGIN
    ALTER TABLE [Members] ADD [SubscriptionPrice] decimal(18,2) NOT NULL DEFAULT 0.0;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260629225044_AddNewMemberEntity'
)
BEGIN
    ALTER TABLE [Members] ADD [Weight] decimal(5,1) NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260629225044_AddNewMemberEntity'
)
BEGIN
    EXEC(N'UPDATE [Permissions] SET [CreatedAt] = ''2026-06-29T22:50:43.4707083Z''
    WHERE [Id] = ''a3b4c5d6-e7f8-9012-a345-678901234567'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260629225044_AddNewMemberEntity'
)
BEGIN
    EXEC(N'UPDATE [Permissions] SET [CreatedAt] = ''2026-06-29T22:50:43.4707061Z''
    WHERE [Id] = ''a7b8c9d0-e1f2-3456-a789-012345678901'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260629225044_AddNewMemberEntity'
)
BEGIN
    EXEC(N'UPDATE [Permissions] SET [CreatedAt] = ''2026-06-29T22:50:43.4707196Z''
    WHERE [Id] = ''a9b0c1d2-e3f4-5678-a901-234567890123'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260629225044_AddNewMemberEntity'
)
BEGIN
    EXEC(N'UPDATE [Permissions] SET [CreatedAt] = ''2026-06-29T22:50:43.4707084Z''
    WHERE [Id] = ''b4c5d6e7-f8a9-0123-b456-789012345678'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260629225044_AddNewMemberEntity'
)
BEGIN
    EXEC(N'UPDATE [Permissions] SET [CreatedAt] = ''2026-06-29T22:50:43.4707064Z''
    WHERE [Id] = ''b8c9d0e1-f2a3-4567-b890-123456789012'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260629225044_AddNewMemberEntity'
)
BEGIN
    EXEC(N'UPDATE [Permissions] SET [CreatedAt] = ''2026-06-29T22:50:43.4707085Z''
    WHERE [Id] = ''c5d6e7f8-a9b0-1234-c567-890123456789'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260629225044_AddNewMemberEntity'
)
BEGIN
    EXEC(N'UPDATE [Permissions] SET [CreatedAt] = ''2026-06-29T22:50:43.4707077Z''
    WHERE [Id] = ''c9d0e1f2-a3b4-5678-c901-234567890123'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260629225044_AddNewMemberEntity'
)
BEGIN
    EXEC(N'UPDATE [Permissions] SET [CreatedAt] = ''2026-06-29T22:50:43.4707079Z''
    WHERE [Id] = ''d0e1f2a3-b4c5-6789-d012-345678901234'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260629225044_AddNewMemberEntity'
)
BEGIN
    EXEC(N'UPDATE [Permissions] SET [CreatedAt] = ''2026-06-29T22:50:43.4707087Z''
    WHERE [Id] = ''d6e7f8a9-b0c1-2345-d678-901234567890'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260629225044_AddNewMemberEntity'
)
BEGIN
    EXEC(N'UPDATE [Permissions] SET [CreatedAt] = ''2026-06-29T22:50:43.4707080Z''
    WHERE [Id] = ''e1f2a3b4-c5d6-7890-e123-456789012345'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260629225044_AddNewMemberEntity'
)
BEGIN
    EXEC(N'UPDATE [Permissions] SET [CreatedAt] = ''2026-06-29T22:50:43.4705122Z''
    WHERE [Id] = ''e5f6a7b8-c9d0-1234-ef56-789012345678'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260629225044_AddNewMemberEntity'
)
BEGIN
    EXEC(N'UPDATE [Permissions] SET [CreatedAt] = ''2026-06-29T22:50:43.4707088Z''
    WHERE [Id] = ''e7f8a9b0-c1d2-3456-e789-012345678901'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260629225044_AddNewMemberEntity'
)
BEGIN
    EXEC(N'UPDATE [Permissions] SET [CreatedAt] = ''2026-06-29T22:50:43.4707082Z''
    WHERE [Id] = ''f2a3b4c5-d6e7-8901-f234-567890123456'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260629225044_AddNewMemberEntity'
)
BEGIN
    EXEC(N'UPDATE [Permissions] SET [CreatedAt] = ''2026-06-29T22:50:43.4707056Z''
    WHERE [Id] = ''f6a7b8c9-d0e1-2345-f678-901234567890'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260629225044_AddNewMemberEntity'
)
BEGIN
    EXEC(N'UPDATE [Permissions] SET [CreatedAt] = ''2026-06-29T22:50:43.4707127Z''
    WHERE [Id] = ''f8a9b0c1-d2e3-4567-f890-123456789012'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260629225044_AddNewMemberEntity'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'CreatedAt', N'Description', N'Name', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[Permissions]'))
        SET IDENTITY_INSERT [Permissions] ON;
    EXEC(N'INSERT INTO [Permissions] ([Id], [CreatedAt], [Description], [Name], [UpdatedAt])
    VALUES (''d1e2f3a4-b5c6-7890-d123-456789012345'', ''2026-06-29T22:50:43.4707198Z'', N''Manage users'', N''users.manage'', NULL)');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'CreatedAt', N'Description', N'Name', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[Permissions]'))
        SET IDENTITY_INSERT [Permissions] OFF;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260629225044_AddNewMemberEntity'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'CreatedAt', N'PermissionId', N'RoleId', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[RolePermissions]'))
        SET IDENTITY_INSERT [RolePermissions] ON;
    EXEC(N'INSERT INTO [RolePermissions] ([Id], [CreatedAt], [PermissionId], [RoleId], [UpdatedAt])
    VALUES (''1900a968-b8f6-425d-89a3-09a344f32341'', ''2026-06-29T22:50:43.4720953Z'', ''b4c5d6e7-f8a9-0123-b456-789012345678'', ''a1b2c3d4-e5f6-7890-abcd-ef1234567890'', NULL),
    (''23681755-d602-4f32-8224-4a35999a165a'', ''2026-06-29T22:50:43.4720953Z'', ''b8c9d0e1-f2a3-4567-b890-123456789012'', ''b2c3d4e5-f6a7-8901-bcde-f12345678901'', NULL),
    (''23b4e224-9e35-4e8c-ae2a-20e550f9f839'', ''2026-06-29T22:50:43.4720953Z'', ''d0e1f2a3-b4c5-6789-d012-345678901234'', ''c3d4e5f6-a7b8-9012-cdef-123456789012'', NULL),
    (''2c2fa57d-3700-46ef-baf5-a83b9deb47ce'', ''2026-06-29T22:50:43.4720953Z'', ''f8a9b0c1-d2e3-4567-f890-123456789012'', ''a1b2c3d4-e5f6-7890-abcd-ef1234567890'', NULL),
    (''320fd0c2-d4fb-40de-b0d1-4b3ae4051ed6'', ''2026-06-29T22:50:43.4720953Z'', ''e5f6a7b8-c9d0-1234-ef56-789012345678'', ''c3d4e5f6-a7b8-9012-cdef-123456789012'', NULL),
    (''355745ae-d2cf-411b-9b8b-63dec21c8af2'', ''2026-06-29T22:50:43.4720953Z'', ''a7b8c9d0-e1f2-3456-a789-012345678901'', ''a1b2c3d4-e5f6-7890-abcd-ef1234567890'', NULL),
    (''41c5b2c9-2871-4cc4-af57-c5b2991598f4'', ''2026-06-29T22:50:43.4720953Z'', ''a3b4c5d6-e7f8-9012-a345-678901234567'', ''b2c3d4e5-f6a7-8901-bcde-f12345678901'', NULL),
    (''4a29139f-b306-47fb-8513-a2f1193ed0fd'', ''2026-06-29T22:50:43.4720953Z'', ''e1f2a3b4-c5d6-7890-e123-456789012345'', ''a1b2c3d4-e5f6-7890-abcd-ef1234567890'', NULL),
    (''4a366db1-b3f1-43ed-898f-2ea474eaf709'', ''2026-06-29T22:50:43.4720953Z'', ''b4c5d6e7-f8a9-0123-b456-789012345678'', ''c3d4e5f6-a7b8-9012-cdef-123456789012'', NULL),
    (''59c0d8cb-b976-487f-a098-1330f6e9082a'', ''2026-06-29T22:50:43.4720953Z'', ''a3b4c5d6-e7f8-9012-a345-678901234567'', ''a1b2c3d4-e5f6-7890-abcd-ef1234567890'', NULL),
    (''7487240c-b9bb-491a-bc54-69fad79a1573'', ''2026-06-29T22:50:43.4720953Z'', ''b8c9d0e1-f2a3-4567-b890-123456789012'', ''a1b2c3d4-e5f6-7890-abcd-ef1234567890'', NULL),
    (''77864273-21ab-4fc7-8a3d-ef3afc51f912'', ''2026-06-29T22:50:43.4720953Z'', ''e1f2a3b4-c5d6-7890-e123-456789012345'', ''b2c3d4e5-f6a7-8901-bcde-f12345678901'', NULL),
    (''780f2da0-31ef-4559-9ffe-1262ed47389d'', ''2026-06-29T22:50:43.4720953Z'', ''e5f6a7b8-c9d0-1234-ef56-789012345678'', ''a1b2c3d4-e5f6-7890-abcd-ef1234567890'', NULL),
    (''8d84b051-ef2a-416a-828c-68bbf7cf5b24'', ''2026-06-29T22:50:43.4720953Z'', ''d0e1f2a3-b4c5-6789-d012-345678901234'', ''a1b2c3d4-e5f6-7890-abcd-ef1234567890'', NULL),
    (''9dccbc0a-9798-4823-9626-88b35df580e6'', ''2026-06-29T22:50:43.4720953Z'', ''e7f8a9b0-c1d2-3456-e789-012345678901'', ''a1b2c3d4-e5f6-7890-abcd-ef1234567890'', NULL),
    (''a90c1bd8-05df-4c01-b0d2-014b3a11d8fd'', ''2026-06-29T22:50:43.4720953Z'', ''d0e1f2a3-b4c5-6789-d012-345678901234'', ''b2c3d4e5-f6a7-8901-bcde-f12345678901'', NULL),
    (''ac7822ca-3bbc-4669-9b6b-07e6f49d45fc'', ''2026-06-29T22:50:43.4720953Z'', ''f2a3b4c5-d6e7-8901-f234-567890123456'', ''a1b2c3d4-e5f6-7890-abcd-ef1234567890'', NULL),
    (''aed39055-9d59-4d6b-9811-6956e05bff29'', ''2026-06-29T22:50:43.4720953Z'', ''a9b0c1d2-e3f4-5678-a901-234567890123'', ''a1b2c3d4-e5f6-7890-abcd-ef1234567890'', NULL),
    (''b5f48274-e6d5-4d17-bd4b-aba6a020d9b4'', ''2026-06-29T22:50:43.4720953Z'', ''d6e7f8a9-b0c1-2345-d678-901234567890'', ''a1b2c3d4-e5f6-7890-abcd-ef1234567890'', NULL),
    (''c72c22d3-d59c-4d46-9a1e-d12267783235'', ''2026-06-29T22:50:43.4720953Z'', ''e5f6a7b8-c9d0-1234-ef56-789012345678'', ''b2c3d4e5-f6a7-8901-bcde-f12345678901'', NULL),
    (''ce9e7b1f-35c6-4c9e-b26e-c7e635ee0083'', ''2026-06-29T22:50:43.4720953Z'', ''c5d6e7f8-a9b0-1234-c567-890123456789'', ''a1b2c3d4-e5f6-7890-abcd-ef1234567890'', NULL),
    (''ef2c129f-b76a-4d20-8b4b-462f4deabbbd'', ''2026-06-29T22:50:43.4720953Z'', ''c9d0e1f2-a3b4-5678-c901-234567890123'', ''a1b2c3d4-e5f6-7890-abcd-ef1234567890'', NULL),
    (''f3d21642-78ec-4ee7-9b2c-4b9893a99ac0'', ''2026-06-29T22:50:43.4720953Z'', ''f2a3b4c5-d6e7-8901-f234-567890123456'', ''b2c3d4e5-f6a7-8901-bcde-f12345678901'', NULL),
    (''f847c126-7fc4-4a31-8198-a7c02bef0c7f'', ''2026-06-29T22:50:43.4720953Z'', ''f6a7b8c9-d0e1-2345-f678-901234567890'', ''a1b2c3d4-e5f6-7890-abcd-ef1234567890'', NULL)');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'CreatedAt', N'PermissionId', N'RoleId', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[RolePermissions]'))
        SET IDENTITY_INSERT [RolePermissions] OFF;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260629225044_AddNewMemberEntity'
)
BEGIN
    EXEC(N'UPDATE [Roles] SET [CreatedAt] = ''2026-06-29T22:50:43.1865414Z''
    WHERE [Id] = ''a1b2c3d4-e5f6-7890-abcd-ef1234567890'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260629225044_AddNewMemberEntity'
)
BEGIN
    EXEC(N'UPDATE [Roles] SET [CreatedAt] = ''2026-06-29T22:50:43.1866419Z''
    WHERE [Id] = ''b2c3d4e5-f6a7-8901-bcde-f12345678901'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260629225044_AddNewMemberEntity'
)
BEGIN
    EXEC(N'UPDATE [Roles] SET [CreatedAt] = ''2026-06-29T22:50:43.1866421Z''
    WHERE [Id] = ''c3d4e5f6-a7b8-9012-cdef-123456789012'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260629225044_AddNewMemberEntity'
)
BEGIN
    EXEC(N'UPDATE [Settings] SET [CreatedAt] = ''2026-06-29T22:50:43.4733349Z''
    WHERE [Id] = ''a5b6c7d8-e9f0-1234-a567-890123456789'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260629225044_AddNewMemberEntity'
)
BEGIN
    EXEC(N'UPDATE [Settings] SET [CreatedAt] = ''2026-06-29T22:50:43.4733349Z''
    WHERE [Id] = ''b0c1d2e3-f4a5-6789-b012-345678901234'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260629225044_AddNewMemberEntity'
)
BEGIN
    EXEC(N'UPDATE [Settings] SET [CreatedAt] = ''2026-06-29T22:50:43.4733349Z''
    WHERE [Id] = ''b6c7d8e9-f0a1-2345-b678-901234567890'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260629225044_AddNewMemberEntity'
)
BEGIN
    EXEC(N'UPDATE [Settings] SET [CreatedAt] = ''2026-06-29T22:50:43.4733349Z''
    WHERE [Id] = ''c1d2e3f4-a5b6-7890-c123-456789012345'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260629225044_AddNewMemberEntity'
)
BEGIN
    EXEC(N'UPDATE [Settings] SET [CreatedAt] = ''2026-06-29T22:50:43.4733349Z''
    WHERE [Id] = ''c7d8e9f0-a1b2-3456-c789-012345678901'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260629225044_AddNewMemberEntity'
)
BEGIN
    EXEC(N'UPDATE [Settings] SET [CreatedAt] = ''2026-06-29T22:50:43.4733349Z''
    WHERE [Id] = ''d2e3f4a5-b6c7-8901-d234-567890123456'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260629225044_AddNewMemberEntity'
)
BEGIN
    EXEC(N'UPDATE [Settings] SET [CreatedAt] = ''2026-06-29T22:50:43.4733349Z''
    WHERE [Id] = ''d8e9f0a1-b2c3-4567-d890-123456789012'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260629225044_AddNewMemberEntity'
)
BEGIN
    EXEC(N'UPDATE [Settings] SET [CreatedAt] = ''2026-06-29T22:50:43.4733349Z''
    WHERE [Id] = ''e3f4a5b6-c7d8-9012-e345-678901234567'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260629225044_AddNewMemberEntity'
)
BEGIN
    EXEC(N'UPDATE [Settings] SET [CreatedAt] = ''2026-06-29T22:50:43.4733349Z''
    WHERE [Id] = ''e9f0a1b2-c3d4-5678-e901-234567890123'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260629225044_AddNewMemberEntity'
)
BEGIN
    EXEC(N'UPDATE [Settings] SET [CreatedAt] = ''2026-06-29T22:50:43.4733349Z''
    WHERE [Id] = ''f4a5b6c7-d8e9-0123-f456-789012345678'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260629225044_AddNewMemberEntity'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''$2a$11$Skgb0mL5DIF3N88WtHaCpeyyeK2k7TRfKtr2uc4qF7l5QfvpuvJ6G''
    WHERE [Id] = ''d4e5f6a7-b8c9-0123-def4-567890123456'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260629225044_AddNewMemberEntity'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'CreatedAt', N'PermissionId', N'RoleId', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[RolePermissions]'))
        SET IDENTITY_INSERT [RolePermissions] ON;
    EXEC(N'INSERT INTO [RolePermissions] ([Id], [CreatedAt], [PermissionId], [RoleId], [UpdatedAt])
    VALUES (''cfd7c3f6-1bc8-4e62-97fd-b2e60c64aa3d'', ''2026-06-29T22:50:43.4720953Z'', ''d1e2f3a4-b5c6-7890-d123-456789012345'', ''a1b2c3d4-e5f6-7890-abcd-ef1234567890'', NULL)');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'CreatedAt', N'PermissionId', N'RoleId', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[RolePermissions]'))
        SET IDENTITY_INSERT [RolePermissions] OFF;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260629225044_AddNewMemberEntity'
)
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [IX_Members_NationalId] ON [Members] ([NationalId]) WHERE [NationalId] IS NOT NULL');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260629225044_AddNewMemberEntity'
)
BEGIN
    CREATE INDEX [IX_Members_PackageId] ON [Members] ([PackageId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260629225044_AddNewMemberEntity'
)
BEGIN
    ALTER TABLE [Members] ADD CONSTRAINT [FK_Members_MembershipPlans_PackageId] FOREIGN KEY ([PackageId]) REFERENCES [MembershipPlans] ([Id]) ON DELETE SET NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260629225044_AddNewMemberEntity'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260629225044_AddNewMemberEntity', N'10.0.9');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260629233223_AddEmailDobGenderNotes'
)
BEGIN
    EXEC(N'DELETE FROM [RolePermissions]
    WHERE [Id] = ''1900a968-b8f6-425d-89a3-09a344f32341'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260629233223_AddEmailDobGenderNotes'
)
BEGIN
    EXEC(N'DELETE FROM [RolePermissions]
    WHERE [Id] = ''23681755-d602-4f32-8224-4a35999a165a'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260629233223_AddEmailDobGenderNotes'
)
BEGIN
    EXEC(N'DELETE FROM [RolePermissions]
    WHERE [Id] = ''23b4e224-9e35-4e8c-ae2a-20e550f9f839'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260629233223_AddEmailDobGenderNotes'
)
BEGIN
    EXEC(N'DELETE FROM [RolePermissions]
    WHERE [Id] = ''2c2fa57d-3700-46ef-baf5-a83b9deb47ce'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260629233223_AddEmailDobGenderNotes'
)
BEGIN
    EXEC(N'DELETE FROM [RolePermissions]
    WHERE [Id] = ''320fd0c2-d4fb-40de-b0d1-4b3ae4051ed6'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260629233223_AddEmailDobGenderNotes'
)
BEGIN
    EXEC(N'DELETE FROM [RolePermissions]
    WHERE [Id] = ''355745ae-d2cf-411b-9b8b-63dec21c8af2'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260629233223_AddEmailDobGenderNotes'
)
BEGIN
    EXEC(N'DELETE FROM [RolePermissions]
    WHERE [Id] = ''41c5b2c9-2871-4cc4-af57-c5b2991598f4'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260629233223_AddEmailDobGenderNotes'
)
BEGIN
    EXEC(N'DELETE FROM [RolePermissions]
    WHERE [Id] = ''4a29139f-b306-47fb-8513-a2f1193ed0fd'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260629233223_AddEmailDobGenderNotes'
)
BEGIN
    EXEC(N'DELETE FROM [RolePermissions]
    WHERE [Id] = ''4a366db1-b3f1-43ed-898f-2ea474eaf709'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260629233223_AddEmailDobGenderNotes'
)
BEGIN
    EXEC(N'DELETE FROM [RolePermissions]
    WHERE [Id] = ''59c0d8cb-b976-487f-a098-1330f6e9082a'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260629233223_AddEmailDobGenderNotes'
)
BEGIN
    EXEC(N'DELETE FROM [RolePermissions]
    WHERE [Id] = ''7487240c-b9bb-491a-bc54-69fad79a1573'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260629233223_AddEmailDobGenderNotes'
)
BEGIN
    EXEC(N'DELETE FROM [RolePermissions]
    WHERE [Id] = ''77864273-21ab-4fc7-8a3d-ef3afc51f912'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260629233223_AddEmailDobGenderNotes'
)
BEGIN
    EXEC(N'DELETE FROM [RolePermissions]
    WHERE [Id] = ''780f2da0-31ef-4559-9ffe-1262ed47389d'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260629233223_AddEmailDobGenderNotes'
)
BEGIN
    EXEC(N'DELETE FROM [RolePermissions]
    WHERE [Id] = ''8d84b051-ef2a-416a-828c-68bbf7cf5b24'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260629233223_AddEmailDobGenderNotes'
)
BEGIN
    EXEC(N'DELETE FROM [RolePermissions]
    WHERE [Id] = ''9dccbc0a-9798-4823-9626-88b35df580e6'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260629233223_AddEmailDobGenderNotes'
)
BEGIN
    EXEC(N'DELETE FROM [RolePermissions]
    WHERE [Id] = ''a90c1bd8-05df-4c01-b0d2-014b3a11d8fd'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260629233223_AddEmailDobGenderNotes'
)
BEGIN
    EXEC(N'DELETE FROM [RolePermissions]
    WHERE [Id] = ''ac7822ca-3bbc-4669-9b6b-07e6f49d45fc'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260629233223_AddEmailDobGenderNotes'
)
BEGIN
    EXEC(N'DELETE FROM [RolePermissions]
    WHERE [Id] = ''aed39055-9d59-4d6b-9811-6956e05bff29'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260629233223_AddEmailDobGenderNotes'
)
BEGIN
    EXEC(N'DELETE FROM [RolePermissions]
    WHERE [Id] = ''b5f48274-e6d5-4d17-bd4b-aba6a020d9b4'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260629233223_AddEmailDobGenderNotes'
)
BEGIN
    EXEC(N'DELETE FROM [RolePermissions]
    WHERE [Id] = ''c72c22d3-d59c-4d46-9a1e-d12267783235'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260629233223_AddEmailDobGenderNotes'
)
BEGIN
    EXEC(N'DELETE FROM [RolePermissions]
    WHERE [Id] = ''ce9e7b1f-35c6-4c9e-b26e-c7e635ee0083'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260629233223_AddEmailDobGenderNotes'
)
BEGIN
    EXEC(N'DELETE FROM [RolePermissions]
    WHERE [Id] = ''cfd7c3f6-1bc8-4e62-97fd-b2e60c64aa3d'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260629233223_AddEmailDobGenderNotes'
)
BEGIN
    EXEC(N'DELETE FROM [RolePermissions]
    WHERE [Id] = ''ef2c129f-b76a-4d20-8b4b-462f4deabbbd'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260629233223_AddEmailDobGenderNotes'
)
BEGIN
    EXEC(N'DELETE FROM [RolePermissions]
    WHERE [Id] = ''f3d21642-78ec-4ee7-9b2c-4b9893a99ac0'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260629233223_AddEmailDobGenderNotes'
)
BEGIN
    EXEC(N'DELETE FROM [RolePermissions]
    WHERE [Id] = ''f847c126-7fc4-4a31-8198-a7c02bef0c7f'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260629233223_AddEmailDobGenderNotes'
)
BEGIN
    ALTER TABLE [Members] ADD [DateOfBirth] datetime2 NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260629233223_AddEmailDobGenderNotes'
)
BEGIN
    ALTER TABLE [Members] ADD [Email] nvarchar(200) NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260629233223_AddEmailDobGenderNotes'
)
BEGIN
    ALTER TABLE [Members] ADD [Gender] nvarchar(10) NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260629233223_AddEmailDobGenderNotes'
)
BEGIN
    ALTER TABLE [Members] ADD [Notes] nvarchar(2000) NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260629233223_AddEmailDobGenderNotes'
)
BEGIN
    EXEC(N'UPDATE [Permissions] SET [CreatedAt] = ''2026-06-29T23:32:21.9941610Z''
    WHERE [Id] = ''a3b4c5d6-e7f8-9012-a345-678901234567'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260629233223_AddEmailDobGenderNotes'
)
BEGIN
    EXEC(N'UPDATE [Permissions] SET [CreatedAt] = ''2026-06-29T23:32:21.9941580Z''
    WHERE [Id] = ''a7b8c9d0-e1f2-3456-a789-012345678901'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260629233223_AddEmailDobGenderNotes'
)
BEGIN
    EXEC(N'UPDATE [Permissions] SET [CreatedAt] = ''2026-06-29T23:32:21.9941632Z''
    WHERE [Id] = ''a9b0c1d2-e3f4-5678-a901-234567890123'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260629233223_AddEmailDobGenderNotes'
)
BEGIN
    EXEC(N'UPDATE [Permissions] SET [CreatedAt] = ''2026-06-29T23:32:21.9941611Z''
    WHERE [Id] = ''b4c5d6e7-f8a9-0123-b456-789012345678'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260629233223_AddEmailDobGenderNotes'
)
BEGIN
    EXEC(N'UPDATE [Permissions] SET [CreatedAt] = ''2026-06-29T23:32:21.9941587Z''
    WHERE [Id] = ''b8c9d0e1-f2a3-4567-b890-123456789012'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260629233223_AddEmailDobGenderNotes'
)
BEGIN
    EXEC(N'UPDATE [Permissions] SET [CreatedAt] = ''2026-06-29T23:32:21.9941618Z''
    WHERE [Id] = ''c5d6e7f8-a9b0-1234-c567-890123456789'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260629233223_AddEmailDobGenderNotes'
)
BEGIN
    EXEC(N'UPDATE [Permissions] SET [CreatedAt] = ''2026-06-29T23:32:21.9941592Z''
    WHERE [Id] = ''c9d0e1f2-a3b4-5678-c901-234567890123'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260629233223_AddEmailDobGenderNotes'
)
BEGIN
    EXEC(N'UPDATE [Permissions] SET [CreatedAt] = ''2026-06-29T23:32:21.9941599Z''
    WHERE [Id] = ''d0e1f2a3-b4c5-6789-d012-345678901234'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260629233223_AddEmailDobGenderNotes'
)
BEGIN
    EXEC(N'UPDATE [Permissions] SET [CreatedAt] = ''2026-06-29T23:32:21.9941644Z''
    WHERE [Id] = ''d1e2f3a4-b5c6-7890-d123-456789012345'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260629233223_AddEmailDobGenderNotes'
)
BEGIN
    EXEC(N'UPDATE [Permissions] SET [CreatedAt] = ''2026-06-29T23:32:21.9941620Z''
    WHERE [Id] = ''d6e7f8a9-b0c1-2345-d678-901234567890'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260629233223_AddEmailDobGenderNotes'
)
BEGIN
    EXEC(N'UPDATE [Permissions] SET [CreatedAt] = ''2026-06-29T23:32:21.9941601Z''
    WHERE [Id] = ''e1f2a3b4-c5d6-7890-e123-456789012345'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260629233223_AddEmailDobGenderNotes'
)
BEGIN
    EXEC(N'UPDATE [Permissions] SET [CreatedAt] = ''2026-06-29T23:32:21.9939826Z''
    WHERE [Id] = ''e5f6a7b8-c9d0-1234-ef56-789012345678'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260629233223_AddEmailDobGenderNotes'
)
BEGIN
    EXEC(N'UPDATE [Permissions] SET [CreatedAt] = ''2026-06-29T23:32:21.9941622Z''
    WHERE [Id] = ''e7f8a9b0-c1d2-3456-e789-012345678901'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260629233223_AddEmailDobGenderNotes'
)
BEGIN
    EXEC(N'UPDATE [Permissions] SET [CreatedAt] = ''2026-06-29T23:32:21.9941609Z''
    WHERE [Id] = ''f2a3b4c5-d6e7-8901-f234-567890123456'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260629233223_AddEmailDobGenderNotes'
)
BEGIN
    EXEC(N'UPDATE [Permissions] SET [CreatedAt] = ''2026-06-29T23:32:21.9941571Z''
    WHERE [Id] = ''f6a7b8c9-d0e1-2345-f678-901234567890'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260629233223_AddEmailDobGenderNotes'
)
BEGIN
    EXEC(N'UPDATE [Permissions] SET [CreatedAt] = ''2026-06-29T23:32:21.9941626Z''
    WHERE [Id] = ''f8a9b0c1-d2e3-4567-f890-123456789012'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260629233223_AddEmailDobGenderNotes'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'CreatedAt', N'PermissionId', N'RoleId', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[RolePermissions]'))
        SET IDENTITY_INSERT [RolePermissions] ON;
    EXEC(N'INSERT INTO [RolePermissions] ([Id], [CreatedAt], [PermissionId], [RoleId], [UpdatedAt])
    VALUES (''06b4e2a3-3e64-43c7-89e2-260601a839cf'', ''2026-06-29T23:32:21.9954574Z'', ''e1f2a3b4-c5d6-7890-e123-456789012345'', ''a1b2c3d4-e5f6-7890-abcd-ef1234567890'', NULL),
    (''107f6514-cd37-4fb8-b968-b03a4ac22252'', ''2026-06-29T23:32:21.9954574Z'', ''f8a9b0c1-d2e3-4567-f890-123456789012'', ''a1b2c3d4-e5f6-7890-abcd-ef1234567890'', NULL),
    (''14aca163-64ae-4262-b1d3-dec8f8cf22f9'', ''2026-06-29T23:32:21.9954574Z'', ''b4c5d6e7-f8a9-0123-b456-789012345678'', ''a1b2c3d4-e5f6-7890-abcd-ef1234567890'', NULL),
    (''201a833a-0392-4cfb-a183-bb194765246b'', ''2026-06-29T23:32:21.9954574Z'', ''b8c9d0e1-f2a3-4567-b890-123456789012'', ''a1b2c3d4-e5f6-7890-abcd-ef1234567890'', NULL),
    (''29a4c7a0-3ada-4bb3-ad8b-e1991249ffc1'', ''2026-06-29T23:32:21.9954574Z'', ''e5f6a7b8-c9d0-1234-ef56-789012345678'', ''b2c3d4e5-f6a7-8901-bcde-f12345678901'', NULL),
    (''32c7f66d-e01b-4fe0-8278-c7a1af30758a'', ''2026-06-29T23:32:21.9954574Z'', ''c5d6e7f8-a9b0-1234-c567-890123456789'', ''a1b2c3d4-e5f6-7890-abcd-ef1234567890'', NULL),
    (''42192226-ece7-4a83-94be-889b1e5790a2'', ''2026-06-29T23:32:21.9954574Z'', ''d0e1f2a3-b4c5-6789-d012-345678901234'', ''c3d4e5f6-a7b8-9012-cdef-123456789012'', NULL),
    (''6d0bf28b-d1ba-4cd3-8dbc-e624696bb6a0'', ''2026-06-29T23:32:21.9954574Z'', ''d0e1f2a3-b4c5-6789-d012-345678901234'', ''b2c3d4e5-f6a7-8901-bcde-f12345678901'', NULL),
    (''6eec2950-8e6b-40be-b8cf-efac072b2d95'', ''2026-06-29T23:32:21.9954574Z'', ''b8c9d0e1-f2a3-4567-b890-123456789012'', ''b2c3d4e5-f6a7-8901-bcde-f12345678901'', NULL),
    (''73478d27-963d-4d94-b1e1-0df19d6d4363'', ''2026-06-29T23:32:21.9954574Z'', ''b4c5d6e7-f8a9-0123-b456-789012345678'', ''c3d4e5f6-a7b8-9012-cdef-123456789012'', NULL),
    (''9420a690-1fd3-4cc2-abc5-7ef8194b7221'', ''2026-06-29T23:32:21.9954574Z'', ''e5f6a7b8-c9d0-1234-ef56-789012345678'', ''a1b2c3d4-e5f6-7890-abcd-ef1234567890'', NULL),
    (''b68512da-be45-4737-9d43-6adb35fddd08'', ''2026-06-29T23:32:21.9954574Z'', ''d6e7f8a9-b0c1-2345-d678-901234567890'', ''a1b2c3d4-e5f6-7890-abcd-ef1234567890'', NULL),
    (''b9b30068-bd6d-49a0-9b4c-3b05d626304e'', ''2026-06-29T23:32:21.9954574Z'', ''f6a7b8c9-d0e1-2345-f678-901234567890'', ''a1b2c3d4-e5f6-7890-abcd-ef1234567890'', NULL),
    (''c5bc220a-727f-4e20-b228-49d4257ef293'', ''2026-06-29T23:32:21.9954574Z'', ''a7b8c9d0-e1f2-3456-a789-012345678901'', ''a1b2c3d4-e5f6-7890-abcd-ef1234567890'', NULL),
    (''c7607ce7-ed6f-43d3-be84-a111800c729e'', ''2026-06-29T23:32:21.9954574Z'', ''a9b0c1d2-e3f4-5678-a901-234567890123'', ''a1b2c3d4-e5f6-7890-abcd-ef1234567890'', NULL),
    (''cacbaf21-ef69-4fdd-9f27-33d30db07844'', ''2026-06-29T23:32:21.9954574Z'', ''d1e2f3a4-b5c6-7890-d123-456789012345'', ''a1b2c3d4-e5f6-7890-abcd-ef1234567890'', NULL),
    (''cc4fe0d6-7401-4ada-9983-87d14d0e7ca7'', ''2026-06-29T23:32:21.9954574Z'', ''f2a3b4c5-d6e7-8901-f234-567890123456'', ''a1b2c3d4-e5f6-7890-abcd-ef1234567890'', NULL),
    (''ce6b1312-f7eb-470e-831a-acab73c726c9'', ''2026-06-29T23:32:21.9954574Z'', ''a3b4c5d6-e7f8-9012-a345-678901234567'', ''a1b2c3d4-e5f6-7890-abcd-ef1234567890'', NULL),
    (''d4c5f256-3f32-4952-8b5b-24faef7eb9d2'', ''2026-06-29T23:32:21.9954574Z'', ''e5f6a7b8-c9d0-1234-ef56-789012345678'', ''c3d4e5f6-a7b8-9012-cdef-123456789012'', NULL),
    (''d53fe3d2-c395-44c4-bfad-4f70cdc62a25'', ''2026-06-29T23:32:21.9954574Z'', ''d0e1f2a3-b4c5-6789-d012-345678901234'', ''a1b2c3d4-e5f6-7890-abcd-ef1234567890'', NULL),
    (''e34bdae1-8312-4e6f-8adc-c9f9147e2932'', ''2026-06-29T23:32:21.9954574Z'', ''c9d0e1f2-a3b4-5678-c901-234567890123'', ''a1b2c3d4-e5f6-7890-abcd-ef1234567890'', NULL),
    (''e997dc01-ab9b-4ec8-90f0-0b18e36d7b03'', ''2026-06-29T23:32:21.9954574Z'', ''e7f8a9b0-c1d2-3456-e789-012345678901'', ''a1b2c3d4-e5f6-7890-abcd-ef1234567890'', NULL),
    (''f1cb26e3-44fd-4ee0-bd5f-90d3f2efef9b'', ''2026-06-29T23:32:21.9954574Z'', ''a3b4c5d6-e7f8-9012-a345-678901234567'', ''b2c3d4e5-f6a7-8901-bcde-f12345678901'', NULL),
    (''f35e202a-ae28-451d-8b52-aa2641e79bda'', ''2026-06-29T23:32:21.9954574Z'', ''e1f2a3b4-c5d6-7890-e123-456789012345'', ''b2c3d4e5-f6a7-8901-bcde-f12345678901'', NULL),
    (''fcc22e3f-56dd-41b8-b10f-c0ab7334ce3f'', ''2026-06-29T23:32:21.9954574Z'', ''f2a3b4c5-d6e7-8901-f234-567890123456'', ''b2c3d4e5-f6a7-8901-bcde-f12345678901'', NULL)');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'CreatedAt', N'PermissionId', N'RoleId', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[RolePermissions]'))
        SET IDENTITY_INSERT [RolePermissions] OFF;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260629233223_AddEmailDobGenderNotes'
)
BEGIN
    EXEC(N'UPDATE [Roles] SET [CreatedAt] = ''2026-06-29T23:32:21.5449533Z''
    WHERE [Id] = ''a1b2c3d4-e5f6-7890-abcd-ef1234567890'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260629233223_AddEmailDobGenderNotes'
)
BEGIN
    EXEC(N'UPDATE [Roles] SET [CreatedAt] = ''2026-06-29T23:32:21.5450596Z''
    WHERE [Id] = ''b2c3d4e5-f6a7-8901-bcde-f12345678901'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260629233223_AddEmailDobGenderNotes'
)
BEGIN
    EXEC(N'UPDATE [Roles] SET [CreatedAt] = ''2026-06-29T23:32:21.5450597Z''
    WHERE [Id] = ''c3d4e5f6-a7b8-9012-cdef-123456789012'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260629233223_AddEmailDobGenderNotes'
)
BEGIN
    EXEC(N'UPDATE [Settings] SET [CreatedAt] = ''2026-06-29T23:32:21.9965114Z''
    WHERE [Id] = ''a5b6c7d8-e9f0-1234-a567-890123456789'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260629233223_AddEmailDobGenderNotes'
)
BEGIN
    EXEC(N'UPDATE [Settings] SET [CreatedAt] = ''2026-06-29T23:32:21.9965114Z''
    WHERE [Id] = ''b0c1d2e3-f4a5-6789-b012-345678901234'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260629233223_AddEmailDobGenderNotes'
)
BEGIN
    EXEC(N'UPDATE [Settings] SET [CreatedAt] = ''2026-06-29T23:32:21.9965114Z''
    WHERE [Id] = ''b6c7d8e9-f0a1-2345-b678-901234567890'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260629233223_AddEmailDobGenderNotes'
)
BEGIN
    EXEC(N'UPDATE [Settings] SET [CreatedAt] = ''2026-06-29T23:32:21.9965114Z''
    WHERE [Id] = ''c1d2e3f4-a5b6-7890-c123-456789012345'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260629233223_AddEmailDobGenderNotes'
)
BEGIN
    EXEC(N'UPDATE [Settings] SET [CreatedAt] = ''2026-06-29T23:32:21.9965114Z''
    WHERE [Id] = ''c7d8e9f0-a1b2-3456-c789-012345678901'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260629233223_AddEmailDobGenderNotes'
)
BEGIN
    EXEC(N'UPDATE [Settings] SET [CreatedAt] = ''2026-06-29T23:32:21.9965114Z''
    WHERE [Id] = ''d2e3f4a5-b6c7-8901-d234-567890123456'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260629233223_AddEmailDobGenderNotes'
)
BEGIN
    EXEC(N'UPDATE [Settings] SET [CreatedAt] = ''2026-06-29T23:32:21.9965114Z''
    WHERE [Id] = ''d8e9f0a1-b2c3-4567-d890-123456789012'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260629233223_AddEmailDobGenderNotes'
)
BEGIN
    EXEC(N'UPDATE [Settings] SET [CreatedAt] = ''2026-06-29T23:32:21.9965114Z''
    WHERE [Id] = ''e3f4a5b6-c7d8-9012-e345-678901234567'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260629233223_AddEmailDobGenderNotes'
)
BEGIN
    EXEC(N'UPDATE [Settings] SET [CreatedAt] = ''2026-06-29T23:32:21.9965114Z''
    WHERE [Id] = ''e9f0a1b2-c3d4-5678-e901-234567890123'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260629233223_AddEmailDobGenderNotes'
)
BEGIN
    EXEC(N'UPDATE [Settings] SET [CreatedAt] = ''2026-06-29T23:32:21.9965114Z''
    WHERE [Id] = ''f4a5b6c7-d8e9-0123-f456-789012345678'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260629233223_AddEmailDobGenderNotes'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''$2a$11$UC/OIv18WQtlCO8Jnpb4K.u/RgKMqULuegoMYz9pyZo80IYY0rqly''
    WHERE [Id] = ''d4e5f6a7-b8c9-0123-def4-567890123456'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260629233223_AddEmailDobGenderNotes'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260629233223_AddEmailDobGenderNotes', N'10.0.9');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260630002214_CreatePackagesTable'
)
BEGIN
    EXEC(N'DELETE FROM [RolePermissions]
    WHERE [Id] = ''06b4e2a3-3e64-43c7-89e2-260601a839cf'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260630002214_CreatePackagesTable'
)
BEGIN
    EXEC(N'DELETE FROM [RolePermissions]
    WHERE [Id] = ''107f6514-cd37-4fb8-b968-b03a4ac22252'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260630002214_CreatePackagesTable'
)
BEGIN
    EXEC(N'DELETE FROM [RolePermissions]
    WHERE [Id] = ''14aca163-64ae-4262-b1d3-dec8f8cf22f9'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260630002214_CreatePackagesTable'
)
BEGIN
    EXEC(N'DELETE FROM [RolePermissions]
    WHERE [Id] = ''201a833a-0392-4cfb-a183-bb194765246b'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260630002214_CreatePackagesTable'
)
BEGIN
    EXEC(N'DELETE FROM [RolePermissions]
    WHERE [Id] = ''29a4c7a0-3ada-4bb3-ad8b-e1991249ffc1'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260630002214_CreatePackagesTable'
)
BEGIN
    EXEC(N'DELETE FROM [RolePermissions]
    WHERE [Id] = ''32c7f66d-e01b-4fe0-8278-c7a1af30758a'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260630002214_CreatePackagesTable'
)
BEGIN
    EXEC(N'DELETE FROM [RolePermissions]
    WHERE [Id] = ''42192226-ece7-4a83-94be-889b1e5790a2'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260630002214_CreatePackagesTable'
)
BEGIN
    EXEC(N'DELETE FROM [RolePermissions]
    WHERE [Id] = ''6d0bf28b-d1ba-4cd3-8dbc-e624696bb6a0'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260630002214_CreatePackagesTable'
)
BEGIN
    EXEC(N'DELETE FROM [RolePermissions]
    WHERE [Id] = ''6eec2950-8e6b-40be-b8cf-efac072b2d95'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260630002214_CreatePackagesTable'
)
BEGIN
    EXEC(N'DELETE FROM [RolePermissions]
    WHERE [Id] = ''73478d27-963d-4d94-b1e1-0df19d6d4363'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260630002214_CreatePackagesTable'
)
BEGIN
    EXEC(N'DELETE FROM [RolePermissions]
    WHERE [Id] = ''9420a690-1fd3-4cc2-abc5-7ef8194b7221'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260630002214_CreatePackagesTable'
)
BEGIN
    EXEC(N'DELETE FROM [RolePermissions]
    WHERE [Id] = ''b68512da-be45-4737-9d43-6adb35fddd08'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260630002214_CreatePackagesTable'
)
BEGIN
    EXEC(N'DELETE FROM [RolePermissions]
    WHERE [Id] = ''b9b30068-bd6d-49a0-9b4c-3b05d626304e'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260630002214_CreatePackagesTable'
)
BEGIN
    EXEC(N'DELETE FROM [RolePermissions]
    WHERE [Id] = ''c5bc220a-727f-4e20-b228-49d4257ef293'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260630002214_CreatePackagesTable'
)
BEGIN
    EXEC(N'DELETE FROM [RolePermissions]
    WHERE [Id] = ''c7607ce7-ed6f-43d3-be84-a111800c729e'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260630002214_CreatePackagesTable'
)
BEGIN
    EXEC(N'DELETE FROM [RolePermissions]
    WHERE [Id] = ''cacbaf21-ef69-4fdd-9f27-33d30db07844'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260630002214_CreatePackagesTable'
)
BEGIN
    EXEC(N'DELETE FROM [RolePermissions]
    WHERE [Id] = ''cc4fe0d6-7401-4ada-9983-87d14d0e7ca7'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260630002214_CreatePackagesTable'
)
BEGIN
    EXEC(N'DELETE FROM [RolePermissions]
    WHERE [Id] = ''ce6b1312-f7eb-470e-831a-acab73c726c9'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260630002214_CreatePackagesTable'
)
BEGIN
    EXEC(N'DELETE FROM [RolePermissions]
    WHERE [Id] = ''d4c5f256-3f32-4952-8b5b-24faef7eb9d2'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260630002214_CreatePackagesTable'
)
BEGIN
    EXEC(N'DELETE FROM [RolePermissions]
    WHERE [Id] = ''d53fe3d2-c395-44c4-bfad-4f70cdc62a25'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260630002214_CreatePackagesTable'
)
BEGIN
    EXEC(N'DELETE FROM [RolePermissions]
    WHERE [Id] = ''e34bdae1-8312-4e6f-8adc-c9f9147e2932'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260630002214_CreatePackagesTable'
)
BEGIN
    EXEC(N'DELETE FROM [RolePermissions]
    WHERE [Id] = ''e997dc01-ab9b-4ec8-90f0-0b18e36d7b03'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260630002214_CreatePackagesTable'
)
BEGIN
    EXEC(N'DELETE FROM [RolePermissions]
    WHERE [Id] = ''f1cb26e3-44fd-4ee0-bd5f-90d3f2efef9b'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260630002214_CreatePackagesTable'
)
BEGIN
    EXEC(N'DELETE FROM [RolePermissions]
    WHERE [Id] = ''f35e202a-ae28-451d-8b52-aa2641e79bda'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260630002214_CreatePackagesTable'
)
BEGIN
    EXEC(N'DELETE FROM [RolePermissions]
    WHERE [Id] = ''fcc22e3f-56dd-41b8-b10f-c0ab7334ce3f'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260630002214_CreatePackagesTable'
)
BEGIN
    CREATE TABLE [Packages] (
        [Id] uniqueidentifier NOT NULL,
        [PackageName] nvarchar(200) NOT NULL,
        [DurationMonths] int NOT NULL,
        [Price] decimal(18,2) NOT NULL,
        [FreePeriodMonths] int NULL,
        [MaxFreezeDays] int NULL,
        [IsActive] bit NOT NULL DEFAULT CAST(1 AS bit),
        [CreatedAt] datetime2 NOT NULL,
        [UpdatedAt] datetime2 NULL,
        CONSTRAINT [PK_Packages] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260630002214_CreatePackagesTable'
)
BEGIN
    EXEC(N'UPDATE [Permissions] SET [CreatedAt] = ''2026-06-30T00:22:13.6002192Z''
    WHERE [Id] = ''a3b4c5d6-e7f8-9012-a345-678901234567'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260630002214_CreatePackagesTable'
)
BEGIN
    EXEC(N'UPDATE [Permissions] SET [CreatedAt] = ''2026-06-30T00:22:13.6002185Z''
    WHERE [Id] = ''a7b8c9d0-e1f2-3456-a789-012345678901'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260630002214_CreatePackagesTable'
)
BEGIN
    EXEC(N'UPDATE [Permissions] SET [CreatedAt] = ''2026-06-30T00:22:13.6002221Z''
    WHERE [Id] = ''a9b0c1d2-e3f4-5678-a901-234567890123'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260630002214_CreatePackagesTable'
)
BEGIN
    EXEC(N'UPDATE [Permissions] SET [CreatedAt] = ''2026-06-30T00:22:13.6002193Z''
    WHERE [Id] = ''b4c5d6e7-f8a9-0123-b456-789012345678'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260630002214_CreatePackagesTable'
)
BEGIN
    EXEC(N'UPDATE [Permissions] SET [CreatedAt] = ''2026-06-30T00:22:13.6002186Z''
    WHERE [Id] = ''b8c9d0e1-f2a3-4567-b890-123456789012'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260630002214_CreatePackagesTable'
)
BEGIN
    EXEC(N'UPDATE [Permissions] SET [CreatedAt] = ''2026-06-30T00:22:13.6002194Z''
    WHERE [Id] = ''c5d6e7f8-a9b0-1234-c567-890123456789'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260630002214_CreatePackagesTable'
)
BEGIN
    EXEC(N'UPDATE [Permissions] SET [CreatedAt] = ''2026-06-30T00:22:13.6002188Z''
    WHERE [Id] = ''c9d0e1f2-a3b4-5678-c901-234567890123'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260630002214_CreatePackagesTable'
)
BEGIN
    EXEC(N'UPDATE [Permissions] SET [CreatedAt] = ''2026-06-30T00:22:13.6002189Z''
    WHERE [Id] = ''d0e1f2a3-b4c5-6789-d012-345678901234'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260630002214_CreatePackagesTable'
)
BEGIN
    EXEC(N'UPDATE [Permissions] SET [CreatedAt] = ''2026-06-30T00:22:13.6002222Z''
    WHERE [Id] = ''d1e2f3a4-b5c6-7890-d123-456789012345'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260630002214_CreatePackagesTable'
)
BEGIN
    EXEC(N'UPDATE [Permissions] SET [CreatedAt] = ''2026-06-30T00:22:13.6002195Z''
    WHERE [Id] = ''d6e7f8a9-b0c1-2345-d678-901234567890'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260630002214_CreatePackagesTable'
)
BEGIN
    EXEC(N'UPDATE [Permissions] SET [CreatedAt] = ''2026-06-30T00:22:13.6002190Z''
    WHERE [Id] = ''e1f2a3b4-c5d6-7890-e123-456789012345'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260630002214_CreatePackagesTable'
)
BEGIN
    EXEC(N'UPDATE [Permissions] SET [CreatedAt] = ''2026-06-30T00:22:13.6001177Z''
    WHERE [Id] = ''e5f6a7b8-c9d0-1234-ef56-789012345678'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260630002214_CreatePackagesTable'
)
BEGIN
    EXEC(N'UPDATE [Permissions] SET [CreatedAt] = ''2026-06-30T00:22:13.6002196Z''
    WHERE [Id] = ''e7f8a9b0-c1d2-3456-e789-012345678901'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260630002214_CreatePackagesTable'
)
BEGIN
    EXEC(N'UPDATE [Permissions] SET [CreatedAt] = ''2026-06-30T00:22:13.6002191Z''
    WHERE [Id] = ''f2a3b4c5-d6e7-8901-f234-567890123456'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260630002214_CreatePackagesTable'
)
BEGIN
    EXEC(N'UPDATE [Permissions] SET [CreatedAt] = ''2026-06-30T00:22:13.6002182Z''
    WHERE [Id] = ''f6a7b8c9-d0e1-2345-f678-901234567890'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260630002214_CreatePackagesTable'
)
BEGIN
    EXEC(N'UPDATE [Permissions] SET [CreatedAt] = ''2026-06-30T00:22:13.6002220Z''
    WHERE [Id] = ''f8a9b0c1-d2e3-4567-f890-123456789012'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260630002214_CreatePackagesTable'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'CreatedAt', N'PermissionId', N'RoleId', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[RolePermissions]'))
        SET IDENTITY_INSERT [RolePermissions] ON;
    EXEC(N'INSERT INTO [RolePermissions] ([Id], [CreatedAt], [PermissionId], [RoleId], [UpdatedAt])
    VALUES (''04e8f242-b73d-45d3-9e78-24b5f992017c'', ''2026-06-30T00:22:13.6010010Z'', ''b8c9d0e1-f2a3-4567-b890-123456789012'', ''b2c3d4e5-f6a7-8901-bcde-f12345678901'', NULL),
    (''10020613-5acb-4350-b7b0-c377d90e67a1'', ''2026-06-30T00:22:13.6010010Z'', ''f2a3b4c5-d6e7-8901-f234-567890123456'', ''a1b2c3d4-e5f6-7890-abcd-ef1234567890'', NULL),
    (''10fec474-03c8-416c-893f-1b9e2fb57135'', ''2026-06-30T00:22:13.6010010Z'', ''e7f8a9b0-c1d2-3456-e789-012345678901'', ''a1b2c3d4-e5f6-7890-abcd-ef1234567890'', NULL),
    (''19b5c70f-18ca-423f-8fe0-dc42d79f980b'', ''2026-06-30T00:22:13.6010010Z'', ''a3b4c5d6-e7f8-9012-a345-678901234567'', ''a1b2c3d4-e5f6-7890-abcd-ef1234567890'', NULL),
    (''2a635dc7-9f45-4eea-ae45-4096a84085b4'', ''2026-06-30T00:22:13.6010010Z'', ''e1f2a3b4-c5d6-7890-e123-456789012345'', ''a1b2c3d4-e5f6-7890-abcd-ef1234567890'', NULL),
    (''2f231b37-db29-457a-9a2b-96f9f8822138'', ''2026-06-30T00:22:13.6010010Z'', ''a3b4c5d6-e7f8-9012-a345-678901234567'', ''b2c3d4e5-f6a7-8901-bcde-f12345678901'', NULL),
    (''41638a05-8850-4d60-a5a5-12939b8ef955'', ''2026-06-30T00:22:13.6010010Z'', ''c5d6e7f8-a9b0-1234-c567-890123456789'', ''a1b2c3d4-e5f6-7890-abcd-ef1234567890'', NULL),
    (''4b75b558-cf77-43a6-ae12-9238b8417e18'', ''2026-06-30T00:22:13.6010010Z'', ''b4c5d6e7-f8a9-0123-b456-789012345678'', ''a1b2c3d4-e5f6-7890-abcd-ef1234567890'', NULL),
    (''4fdb6a94-c071-46a2-8f17-c4dbce3e302b'', ''2026-06-30T00:22:13.6010010Z'', ''f6a7b8c9-d0e1-2345-f678-901234567890'', ''a1b2c3d4-e5f6-7890-abcd-ef1234567890'', NULL),
    (''50ae5e39-dadf-459b-aa62-9729af97233e'', ''2026-06-30T00:22:13.6010010Z'', ''b8c9d0e1-f2a3-4567-b890-123456789012'', ''a1b2c3d4-e5f6-7890-abcd-ef1234567890'', NULL),
    (''637da492-5705-4f90-91b6-e672baad10b7'', ''2026-06-30T00:22:13.6010010Z'', ''f8a9b0c1-d2e3-4567-f890-123456789012'', ''a1b2c3d4-e5f6-7890-abcd-ef1234567890'', NULL),
    (''6549e7e0-65dd-4679-81b1-c98280701037'', ''2026-06-30T00:22:13.6010010Z'', ''e5f6a7b8-c9d0-1234-ef56-789012345678'', ''c3d4e5f6-a7b8-9012-cdef-123456789012'', NULL),
    (''830182ee-abd2-4b42-a9f5-5c8d496e6174'', ''2026-06-30T00:22:13.6010010Z'', ''b4c5d6e7-f8a9-0123-b456-789012345678'', ''c3d4e5f6-a7b8-9012-cdef-123456789012'', NULL),
    (''8873697e-47d2-4f37-8783-82d85b84e991'', ''2026-06-30T00:22:13.6010010Z'', ''c9d0e1f2-a3b4-5678-c901-234567890123'', ''a1b2c3d4-e5f6-7890-abcd-ef1234567890'', NULL),
    (''8e7a9823-e8fe-4986-91d9-5496381d8bcc'', ''2026-06-30T00:22:13.6010010Z'', ''e5f6a7b8-c9d0-1234-ef56-789012345678'', ''a1b2c3d4-e5f6-7890-abcd-ef1234567890'', NULL),
    (''97a57c51-ca8f-4944-914f-9f875f881ee1'', ''2026-06-30T00:22:13.6010010Z'', ''f2a3b4c5-d6e7-8901-f234-567890123456'', ''b2c3d4e5-f6a7-8901-bcde-f12345678901'', NULL),
    (''ab78c11f-f655-4898-ba30-27c927c25440'', ''2026-06-30T00:22:13.6010010Z'', ''a9b0c1d2-e3f4-5678-a901-234567890123'', ''a1b2c3d4-e5f6-7890-abcd-ef1234567890'', NULL),
    (''acf601f3-1da5-423b-8ff0-85d5f43ea386'', ''2026-06-30T00:22:13.6010010Z'', ''d0e1f2a3-b4c5-6789-d012-345678901234'', ''b2c3d4e5-f6a7-8901-bcde-f12345678901'', NULL),
    (''b00855ba-6df2-4689-9c59-9d899e43b72a'', ''2026-06-30T00:22:13.6010010Z'', ''d0e1f2a3-b4c5-6789-d012-345678901234'', ''c3d4e5f6-a7b8-9012-cdef-123456789012'', NULL),
    (''d320fed0-559b-47cd-9eb8-88bc410b1ae2'', ''2026-06-30T00:22:13.6010010Z'', ''a7b8c9d0-e1f2-3456-a789-012345678901'', ''a1b2c3d4-e5f6-7890-abcd-ef1234567890'', NULL),
    (''d6033d44-5645-42cb-90a3-8639e470ec17'', ''2026-06-30T00:22:13.6010010Z'', ''e5f6a7b8-c9d0-1234-ef56-789012345678'', ''b2c3d4e5-f6a7-8901-bcde-f12345678901'', NULL),
    (''d8e14f1c-fc46-47b7-82fd-d4956c0d7e75'', ''2026-06-30T00:22:13.6010010Z'', ''d6e7f8a9-b0c1-2345-d678-901234567890'', ''a1b2c3d4-e5f6-7890-abcd-ef1234567890'', NULL),
    (''e2314de8-dafd-4fa9-96cb-89358a55723b'', ''2026-06-30T00:22:13.6010010Z'', ''d0e1f2a3-b4c5-6789-d012-345678901234'', ''a1b2c3d4-e5f6-7890-abcd-ef1234567890'', NULL),
    (''f980d7e9-1062-40a3-bb0c-14610f7bc282'', ''2026-06-30T00:22:13.6010010Z'', ''d1e2f3a4-b5c6-7890-d123-456789012345'', ''a1b2c3d4-e5f6-7890-abcd-ef1234567890'', NULL),
    (''fe9b534c-25b9-4f47-ad2f-06d103554579'', ''2026-06-30T00:22:13.6010010Z'', ''e1f2a3b4-c5d6-7890-e123-456789012345'', ''b2c3d4e5-f6a7-8901-bcde-f12345678901'', NULL)');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'CreatedAt', N'PermissionId', N'RoleId', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[RolePermissions]'))
        SET IDENTITY_INSERT [RolePermissions] OFF;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260630002214_CreatePackagesTable'
)
BEGIN
    EXEC(N'UPDATE [Roles] SET [CreatedAt] = ''2026-06-30T00:22:13.1901234Z''
    WHERE [Id] = ''a1b2c3d4-e5f6-7890-abcd-ef1234567890'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260630002214_CreatePackagesTable'
)
BEGIN
    EXEC(N'UPDATE [Roles] SET [CreatedAt] = ''2026-06-30T00:22:13.1902334Z''
    WHERE [Id] = ''b2c3d4e5-f6a7-8901-bcde-f12345678901'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260630002214_CreatePackagesTable'
)
BEGIN
    EXEC(N'UPDATE [Roles] SET [CreatedAt] = ''2026-06-30T00:22:13.1902337Z''
    WHERE [Id] = ''c3d4e5f6-a7b8-9012-cdef-123456789012'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260630002214_CreatePackagesTable'
)
BEGIN
    EXEC(N'UPDATE [Settings] SET [CreatedAt] = ''2026-06-30T00:22:13.6017501Z''
    WHERE [Id] = ''a5b6c7d8-e9f0-1234-a567-890123456789'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260630002214_CreatePackagesTable'
)
BEGIN
    EXEC(N'UPDATE [Settings] SET [CreatedAt] = ''2026-06-30T00:22:13.6017501Z''
    WHERE [Id] = ''b0c1d2e3-f4a5-6789-b012-345678901234'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260630002214_CreatePackagesTable'
)
BEGIN
    EXEC(N'UPDATE [Settings] SET [CreatedAt] = ''2026-06-30T00:22:13.6017501Z''
    WHERE [Id] = ''b6c7d8e9-f0a1-2345-b678-901234567890'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260630002214_CreatePackagesTable'
)
BEGIN
    EXEC(N'UPDATE [Settings] SET [CreatedAt] = ''2026-06-30T00:22:13.6017501Z''
    WHERE [Id] = ''c1d2e3f4-a5b6-7890-c123-456789012345'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260630002214_CreatePackagesTable'
)
BEGIN
    EXEC(N'UPDATE [Settings] SET [CreatedAt] = ''2026-06-30T00:22:13.6017501Z''
    WHERE [Id] = ''c7d8e9f0-a1b2-3456-c789-012345678901'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260630002214_CreatePackagesTable'
)
BEGIN
    EXEC(N'UPDATE [Settings] SET [CreatedAt] = ''2026-06-30T00:22:13.6017501Z''
    WHERE [Id] = ''d2e3f4a5-b6c7-8901-d234-567890123456'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260630002214_CreatePackagesTable'
)
BEGIN
    EXEC(N'UPDATE [Settings] SET [CreatedAt] = ''2026-06-30T00:22:13.6017501Z''
    WHERE [Id] = ''d8e9f0a1-b2c3-4567-d890-123456789012'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260630002214_CreatePackagesTable'
)
BEGIN
    EXEC(N'UPDATE [Settings] SET [CreatedAt] = ''2026-06-30T00:22:13.6017501Z''
    WHERE [Id] = ''e3f4a5b6-c7d8-9012-e345-678901234567'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260630002214_CreatePackagesTable'
)
BEGIN
    EXEC(N'UPDATE [Settings] SET [CreatedAt] = ''2026-06-30T00:22:13.6017501Z''
    WHERE [Id] = ''e9f0a1b2-c3d4-5678-e901-234567890123'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260630002214_CreatePackagesTable'
)
BEGIN
    EXEC(N'UPDATE [Settings] SET [CreatedAt] = ''2026-06-30T00:22:13.6017501Z''
    WHERE [Id] = ''f4a5b6c7-d8e9-0123-f456-789012345678'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260630002214_CreatePackagesTable'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''$2a$11$rCj35iuQWzh8Xw98GbFx5eW7F9XNxE.IK2fN7Mrh.KoPbsK91ePMS''
    WHERE [Id] = ''d4e5f6a7-b8c9-0123-def4-567890123456'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260630002214_CreatePackagesTable'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260630002214_CreatePackagesTable', N'10.0.9');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260630013005_UpdateNationalityNationalIdToRequired'
)
BEGIN
    DROP INDEX [IX_Members_NationalId] ON [Members];
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260630013005_UpdateNationalityNationalIdToRequired'
)
BEGIN
    EXEC(N'DELETE FROM [RolePermissions]
    WHERE [Id] = ''04e8f242-b73d-45d3-9e78-24b5f992017c'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260630013005_UpdateNationalityNationalIdToRequired'
)
BEGIN
    EXEC(N'DELETE FROM [RolePermissions]
    WHERE [Id] = ''10020613-5acb-4350-b7b0-c377d90e67a1'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260630013005_UpdateNationalityNationalIdToRequired'
)
BEGIN
    EXEC(N'DELETE FROM [RolePermissions]
    WHERE [Id] = ''10fec474-03c8-416c-893f-1b9e2fb57135'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260630013005_UpdateNationalityNationalIdToRequired'
)
BEGIN
    EXEC(N'DELETE FROM [RolePermissions]
    WHERE [Id] = ''19b5c70f-18ca-423f-8fe0-dc42d79f980b'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260630013005_UpdateNationalityNationalIdToRequired'
)
BEGIN
    EXEC(N'DELETE FROM [RolePermissions]
    WHERE [Id] = ''2a635dc7-9f45-4eea-ae45-4096a84085b4'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260630013005_UpdateNationalityNationalIdToRequired'
)
BEGIN
    EXEC(N'DELETE FROM [RolePermissions]
    WHERE [Id] = ''2f231b37-db29-457a-9a2b-96f9f8822138'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260630013005_UpdateNationalityNationalIdToRequired'
)
BEGIN
    EXEC(N'DELETE FROM [RolePermissions]
    WHERE [Id] = ''41638a05-8850-4d60-a5a5-12939b8ef955'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260630013005_UpdateNationalityNationalIdToRequired'
)
BEGIN
    EXEC(N'DELETE FROM [RolePermissions]
    WHERE [Id] = ''4b75b558-cf77-43a6-ae12-9238b8417e18'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260630013005_UpdateNationalityNationalIdToRequired'
)
BEGIN
    EXEC(N'DELETE FROM [RolePermissions]
    WHERE [Id] = ''4fdb6a94-c071-46a2-8f17-c4dbce3e302b'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260630013005_UpdateNationalityNationalIdToRequired'
)
BEGIN
    EXEC(N'DELETE FROM [RolePermissions]
    WHERE [Id] = ''50ae5e39-dadf-459b-aa62-9729af97233e'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260630013005_UpdateNationalityNationalIdToRequired'
)
BEGIN
    EXEC(N'DELETE FROM [RolePermissions]
    WHERE [Id] = ''637da492-5705-4f90-91b6-e672baad10b7'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260630013005_UpdateNationalityNationalIdToRequired'
)
BEGIN
    EXEC(N'DELETE FROM [RolePermissions]
    WHERE [Id] = ''6549e7e0-65dd-4679-81b1-c98280701037'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260630013005_UpdateNationalityNationalIdToRequired'
)
BEGIN
    EXEC(N'DELETE FROM [RolePermissions]
    WHERE [Id] = ''830182ee-abd2-4b42-a9f5-5c8d496e6174'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260630013005_UpdateNationalityNationalIdToRequired'
)
BEGIN
    EXEC(N'DELETE FROM [RolePermissions]
    WHERE [Id] = ''8873697e-47d2-4f37-8783-82d85b84e991'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260630013005_UpdateNationalityNationalIdToRequired'
)
BEGIN
    EXEC(N'DELETE FROM [RolePermissions]
    WHERE [Id] = ''8e7a9823-e8fe-4986-91d9-5496381d8bcc'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260630013005_UpdateNationalityNationalIdToRequired'
)
BEGIN
    EXEC(N'DELETE FROM [RolePermissions]
    WHERE [Id] = ''97a57c51-ca8f-4944-914f-9f875f881ee1'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260630013005_UpdateNationalityNationalIdToRequired'
)
BEGIN
    EXEC(N'DELETE FROM [RolePermissions]
    WHERE [Id] = ''ab78c11f-f655-4898-ba30-27c927c25440'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260630013005_UpdateNationalityNationalIdToRequired'
)
BEGIN
    EXEC(N'DELETE FROM [RolePermissions]
    WHERE [Id] = ''acf601f3-1da5-423b-8ff0-85d5f43ea386'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260630013005_UpdateNationalityNationalIdToRequired'
)
BEGIN
    EXEC(N'DELETE FROM [RolePermissions]
    WHERE [Id] = ''b00855ba-6df2-4689-9c59-9d899e43b72a'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260630013005_UpdateNationalityNationalIdToRequired'
)
BEGIN
    EXEC(N'DELETE FROM [RolePermissions]
    WHERE [Id] = ''d320fed0-559b-47cd-9eb8-88bc410b1ae2'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260630013005_UpdateNationalityNationalIdToRequired'
)
BEGIN
    EXEC(N'DELETE FROM [RolePermissions]
    WHERE [Id] = ''d6033d44-5645-42cb-90a3-8639e470ec17'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260630013005_UpdateNationalityNationalIdToRequired'
)
BEGIN
    EXEC(N'DELETE FROM [RolePermissions]
    WHERE [Id] = ''d8e14f1c-fc46-47b7-82fd-d4956c0d7e75'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260630013005_UpdateNationalityNationalIdToRequired'
)
BEGIN
    EXEC(N'DELETE FROM [RolePermissions]
    WHERE [Id] = ''e2314de8-dafd-4fa9-96cb-89358a55723b'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260630013005_UpdateNationalityNationalIdToRequired'
)
BEGIN
    EXEC(N'DELETE FROM [RolePermissions]
    WHERE [Id] = ''f980d7e9-1062-40a3-bb0c-14610f7bc282'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260630013005_UpdateNationalityNationalIdToRequired'
)
BEGIN
    EXEC(N'DELETE FROM [RolePermissions]
    WHERE [Id] = ''fe9b534c-25b9-4f47-ad2f-06d103554579'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260630013005_UpdateNationalityNationalIdToRequired'
)
BEGIN
    DECLARE @var8 nvarchar(max);
    SELECT @var8 = QUOTENAME([d].[name])
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Members]') AND [c].[name] = N'Nationality');
    IF @var8 IS NOT NULL EXEC(N'ALTER TABLE [Members] DROP CONSTRAINT ' + @var8 + ';');
    EXEC(N'UPDATE [Members] SET [Nationality] = N'''' WHERE [Nationality] IS NULL');
    ALTER TABLE [Members] ALTER COLUMN [Nationality] nvarchar(100) NOT NULL;
    ALTER TABLE [Members] ADD DEFAULT N'' FOR [Nationality];
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260630013005_UpdateNationalityNationalIdToRequired'
)
BEGIN
    DECLARE @var9 nvarchar(max);
    SELECT @var9 = QUOTENAME([d].[name])
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Members]') AND [c].[name] = N'NationalId');
    IF @var9 IS NOT NULL EXEC(N'ALTER TABLE [Members] DROP CONSTRAINT ' + @var9 + ';');
    EXEC(N'UPDATE [Members] SET [NationalId] = N'''' WHERE [NationalId] IS NULL');
    ALTER TABLE [Members] ALTER COLUMN [NationalId] nvarchar(14) NOT NULL;
    ALTER TABLE [Members] ADD DEFAULT N'' FOR [NationalId];
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260630013005_UpdateNationalityNationalIdToRequired'
)
BEGIN
    EXEC(N'UPDATE [Permissions] SET [CreatedAt] = ''2026-06-30T01:30:04.7204027Z''
    WHERE [Id] = ''a3b4c5d6-e7f8-9012-a345-678901234567'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260630013005_UpdateNationalityNationalIdToRequired'
)
BEGIN
    EXEC(N'UPDATE [Permissions] SET [CreatedAt] = ''2026-06-30T01:30:04.7204022Z''
    WHERE [Id] = ''a7b8c9d0-e1f2-3456-a789-012345678901'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260630013005_UpdateNationalityNationalIdToRequired'
)
BEGIN
    EXEC(N'UPDATE [Permissions] SET [CreatedAt] = ''2026-06-30T01:30:04.7204046Z''
    WHERE [Id] = ''a9b0c1d2-e3f4-5678-a901-234567890123'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260630013005_UpdateNationalityNationalIdToRequired'
)
BEGIN
    EXEC(N'UPDATE [Permissions] SET [CreatedAt] = ''2026-06-30T01:30:04.7204028Z''
    WHERE [Id] = ''b4c5d6e7-f8a9-0123-b456-789012345678'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260630013005_UpdateNationalityNationalIdToRequired'
)
BEGIN
    EXEC(N'UPDATE [Permissions] SET [CreatedAt] = ''2026-06-30T01:30:04.7204023Z''
    WHERE [Id] = ''b8c9d0e1-f2a3-4567-b890-123456789012'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260630013005_UpdateNationalityNationalIdToRequired'
)
BEGIN
    EXEC(N'UPDATE [Permissions] SET [CreatedAt] = ''2026-06-30T01:30:04.7204029Z''
    WHERE [Id] = ''c5d6e7f8-a9b0-1234-c567-890123456789'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260630013005_UpdateNationalityNationalIdToRequired'
)
BEGIN
    EXEC(N'UPDATE [Permissions] SET [CreatedAt] = ''2026-06-30T01:30:04.7204024Z''
    WHERE [Id] = ''c9d0e1f2-a3b4-5678-c901-234567890123'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260630013005_UpdateNationalityNationalIdToRequired'
)
BEGIN
    EXEC(N'UPDATE [Permissions] SET [CreatedAt] = ''2026-06-30T01:30:04.7204025Z''
    WHERE [Id] = ''d0e1f2a3-b4c5-6789-d012-345678901234'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260630013005_UpdateNationalityNationalIdToRequired'
)
BEGIN
    EXEC(N'UPDATE [Permissions] SET [CreatedAt] = ''2026-06-30T01:30:04.7204046Z''
    WHERE [Id] = ''d1e2f3a4-b5c6-7890-d123-456789012345'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260630013005_UpdateNationalityNationalIdToRequired'
)
BEGIN
    EXEC(N'UPDATE [Permissions] SET [CreatedAt] = ''2026-06-30T01:30:04.7204030Z''
    WHERE [Id] = ''d6e7f8a9-b0c1-2345-d678-901234567890'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260630013005_UpdateNationalityNationalIdToRequired'
)
BEGIN
    EXEC(N'UPDATE [Permissions] SET [CreatedAt] = ''2026-06-30T01:30:04.7204025Z''
    WHERE [Id] = ''e1f2a3b4-c5d6-7890-e123-456789012345'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260630013005_UpdateNationalityNationalIdToRequired'
)
BEGIN
    EXEC(N'UPDATE [Permissions] SET [CreatedAt] = ''2026-06-30T01:30:04.7202945Z''
    WHERE [Id] = ''e5f6a7b8-c9d0-1234-ef56-789012345678'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260630013005_UpdateNationalityNationalIdToRequired'
)
BEGIN
    EXEC(N'UPDATE [Permissions] SET [CreatedAt] = ''2026-06-30T01:30:04.7204031Z''
    WHERE [Id] = ''e7f8a9b0-c1d2-3456-e789-012345678901'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260630013005_UpdateNationalityNationalIdToRequired'
)
BEGIN
    EXEC(N'UPDATE [Permissions] SET [CreatedAt] = ''2026-06-30T01:30:04.7204026Z''
    WHERE [Id] = ''f2a3b4c5-d6e7-8901-f234-567890123456'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260630013005_UpdateNationalityNationalIdToRequired'
)
BEGIN
    EXEC(N'UPDATE [Permissions] SET [CreatedAt] = ''2026-06-30T01:30:04.7204020Z''
    WHERE [Id] = ''f6a7b8c9-d0e1-2345-f678-901234567890'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260630013005_UpdateNationalityNationalIdToRequired'
)
BEGIN
    EXEC(N'UPDATE [Permissions] SET [CreatedAt] = ''2026-06-30T01:30:04.7204045Z''
    WHERE [Id] = ''f8a9b0c1-d2e3-4567-f890-123456789012'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260630013005_UpdateNationalityNationalIdToRequired'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'CreatedAt', N'PermissionId', N'RoleId', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[RolePermissions]'))
        SET IDENTITY_INSERT [RolePermissions] ON;
    EXEC(N'INSERT INTO [RolePermissions] ([Id], [CreatedAt], [PermissionId], [RoleId], [UpdatedAt])
    VALUES (''0cd25d71-346b-3908-42cf-d84115ab6e8b'', ''2026-06-30T01:30:04.7211835Z'', ''f2a3b4c5-d6e7-8901-f234-567890123456'', ''b2c3d4e5-f6a7-8901-bcde-f12345678901'', NULL),
    (''12205256-ff83-d1a7-ca36-ef013e4348dd'', ''2026-06-30T01:30:04.7211835Z'', ''d0e1f2a3-b4c5-6789-d012-345678901234'', ''c3d4e5f6-a7b8-9012-cdef-123456789012'', NULL),
    (''18ca3a8f-4030-37b6-bb6c-51d7f78326bf'', ''2026-06-30T01:30:04.7211835Z'', ''d1e2f3a4-b5c6-7890-d123-456789012345'', ''a1b2c3d4-e5f6-7890-abcd-ef1234567890'', NULL),
    (''1a447bea-0884-a7b8-5c95-9aa22876b650'', ''2026-06-30T01:30:04.7211835Z'', ''e7f8a9b0-c1d2-3456-e789-012345678901'', ''a1b2c3d4-e5f6-7890-abcd-ef1234567890'', NULL),
    (''37fafc7e-cec4-f1e7-0639-0769adc78e8c'', ''2026-06-30T01:30:04.7211835Z'', ''b8c9d0e1-f2a3-4567-b890-123456789012'', ''b2c3d4e5-f6a7-8901-bcde-f12345678901'', NULL),
    (''38e68501-6aa4-2723-251e-83ab85145eda'', ''2026-06-30T01:30:04.7211835Z'', ''b4c5d6e7-f8a9-0123-b456-789012345678'', ''a1b2c3d4-e5f6-7890-abcd-ef1234567890'', NULL),
    (''411c0941-f12c-e117-e542-7561547fae68'', ''2026-06-30T01:30:04.7211835Z'', ''a3b4c5d6-e7f8-9012-a345-678901234567'', ''a1b2c3d4-e5f6-7890-abcd-ef1234567890'', NULL),
    (''578d3c75-7400-e248-983f-cb4d4f13fdd3'', ''2026-06-30T01:30:04.7211835Z'', ''e5f6a7b8-c9d0-1234-ef56-789012345678'', ''b2c3d4e5-f6a7-8901-bcde-f12345678901'', NULL),
    (''618876f6-daa1-fc83-c047-480bf88c794e'', ''2026-06-30T01:30:04.7211835Z'', ''a3b4c5d6-e7f8-9012-a345-678901234567'', ''b2c3d4e5-f6a7-8901-bcde-f12345678901'', NULL),
    (''7b9944e2-4c52-0d53-1edd-ccd1d7316518'', ''2026-06-30T01:30:04.7211835Z'', ''e1f2a3b4-c5d6-7890-e123-456789012345'', ''b2c3d4e5-f6a7-8901-bcde-f12345678901'', NULL),
    (''7d73a719-0bf4-f70e-384e-ef6a71bdc88f'', ''2026-06-30T01:30:04.7211835Z'', ''a7b8c9d0-e1f2-3456-a789-012345678901'', ''a1b2c3d4-e5f6-7890-abcd-ef1234567890'', NULL),
    (''7dc649b3-0e4d-a02d-25b2-1533c6da1485'', ''2026-06-30T01:30:04.7211835Z'', ''e5f6a7b8-c9d0-1234-ef56-789012345678'', ''a1b2c3d4-e5f6-7890-abcd-ef1234567890'', NULL),
    (''94e3232d-330d-080b-3968-8c0b0422abd3'', ''2026-06-30T01:30:04.7211835Z'', ''c9d0e1f2-a3b4-5678-c901-234567890123'', ''a1b2c3d4-e5f6-7890-abcd-ef1234567890'', NULL),
    (''95403e61-b5ef-2b2e-e6db-cb2875fdd651'', ''2026-06-30T01:30:04.7211835Z'', ''f2a3b4c5-d6e7-8901-f234-567890123456'', ''a1b2c3d4-e5f6-7890-abcd-ef1234567890'', NULL),
    (''97577489-44d4-9fca-3e5a-a7ff99c25f60'', ''2026-06-30T01:30:04.7211835Z'', ''d0e1f2a3-b4c5-6789-d012-345678901234'', ''a1b2c3d4-e5f6-7890-abcd-ef1234567890'', NULL),
    (''be8fe1d9-d5f7-fc0d-1c62-f23ec408ee0d'', ''2026-06-30T01:30:04.7211835Z'', ''b8c9d0e1-f2a3-4567-b890-123456789012'', ''a1b2c3d4-e5f6-7890-abcd-ef1234567890'', NULL),
    (''c01cc128-784d-f6dd-8543-496e731fa31a'', ''2026-06-30T01:30:04.7211835Z'', ''f6a7b8c9-d0e1-2345-f678-901234567890'', ''a1b2c3d4-e5f6-7890-abcd-ef1234567890'', NULL),
    (''c4522deb-c1b9-9234-005e-bdb3b1c5d30b'', ''2026-06-30T01:30:04.7211835Z'', ''e5f6a7b8-c9d0-1234-ef56-789012345678'', ''c3d4e5f6-a7b8-9012-cdef-123456789012'', NULL),
    (''ce45517e-5f0e-7330-9534-bae4dcee4905'', ''2026-06-30T01:30:04.7211835Z'', ''f8a9b0c1-d2e3-4567-f890-123456789012'', ''a1b2c3d4-e5f6-7890-abcd-ef1234567890'', NULL),
    (''d487e031-bfa2-73dc-4031-273dbfae68c5'', ''2026-06-30T01:30:04.7211835Z'', ''a9b0c1d2-e3f4-5678-a901-234567890123'', ''a1b2c3d4-e5f6-7890-abcd-ef1234567890'', NULL),
    (''dde6c68e-c7ec-19c2-65a8-d1c9abf4dbd1'', ''2026-06-30T01:30:04.7211835Z'', ''d0e1f2a3-b4c5-6789-d012-345678901234'', ''b2c3d4e5-f6a7-8901-bcde-f12345678901'', NULL),
    (''df7c114a-08b2-ca57-d10a-9458a9126cc7'', ''2026-06-30T01:30:04.7211835Z'', ''b4c5d6e7-f8a9-0123-b456-789012345678'', ''c3d4e5f6-a7b8-9012-cdef-123456789012'', NULL),
    (''f6d95421-adde-78dc-a3e8-ada50506e534'', ''2026-06-30T01:30:04.7211835Z'', ''d6e7f8a9-b0c1-2345-d678-901234567890'', ''a1b2c3d4-e5f6-7890-abcd-ef1234567890'', NULL),
    (''faed05e8-a7e0-30e1-7f2f-3f59a113fd18'', ''2026-06-30T01:30:04.7211835Z'', ''c5d6e7f8-a9b0-1234-c567-890123456789'', ''a1b2c3d4-e5f6-7890-abcd-ef1234567890'', NULL),
    (''fb0338fb-fc95-ac51-0a37-7cc90b17ec50'', ''2026-06-30T01:30:04.7211835Z'', ''e1f2a3b4-c5d6-7890-e123-456789012345'', ''a1b2c3d4-e5f6-7890-abcd-ef1234567890'', NULL)');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'CreatedAt', N'PermissionId', N'RoleId', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[RolePermissions]'))
        SET IDENTITY_INSERT [RolePermissions] OFF;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260630013005_UpdateNationalityNationalIdToRequired'
)
BEGIN
    EXEC(N'UPDATE [Roles] SET [CreatedAt] = ''2026-06-30T01:30:04.4942667Z''
    WHERE [Id] = ''a1b2c3d4-e5f6-7890-abcd-ef1234567890'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260630013005_UpdateNationalityNationalIdToRequired'
)
BEGIN
    EXEC(N'UPDATE [Roles] SET [CreatedAt] = ''2026-06-30T01:30:04.4943713Z''
    WHERE [Id] = ''b2c3d4e5-f6a7-8901-bcde-f12345678901'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260630013005_UpdateNationalityNationalIdToRequired'
)
BEGIN
    EXEC(N'UPDATE [Roles] SET [CreatedAt] = ''2026-06-30T01:30:04.4943714Z''
    WHERE [Id] = ''c3d4e5f6-a7b8-9012-cdef-123456789012'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260630013005_UpdateNationalityNationalIdToRequired'
)
BEGIN
    EXEC(N'UPDATE [Settings] SET [CreatedAt] = ''2026-06-30T01:30:04.7230153Z''
    WHERE [Id] = ''a5b6c7d8-e9f0-1234-a567-890123456789'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260630013005_UpdateNationalityNationalIdToRequired'
)
BEGIN
    EXEC(N'UPDATE [Settings] SET [CreatedAt] = ''2026-06-30T01:30:04.7230153Z''
    WHERE [Id] = ''b0c1d2e3-f4a5-6789-b012-345678901234'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260630013005_UpdateNationalityNationalIdToRequired'
)
BEGIN
    EXEC(N'UPDATE [Settings] SET [CreatedAt] = ''2026-06-30T01:30:04.7230153Z''
    WHERE [Id] = ''b6c7d8e9-f0a1-2345-b678-901234567890'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260630013005_UpdateNationalityNationalIdToRequired'
)
BEGIN
    EXEC(N'UPDATE [Settings] SET [CreatedAt] = ''2026-06-30T01:30:04.7230153Z''
    WHERE [Id] = ''c1d2e3f4-a5b6-7890-c123-456789012345'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260630013005_UpdateNationalityNationalIdToRequired'
)
BEGIN
    EXEC(N'UPDATE [Settings] SET [CreatedAt] = ''2026-06-30T01:30:04.7230153Z''
    WHERE [Id] = ''c7d8e9f0-a1b2-3456-c789-012345678901'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260630013005_UpdateNationalityNationalIdToRequired'
)
BEGIN
    EXEC(N'UPDATE [Settings] SET [CreatedAt] = ''2026-06-30T01:30:04.7230153Z''
    WHERE [Id] = ''d2e3f4a5-b6c7-8901-d234-567890123456'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260630013005_UpdateNationalityNationalIdToRequired'
)
BEGIN
    EXEC(N'UPDATE [Settings] SET [CreatedAt] = ''2026-06-30T01:30:04.7230153Z''
    WHERE [Id] = ''d8e9f0a1-b2c3-4567-d890-123456789012'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260630013005_UpdateNationalityNationalIdToRequired'
)
BEGIN
    EXEC(N'UPDATE [Settings] SET [CreatedAt] = ''2026-06-30T01:30:04.7230153Z''
    WHERE [Id] = ''e3f4a5b6-c7d8-9012-e345-678901234567'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260630013005_UpdateNationalityNationalIdToRequired'
)
BEGIN
    EXEC(N'UPDATE [Settings] SET [CreatedAt] = ''2026-06-30T01:30:04.7230153Z''
    WHERE [Id] = ''e9f0a1b2-c3d4-5678-e901-234567890123'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260630013005_UpdateNationalityNationalIdToRequired'
)
BEGIN
    EXEC(N'UPDATE [Settings] SET [CreatedAt] = ''2026-06-30T01:30:04.7230153Z''
    WHERE [Id] = ''f4a5b6c7-d8e9-0123-f456-789012345678'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260630013005_UpdateNationalityNationalIdToRequired'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''$2a$11$Uuso/x/mhWPcVrNHqin8ru1lvWqNiEXRC9FBNoN4GM9MuJSvZPpUm''
    WHERE [Id] = ''d4e5f6a7-b8c9-0123-def4-567890123456'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260630013005_UpdateNationalityNationalIdToRequired'
)
BEGIN

    UPDATE m
    SET m.NationalId = LEFT('TEMP-' + REPLACE(CONVERT(NVARCHAR(36), m.Id), '-', ''), 14)
    FROM Members m
    WHERE m.NationalId = ''
       OR m.NationalId IS NULL

END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260630013005_UpdateNationalityNationalIdToRequired'
)
BEGIN
    CREATE UNIQUE INDEX [IX_Members_NationalId] ON [Members] ([NationalId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260630013005_UpdateNationalityNationalIdToRequired'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260630013005_UpdateNationalityNationalIdToRequired', N'10.0.9');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260701004558_CreateOffersTable'
)
BEGIN
    CREATE TABLE [Offers] (
        [Id] uniqueidentifier NOT NULL,
        [OfferTitle] nvarchar(200) NOT NULL,
        [LinkedPackageId] uniqueidentifier NULL,
        [OfferType] nvarchar(30) NOT NULL,
        [BonusMonths] int NULL,
        [BonusDays] int NULL,
        [OfferPrice] decimal(18,2) NULL,
        [ExtraFreezeDays] int NULL,
        [Description] nvarchar(2000) NULL,
        [StartDate] datetime2 NOT NULL,
        [EndDate] datetime2 NOT NULL,
        [IsActive] bit NOT NULL DEFAULT CAST(1 AS bit),
        [CreatedAt] datetime2 NOT NULL,
        [UpdatedAt] datetime2 NULL,
        CONSTRAINT [PK_Offers] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Offers_MembershipPlans_LinkedPackageId] FOREIGN KEY ([LinkedPackageId]) REFERENCES [MembershipPlans] ([Id]) ON DELETE SET NULL
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260701004558_CreateOffersTable'
)
BEGIN
    CREATE INDEX [IX_Offers_IsActive] ON [Offers] ([IsActive]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260701004558_CreateOffersTable'
)
BEGIN
    CREATE INDEX [IX_Offers_LinkedPackageId] ON [Offers] ([LinkedPackageId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260701004558_CreateOffersTable'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260701004558_CreateOffersTable', N'10.0.9');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260701220522_RemovePackagesModule'
)
BEGIN
    DROP TABLE [Packages];
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260701220522_RemovePackagesModule'
)
BEGIN
    EXEC(N'UPDATE [Permissions] SET [CreatedAt] = ''2026-07-01T22:05:20.8885030Z''
    WHERE [Id] = ''a3b4c5d6-e7f8-9012-a345-678901234567'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260701220522_RemovePackagesModule'
)
BEGIN
    EXEC(N'UPDATE [Permissions] SET [CreatedAt] = ''2026-07-01T22:05:20.8885024Z''
    WHERE [Id] = ''a7b8c9d0-e1f2-3456-a789-012345678901'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260701220522_RemovePackagesModule'
)
BEGIN
    EXEC(N'UPDATE [Permissions] SET [CreatedAt] = ''2026-07-01T22:05:20.8885035Z''
    WHERE [Id] = ''a9b0c1d2-e3f4-5678-a901-234567890123'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260701220522_RemovePackagesModule'
)
BEGIN
    EXEC(N'UPDATE [Permissions] SET [CreatedAt] = ''2026-07-01T22:05:20.8885031Z''
    WHERE [Id] = ''b4c5d6e7-f8a9-0123-b456-789012345678'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260701220522_RemovePackagesModule'
)
BEGIN
    EXEC(N'UPDATE [Permissions] SET [CreatedAt] = ''2026-07-01T22:05:20.8885026Z''
    WHERE [Id] = ''b8c9d0e1-f2a3-4567-b890-123456789012'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260701220522_RemovePackagesModule'
)
BEGIN
    EXEC(N'UPDATE [Permissions] SET [CreatedAt] = ''2026-07-01T22:05:20.8885032Z''
    WHERE [Id] = ''c5d6e7f8-a9b0-1234-c567-890123456789'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260701220522_RemovePackagesModule'
)
BEGIN
    EXEC(N'UPDATE [Permissions] SET [CreatedAt] = ''2026-07-01T22:05:20.8885027Z''
    WHERE [Id] = ''c9d0e1f2-a3b4-5678-c901-234567890123'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260701220522_RemovePackagesModule'
)
BEGIN
    EXEC(N'UPDATE [Permissions] SET [CreatedAt] = ''2026-07-01T22:05:20.8885028Z''
    WHERE [Id] = ''d0e1f2a3-b4c5-6789-d012-345678901234'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260701220522_RemovePackagesModule'
)
BEGIN
    EXEC(N'UPDATE [Permissions] SET [CreatedAt] = ''2026-07-01T22:05:20.8885035Z''
    WHERE [Id] = ''d1e2f3a4-b5c6-7890-d123-456789012345'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260701220522_RemovePackagesModule'
)
BEGIN
    EXEC(N'UPDATE [Permissions] SET [CreatedAt] = ''2026-07-01T22:05:20.8885032Z''
    WHERE [Id] = ''d6e7f8a9-b0c1-2345-d678-901234567890'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260701220522_RemovePackagesModule'
)
BEGIN
    EXEC(N'UPDATE [Permissions] SET [CreatedAt] = ''2026-07-01T22:05:20.8885028Z''
    WHERE [Id] = ''e1f2a3b4-c5d6-7890-e123-456789012345'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260701220522_RemovePackagesModule'
)
BEGIN
    EXEC(N'UPDATE [Permissions] SET [CreatedAt] = ''2026-07-01T22:05:20.8884105Z''
    WHERE [Id] = ''e5f6a7b8-c9d0-1234-ef56-789012345678'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260701220522_RemovePackagesModule'
)
BEGIN
    EXEC(N'UPDATE [Permissions] SET [CreatedAt] = ''2026-07-01T22:05:20.8885033Z''
    WHERE [Id] = ''e7f8a9b0-c1d2-3456-e789-012345678901'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260701220522_RemovePackagesModule'
)
BEGIN
    EXEC(N'UPDATE [Permissions] SET [CreatedAt] = ''2026-07-01T22:05:20.8885029Z''
    WHERE [Id] = ''f2a3b4c5-d6e7-8901-f234-567890123456'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260701220522_RemovePackagesModule'
)
BEGIN
    EXEC(N'UPDATE [Permissions] SET [CreatedAt] = ''2026-07-01T22:05:20.8885022Z''
    WHERE [Id] = ''f6a7b8c9-d0e1-2345-f678-901234567890'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260701220522_RemovePackagesModule'
)
BEGIN
    EXEC(N'UPDATE [Permissions] SET [CreatedAt] = ''2026-07-01T22:05:20.8885034Z''
    WHERE [Id] = ''f8a9b0c1-d2e3-4567-f890-123456789012'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260701220522_RemovePackagesModule'
)
BEGIN
    EXEC(N'UPDATE [RolePermissions] SET [CreatedAt] = ''2026-07-01T22:05:20.8892406Z''
    WHERE [Id] = ''0cd25d71-346b-3908-42cf-d84115ab6e8b'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260701220522_RemovePackagesModule'
)
BEGIN
    EXEC(N'UPDATE [RolePermissions] SET [CreatedAt] = ''2026-07-01T22:05:20.8892406Z''
    WHERE [Id] = ''12205256-ff83-d1a7-ca36-ef013e4348dd'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260701220522_RemovePackagesModule'
)
BEGIN
    EXEC(N'UPDATE [RolePermissions] SET [CreatedAt] = ''2026-07-01T22:05:20.8892406Z''
    WHERE [Id] = ''18ca3a8f-4030-37b6-bb6c-51d7f78326bf'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260701220522_RemovePackagesModule'
)
BEGIN
    EXEC(N'UPDATE [RolePermissions] SET [CreatedAt] = ''2026-07-01T22:05:20.8892406Z''
    WHERE [Id] = ''1a447bea-0884-a7b8-5c95-9aa22876b650'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260701220522_RemovePackagesModule'
)
BEGIN
    EXEC(N'UPDATE [RolePermissions] SET [CreatedAt] = ''2026-07-01T22:05:20.8892406Z''
    WHERE [Id] = ''37fafc7e-cec4-f1e7-0639-0769adc78e8c'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260701220522_RemovePackagesModule'
)
BEGIN
    EXEC(N'UPDATE [RolePermissions] SET [CreatedAt] = ''2026-07-01T22:05:20.8892406Z''
    WHERE [Id] = ''38e68501-6aa4-2723-251e-83ab85145eda'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260701220522_RemovePackagesModule'
)
BEGIN
    EXEC(N'UPDATE [RolePermissions] SET [CreatedAt] = ''2026-07-01T22:05:20.8892406Z''
    WHERE [Id] = ''411c0941-f12c-e117-e542-7561547fae68'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260701220522_RemovePackagesModule'
)
BEGIN
    EXEC(N'UPDATE [RolePermissions] SET [CreatedAt] = ''2026-07-01T22:05:20.8892406Z''
    WHERE [Id] = ''578d3c75-7400-e248-983f-cb4d4f13fdd3'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260701220522_RemovePackagesModule'
)
BEGIN
    EXEC(N'UPDATE [RolePermissions] SET [CreatedAt] = ''2026-07-01T22:05:20.8892406Z''
    WHERE [Id] = ''618876f6-daa1-fc83-c047-480bf88c794e'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260701220522_RemovePackagesModule'
)
BEGIN
    EXEC(N'UPDATE [RolePermissions] SET [CreatedAt] = ''2026-07-01T22:05:20.8892406Z''
    WHERE [Id] = ''7b9944e2-4c52-0d53-1edd-ccd1d7316518'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260701220522_RemovePackagesModule'
)
BEGIN
    EXEC(N'UPDATE [RolePermissions] SET [CreatedAt] = ''2026-07-01T22:05:20.8892406Z''
    WHERE [Id] = ''7d73a719-0bf4-f70e-384e-ef6a71bdc88f'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260701220522_RemovePackagesModule'
)
BEGIN
    EXEC(N'UPDATE [RolePermissions] SET [CreatedAt] = ''2026-07-01T22:05:20.8892406Z''
    WHERE [Id] = ''7dc649b3-0e4d-a02d-25b2-1533c6da1485'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260701220522_RemovePackagesModule'
)
BEGIN
    EXEC(N'UPDATE [RolePermissions] SET [CreatedAt] = ''2026-07-01T22:05:20.8892406Z''
    WHERE [Id] = ''94e3232d-330d-080b-3968-8c0b0422abd3'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260701220522_RemovePackagesModule'
)
BEGIN
    EXEC(N'UPDATE [RolePermissions] SET [CreatedAt] = ''2026-07-01T22:05:20.8892406Z''
    WHERE [Id] = ''95403e61-b5ef-2b2e-e6db-cb2875fdd651'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260701220522_RemovePackagesModule'
)
BEGIN
    EXEC(N'UPDATE [RolePermissions] SET [CreatedAt] = ''2026-07-01T22:05:20.8892406Z''
    WHERE [Id] = ''97577489-44d4-9fca-3e5a-a7ff99c25f60'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260701220522_RemovePackagesModule'
)
BEGIN
    EXEC(N'UPDATE [RolePermissions] SET [CreatedAt] = ''2026-07-01T22:05:20.8892406Z''
    WHERE [Id] = ''be8fe1d9-d5f7-fc0d-1c62-f23ec408ee0d'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260701220522_RemovePackagesModule'
)
BEGIN
    EXEC(N'UPDATE [RolePermissions] SET [CreatedAt] = ''2026-07-01T22:05:20.8892406Z''
    WHERE [Id] = ''c01cc128-784d-f6dd-8543-496e731fa31a'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260701220522_RemovePackagesModule'
)
BEGIN
    EXEC(N'UPDATE [RolePermissions] SET [CreatedAt] = ''2026-07-01T22:05:20.8892406Z''
    WHERE [Id] = ''c4522deb-c1b9-9234-005e-bdb3b1c5d30b'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260701220522_RemovePackagesModule'
)
BEGIN
    EXEC(N'UPDATE [RolePermissions] SET [CreatedAt] = ''2026-07-01T22:05:20.8892406Z''
    WHERE [Id] = ''ce45517e-5f0e-7330-9534-bae4dcee4905'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260701220522_RemovePackagesModule'
)
BEGIN
    EXEC(N'UPDATE [RolePermissions] SET [CreatedAt] = ''2026-07-01T22:05:20.8892406Z''
    WHERE [Id] = ''d487e031-bfa2-73dc-4031-273dbfae68c5'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260701220522_RemovePackagesModule'
)
BEGIN
    EXEC(N'UPDATE [RolePermissions] SET [CreatedAt] = ''2026-07-01T22:05:20.8892406Z''
    WHERE [Id] = ''dde6c68e-c7ec-19c2-65a8-d1c9abf4dbd1'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260701220522_RemovePackagesModule'
)
BEGIN
    EXEC(N'UPDATE [RolePermissions] SET [CreatedAt] = ''2026-07-01T22:05:20.8892406Z''
    WHERE [Id] = ''df7c114a-08b2-ca57-d10a-9458a9126cc7'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260701220522_RemovePackagesModule'
)
BEGIN
    EXEC(N'UPDATE [RolePermissions] SET [CreatedAt] = ''2026-07-01T22:05:20.8892406Z''
    WHERE [Id] = ''f6d95421-adde-78dc-a3e8-ada50506e534'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260701220522_RemovePackagesModule'
)
BEGIN
    EXEC(N'UPDATE [RolePermissions] SET [CreatedAt] = ''2026-07-01T22:05:20.8892406Z''
    WHERE [Id] = ''faed05e8-a7e0-30e1-7f2f-3f59a113fd18'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260701220522_RemovePackagesModule'
)
BEGIN
    EXEC(N'UPDATE [RolePermissions] SET [CreatedAt] = ''2026-07-01T22:05:20.8892406Z''
    WHERE [Id] = ''fb0338fb-fc95-ac51-0a37-7cc90b17ec50'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260701220522_RemovePackagesModule'
)
BEGIN
    EXEC(N'UPDATE [Roles] SET [CreatedAt] = ''2026-07-01T22:05:20.4570945Z''
    WHERE [Id] = ''a1b2c3d4-e5f6-7890-abcd-ef1234567890'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260701220522_RemovePackagesModule'
)
BEGIN
    EXEC(N'UPDATE [Roles] SET [CreatedAt] = ''2026-07-01T22:05:20.4571891Z''
    WHERE [Id] = ''b2c3d4e5-f6a7-8901-bcde-f12345678901'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260701220522_RemovePackagesModule'
)
BEGIN
    EXEC(N'UPDATE [Roles] SET [CreatedAt] = ''2026-07-01T22:05:20.4571893Z''
    WHERE [Id] = ''c3d4e5f6-a7b8-9012-cdef-123456789012'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260701220522_RemovePackagesModule'
)
BEGIN
    EXEC(N'UPDATE [Settings] SET [CreatedAt] = ''2026-07-01T22:05:20.8907420Z''
    WHERE [Id] = ''a5b6c7d8-e9f0-1234-a567-890123456789'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260701220522_RemovePackagesModule'
)
BEGIN
    EXEC(N'UPDATE [Settings] SET [CreatedAt] = ''2026-07-01T22:05:20.8907420Z''
    WHERE [Id] = ''b0c1d2e3-f4a5-6789-b012-345678901234'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260701220522_RemovePackagesModule'
)
BEGIN
    EXEC(N'UPDATE [Settings] SET [CreatedAt] = ''2026-07-01T22:05:20.8907420Z''
    WHERE [Id] = ''b6c7d8e9-f0a1-2345-b678-901234567890'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260701220522_RemovePackagesModule'
)
BEGIN
    EXEC(N'UPDATE [Settings] SET [CreatedAt] = ''2026-07-01T22:05:20.8907420Z''
    WHERE [Id] = ''c1d2e3f4-a5b6-7890-c123-456789012345'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260701220522_RemovePackagesModule'
)
BEGIN
    EXEC(N'UPDATE [Settings] SET [CreatedAt] = ''2026-07-01T22:05:20.8907420Z''
    WHERE [Id] = ''c7d8e9f0-a1b2-3456-c789-012345678901'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260701220522_RemovePackagesModule'
)
BEGIN
    EXEC(N'UPDATE [Settings] SET [CreatedAt] = ''2026-07-01T22:05:20.8907420Z''
    WHERE [Id] = ''d2e3f4a5-b6c7-8901-d234-567890123456'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260701220522_RemovePackagesModule'
)
BEGIN
    EXEC(N'UPDATE [Settings] SET [CreatedAt] = ''2026-07-01T22:05:20.8907420Z''
    WHERE [Id] = ''d8e9f0a1-b2c3-4567-d890-123456789012'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260701220522_RemovePackagesModule'
)
BEGIN
    EXEC(N'UPDATE [Settings] SET [CreatedAt] = ''2026-07-01T22:05:20.8907420Z''
    WHERE [Id] = ''e3f4a5b6-c7d8-9012-e345-678901234567'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260701220522_RemovePackagesModule'
)
BEGIN
    EXEC(N'UPDATE [Settings] SET [CreatedAt] = ''2026-07-01T22:05:20.8907420Z''
    WHERE [Id] = ''e9f0a1b2-c3d4-5678-e901-234567890123'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260701220522_RemovePackagesModule'
)
BEGIN
    EXEC(N'UPDATE [Settings] SET [CreatedAt] = ''2026-07-01T22:05:20.8907420Z''
    WHERE [Id] = ''f4a5b6c7-d8e9-0123-f456-789012345678'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260701220522_RemovePackagesModule'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''$2a$11$rSsZIqkXcxzB1goFPOdWjOZvy7WsCLM4vJpVoTNXBtfNTk//jmB4G''
    WHERE [Id] = ''d4e5f6a7-b8c9-0123-def4-567890123456'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260701220522_RemovePackagesModule'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260701220522_RemovePackagesModule', N'10.0.9');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260701231939_DropPaymentsTable'
)
BEGIN
    DROP TABLE [Payments];
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260701231939_DropPaymentsTable'
)
BEGIN
    EXEC(N'UPDATE [Permissions] SET [CreatedAt] = ''2026-07-01T23:19:37.3656003Z''
    WHERE [Id] = ''a3b4c5d6-e7f8-9012-a345-678901234567'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260701231939_DropPaymentsTable'
)
BEGIN
    EXEC(N'UPDATE [Permissions] SET [CreatedAt] = ''2026-07-01T23:19:37.3655997Z''
    WHERE [Id] = ''a7b8c9d0-e1f2-3456-a789-012345678901'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260701231939_DropPaymentsTable'
)
BEGIN
    EXEC(N'UPDATE [Permissions] SET [CreatedAt] = ''2026-07-01T23:19:37.3656008Z''
    WHERE [Id] = ''a9b0c1d2-e3f4-5678-a901-234567890123'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260701231939_DropPaymentsTable'
)
BEGIN
    EXEC(N'UPDATE [Permissions] SET [CreatedAt] = ''2026-07-01T23:19:37.3656004Z''
    WHERE [Id] = ''b4c5d6e7-f8a9-0123-b456-789012345678'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260701231939_DropPaymentsTable'
)
BEGIN
    EXEC(N'UPDATE [Permissions] SET [CreatedAt] = ''2026-07-01T23:19:37.3655998Z''
    WHERE [Id] = ''b8c9d0e1-f2a3-4567-b890-123456789012'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260701231939_DropPaymentsTable'
)
BEGIN
    EXEC(N'UPDATE [Permissions] SET [CreatedAt] = ''2026-07-01T23:19:37.3656004Z''
    WHERE [Id] = ''c5d6e7f8-a9b0-1234-c567-890123456789'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260701231939_DropPaymentsTable'
)
BEGIN
    EXEC(N'UPDATE [Permissions] SET [CreatedAt] = ''2026-07-01T23:19:37.3655999Z''
    WHERE [Id] = ''c9d0e1f2-a3b4-5678-c901-234567890123'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260701231939_DropPaymentsTable'
)
BEGIN
    EXEC(N'UPDATE [Permissions] SET [CreatedAt] = ''2026-07-01T23:19:37.3656000Z''
    WHERE [Id] = ''d0e1f2a3-b4c5-6789-d012-345678901234'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260701231939_DropPaymentsTable'
)
BEGIN
    EXEC(N'UPDATE [Permissions] SET [CreatedAt] = ''2026-07-01T23:19:37.3656008Z''
    WHERE [Id] = ''d1e2f3a4-b5c6-7890-d123-456789012345'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260701231939_DropPaymentsTable'
)
BEGIN
    EXEC(N'UPDATE [Permissions] SET [CreatedAt] = ''2026-07-01T23:19:37.3656005Z''
    WHERE [Id] = ''d6e7f8a9-b0c1-2345-d678-901234567890'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260701231939_DropPaymentsTable'
)
BEGIN
    EXEC(N'UPDATE [Permissions] SET [CreatedAt] = ''2026-07-01T23:19:37.3656001Z''
    WHERE [Id] = ''e1f2a3b4-c5d6-7890-e123-456789012345'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260701231939_DropPaymentsTable'
)
BEGIN
    EXEC(N'UPDATE [Permissions] SET [CreatedAt] = ''2026-07-01T23:19:37.3655026Z''
    WHERE [Id] = ''e5f6a7b8-c9d0-1234-ef56-789012345678'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260701231939_DropPaymentsTable'
)
BEGIN
    EXEC(N'UPDATE [Permissions] SET [CreatedAt] = ''2026-07-01T23:19:37.3656006Z''
    WHERE [Id] = ''e7f8a9b0-c1d2-3456-e789-012345678901'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260701231939_DropPaymentsTable'
)
BEGIN
    EXEC(N'UPDATE [Permissions] SET [CreatedAt] = ''2026-07-01T23:19:37.3656002Z''
    WHERE [Id] = ''f2a3b4c5-d6e7-8901-f234-567890123456'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260701231939_DropPaymentsTable'
)
BEGIN
    EXEC(N'UPDATE [Permissions] SET [CreatedAt] = ''2026-07-01T23:19:37.3655995Z''
    WHERE [Id] = ''f6a7b8c9-d0e1-2345-f678-901234567890'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260701231939_DropPaymentsTable'
)
BEGIN
    EXEC(N'UPDATE [Permissions] SET [CreatedAt] = ''2026-07-01T23:19:37.3656007Z''
    WHERE [Id] = ''f8a9b0c1-d2e3-4567-f890-123456789012'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260701231939_DropPaymentsTable'
)
BEGIN
    EXEC(N'UPDATE [RolePermissions] SET [CreatedAt] = ''2026-07-01T23:19:37.3663870Z''
    WHERE [Id] = ''0cd25d71-346b-3908-42cf-d84115ab6e8b'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260701231939_DropPaymentsTable'
)
BEGIN
    EXEC(N'UPDATE [RolePermissions] SET [CreatedAt] = ''2026-07-01T23:19:37.3663870Z''
    WHERE [Id] = ''12205256-ff83-d1a7-ca36-ef013e4348dd'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260701231939_DropPaymentsTable'
)
BEGIN
    EXEC(N'UPDATE [RolePermissions] SET [CreatedAt] = ''2026-07-01T23:19:37.3663870Z''
    WHERE [Id] = ''18ca3a8f-4030-37b6-bb6c-51d7f78326bf'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260701231939_DropPaymentsTable'
)
BEGIN
    EXEC(N'UPDATE [RolePermissions] SET [CreatedAt] = ''2026-07-01T23:19:37.3663870Z''
    WHERE [Id] = ''1a447bea-0884-a7b8-5c95-9aa22876b650'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260701231939_DropPaymentsTable'
)
BEGIN
    EXEC(N'UPDATE [RolePermissions] SET [CreatedAt] = ''2026-07-01T23:19:37.3663870Z''
    WHERE [Id] = ''37fafc7e-cec4-f1e7-0639-0769adc78e8c'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260701231939_DropPaymentsTable'
)
BEGIN
    EXEC(N'UPDATE [RolePermissions] SET [CreatedAt] = ''2026-07-01T23:19:37.3663870Z''
    WHERE [Id] = ''38e68501-6aa4-2723-251e-83ab85145eda'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260701231939_DropPaymentsTable'
)
BEGIN
    EXEC(N'UPDATE [RolePermissions] SET [CreatedAt] = ''2026-07-01T23:19:37.3663870Z''
    WHERE [Id] = ''411c0941-f12c-e117-e542-7561547fae68'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260701231939_DropPaymentsTable'
)
BEGIN
    EXEC(N'UPDATE [RolePermissions] SET [CreatedAt] = ''2026-07-01T23:19:37.3663870Z''
    WHERE [Id] = ''578d3c75-7400-e248-983f-cb4d4f13fdd3'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260701231939_DropPaymentsTable'
)
BEGIN
    EXEC(N'UPDATE [RolePermissions] SET [CreatedAt] = ''2026-07-01T23:19:37.3663870Z''
    WHERE [Id] = ''618876f6-daa1-fc83-c047-480bf88c794e'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260701231939_DropPaymentsTable'
)
BEGIN
    EXEC(N'UPDATE [RolePermissions] SET [CreatedAt] = ''2026-07-01T23:19:37.3663870Z''
    WHERE [Id] = ''7b9944e2-4c52-0d53-1edd-ccd1d7316518'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260701231939_DropPaymentsTable'
)
BEGIN
    EXEC(N'UPDATE [RolePermissions] SET [CreatedAt] = ''2026-07-01T23:19:37.3663870Z''
    WHERE [Id] = ''7d73a719-0bf4-f70e-384e-ef6a71bdc88f'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260701231939_DropPaymentsTable'
)
BEGIN
    EXEC(N'UPDATE [RolePermissions] SET [CreatedAt] = ''2026-07-01T23:19:37.3663870Z''
    WHERE [Id] = ''7dc649b3-0e4d-a02d-25b2-1533c6da1485'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260701231939_DropPaymentsTable'
)
BEGIN
    EXEC(N'UPDATE [RolePermissions] SET [CreatedAt] = ''2026-07-01T23:19:37.3663870Z''
    WHERE [Id] = ''94e3232d-330d-080b-3968-8c0b0422abd3'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260701231939_DropPaymentsTable'
)
BEGIN
    EXEC(N'UPDATE [RolePermissions] SET [CreatedAt] = ''2026-07-01T23:19:37.3663870Z''
    WHERE [Id] = ''95403e61-b5ef-2b2e-e6db-cb2875fdd651'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260701231939_DropPaymentsTable'
)
BEGIN
    EXEC(N'UPDATE [RolePermissions] SET [CreatedAt] = ''2026-07-01T23:19:37.3663870Z''
    WHERE [Id] = ''97577489-44d4-9fca-3e5a-a7ff99c25f60'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260701231939_DropPaymentsTable'
)
BEGIN
    EXEC(N'UPDATE [RolePermissions] SET [CreatedAt] = ''2026-07-01T23:19:37.3663870Z''
    WHERE [Id] = ''be8fe1d9-d5f7-fc0d-1c62-f23ec408ee0d'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260701231939_DropPaymentsTable'
)
BEGIN
    EXEC(N'UPDATE [RolePermissions] SET [CreatedAt] = ''2026-07-01T23:19:37.3663870Z''
    WHERE [Id] = ''c01cc128-784d-f6dd-8543-496e731fa31a'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260701231939_DropPaymentsTable'
)
BEGIN
    EXEC(N'UPDATE [RolePermissions] SET [CreatedAt] = ''2026-07-01T23:19:37.3663870Z''
    WHERE [Id] = ''c4522deb-c1b9-9234-005e-bdb3b1c5d30b'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260701231939_DropPaymentsTable'
)
BEGIN
    EXEC(N'UPDATE [RolePermissions] SET [CreatedAt] = ''2026-07-01T23:19:37.3663870Z''
    WHERE [Id] = ''ce45517e-5f0e-7330-9534-bae4dcee4905'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260701231939_DropPaymentsTable'
)
BEGIN
    EXEC(N'UPDATE [RolePermissions] SET [CreatedAt] = ''2026-07-01T23:19:37.3663870Z''
    WHERE [Id] = ''d487e031-bfa2-73dc-4031-273dbfae68c5'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260701231939_DropPaymentsTable'
)
BEGIN
    EXEC(N'UPDATE [RolePermissions] SET [CreatedAt] = ''2026-07-01T23:19:37.3663870Z''
    WHERE [Id] = ''dde6c68e-c7ec-19c2-65a8-d1c9abf4dbd1'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260701231939_DropPaymentsTable'
)
BEGIN
    EXEC(N'UPDATE [RolePermissions] SET [CreatedAt] = ''2026-07-01T23:19:37.3663870Z''
    WHERE [Id] = ''df7c114a-08b2-ca57-d10a-9458a9126cc7'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260701231939_DropPaymentsTable'
)
BEGIN
    EXEC(N'UPDATE [RolePermissions] SET [CreatedAt] = ''2026-07-01T23:19:37.3663870Z''
    WHERE [Id] = ''f6d95421-adde-78dc-a3e8-ada50506e534'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260701231939_DropPaymentsTable'
)
BEGIN
    EXEC(N'UPDATE [RolePermissions] SET [CreatedAt] = ''2026-07-01T23:19:37.3663870Z''
    WHERE [Id] = ''faed05e8-a7e0-30e1-7f2f-3f59a113fd18'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260701231939_DropPaymentsTable'
)
BEGIN
    EXEC(N'UPDATE [RolePermissions] SET [CreatedAt] = ''2026-07-01T23:19:37.3663870Z''
    WHERE [Id] = ''fb0338fb-fc95-ac51-0a37-7cc90b17ec50'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260701231939_DropPaymentsTable'
)
BEGIN
    EXEC(N'UPDATE [Roles] SET [CreatedAt] = ''2026-07-01T23:19:36.9517191Z''
    WHERE [Id] = ''a1b2c3d4-e5f6-7890-abcd-ef1234567890'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260701231939_DropPaymentsTable'
)
BEGIN
    EXEC(N'UPDATE [Roles] SET [CreatedAt] = ''2026-07-01T23:19:36.9518352Z''
    WHERE [Id] = ''b2c3d4e5-f6a7-8901-bcde-f12345678901'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260701231939_DropPaymentsTable'
)
BEGIN
    EXEC(N'UPDATE [Roles] SET [CreatedAt] = ''2026-07-01T23:19:36.9518358Z''
    WHERE [Id] = ''c3d4e5f6-a7b8-9012-cdef-123456789012'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260701231939_DropPaymentsTable'
)
BEGIN
    EXEC(N'UPDATE [Settings] SET [CreatedAt] = ''2026-07-01T23:19:37.3678691Z''
    WHERE [Id] = ''a5b6c7d8-e9f0-1234-a567-890123456789'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260701231939_DropPaymentsTable'
)
BEGIN
    EXEC(N'UPDATE [Settings] SET [CreatedAt] = ''2026-07-01T23:19:37.3678691Z''
    WHERE [Id] = ''b0c1d2e3-f4a5-6789-b012-345678901234'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260701231939_DropPaymentsTable'
)
BEGIN
    EXEC(N'UPDATE [Settings] SET [CreatedAt] = ''2026-07-01T23:19:37.3678691Z''
    WHERE [Id] = ''b6c7d8e9-f0a1-2345-b678-901234567890'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260701231939_DropPaymentsTable'
)
BEGIN
    EXEC(N'UPDATE [Settings] SET [CreatedAt] = ''2026-07-01T23:19:37.3678691Z''
    WHERE [Id] = ''c1d2e3f4-a5b6-7890-c123-456789012345'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260701231939_DropPaymentsTable'
)
BEGIN
    EXEC(N'UPDATE [Settings] SET [CreatedAt] = ''2026-07-01T23:19:37.3678691Z''
    WHERE [Id] = ''c7d8e9f0-a1b2-3456-c789-012345678901'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260701231939_DropPaymentsTable'
)
BEGIN
    EXEC(N'UPDATE [Settings] SET [CreatedAt] = ''2026-07-01T23:19:37.3678691Z''
    WHERE [Id] = ''d2e3f4a5-b6c7-8901-d234-567890123456'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260701231939_DropPaymentsTable'
)
BEGIN
    EXEC(N'UPDATE [Settings] SET [CreatedAt] = ''2026-07-01T23:19:37.3678691Z''
    WHERE [Id] = ''d8e9f0a1-b2c3-4567-d890-123456789012'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260701231939_DropPaymentsTable'
)
BEGIN
    EXEC(N'UPDATE [Settings] SET [CreatedAt] = ''2026-07-01T23:19:37.3678691Z''
    WHERE [Id] = ''e3f4a5b6-c7d8-9012-e345-678901234567'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260701231939_DropPaymentsTable'
)
BEGIN
    EXEC(N'UPDATE [Settings] SET [CreatedAt] = ''2026-07-01T23:19:37.3678691Z''
    WHERE [Id] = ''e9f0a1b2-c3d4-5678-e901-234567890123'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260701231939_DropPaymentsTable'
)
BEGIN
    EXEC(N'UPDATE [Settings] SET [CreatedAt] = ''2026-07-01T23:19:37.3678691Z''
    WHERE [Id] = ''f4a5b6c7-d8e9-0123-f456-789012345678'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260701231939_DropPaymentsTable'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''$2a$11$T5XPM1VTYOweLRV/DdLmjemlt3AWgoYRCA1z58g6mH49zuA4gkCwK''
    WHERE [Id] = ''d4e5f6a7-b8c9-0123-def4-567890123456'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260701231939_DropPaymentsTable'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260701231939_DropPaymentsTable', N'10.0.9');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260701233341_CreateSubscriptionModule'
)
BEGIN
    CREATE TABLE [Subscriptions] (
        [Id] uniqueidentifier NOT NULL,
        [ReceiptNumber] nvarchar(20) NOT NULL,
        [MemberId] uniqueidentifier NOT NULL,
        [PlanId] uniqueidentifier NOT NULL,
        [OfferId] uniqueidentifier NULL,
        [TotalSubscriptionValue] decimal(18,2) NOT NULL,
        [AmountPaid] decimal(18,2) NOT NULL,
        [RemainingBalance] decimal(18,2) NOT NULL,
        [PaymentMethod] nvarchar(20) NOT NULL,
        [StartDate] datetime2 NOT NULL,
        [ExpirationDate] datetime2 NOT NULL,
        [Status] nvarchar(20) NOT NULL,
        [FreezeStart] datetime2 NULL,
        [FreezeEnd] datetime2 NULL,
        [TotalFreezeDays] int NOT NULL,
        [AdminSignature] nvarchar(500) NULL,
        [Notes] nvarchar(1000) NULL,
        [CreatedAt] datetime2 NOT NULL,
        [UpdatedAt] datetime2 NULL,
        CONSTRAINT [PK_Subscriptions] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Subscriptions_Members_MemberId] FOREIGN KEY ([MemberId]) REFERENCES [Members] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_Subscriptions_MembershipPlans_PlanId] FOREIGN KEY ([PlanId]) REFERENCES [MembershipPlans] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_Subscriptions_Offers_OfferId] FOREIGN KEY ([OfferId]) REFERENCES [Offers] ([Id]) ON DELETE SET NULL
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260701233341_CreateSubscriptionModule'
)
BEGIN
    CREATE TABLE [SubscriptionFreezeHistories] (
        [Id] uniqueidentifier NOT NULL,
        [SubscriptionId] uniqueidentifier NOT NULL,
        [FreezeStart] datetime2 NOT NULL,
        [FreezeEnd] datetime2 NOT NULL,
        [FreezeDays] int NOT NULL,
        [Reason] nvarchar(500) NULL,
        [UnfreezeDate] datetime2 NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        [UpdatedAt] datetime2 NULL,
        CONSTRAINT [PK_SubscriptionFreezeHistories] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_SubscriptionFreezeHistories_Subscriptions_SubscriptionId] FOREIGN KEY ([SubscriptionId]) REFERENCES [Subscriptions] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260701233341_CreateSubscriptionModule'
)
BEGIN
    CREATE TABLE [SubscriptionPayments] (
        [Id] uniqueidentifier NOT NULL,
        [SubscriptionId] uniqueidentifier NOT NULL,
        [Amount] decimal(18,2) NOT NULL,
        [PaymentMethod] nvarchar(20) NOT NULL,
        [ReferenceNumber] nvarchar(100) NULL,
        [EmployeeId] uniqueidentifier NULL,
        [Notes] nvarchar(500) NULL,
        [RunningBalance] decimal(18,2) NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        [UpdatedAt] datetime2 NULL,
        CONSTRAINT [PK_SubscriptionPayments] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_SubscriptionPayments_Subscriptions_SubscriptionId] FOREIGN KEY ([SubscriptionId]) REFERENCES [Subscriptions] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_SubscriptionPayments_Users_EmployeeId] FOREIGN KEY ([EmployeeId]) REFERENCES [Users] ([Id]) ON DELETE SET NULL
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260701233341_CreateSubscriptionModule'
)
BEGIN
    CREATE TABLE [SubscriptionTransactionLogs] (
        [Id] uniqueidentifier NOT NULL,
        [SubscriptionId] uniqueidentifier NOT NULL,
        [Action] nvarchar(50) NOT NULL,
        [Description] nvarchar(1000) NOT NULL,
        [PerformedBy] uniqueidentifier NULL,
        [CreatedAt] datetime2 NOT NULL,
        [UpdatedAt] datetime2 NULL,
        CONSTRAINT [PK_SubscriptionTransactionLogs] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_SubscriptionTransactionLogs_Subscriptions_SubscriptionId] FOREIGN KEY ([SubscriptionId]) REFERENCES [Subscriptions] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_SubscriptionTransactionLogs_Users_PerformedBy] FOREIGN KEY ([PerformedBy]) REFERENCES [Users] ([Id]) ON DELETE SET NULL
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260701233341_CreateSubscriptionModule'
)
BEGIN
    EXEC(N'UPDATE [Permissions] SET [CreatedAt] = ''2026-07-01T23:33:39.0577682Z''
    WHERE [Id] = ''a3b4c5d6-e7f8-9012-a345-678901234567'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260701233341_CreateSubscriptionModule'
)
BEGIN
    EXEC(N'UPDATE [Permissions] SET [CreatedAt] = ''2026-07-01T23:33:39.0577650Z''
    WHERE [Id] = ''a7b8c9d0-e1f2-3456-a789-012345678901'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260701233341_CreateSubscriptionModule'
)
BEGIN
    EXEC(N'UPDATE [Permissions] SET [CreatedAt] = ''2026-07-01T23:33:39.0577687Z''
    WHERE [Id] = ''a9b0c1d2-e3f4-5678-a901-234567890123'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260701233341_CreateSubscriptionModule'
)
BEGIN
    EXEC(N'UPDATE [Permissions] SET [CreatedAt] = ''2026-07-01T23:33:39.0577683Z''
    WHERE [Id] = ''b4c5d6e7-f8a9-0123-b456-789012345678'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260701233341_CreateSubscriptionModule'
)
BEGIN
    EXEC(N'UPDATE [Permissions] SET [CreatedAt] = ''2026-07-01T23:33:39.0577652Z''
    WHERE [Id] = ''b8c9d0e1-f2a3-4567-b890-123456789012'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260701233341_CreateSubscriptionModule'
)
BEGIN
    EXEC(N'UPDATE [Permissions] SET [CreatedAt] = ''2026-07-01T23:33:39.0577683Z''
    WHERE [Id] = ''c5d6e7f8-a9b0-1234-c567-890123456789'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260701233341_CreateSubscriptionModule'
)
BEGIN
    EXEC(N'UPDATE [Permissions] SET [CreatedAt] = ''2026-07-01T23:33:39.0577676Z''
    WHERE [Id] = ''c9d0e1f2-a3b4-5678-c901-234567890123'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260701233341_CreateSubscriptionModule'
)
BEGIN
    EXEC(N'UPDATE [Permissions] SET [CreatedAt] = ''2026-07-01T23:33:39.0577677Z''
    WHERE [Id] = ''d0e1f2a3-b4c5-6789-d012-345678901234'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260701233341_CreateSubscriptionModule'
)
BEGIN
    EXEC(N'UPDATE [Permissions] SET [CreatedAt] = ''2026-07-01T23:33:39.0577688Z''
    WHERE [Id] = ''d1e2f3a4-b5c6-7890-d123-456789012345'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260701233341_CreateSubscriptionModule'
)
BEGIN
    EXEC(N'UPDATE [Permissions] SET [CreatedAt] = ''2026-07-01T23:33:39.0577684Z''
    WHERE [Id] = ''d6e7f8a9-b0c1-2345-d678-901234567890'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260701233341_CreateSubscriptionModule'
)
BEGIN
    EXEC(N'UPDATE [Permissions] SET [CreatedAt] = ''2026-07-01T23:33:39.0577679Z''
    WHERE [Id] = ''e1f2a3b4-c5d6-7890-e123-456789012345'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260701233341_CreateSubscriptionModule'
)
BEGIN
    EXEC(N'UPDATE [Permissions] SET [CreatedAt] = ''2026-07-01T23:33:39.0576403Z''
    WHERE [Id] = ''e5f6a7b8-c9d0-1234-ef56-789012345678'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260701233341_CreateSubscriptionModule'
)
BEGIN
    EXEC(N'UPDATE [Permissions] SET [CreatedAt] = ''2026-07-01T23:33:39.0577685Z''
    WHERE [Id] = ''e7f8a9b0-c1d2-3456-e789-012345678901'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260701233341_CreateSubscriptionModule'
)
BEGIN
    EXEC(N'UPDATE [Permissions] SET [CreatedAt] = ''2026-07-01T23:33:39.0577680Z''
    WHERE [Id] = ''f2a3b4c5-d6e7-8901-f234-567890123456'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260701233341_CreateSubscriptionModule'
)
BEGIN
    EXEC(N'UPDATE [Permissions] SET [CreatedAt] = ''2026-07-01T23:33:39.0577647Z''
    WHERE [Id] = ''f6a7b8c9-d0e1-2345-f678-901234567890'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260701233341_CreateSubscriptionModule'
)
BEGIN
    EXEC(N'UPDATE [Permissions] SET [CreatedAt] = ''2026-07-01T23:33:39.0577686Z''
    WHERE [Id] = ''f8a9b0c1-d2e3-4567-f890-123456789012'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260701233341_CreateSubscriptionModule'
)
BEGIN
    EXEC(N'UPDATE [RolePermissions] SET [CreatedAt] = ''2026-07-01T23:33:39.0585526Z''
    WHERE [Id] = ''0cd25d71-346b-3908-42cf-d84115ab6e8b'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260701233341_CreateSubscriptionModule'
)
BEGIN
    EXEC(N'UPDATE [RolePermissions] SET [CreatedAt] = ''2026-07-01T23:33:39.0585526Z''
    WHERE [Id] = ''12205256-ff83-d1a7-ca36-ef013e4348dd'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260701233341_CreateSubscriptionModule'
)
BEGIN
    EXEC(N'UPDATE [RolePermissions] SET [CreatedAt] = ''2026-07-01T23:33:39.0585526Z''
    WHERE [Id] = ''18ca3a8f-4030-37b6-bb6c-51d7f78326bf'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260701233341_CreateSubscriptionModule'
)
BEGIN
    EXEC(N'UPDATE [RolePermissions] SET [CreatedAt] = ''2026-07-01T23:33:39.0585526Z''
    WHERE [Id] = ''1a447bea-0884-a7b8-5c95-9aa22876b650'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260701233341_CreateSubscriptionModule'
)
BEGIN
    EXEC(N'UPDATE [RolePermissions] SET [CreatedAt] = ''2026-07-01T23:33:39.0585526Z''
    WHERE [Id] = ''37fafc7e-cec4-f1e7-0639-0769adc78e8c'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260701233341_CreateSubscriptionModule'
)
BEGIN
    EXEC(N'UPDATE [RolePermissions] SET [CreatedAt] = ''2026-07-01T23:33:39.0585526Z''
    WHERE [Id] = ''38e68501-6aa4-2723-251e-83ab85145eda'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260701233341_CreateSubscriptionModule'
)
BEGIN
    EXEC(N'UPDATE [RolePermissions] SET [CreatedAt] = ''2026-07-01T23:33:39.0585526Z''
    WHERE [Id] = ''411c0941-f12c-e117-e542-7561547fae68'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260701233341_CreateSubscriptionModule'
)
BEGIN
    EXEC(N'UPDATE [RolePermissions] SET [CreatedAt] = ''2026-07-01T23:33:39.0585526Z''
    WHERE [Id] = ''578d3c75-7400-e248-983f-cb4d4f13fdd3'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260701233341_CreateSubscriptionModule'
)
BEGIN
    EXEC(N'UPDATE [RolePermissions] SET [CreatedAt] = ''2026-07-01T23:33:39.0585526Z''
    WHERE [Id] = ''618876f6-daa1-fc83-c047-480bf88c794e'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260701233341_CreateSubscriptionModule'
)
BEGIN
    EXEC(N'UPDATE [RolePermissions] SET [CreatedAt] = ''2026-07-01T23:33:39.0585526Z''
    WHERE [Id] = ''7b9944e2-4c52-0d53-1edd-ccd1d7316518'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260701233341_CreateSubscriptionModule'
)
BEGIN
    EXEC(N'UPDATE [RolePermissions] SET [CreatedAt] = ''2026-07-01T23:33:39.0585526Z''
    WHERE [Id] = ''7d73a719-0bf4-f70e-384e-ef6a71bdc88f'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260701233341_CreateSubscriptionModule'
)
BEGIN
    EXEC(N'UPDATE [RolePermissions] SET [CreatedAt] = ''2026-07-01T23:33:39.0585526Z''
    WHERE [Id] = ''7dc649b3-0e4d-a02d-25b2-1533c6da1485'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260701233341_CreateSubscriptionModule'
)
BEGIN
    EXEC(N'UPDATE [RolePermissions] SET [CreatedAt] = ''2026-07-01T23:33:39.0585526Z''
    WHERE [Id] = ''94e3232d-330d-080b-3968-8c0b0422abd3'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260701233341_CreateSubscriptionModule'
)
BEGIN
    EXEC(N'UPDATE [RolePermissions] SET [CreatedAt] = ''2026-07-01T23:33:39.0585526Z''
    WHERE [Id] = ''95403e61-b5ef-2b2e-e6db-cb2875fdd651'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260701233341_CreateSubscriptionModule'
)
BEGIN
    EXEC(N'UPDATE [RolePermissions] SET [CreatedAt] = ''2026-07-01T23:33:39.0585526Z''
    WHERE [Id] = ''97577489-44d4-9fca-3e5a-a7ff99c25f60'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260701233341_CreateSubscriptionModule'
)
BEGIN
    EXEC(N'UPDATE [RolePermissions] SET [CreatedAt] = ''2026-07-01T23:33:39.0585526Z''
    WHERE [Id] = ''be8fe1d9-d5f7-fc0d-1c62-f23ec408ee0d'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260701233341_CreateSubscriptionModule'
)
BEGIN
    EXEC(N'UPDATE [RolePermissions] SET [CreatedAt] = ''2026-07-01T23:33:39.0585526Z''
    WHERE [Id] = ''c01cc128-784d-f6dd-8543-496e731fa31a'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260701233341_CreateSubscriptionModule'
)
BEGIN
    EXEC(N'UPDATE [RolePermissions] SET [CreatedAt] = ''2026-07-01T23:33:39.0585526Z''
    WHERE [Id] = ''c4522deb-c1b9-9234-005e-bdb3b1c5d30b'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260701233341_CreateSubscriptionModule'
)
BEGIN
    EXEC(N'UPDATE [RolePermissions] SET [CreatedAt] = ''2026-07-01T23:33:39.0585526Z''
    WHERE [Id] = ''ce45517e-5f0e-7330-9534-bae4dcee4905'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260701233341_CreateSubscriptionModule'
)
BEGIN
    EXEC(N'UPDATE [RolePermissions] SET [CreatedAt] = ''2026-07-01T23:33:39.0585526Z''
    WHERE [Id] = ''d487e031-bfa2-73dc-4031-273dbfae68c5'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260701233341_CreateSubscriptionModule'
)
BEGIN
    EXEC(N'UPDATE [RolePermissions] SET [CreatedAt] = ''2026-07-01T23:33:39.0585526Z''
    WHERE [Id] = ''dde6c68e-c7ec-19c2-65a8-d1c9abf4dbd1'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260701233341_CreateSubscriptionModule'
)
BEGIN
    EXEC(N'UPDATE [RolePermissions] SET [CreatedAt] = ''2026-07-01T23:33:39.0585526Z''
    WHERE [Id] = ''df7c114a-08b2-ca57-d10a-9458a9126cc7'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260701233341_CreateSubscriptionModule'
)
BEGIN
    EXEC(N'UPDATE [RolePermissions] SET [CreatedAt] = ''2026-07-01T23:33:39.0585526Z''
    WHERE [Id] = ''f6d95421-adde-78dc-a3e8-ada50506e534'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260701233341_CreateSubscriptionModule'
)
BEGIN
    EXEC(N'UPDATE [RolePermissions] SET [CreatedAt] = ''2026-07-01T23:33:39.0585526Z''
    WHERE [Id] = ''faed05e8-a7e0-30e1-7f2f-3f59a113fd18'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260701233341_CreateSubscriptionModule'
)
BEGIN
    EXEC(N'UPDATE [RolePermissions] SET [CreatedAt] = ''2026-07-01T23:33:39.0585526Z''
    WHERE [Id] = ''fb0338fb-fc95-ac51-0a37-7cc90b17ec50'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260701233341_CreateSubscriptionModule'
)
BEGIN
    EXEC(N'UPDATE [Roles] SET [CreatedAt] = ''2026-07-01T23:33:38.6093396Z''
    WHERE [Id] = ''a1b2c3d4-e5f6-7890-abcd-ef1234567890'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260701233341_CreateSubscriptionModule'
)
BEGIN
    EXEC(N'UPDATE [Roles] SET [CreatedAt] = ''2026-07-01T23:33:38.6094715Z''
    WHERE [Id] = ''b2c3d4e5-f6a7-8901-bcde-f12345678901'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260701233341_CreateSubscriptionModule'
)
BEGIN
    EXEC(N'UPDATE [Roles] SET [CreatedAt] = ''2026-07-01T23:33:38.6094717Z''
    WHERE [Id] = ''c3d4e5f6-a7b8-9012-cdef-123456789012'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260701233341_CreateSubscriptionModule'
)
BEGIN
    EXEC(N'UPDATE [Settings] SET [CreatedAt] = ''2026-07-01T23:33:39.0600565Z''
    WHERE [Id] = ''a5b6c7d8-e9f0-1234-a567-890123456789'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260701233341_CreateSubscriptionModule'
)
BEGIN
    EXEC(N'UPDATE [Settings] SET [CreatedAt] = ''2026-07-01T23:33:39.0600565Z''
    WHERE [Id] = ''b0c1d2e3-f4a5-6789-b012-345678901234'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260701233341_CreateSubscriptionModule'
)
BEGIN
    EXEC(N'UPDATE [Settings] SET [CreatedAt] = ''2026-07-01T23:33:39.0600565Z''
    WHERE [Id] = ''b6c7d8e9-f0a1-2345-b678-901234567890'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260701233341_CreateSubscriptionModule'
)
BEGIN
    EXEC(N'UPDATE [Settings] SET [CreatedAt] = ''2026-07-01T23:33:39.0600565Z''
    WHERE [Id] = ''c1d2e3f4-a5b6-7890-c123-456789012345'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260701233341_CreateSubscriptionModule'
)
BEGIN
    EXEC(N'UPDATE [Settings] SET [CreatedAt] = ''2026-07-01T23:33:39.0600565Z''
    WHERE [Id] = ''c7d8e9f0-a1b2-3456-c789-012345678901'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260701233341_CreateSubscriptionModule'
)
BEGIN
    EXEC(N'UPDATE [Settings] SET [CreatedAt] = ''2026-07-01T23:33:39.0600565Z''
    WHERE [Id] = ''d2e3f4a5-b6c7-8901-d234-567890123456'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260701233341_CreateSubscriptionModule'
)
BEGIN
    EXEC(N'UPDATE [Settings] SET [CreatedAt] = ''2026-07-01T23:33:39.0600565Z''
    WHERE [Id] = ''d8e9f0a1-b2c3-4567-d890-123456789012'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260701233341_CreateSubscriptionModule'
)
BEGIN
    EXEC(N'UPDATE [Settings] SET [CreatedAt] = ''2026-07-01T23:33:39.0600565Z''
    WHERE [Id] = ''e3f4a5b6-c7d8-9012-e345-678901234567'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260701233341_CreateSubscriptionModule'
)
BEGIN
    EXEC(N'UPDATE [Settings] SET [CreatedAt] = ''2026-07-01T23:33:39.0600565Z''
    WHERE [Id] = ''e9f0a1b2-c3d4-5678-e901-234567890123'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260701233341_CreateSubscriptionModule'
)
BEGIN
    EXEC(N'UPDATE [Settings] SET [CreatedAt] = ''2026-07-01T23:33:39.0600565Z''
    WHERE [Id] = ''f4a5b6c7-d8e9-0123-f456-789012345678'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260701233341_CreateSubscriptionModule'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''$2a$11$uMHEZbu/qmOybTH6otB6eumV0F1Qvs5TKpOo/Kh9KJlIspYFpm/ay''
    WHERE [Id] = ''d4e5f6a7-b8c9-0123-def4-567890123456'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260701233341_CreateSubscriptionModule'
)
BEGIN
    CREATE INDEX [IX_SubscriptionFreezeHistories_SubscriptionId] ON [SubscriptionFreezeHistories] ([SubscriptionId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260701233341_CreateSubscriptionModule'
)
BEGIN
    CREATE INDEX [IX_SubscriptionPayments_EmployeeId] ON [SubscriptionPayments] ([EmployeeId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260701233341_CreateSubscriptionModule'
)
BEGIN
    CREATE INDEX [IX_SubscriptionPayments_SubscriptionId] ON [SubscriptionPayments] ([SubscriptionId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260701233341_CreateSubscriptionModule'
)
BEGIN
    CREATE INDEX [IX_Subscriptions_MemberId_Status] ON [Subscriptions] ([MemberId], [Status]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260701233341_CreateSubscriptionModule'
)
BEGIN
    CREATE INDEX [IX_Subscriptions_OfferId] ON [Subscriptions] ([OfferId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260701233341_CreateSubscriptionModule'
)
BEGIN
    CREATE INDEX [IX_Subscriptions_PlanId] ON [Subscriptions] ([PlanId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260701233341_CreateSubscriptionModule'
)
BEGIN
    CREATE UNIQUE INDEX [IX_Subscriptions_ReceiptNumber] ON [Subscriptions] ([ReceiptNumber]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260701233341_CreateSubscriptionModule'
)
BEGIN
    CREATE INDEX [IX_Subscriptions_Status] ON [Subscriptions] ([Status]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260701233341_CreateSubscriptionModule'
)
BEGIN
    CREATE INDEX [IX_SubscriptionTransactionLogs_PerformedBy] ON [SubscriptionTransactionLogs] ([PerformedBy]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260701233341_CreateSubscriptionModule'
)
BEGIN
    CREATE INDEX [IX_SubscriptionTransactionLogs_SubscriptionId] ON [SubscriptionTransactionLogs] ([SubscriptionId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260701233341_CreateSubscriptionModule'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260701233341_CreateSubscriptionModule', N'10.0.9');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702002156_AddRolePermissionModule'
)
BEGIN
    EXEC(N'DELETE FROM [RolePermissions]
    WHERE [Id] = ''0cd25d71-346b-3908-42cf-d84115ab6e8b'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702002156_AddRolePermissionModule'
)
BEGIN
    EXEC(N'DELETE FROM [RolePermissions]
    WHERE [Id] = ''12205256-ff83-d1a7-ca36-ef013e4348dd'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702002156_AddRolePermissionModule'
)
BEGIN
    EXEC(N'DELETE FROM [RolePermissions]
    WHERE [Id] = ''18ca3a8f-4030-37b6-bb6c-51d7f78326bf'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702002156_AddRolePermissionModule'
)
BEGIN
    EXEC(N'DELETE FROM [RolePermissions]
    WHERE [Id] = ''1a447bea-0884-a7b8-5c95-9aa22876b650'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702002156_AddRolePermissionModule'
)
BEGIN
    EXEC(N'DELETE FROM [RolePermissions]
    WHERE [Id] = ''37fafc7e-cec4-f1e7-0639-0769adc78e8c'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702002156_AddRolePermissionModule'
)
BEGIN
    EXEC(N'DELETE FROM [RolePermissions]
    WHERE [Id] = ''38e68501-6aa4-2723-251e-83ab85145eda'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702002156_AddRolePermissionModule'
)
BEGIN
    EXEC(N'DELETE FROM [RolePermissions]
    WHERE [Id] = ''411c0941-f12c-e117-e542-7561547fae68'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702002156_AddRolePermissionModule'
)
BEGIN
    EXEC(N'DELETE FROM [RolePermissions]
    WHERE [Id] = ''578d3c75-7400-e248-983f-cb4d4f13fdd3'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702002156_AddRolePermissionModule'
)
BEGIN
    EXEC(N'DELETE FROM [RolePermissions]
    WHERE [Id] = ''618876f6-daa1-fc83-c047-480bf88c794e'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702002156_AddRolePermissionModule'
)
BEGIN
    EXEC(N'DELETE FROM [RolePermissions]
    WHERE [Id] = ''7b9944e2-4c52-0d53-1edd-ccd1d7316518'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702002156_AddRolePermissionModule'
)
BEGIN
    EXEC(N'DELETE FROM [RolePermissions]
    WHERE [Id] = ''7d73a719-0bf4-f70e-384e-ef6a71bdc88f'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702002156_AddRolePermissionModule'
)
BEGIN
    EXEC(N'DELETE FROM [RolePermissions]
    WHERE [Id] = ''7dc649b3-0e4d-a02d-25b2-1533c6da1485'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702002156_AddRolePermissionModule'
)
BEGIN
    EXEC(N'DELETE FROM [RolePermissions]
    WHERE [Id] = ''94e3232d-330d-080b-3968-8c0b0422abd3'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702002156_AddRolePermissionModule'
)
BEGIN
    EXEC(N'DELETE FROM [RolePermissions]
    WHERE [Id] = ''95403e61-b5ef-2b2e-e6db-cb2875fdd651'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702002156_AddRolePermissionModule'
)
BEGIN
    EXEC(N'DELETE FROM [RolePermissions]
    WHERE [Id] = ''97577489-44d4-9fca-3e5a-a7ff99c25f60'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702002156_AddRolePermissionModule'
)
BEGIN
    EXEC(N'DELETE FROM [RolePermissions]
    WHERE [Id] = ''be8fe1d9-d5f7-fc0d-1c62-f23ec408ee0d'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702002156_AddRolePermissionModule'
)
BEGIN
    EXEC(N'DELETE FROM [RolePermissions]
    WHERE [Id] = ''c01cc128-784d-f6dd-8543-496e731fa31a'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702002156_AddRolePermissionModule'
)
BEGIN
    EXEC(N'DELETE FROM [RolePermissions]
    WHERE [Id] = ''c4522deb-c1b9-9234-005e-bdb3b1c5d30b'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702002156_AddRolePermissionModule'
)
BEGIN
    EXEC(N'DELETE FROM [RolePermissions]
    WHERE [Id] = ''ce45517e-5f0e-7330-9534-bae4dcee4905'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702002156_AddRolePermissionModule'
)
BEGIN
    EXEC(N'DELETE FROM [RolePermissions]
    WHERE [Id] = ''d487e031-bfa2-73dc-4031-273dbfae68c5'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702002156_AddRolePermissionModule'
)
BEGIN
    EXEC(N'DELETE FROM [RolePermissions]
    WHERE [Id] = ''dde6c68e-c7ec-19c2-65a8-d1c9abf4dbd1'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702002156_AddRolePermissionModule'
)
BEGIN
    EXEC(N'DELETE FROM [RolePermissions]
    WHERE [Id] = ''df7c114a-08b2-ca57-d10a-9458a9126cc7'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702002156_AddRolePermissionModule'
)
BEGIN
    EXEC(N'DELETE FROM [RolePermissions]
    WHERE [Id] = ''f6d95421-adde-78dc-a3e8-ada50506e534'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702002156_AddRolePermissionModule'
)
BEGIN
    EXEC(N'DELETE FROM [RolePermissions]
    WHERE [Id] = ''faed05e8-a7e0-30e1-7f2f-3f59a113fd18'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702002156_AddRolePermissionModule'
)
BEGIN
    EXEC(N'DELETE FROM [RolePermissions]
    WHERE [Id] = ''fb0338fb-fc95-ac51-0a37-7cc90b17ec50'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702002156_AddRolePermissionModule'
)
BEGIN
    EXEC(N'DELETE FROM [Permissions]
    WHERE [Id] = ''a3b4c5d6-e7f8-9012-a345-678901234567'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702002156_AddRolePermissionModule'
)
BEGIN
    EXEC(N'DELETE FROM [Permissions]
    WHERE [Id] = ''a7b8c9d0-e1f2-3456-a789-012345678901'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702002156_AddRolePermissionModule'
)
BEGIN
    EXEC(N'DELETE FROM [Permissions]
    WHERE [Id] = ''a9b0c1d2-e3f4-5678-a901-234567890123'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702002156_AddRolePermissionModule'
)
BEGIN
    EXEC(N'DELETE FROM [Permissions]
    WHERE [Id] = ''b4c5d6e7-f8a9-0123-b456-789012345678'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702002156_AddRolePermissionModule'
)
BEGIN
    EXEC(N'DELETE FROM [Permissions]
    WHERE [Id] = ''b8c9d0e1-f2a3-4567-b890-123456789012'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702002156_AddRolePermissionModule'
)
BEGIN
    EXEC(N'DELETE FROM [Permissions]
    WHERE [Id] = ''c5d6e7f8-a9b0-1234-c567-890123456789'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702002156_AddRolePermissionModule'
)
BEGIN
    EXEC(N'DELETE FROM [Permissions]
    WHERE [Id] = ''c9d0e1f2-a3b4-5678-c901-234567890123'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702002156_AddRolePermissionModule'
)
BEGIN
    EXEC(N'DELETE FROM [Permissions]
    WHERE [Id] = ''d0e1f2a3-b4c5-6789-d012-345678901234'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702002156_AddRolePermissionModule'
)
BEGIN
    EXEC(N'DELETE FROM [Permissions]
    WHERE [Id] = ''d1e2f3a4-b5c6-7890-d123-456789012345'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702002156_AddRolePermissionModule'
)
BEGIN
    EXEC(N'DELETE FROM [Permissions]
    WHERE [Id] = ''d6e7f8a9-b0c1-2345-d678-901234567890'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702002156_AddRolePermissionModule'
)
BEGIN
    EXEC(N'DELETE FROM [Permissions]
    WHERE [Id] = ''e1f2a3b4-c5d6-7890-e123-456789012345'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702002156_AddRolePermissionModule'
)
BEGIN
    EXEC(N'DELETE FROM [Permissions]
    WHERE [Id] = ''e5f6a7b8-c9d0-1234-ef56-789012345678'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702002156_AddRolePermissionModule'
)
BEGIN
    EXEC(N'DELETE FROM [Permissions]
    WHERE [Id] = ''e7f8a9b0-c1d2-3456-e789-012345678901'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702002156_AddRolePermissionModule'
)
BEGIN
    EXEC(N'DELETE FROM [Permissions]
    WHERE [Id] = ''f2a3b4c5-d6e7-8901-f234-567890123456'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702002156_AddRolePermissionModule'
)
BEGIN
    EXEC(N'DELETE FROM [Permissions]
    WHERE [Id] = ''f6a7b8c9-d0e1-2345-f678-901234567890'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702002156_AddRolePermissionModule'
)
BEGIN
    EXEC(N'DELETE FROM [Permissions]
    WHERE [Id] = ''f8a9b0c1-d2e3-4567-f890-123456789012'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702002156_AddRolePermissionModule'
)
BEGIN
    ALTER TABLE [Roles] ADD [IsActive] bit NOT NULL DEFAULT CAST(0 AS bit);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702002156_AddRolePermissionModule'
)
BEGIN
    ALTER TABLE [Permissions] ADD [Module] nvarchar(max) NOT NULL DEFAULT N'';
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702002156_AddRolePermissionModule'
)
BEGIN
    CREATE TABLE [PermissionAuditLogs] (
        [Id] uniqueidentifier NOT NULL,
        [UserId] uniqueidentifier NULL,
        [Action] nvarchar(max) NOT NULL,
        [RoleName] nvarchar(max) NOT NULL,
        [OldPermissions] nvarchar(max) NULL,
        [NewPermissions] nvarchar(max) NULL,
        [IpAddress] nvarchar(max) NULL,
        [CreatedAt] datetime2 NOT NULL,
        [UpdatedAt] datetime2 NULL,
        CONSTRAINT [PK_PermissionAuditLogs] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_PermissionAuditLogs_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702002156_AddRolePermissionModule'
)
BEGIN
    CREATE TABLE [WhatsAppTemplates] (
        [Id] uniqueidentifier NOT NULL,
        [Name] nvarchar(200) NOT NULL,
        [MessageBody] nvarchar(2000) NOT NULL,
        [IsActive] bit NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        [UpdatedAt] datetime2 NULL,
        CONSTRAINT [PK_WhatsAppTemplates] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702002156_AddRolePermissionModule'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'CreatedAt', N'Description', N'Module', N'Name', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[Permissions]'))
        SET IDENTITY_INSERT [Permissions] ON;
    EXEC(N'INSERT INTO [Permissions] ([Id], [CreatedAt], [Description], [Module], [Name], [UpdatedAt])
    VALUES (''d1000001-0000-0000-0000-000000000001'', ''2026-07-02T00:21:54.4992200Z'', N''View main dashboard'', N''Dashboard'', N''Dashboard.View'', NULL),
    (''d1000001-0000-0000-0000-000000000002'', ''2026-07-02T00:21:54.4992200Z'', N''View revenue dashboard'', N''Dashboard'', N''Dashboard.Revenue'', NULL),
    (''d1000002-0000-0000-0000-000000000001'', ''2026-07-02T00:21:54.4992200Z'', N''View members list'', N''Members'', N''Members.View'', NULL),
    (''d1000002-0000-0000-0000-000000000002'', ''2026-07-02T00:21:54.4992200Z'', N''Create new members'', N''Members'', N''Members.Create'', NULL),
    (''d1000002-0000-0000-0000-000000000003'', ''2026-07-02T00:21:54.4992200Z'', N''Edit existing members'', N''Members'', N''Members.Edit'', NULL),
    (''d1000002-0000-0000-0000-000000000004'', ''2026-07-02T00:21:54.4992200Z'', N''Delete members'', N''Members'', N''Members.Delete'', NULL),
    (''d1000003-0000-0000-0000-000000000001'', ''2026-07-02T00:21:54.4992200Z'', N''View plans'', N''Plans'', N''Plans.View'', NULL),
    (''d1000003-0000-0000-0000-000000000002'', ''2026-07-02T00:21:54.4992200Z'', N''Create plans'', N''Plans'', N''Plans.Create'', NULL),
    (''d1000003-0000-0000-0000-000000000003'', ''2026-07-02T00:21:54.4992200Z'', N''Edit plans'', N''Plans'', N''Plans.Edit'', NULL),
    (''d1000003-0000-0000-0000-000000000004'', ''2026-07-02T00:21:54.4992200Z'', N''Delete plans'', N''Plans'', N''Plans.Delete'', NULL),
    (''d1000004-0000-0000-0000-000000000001'', ''2026-07-02T00:21:54.4992200Z'', N''View offers'', N''Offers'', N''Offers.View'', NULL),
    (''d1000004-0000-0000-0000-000000000002'', ''2026-07-02T00:21:54.4992200Z'', N''Create offers'', N''Offers'', N''Offers.Create'', NULL),
    (''d1000004-0000-0000-0000-000000000003'', ''2026-07-02T00:21:54.4992200Z'', N''Edit offers'', N''Offers'', N''Offers.Edit'', NULL),
    (''d1000004-0000-0000-0000-000000000004'', ''2026-07-02T00:21:54.4992200Z'', N''Delete offers'', N''Offers'', N''Offers.Delete'', NULL),
    (''d1000005-0000-0000-0000-000000000001'', ''2026-07-02T00:21:54.4992200Z'', N''View subscriptions'', N''Subscriptions'', N''Subscriptions.View'', NULL),
    (''d1000005-0000-0000-0000-000000000002'', ''2026-07-02T00:21:54.4992200Z'', N''Create subscriptions'', N''Subscriptions'', N''Subscriptions.Create'', NULL),
    (''d1000005-0000-0000-0000-000000000003'', ''2026-07-02T00:21:54.4992200Z'', N''Edit subscriptions'', N''Subscriptions'', N''Subscriptions.Edit'', NULL),
    (''d1000005-0000-0000-0000-000000000004'', ''2026-07-02T00:21:54.4992200Z'', N''Freeze subscriptions'', N''Subscriptions'', N''Subscriptions.Freeze'', NULL),
    (''d1000005-0000-0000-0000-000000000005'', ''2026-07-02T00:21:54.4992200Z'', N''Unfreeze subscriptions'', N''Subscriptions'', N''Subscriptions.Unfreeze'', NULL),
    (''d1000005-0000-0000-0000-000000000006'', ''2026-07-02T00:21:54.4992200Z'', N''Renew subscriptions'', N''Subscriptions'', N''Subscriptions.Renew'', NULL),
    (''d1000005-0000-0000-0000-000000000007'', ''2026-07-02T00:21:54.4992200Z'', N''View payment history'', N''Subscriptions'', N''Subscriptions.PaymentHistory'', NULL),
    (''d1000006-0000-0000-0000-000000000001'', ''2026-07-02T00:21:54.4992200Z'', N''View leads'', N''Leads'', N''Leads.View'', NULL),
    (''d1000006-0000-0000-0000-000000000002'', ''2026-07-02T00:21:54.4992200Z'', N''Create leads'', N''Leads'', N''Leads.Create'', NULL),
    (''d1000006-0000-0000-0000-000000000003'', ''2026-07-02T00:21:54.4992200Z'', N''Edit leads'', N''Leads'', N''Leads.Edit'', NULL),
    (''d1000006-0000-0000-0000-000000000004'', ''2026-07-02T00:21:54.4992200Z'', N''Convert leads to members'', N''Leads'', N''Leads.Convert'', NULL),
    (''d1000007-0000-0000-0000-000000000001'', ''2026-07-02T00:21:54.4992200Z'', N''View attendance'', N''Attendance'', N''Attendance.View'', NULL),
    (''d1000007-0000-0000-0000-000000000002'', ''2026-07-02T00:21:54.4992200Z'', N''Export attendance'', N''Attendance'', N''Attendance.Export'', NULL),
    (''d1000008-0000-0000-0000-000000000001'', ''2026-07-02T00:21:54.4992200Z'', N''Send WhatsApp messages'', N''WhatsApp'', N''WhatsApp.Send'', NULL),
    (''d1000008-0000-0000-0000-000000000002'', ''2026-07-02T00:21:54.4992200Z'', N''Manual broadcast via WhatsApp'', N''WhatsApp'', N''WhatsApp.Broadcast'', NULL),
    (''d1000009-0000-0000-0000-000000000001'', ''2026-07-02T00:21:54.4992200Z'', N''Import data'', N''Import/Export'', N''ImportExport.Import'', NULL),
    (''d1000009-0000-0000-0000-000000000002'', ''2026-07-02T00:21:54.4992200Z'', N''Export data'', N''Import/Export'', N''ImportExport.Export'', NULL),
    (''d1000010-0000-0000-0000-000000000001'', ''2026-07-02T00:21:54.4992200Z'', N''View settings'', N''Settings'', N''Settings.View'', NULL),
    (''d1000010-0000-0000-0000-000000000002'', ''2026-07-02T00:21:54.4992200Z'', N''Edit settings'', N''Settings'', N''Settings.Edit'', NULL),
    (''d1000011-0000-0000-0000-000000000001'', ''2026-07-02T00:21:54.4992200Z'', N''View users'', N''User Management'', N''Users.View'', NULL),
    (''d1000011-0000-0000-0000-000000000002'', ''2026-07-02T00:21:54.4992200Z'', N''Create users'', N''User Management'', N''Users.Create'', NULL),
    (''d1000011-0000-0000-0000-000000000003'', ''2026-07-02T00:21:54.4992200Z'', N''Edit users'', N''User Management'', N''Users.Edit'', NULL),
    (''d1000011-0000-0000-0000-000000000004'', ''2026-07-02T00:21:54.4992200Z'', N''Delete users'', N''User Management'', N''Users.Delete'', NULL),
    (''d1000012-0000-0000-0000-000000000001'', ''2026-07-02T00:21:54.4992200Z'', N''View roles & permissions'', N''Roles & Permissions'', N''Roles.View'', NULL),
    (''d1000012-0000-0000-0000-000000000002'', ''2026-07-02T00:21:54.4992200Z'', N''Create roles'', N''Roles & Permissions'', N''Roles.Create'', NULL),
    (''d1000012-0000-0000-0000-000000000003'', ''2026-07-02T00:21:54.4992200Z'', N''Edit roles'', N''Roles & Permissions'', N''Roles.Edit'', NULL),
    (''d1000012-0000-0000-0000-000000000004'', ''2026-07-02T00:21:54.4992200Z'', N''Delete roles'', N''Roles & Permissions'', N''Roles.Delete'', NULL),
    (''d1000013-0000-0000-0000-000000000001'', ''2026-07-02T00:21:54.4992200Z'', N''View devices'', N''Devices'', N''Devices.View'', NULL);
    INSERT INTO [Permissions] ([Id], [CreatedAt], [Description], [Module], [Name], [UpdatedAt])
    VALUES (''d1000013-0000-0000-0000-000000000002'', ''2026-07-02T00:21:54.4992200Z'', N''Manage devices'', N''Devices'', N''Devices.Manage'', NULL),
    (''d1000014-0000-0000-0000-000000000001'', ''2026-07-02T00:21:54.4992200Z'', N''Manage backups'', N''Backup'', N''Backup.Manage'', NULL),
    (''d1000015-0000-0000-0000-000000000001'', ''2026-07-02T00:21:54.4992200Z'', N''View notifications'', N''Notifications'', N''Notifications.View'', NULL),
    (''d1000015-0000-0000-0000-000000000002'', ''2026-07-02T00:21:54.4992200Z'', N''Send notifications'', N''Notifications'', N''Notifications.Send'', NULL),
    (''d1000016-0000-0000-0000-000000000001'', ''2026-07-02T00:21:54.4992200Z'', N''View memberships'', N''Memberships'', N''Memberships.View'', NULL),
    (''d1000016-0000-0000-0000-000000000002'', ''2026-07-02T00:21:54.4992200Z'', N''Manage memberships'', N''Memberships'', N''Memberships.Manage'', NULL)');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'CreatedAt', N'Description', N'Module', N'Name', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[Permissions]'))
        SET IDENTITY_INSERT [Permissions] OFF;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702002156_AddRolePermissionModule'
)
BEGIN
    EXEC(N'UPDATE [Roles] SET [CreatedAt] = ''2026-07-02T00:21:53.9911928Z'', [IsActive] = CAST(1 AS bit)
    WHERE [Id] = ''a1b2c3d4-e5f6-7890-abcd-ef1234567890'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702002156_AddRolePermissionModule'
)
BEGIN
    EXEC(N'UPDATE [Roles] SET [CreatedAt] = ''2026-07-02T00:21:53.9911928Z'', [IsActive] = CAST(1 AS bit)
    WHERE [Id] = ''b2c3d4e5-f6a7-8901-bcde-f12345678901'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702002156_AddRolePermissionModule'
)
BEGIN
    EXEC(N'UPDATE [Roles] SET [CreatedAt] = ''2026-07-02T00:21:53.9911928Z'', [IsActive] = CAST(0 AS bit)
    WHERE [Id] = ''c3d4e5f6-a7b8-9012-cdef-123456789012'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702002156_AddRolePermissionModule'
)
BEGIN
    EXEC(N'UPDATE [Settings] SET [CreatedAt] = ''2026-07-02T00:21:54.5061853Z''
    WHERE [Id] = ''a5b6c7d8-e9f0-1234-a567-890123456789'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702002156_AddRolePermissionModule'
)
BEGIN
    EXEC(N'UPDATE [Settings] SET [CreatedAt] = ''2026-07-02T00:21:54.5061853Z''
    WHERE [Id] = ''b0c1d2e3-f4a5-6789-b012-345678901234'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702002156_AddRolePermissionModule'
)
BEGIN
    EXEC(N'UPDATE [Settings] SET [CreatedAt] = ''2026-07-02T00:21:54.5061853Z''
    WHERE [Id] = ''b6c7d8e9-f0a1-2345-b678-901234567890'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702002156_AddRolePermissionModule'
)
BEGIN
    EXEC(N'UPDATE [Settings] SET [CreatedAt] = ''2026-07-02T00:21:54.5061853Z''
    WHERE [Id] = ''c1d2e3f4-a5b6-7890-c123-456789012345'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702002156_AddRolePermissionModule'
)
BEGIN
    EXEC(N'UPDATE [Settings] SET [CreatedAt] = ''2026-07-02T00:21:54.5061853Z''
    WHERE [Id] = ''c7d8e9f0-a1b2-3456-c789-012345678901'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702002156_AddRolePermissionModule'
)
BEGIN
    EXEC(N'UPDATE [Settings] SET [CreatedAt] = ''2026-07-02T00:21:54.5061853Z''
    WHERE [Id] = ''d2e3f4a5-b6c7-8901-d234-567890123456'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702002156_AddRolePermissionModule'
)
BEGIN
    EXEC(N'UPDATE [Settings] SET [CreatedAt] = ''2026-07-02T00:21:54.5061853Z''
    WHERE [Id] = ''d8e9f0a1-b2c3-4567-d890-123456789012'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702002156_AddRolePermissionModule'
)
BEGIN
    EXEC(N'UPDATE [Settings] SET [CreatedAt] = ''2026-07-02T00:21:54.5061853Z''
    WHERE [Id] = ''e3f4a5b6-c7d8-9012-e345-678901234567'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702002156_AddRolePermissionModule'
)
BEGIN
    EXEC(N'UPDATE [Settings] SET [CreatedAt] = ''2026-07-02T00:21:54.5061853Z''
    WHERE [Id] = ''e9f0a1b2-c3d4-5678-e901-234567890123'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702002156_AddRolePermissionModule'
)
BEGIN
    EXEC(N'UPDATE [Settings] SET [CreatedAt] = ''2026-07-02T00:21:54.5061853Z''
    WHERE [Id] = ''f4a5b6c7-d8e9-0123-f456-789012345678'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702002156_AddRolePermissionModule'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''$2a$11$UwdzwSSzWIuGgiWmmhFNseBNrLh6GgbrXIhfdsMiZZEkmN5LRPQg.''
    WHERE [Id] = ''d4e5f6a7-b8c9-0123-def4-567890123456'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702002156_AddRolePermissionModule'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'CreatedAt', N'PermissionId', N'RoleId', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[RolePermissions]'))
        SET IDENTITY_INSERT [RolePermissions] ON;
    EXEC(N'INSERT INTO [RolePermissions] ([Id], [CreatedAt], [PermissionId], [RoleId], [UpdatedAt])
    VALUES (''005f9b64-4d0a-6c82-947e-e57cfe738452'', ''2026-07-02T00:21:54.5029626Z'', ''d1000012-0000-0000-0000-000000000003'', ''a1b2c3d4-e5f6-7890-abcd-ef1234567890'', NULL),
    (''05195fdb-cd30-4561-9168-bbeb6f886a69'', ''2026-07-02T00:21:54.5029626Z'', ''d1000006-0000-0000-0000-000000000003'', ''b2c3d4e5-f6a7-8901-bcde-f12345678901'', NULL),
    (''0a025850-9197-728d-2039-14fa13c954ce'', ''2026-07-02T00:21:54.5029626Z'', ''d1000006-0000-0000-0000-000000000004'', ''b2c3d4e5-f6a7-8901-bcde-f12345678901'', NULL),
    (''0e32d4f0-e31f-1692-6763-43d06de36c0e'', ''2026-07-02T00:21:54.5029626Z'', ''d1000012-0000-0000-0000-000000000002'', ''a1b2c3d4-e5f6-7890-abcd-ef1234567890'', NULL),
    (''1def5b70-20cb-7e7b-b398-51bae635cb4c'', ''2026-07-02T00:21:54.5029626Z'', ''d1000011-0000-0000-0000-000000000002'', ''a1b2c3d4-e5f6-7890-abcd-ef1234567890'', NULL),
    (''2babdd1d-2af8-85c1-be3d-93660af5d9ca'', ''2026-07-02T00:21:54.5029626Z'', ''d1000001-0000-0000-0000-000000000001'', ''a1b2c3d4-e5f6-7890-abcd-ef1234567890'', NULL),
    (''2cc74796-4a71-5342-2d08-a37e9154996d'', ''2026-07-02T00:21:54.5029626Z'', ''d1000002-0000-0000-0000-000000000001'', ''a1b2c3d4-e5f6-7890-abcd-ef1234567890'', NULL),
    (''32c5144e-c485-1183-98b8-403fbb277ebd'', ''2026-07-02T00:21:54.5029626Z'', ''d1000016-0000-0000-0000-000000000001'', ''b2c3d4e5-f6a7-8901-bcde-f12345678901'', NULL),
    (''35837f97-2623-ab10-0cbd-370671326f89'', ''2026-07-02T00:21:54.5029626Z'', ''d1000002-0000-0000-0000-000000000004'', ''a1b2c3d4-e5f6-7890-abcd-ef1234567890'', NULL),
    (''3c0e6bfe-9913-56af-20cb-cc7f4e36a4a6'', ''2026-07-02T00:21:54.5029626Z'', ''d1000015-0000-0000-0000-000000000002'', ''a1b2c3d4-e5f6-7890-abcd-ef1234567890'', NULL),
    (''430af60c-cfb8-d59a-e833-8cd110ff7f06'', ''2026-07-02T00:21:54.5029626Z'', ''d1000005-0000-0000-0000-000000000004'', ''b2c3d4e5-f6a7-8901-bcde-f12345678901'', NULL),
    (''43fe5635-7b4d-ab3d-a51e-3559aced41c5'', ''2026-07-02T00:21:54.5029626Z'', ''d1000005-0000-0000-0000-000000000001'', ''a1b2c3d4-e5f6-7890-abcd-ef1234567890'', NULL),
    (''45a0ab26-2f4b-7f21-a073-171f907faf77'', ''2026-07-02T00:21:54.5029626Z'', ''d1000002-0000-0000-0000-000000000002'', ''b2c3d4e5-f6a7-8901-bcde-f12345678901'', NULL),
    (''477c3b9c-bdb3-a703-0b56-f6516dcfc7e5'', ''2026-07-02T00:21:54.5029626Z'', ''d1000009-0000-0000-0000-000000000001'', ''b2c3d4e5-f6a7-8901-bcde-f12345678901'', NULL),
    (''48e05a34-1b30-1cdc-ca0d-974cb2c3ebd8'', ''2026-07-02T00:21:54.5029626Z'', ''d1000001-0000-0000-0000-000000000001'', ''b2c3d4e5-f6a7-8901-bcde-f12345678901'', NULL),
    (''4d02d95c-4aab-3930-8c9f-b46c21d1be4e'', ''2026-07-02T00:21:54.5029626Z'', ''d1000005-0000-0000-0000-000000000002'', ''a1b2c3d4-e5f6-7890-abcd-ef1234567890'', NULL),
    (''5334b704-3d25-6b96-517f-6443fb11c414'', ''2026-07-02T00:21:54.5029626Z'', ''d1000005-0000-0000-0000-000000000004'', ''a1b2c3d4-e5f6-7890-abcd-ef1234567890'', NULL),
    (''55690f65-12c3-9922-c4b1-47b34caedde2'', ''2026-07-02T00:21:54.5029626Z'', ''d1000002-0000-0000-0000-000000000003'', ''b2c3d4e5-f6a7-8901-bcde-f12345678901'', NULL),
    (''557952bd-50d0-d062-f35e-7fe6224bd482'', ''2026-07-02T00:21:54.5029626Z'', ''d1000002-0000-0000-0000-000000000003'', ''a1b2c3d4-e5f6-7890-abcd-ef1234567890'', NULL),
    (''566928b3-00dc-fb90-39b7-af77eb0f35b1'', ''2026-07-02T00:21:54.5029626Z'', ''d1000006-0000-0000-0000-000000000001'', ''b2c3d4e5-f6a7-8901-bcde-f12345678901'', NULL),
    (''5f9b5de3-e563-a2d9-672a-91816ba23dd6'', ''2026-07-02T00:21:54.5029626Z'', ''d1000006-0000-0000-0000-000000000001'', ''a1b2c3d4-e5f6-7890-abcd-ef1234567890'', NULL),
    (''63139b75-fbfc-2bcf-36d2-43bc185b9a05'', ''2026-07-02T00:21:54.5029626Z'', ''d1000003-0000-0000-0000-000000000002'', ''a1b2c3d4-e5f6-7890-abcd-ef1234567890'', NULL),
    (''6604e362-f7af-bb42-e9e1-448a76c8e188'', ''2026-07-02T00:21:54.5029626Z'', ''d1000008-0000-0000-0000-000000000002'', ''a1b2c3d4-e5f6-7890-abcd-ef1234567890'', NULL),
    (''6a745601-638c-730b-cda2-d7a67e9f5a16'', ''2026-07-02T00:21:54.5029626Z'', ''d1000005-0000-0000-0000-000000000007'', ''b2c3d4e5-f6a7-8901-bcde-f12345678901'', NULL),
    (''6b5ac608-95b1-cd3e-d869-0fdf79a09fd0'', ''2026-07-02T00:21:54.5029626Z'', ''d1000012-0000-0000-0000-000000000004'', ''a1b2c3d4-e5f6-7890-abcd-ef1234567890'', NULL),
    (''6b6ec0d1-eb14-8d88-e615-78e6fe6177bf'', ''2026-07-02T00:21:54.5029626Z'', ''d1000007-0000-0000-0000-000000000002'', ''a1b2c3d4-e5f6-7890-abcd-ef1234567890'', NULL),
    (''70ec0ef2-a390-97c2-76ef-1678f6cf8fbf'', ''2026-07-02T00:21:54.5029626Z'', ''d1000001-0000-0000-0000-000000000002'', ''a1b2c3d4-e5f6-7890-abcd-ef1234567890'', NULL),
    (''7297595c-0a29-eb0d-19be-c531ee673c5b'', ''2026-07-02T00:21:54.5029626Z'', ''d1000005-0000-0000-0000-000000000005'', ''b2c3d4e5-f6a7-8901-bcde-f12345678901'', NULL),
    (''72bf7e60-3016-f5ff-ec42-ddfc1e27acfc'', ''2026-07-02T00:21:54.5029626Z'', ''d1000012-0000-0000-0000-000000000001'', ''a1b2c3d4-e5f6-7890-abcd-ef1234567890'', NULL),
    (''7347301a-7584-8075-4aab-788b19078a62'', ''2026-07-02T00:21:54.5029626Z'', ''d1000002-0000-0000-0000-000000000002'', ''a1b2c3d4-e5f6-7890-abcd-ef1234567890'', NULL),
    (''78615f0f-b666-f010-e57b-dfb05c66f035'', ''2026-07-02T00:21:54.5029626Z'', ''d1000001-0000-0000-0000-000000000001'', ''c3d4e5f6-a7b8-9012-cdef-123456789012'', NULL),
    (''78b53d72-0ba7-6249-68a8-bedd284304f4'', ''2026-07-02T00:21:54.5029626Z'', ''d1000010-0000-0000-0000-000000000001'', ''a1b2c3d4-e5f6-7890-abcd-ef1234567890'', NULL),
    (''7916078f-5d85-86e1-8747-b5dd207f40b5'', ''2026-07-02T00:21:54.5029626Z'', ''d1000005-0000-0000-0000-000000000001'', ''b2c3d4e5-f6a7-8901-bcde-f12345678901'', NULL),
    (''82cdc680-60c7-ca13-1fba-0c1725b381fa'', ''2026-07-02T00:21:54.5029626Z'', ''d1000006-0000-0000-0000-000000000004'', ''a1b2c3d4-e5f6-7890-abcd-ef1234567890'', NULL),
    (''85bf1ab4-8b34-6fb0-9ad7-cbbd215d0949'', ''2026-07-02T00:21:54.5029626Z'', ''d1000009-0000-0000-0000-000000000002'', ''a1b2c3d4-e5f6-7890-abcd-ef1234567890'', NULL),
    (''8680abc3-7a3f-fc42-ec4b-282165955f1c'', ''2026-07-02T00:21:54.5029626Z'', ''d1000005-0000-0000-0000-000000000002'', ''b2c3d4e5-f6a7-8901-bcde-f12345678901'', NULL),
    (''86f4523e-677d-889c-8e00-7ee5a50d94c5'', ''2026-07-02T00:21:54.5029626Z'', ''d1000006-0000-0000-0000-000000000002'', ''a1b2c3d4-e5f6-7890-abcd-ef1234567890'', NULL),
    (''89d0bc79-33f4-2a4d-f951-da329c328c20'', ''2026-07-02T00:21:54.5029626Z'', ''d1000016-0000-0000-0000-000000000002'', ''a1b2c3d4-e5f6-7890-abcd-ef1234567890'', NULL),
    (''8ef92960-4f9f-70c2-58b7-a0fc32743d95'', ''2026-07-02T00:21:54.5029626Z'', ''d1000009-0000-0000-0000-000000000002'', ''b2c3d4e5-f6a7-8901-bcde-f12345678901'', NULL),
    (''904ee737-7848-bded-ec47-ed74c6b3b5e5'', ''2026-07-02T00:21:54.5029626Z'', ''d1000015-0000-0000-0000-000000000002'', ''b2c3d4e5-f6a7-8901-bcde-f12345678901'', NULL),
    (''9cbf51af-d816-7ca8-28cf-67d124fb62c0'', ''2026-07-02T00:21:54.5029626Z'', ''d1000005-0000-0000-0000-000000000003'', ''a1b2c3d4-e5f6-7890-abcd-ef1234567890'', NULL),
    (''a54e820b-654c-d289-f188-11d672a9a217'', ''2026-07-02T00:21:54.5029626Z'', ''d1000010-0000-0000-0000-000000000002'', ''a1b2c3d4-e5f6-7890-abcd-ef1234567890'', NULL);
    INSERT INTO [RolePermissions] ([Id], [CreatedAt], [PermissionId], [RoleId], [UpdatedAt])
    VALUES (''a5574f00-d065-186a-8630-9c6de44235eb'', ''2026-07-02T00:21:54.5029626Z'', ''d1000016-0000-0000-0000-000000000001'', ''a1b2c3d4-e5f6-7890-abcd-ef1234567890'', NULL),
    (''a9035ef4-18c2-3e13-d5cc-99fc5e63ed31'', ''2026-07-02T00:21:54.5029626Z'', ''d1000006-0000-0000-0000-000000000002'', ''b2c3d4e5-f6a7-8901-bcde-f12345678901'', NULL),
    (''ace9a66b-d17d-2b50-17b3-67eb40dcf41a'', ''2026-07-02T00:21:54.5029626Z'', ''d1000011-0000-0000-0000-000000000004'', ''a1b2c3d4-e5f6-7890-abcd-ef1234567890'', NULL),
    (''ad012924-5237-6205-38fe-d48cedc33d4d'', ''2026-07-02T00:21:54.5029626Z'', ''d1000003-0000-0000-0000-000000000004'', ''a1b2c3d4-e5f6-7890-abcd-ef1234567890'', NULL),
    (''b2f04998-b570-47d2-8236-0756a4e219e2'', ''2026-07-02T00:21:54.5029626Z'', ''d1000003-0000-0000-0000-000000000001'', ''a1b2c3d4-e5f6-7890-abcd-ef1234567890'', NULL),
    (''b4e93b8d-5dc3-24cf-da95-fb3403b3749e'', ''2026-07-02T00:21:54.5029626Z'', ''d1000008-0000-0000-0000-000000000001'', ''a1b2c3d4-e5f6-7890-abcd-ef1234567890'', NULL),
    (''b5e24ad4-8420-94b6-b8fc-9dc56e2a445a'', ''2026-07-02T00:21:54.5029626Z'', ''d1000004-0000-0000-0000-000000000003'', ''a1b2c3d4-e5f6-7890-abcd-ef1234567890'', NULL),
    (''b94ea0da-6b9a-ea4f-6ac8-08b4e88272fe'', ''2026-07-02T00:21:54.5029626Z'', ''d1000005-0000-0000-0000-000000000007'', ''a1b2c3d4-e5f6-7890-abcd-ef1234567890'', NULL),
    (''bbe26c6f-807b-dba2-eef3-33cd059b3296'', ''2026-07-02T00:21:54.5029626Z'', ''d1000004-0000-0000-0000-000000000001'', ''a1b2c3d4-e5f6-7890-abcd-ef1234567890'', NULL),
    (''bc79a3c2-f98e-9c5b-5c26-3072265353bc'', ''2026-07-02T00:21:54.5029626Z'', ''d1000014-0000-0000-0000-000000000001'', ''a1b2c3d4-e5f6-7890-abcd-ef1234567890'', NULL),
    (''bde31dac-221c-d02b-fca6-51fa576f9b89'', ''2026-07-02T00:21:54.5029626Z'', ''d1000008-0000-0000-0000-000000000001'', ''b2c3d4e5-f6a7-8901-bcde-f12345678901'', NULL),
    (''bf6ac31e-ca1d-b678-8e6e-3cc7b7dac946'', ''2026-07-02T00:21:54.5029626Z'', ''d1000015-0000-0000-0000-000000000001'', ''a1b2c3d4-e5f6-7890-abcd-ef1234567890'', NULL),
    (''bf70e113-e074-6b40-6534-fb4c1f9231d3'', ''2026-07-02T00:21:54.5029626Z'', ''d1000013-0000-0000-0000-000000000001'', ''a1b2c3d4-e5f6-7890-abcd-ef1234567890'', NULL),
    (''c175a900-6ce3-e698-94df-f33b4ed6bb50'', ''2026-07-02T00:21:54.5029626Z'', ''d1000007-0000-0000-0000-000000000001'', ''a1b2c3d4-e5f6-7890-abcd-ef1234567890'', NULL),
    (''c1a0d8cb-6b48-74ec-9919-d577f704c514'', ''2026-07-02T00:21:54.5029626Z'', ''d1000011-0000-0000-0000-000000000003'', ''a1b2c3d4-e5f6-7890-abcd-ef1234567890'', NULL),
    (''c24d281f-89aa-dd6c-c264-59d5b5ba0faa'', ''2026-07-02T00:21:54.5029626Z'', ''d1000011-0000-0000-0000-000000000001'', ''a1b2c3d4-e5f6-7890-abcd-ef1234567890'', NULL),
    (''ce25f18c-ed21-3f6b-8dd1-82406f135d61'', ''2026-07-02T00:21:54.5029626Z'', ''d1000005-0000-0000-0000-000000000005'', ''a1b2c3d4-e5f6-7890-abcd-ef1234567890'', NULL),
    (''ceadb402-0bb1-ea6e-7525-16b7a2ba8317'', ''2026-07-02T00:21:54.5029626Z'', ''d1000004-0000-0000-0000-000000000002'', ''a1b2c3d4-e5f6-7890-abcd-ef1234567890'', NULL),
    (''d138659a-966d-09d0-2309-f233f3f232d1'', ''2026-07-02T00:21:54.5029626Z'', ''d1000005-0000-0000-0000-000000000006'', ''b2c3d4e5-f6a7-8901-bcde-f12345678901'', NULL),
    (''d5c2ad14-559d-e8a6-0702-d8c9f26af634'', ''2026-07-02T00:21:54.5029626Z'', ''d1000005-0000-0000-0000-000000000006'', ''a1b2c3d4-e5f6-7890-abcd-ef1234567890'', NULL),
    (''ddaa2356-dbdd-cef7-9cde-69e9fe7387a8'', ''2026-07-02T00:21:54.5029626Z'', ''d1000003-0000-0000-0000-000000000003'', ''a1b2c3d4-e5f6-7890-abcd-ef1234567890'', NULL),
    (''e02b85cd-9dd3-5a0e-0700-cd4e3f2d9ed4'', ''2026-07-02T00:21:54.5029626Z'', ''d1000015-0000-0000-0000-000000000001'', ''c3d4e5f6-a7b8-9012-cdef-123456789012'', NULL),
    (''e2609822-210e-b891-0a3b-4be9978968e8'', ''2026-07-02T00:21:54.5029626Z'', ''d1000006-0000-0000-0000-000000000003'', ''a1b2c3d4-e5f6-7890-abcd-ef1234567890'', NULL),
    (''e296cb77-368f-3b74-5b92-300a6e222336'', ''2026-07-02T00:21:54.5029626Z'', ''d1000006-0000-0000-0000-000000000001'', ''c3d4e5f6-a7b8-9012-cdef-123456789012'', NULL),
    (''e2b9095b-3271-9fb3-b450-3c31cf89f018'', ''2026-07-02T00:21:54.5029626Z'', ''d1000013-0000-0000-0000-000000000002'', ''a1b2c3d4-e5f6-7890-abcd-ef1234567890'', NULL),
    (''e4d49503-81db-8555-9c3e-63285b5fb0e3'', ''2026-07-02T00:21:54.5029626Z'', ''d1000015-0000-0000-0000-000000000001'', ''b2c3d4e5-f6a7-8901-bcde-f12345678901'', NULL),
    (''e71d339d-af13-2cae-30f1-389d04db8597'', ''2026-07-02T00:21:54.5029626Z'', ''d1000002-0000-0000-0000-000000000001'', ''b2c3d4e5-f6a7-8901-bcde-f12345678901'', NULL),
    (''e891d17c-d8a0-4681-2e67-c2535102b824'', ''2026-07-02T00:21:54.5029626Z'', ''d1000004-0000-0000-0000-000000000004'', ''a1b2c3d4-e5f6-7890-abcd-ef1234567890'', NULL),
    (''f22816d7-77fb-13d5-6c4a-f0a724d75f2d'', ''2026-07-02T00:21:54.5029626Z'', ''d1000002-0000-0000-0000-000000000001'', ''c3d4e5f6-a7b8-9012-cdef-123456789012'', NULL),
    (''f71bec3e-d4d4-0866-0ed8-278392b16cc5'', ''2026-07-02T00:21:54.5029626Z'', ''d1000007-0000-0000-0000-000000000002'', ''b2c3d4e5-f6a7-8901-bcde-f12345678901'', NULL),
    (''f7b250ee-574b-ccf7-78da-b6b63b865ed6'', ''2026-07-02T00:21:54.5029626Z'', ''d1000007-0000-0000-0000-000000000001'', ''b2c3d4e5-f6a7-8901-bcde-f12345678901'', NULL),
    (''fbf2ce76-86d4-1963-ba14-10b9ff31b052'', ''2026-07-02T00:21:54.5029626Z'', ''d1000009-0000-0000-0000-000000000001'', ''a1b2c3d4-e5f6-7890-abcd-ef1234567890'', NULL),
    (''ff6fb9b4-3a44-22b2-9239-a335bc2aa23e'', ''2026-07-02T00:21:54.5029626Z'', ''d1000007-0000-0000-0000-000000000001'', ''c3d4e5f6-a7b8-9012-cdef-123456789012'', NULL)');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'CreatedAt', N'PermissionId', N'RoleId', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[RolePermissions]'))
        SET IDENTITY_INSERT [RolePermissions] OFF;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702002156_AddRolePermissionModule'
)
BEGIN
    CREATE INDEX [IX_PermissionAuditLogs_UserId] ON [PermissionAuditLogs] ([UserId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702002156_AddRolePermissionModule'
)
BEGIN
    CREATE UNIQUE INDEX [IX_WhatsAppTemplates_Name] ON [WhatsAppTemplates] ([Name]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702002156_AddRolePermissionModule'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260702002156_AddRolePermissionModule', N'10.0.9');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702002157_CreateWhatsAppTemplates'
)
BEGIN
    EXEC(N'DELETE FROM [RolePermissions]
    WHERE [Id] = ''0cd25d71-346b-3908-42cf-d84115ab6e8b'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702002157_CreateWhatsAppTemplates'
)
BEGIN
    EXEC(N'DELETE FROM [RolePermissions]
    WHERE [Id] = ''12205256-ff83-d1a7-ca36-ef013e4348dd'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702002157_CreateWhatsAppTemplates'
)
BEGIN
    EXEC(N'DELETE FROM [RolePermissions]
    WHERE [Id] = ''18ca3a8f-4030-37b6-bb6c-51d7f78326bf'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702002157_CreateWhatsAppTemplates'
)
BEGIN
    EXEC(N'DELETE FROM [RolePermissions]
    WHERE [Id] = ''1a447bea-0884-a7b8-5c95-9aa22876b650'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702002157_CreateWhatsAppTemplates'
)
BEGIN
    EXEC(N'DELETE FROM [RolePermissions]
    WHERE [Id] = ''37fafc7e-cec4-f1e7-0639-0769adc78e8c'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702002157_CreateWhatsAppTemplates'
)
BEGIN
    EXEC(N'DELETE FROM [RolePermissions]
    WHERE [Id] = ''38e68501-6aa4-2723-251e-83ab85145eda'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702002157_CreateWhatsAppTemplates'
)
BEGIN
    EXEC(N'DELETE FROM [RolePermissions]
    WHERE [Id] = ''411c0941-f12c-e117-e542-7561547fae68'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702002157_CreateWhatsAppTemplates'
)
BEGIN
    EXEC(N'DELETE FROM [RolePermissions]
    WHERE [Id] = ''578d3c75-7400-e248-983f-cb4d4f13fdd3'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702002157_CreateWhatsAppTemplates'
)
BEGIN
    EXEC(N'DELETE FROM [RolePermissions]
    WHERE [Id] = ''618876f6-daa1-fc83-c047-480bf88c794e'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702002157_CreateWhatsAppTemplates'
)
BEGIN
    EXEC(N'DELETE FROM [RolePermissions]
    WHERE [Id] = ''7b9944e2-4c52-0d53-1edd-ccd1d7316518'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702002157_CreateWhatsAppTemplates'
)
BEGIN
    EXEC(N'DELETE FROM [RolePermissions]
    WHERE [Id] = ''7d73a719-0bf4-f70e-384e-ef6a71bdc88f'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702002157_CreateWhatsAppTemplates'
)
BEGIN
    EXEC(N'DELETE FROM [RolePermissions]
    WHERE [Id] = ''7dc649b3-0e4d-a02d-25b2-1533c6da1485'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702002157_CreateWhatsAppTemplates'
)
BEGIN
    EXEC(N'DELETE FROM [RolePermissions]
    WHERE [Id] = ''94e3232d-330d-080b-3968-8c0b0422abd3'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702002157_CreateWhatsAppTemplates'
)
BEGIN
    EXEC(N'DELETE FROM [RolePermissions]
    WHERE [Id] = ''95403e61-b5ef-2b2e-e6db-cb2875fdd651'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702002157_CreateWhatsAppTemplates'
)
BEGIN
    EXEC(N'DELETE FROM [RolePermissions]
    WHERE [Id] = ''97577489-44d4-9fca-3e5a-a7ff99c25f60'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702002157_CreateWhatsAppTemplates'
)
BEGIN
    EXEC(N'DELETE FROM [RolePermissions]
    WHERE [Id] = ''be8fe1d9-d5f7-fc0d-1c62-f23ec408ee0d'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702002157_CreateWhatsAppTemplates'
)
BEGIN
    EXEC(N'DELETE FROM [RolePermissions]
    WHERE [Id] = ''c01cc128-784d-f6dd-8543-496e731fa31a'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702002157_CreateWhatsAppTemplates'
)
BEGIN
    EXEC(N'DELETE FROM [RolePermissions]
    WHERE [Id] = ''c4522deb-c1b9-9234-005e-bdb3b1c5d30b'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702002157_CreateWhatsAppTemplates'
)
BEGIN
    EXEC(N'DELETE FROM [RolePermissions]
    WHERE [Id] = ''ce45517e-5f0e-7330-9534-bae4dcee4905'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702002157_CreateWhatsAppTemplates'
)
BEGIN
    EXEC(N'DELETE FROM [RolePermissions]
    WHERE [Id] = ''d487e031-bfa2-73dc-4031-273dbfae68c5'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702002157_CreateWhatsAppTemplates'
)
BEGIN
    EXEC(N'DELETE FROM [RolePermissions]
    WHERE [Id] = ''dde6c68e-c7ec-19c2-65a8-d1c9abf4dbd1'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702002157_CreateWhatsAppTemplates'
)
BEGIN
    EXEC(N'DELETE FROM [RolePermissions]
    WHERE [Id] = ''df7c114a-08b2-ca57-d10a-9458a9126cc7'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702002157_CreateWhatsAppTemplates'
)
BEGIN
    EXEC(N'DELETE FROM [RolePermissions]
    WHERE [Id] = ''f6d95421-adde-78dc-a3e8-ada50506e534'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702002157_CreateWhatsAppTemplates'
)
BEGIN
    EXEC(N'DELETE FROM [RolePermissions]
    WHERE [Id] = ''faed05e8-a7e0-30e1-7f2f-3f59a113fd18'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702002157_CreateWhatsAppTemplates'
)
BEGIN
    EXEC(N'DELETE FROM [RolePermissions]
    WHERE [Id] = ''fb0338fb-fc95-ac51-0a37-7cc90b17ec50'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702002157_CreateWhatsAppTemplates'
)
BEGIN
    EXEC(N'DELETE FROM [Permissions]
    WHERE [Id] = ''a3b4c5d6-e7f8-9012-a345-678901234567'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702002157_CreateWhatsAppTemplates'
)
BEGIN
    EXEC(N'DELETE FROM [Permissions]
    WHERE [Id] = ''a7b8c9d0-e1f2-3456-a789-012345678901'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702002157_CreateWhatsAppTemplates'
)
BEGIN
    EXEC(N'DELETE FROM [Permissions]
    WHERE [Id] = ''a9b0c1d2-e3f4-5678-a901-234567890123'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702002157_CreateWhatsAppTemplates'
)
BEGIN
    EXEC(N'DELETE FROM [Permissions]
    WHERE [Id] = ''b4c5d6e7-f8a9-0123-b456-789012345678'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702002157_CreateWhatsAppTemplates'
)
BEGIN
    EXEC(N'DELETE FROM [Permissions]
    WHERE [Id] = ''b8c9d0e1-f2a3-4567-b890-123456789012'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702002157_CreateWhatsAppTemplates'
)
BEGIN
    EXEC(N'DELETE FROM [Permissions]
    WHERE [Id] = ''c5d6e7f8-a9b0-1234-c567-890123456789'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702002157_CreateWhatsAppTemplates'
)
BEGIN
    EXEC(N'DELETE FROM [Permissions]
    WHERE [Id] = ''c9d0e1f2-a3b4-5678-c901-234567890123'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702002157_CreateWhatsAppTemplates'
)
BEGIN
    EXEC(N'DELETE FROM [Permissions]
    WHERE [Id] = ''d0e1f2a3-b4c5-6789-d012-345678901234'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702002157_CreateWhatsAppTemplates'
)
BEGIN
    EXEC(N'DELETE FROM [Permissions]
    WHERE [Id] = ''d1e2f3a4-b5c6-7890-d123-456789012345'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702002157_CreateWhatsAppTemplates'
)
BEGIN
    EXEC(N'DELETE FROM [Permissions]
    WHERE [Id] = ''d6e7f8a9-b0c1-2345-d678-901234567890'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702002157_CreateWhatsAppTemplates'
)
BEGIN
    EXEC(N'DELETE FROM [Permissions]
    WHERE [Id] = ''e1f2a3b4-c5d6-7890-e123-456789012345'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702002157_CreateWhatsAppTemplates'
)
BEGIN
    EXEC(N'DELETE FROM [Permissions]
    WHERE [Id] = ''e5f6a7b8-c9d0-1234-ef56-789012345678'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702002157_CreateWhatsAppTemplates'
)
BEGIN
    EXEC(N'DELETE FROM [Permissions]
    WHERE [Id] = ''e7f8a9b0-c1d2-3456-e789-012345678901'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702002157_CreateWhatsAppTemplates'
)
BEGIN
    EXEC(N'DELETE FROM [Permissions]
    WHERE [Id] = ''f2a3b4c5-d6e7-8901-f234-567890123456'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702002157_CreateWhatsAppTemplates'
)
BEGIN
    EXEC(N'DELETE FROM [Permissions]
    WHERE [Id] = ''f6a7b8c9-d0e1-2345-f678-901234567890'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702002157_CreateWhatsAppTemplates'
)
BEGIN
    EXEC(N'DELETE FROM [Permissions]
    WHERE [Id] = ''f8a9b0c1-d2e3-4567-f890-123456789012'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702002157_CreateWhatsAppTemplates'
)
BEGIN
    ALTER TABLE [Roles] ADD [IsActive] bit NOT NULL DEFAULT CAST(0 AS bit);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702002157_CreateWhatsAppTemplates'
)
BEGIN
    ALTER TABLE [Permissions] ADD [Module] nvarchar(max) NOT NULL DEFAULT N'';
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702002157_CreateWhatsAppTemplates'
)
BEGIN
    CREATE TABLE [PermissionAuditLogs] (
        [Id] uniqueidentifier NOT NULL,
        [UserId] uniqueidentifier NULL,
        [Action] nvarchar(max) NOT NULL,
        [RoleName] nvarchar(max) NOT NULL,
        [OldPermissions] nvarchar(max) NULL,
        [NewPermissions] nvarchar(max) NULL,
        [IpAddress] nvarchar(max) NULL,
        [CreatedAt] datetime2 NOT NULL,
        [UpdatedAt] datetime2 NULL,
        CONSTRAINT [PK_PermissionAuditLogs] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_PermissionAuditLogs_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702002157_CreateWhatsAppTemplates'
)
BEGIN
    CREATE TABLE [WhatsAppTemplates] (
        [Id] uniqueidentifier NOT NULL,
        [Name] nvarchar(200) NOT NULL,
        [MessageBody] nvarchar(2000) NOT NULL,
        [IsActive] bit NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        [UpdatedAt] datetime2 NULL,
        CONSTRAINT [PK_WhatsAppTemplates] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702002157_CreateWhatsAppTemplates'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'CreatedAt', N'Description', N'Module', N'Name', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[Permissions]'))
        SET IDENTITY_INSERT [Permissions] ON;
    EXEC(N'INSERT INTO [Permissions] ([Id], [CreatedAt], [Description], [Module], [Name], [UpdatedAt])
    VALUES (''d1000001-0000-0000-0000-000000000001'', ''2026-07-02T00:21:55.5220179Z'', N''View main dashboard'', N''Dashboard'', N''Dashboard.View'', NULL),
    (''d1000001-0000-0000-0000-000000000002'', ''2026-07-02T00:21:55.5220179Z'', N''View revenue dashboard'', N''Dashboard'', N''Dashboard.Revenue'', NULL),
    (''d1000002-0000-0000-0000-000000000001'', ''2026-07-02T00:21:55.5220179Z'', N''View members list'', N''Members'', N''Members.View'', NULL),
    (''d1000002-0000-0000-0000-000000000002'', ''2026-07-02T00:21:55.5220179Z'', N''Create new members'', N''Members'', N''Members.Create'', NULL),
    (''d1000002-0000-0000-0000-000000000003'', ''2026-07-02T00:21:55.5220179Z'', N''Edit existing members'', N''Members'', N''Members.Edit'', NULL),
    (''d1000002-0000-0000-0000-000000000004'', ''2026-07-02T00:21:55.5220179Z'', N''Delete members'', N''Members'', N''Members.Delete'', NULL),
    (''d1000003-0000-0000-0000-000000000001'', ''2026-07-02T00:21:55.5220179Z'', N''View plans'', N''Plans'', N''Plans.View'', NULL),
    (''d1000003-0000-0000-0000-000000000002'', ''2026-07-02T00:21:55.5220179Z'', N''Create plans'', N''Plans'', N''Plans.Create'', NULL),
    (''d1000003-0000-0000-0000-000000000003'', ''2026-07-02T00:21:55.5220179Z'', N''Edit plans'', N''Plans'', N''Plans.Edit'', NULL),
    (''d1000003-0000-0000-0000-000000000004'', ''2026-07-02T00:21:55.5220179Z'', N''Delete plans'', N''Plans'', N''Plans.Delete'', NULL),
    (''d1000004-0000-0000-0000-000000000001'', ''2026-07-02T00:21:55.5220179Z'', N''View offers'', N''Offers'', N''Offers.View'', NULL),
    (''d1000004-0000-0000-0000-000000000002'', ''2026-07-02T00:21:55.5220179Z'', N''Create offers'', N''Offers'', N''Offers.Create'', NULL),
    (''d1000004-0000-0000-0000-000000000003'', ''2026-07-02T00:21:55.5220179Z'', N''Edit offers'', N''Offers'', N''Offers.Edit'', NULL),
    (''d1000004-0000-0000-0000-000000000004'', ''2026-07-02T00:21:55.5220179Z'', N''Delete offers'', N''Offers'', N''Offers.Delete'', NULL),
    (''d1000005-0000-0000-0000-000000000001'', ''2026-07-02T00:21:55.5220179Z'', N''View subscriptions'', N''Subscriptions'', N''Subscriptions.View'', NULL),
    (''d1000005-0000-0000-0000-000000000002'', ''2026-07-02T00:21:55.5220179Z'', N''Create subscriptions'', N''Subscriptions'', N''Subscriptions.Create'', NULL),
    (''d1000005-0000-0000-0000-000000000003'', ''2026-07-02T00:21:55.5220179Z'', N''Edit subscriptions'', N''Subscriptions'', N''Subscriptions.Edit'', NULL),
    (''d1000005-0000-0000-0000-000000000004'', ''2026-07-02T00:21:55.5220179Z'', N''Freeze subscriptions'', N''Subscriptions'', N''Subscriptions.Freeze'', NULL),
    (''d1000005-0000-0000-0000-000000000005'', ''2026-07-02T00:21:55.5220179Z'', N''Unfreeze subscriptions'', N''Subscriptions'', N''Subscriptions.Unfreeze'', NULL),
    (''d1000005-0000-0000-0000-000000000006'', ''2026-07-02T00:21:55.5220179Z'', N''Renew subscriptions'', N''Subscriptions'', N''Subscriptions.Renew'', NULL),
    (''d1000005-0000-0000-0000-000000000007'', ''2026-07-02T00:21:55.5220179Z'', N''View payment history'', N''Subscriptions'', N''Subscriptions.PaymentHistory'', NULL),
    (''d1000006-0000-0000-0000-000000000001'', ''2026-07-02T00:21:55.5220179Z'', N''View leads'', N''Leads'', N''Leads.View'', NULL),
    (''d1000006-0000-0000-0000-000000000002'', ''2026-07-02T00:21:55.5220179Z'', N''Create leads'', N''Leads'', N''Leads.Create'', NULL),
    (''d1000006-0000-0000-0000-000000000003'', ''2026-07-02T00:21:55.5220179Z'', N''Edit leads'', N''Leads'', N''Leads.Edit'', NULL),
    (''d1000006-0000-0000-0000-000000000004'', ''2026-07-02T00:21:55.5220179Z'', N''Convert leads to members'', N''Leads'', N''Leads.Convert'', NULL),
    (''d1000007-0000-0000-0000-000000000001'', ''2026-07-02T00:21:55.5220179Z'', N''View attendance'', N''Attendance'', N''Attendance.View'', NULL),
    (''d1000007-0000-0000-0000-000000000002'', ''2026-07-02T00:21:55.5220179Z'', N''Export attendance'', N''Attendance'', N''Attendance.Export'', NULL),
    (''d1000008-0000-0000-0000-000000000001'', ''2026-07-02T00:21:55.5220179Z'', N''Send WhatsApp messages'', N''WhatsApp'', N''WhatsApp.Send'', NULL),
    (''d1000008-0000-0000-0000-000000000002'', ''2026-07-02T00:21:55.5220179Z'', N''Manual broadcast via WhatsApp'', N''WhatsApp'', N''WhatsApp.Broadcast'', NULL),
    (''d1000009-0000-0000-0000-000000000001'', ''2026-07-02T00:21:55.5220179Z'', N''Import data'', N''Import/Export'', N''ImportExport.Import'', NULL),
    (''d1000009-0000-0000-0000-000000000002'', ''2026-07-02T00:21:55.5220179Z'', N''Export data'', N''Import/Export'', N''ImportExport.Export'', NULL),
    (''d1000010-0000-0000-0000-000000000001'', ''2026-07-02T00:21:55.5220179Z'', N''View settings'', N''Settings'', N''Settings.View'', NULL),
    (''d1000010-0000-0000-0000-000000000002'', ''2026-07-02T00:21:55.5220179Z'', N''Edit settings'', N''Settings'', N''Settings.Edit'', NULL),
    (''d1000011-0000-0000-0000-000000000001'', ''2026-07-02T00:21:55.5220179Z'', N''View users'', N''User Management'', N''Users.View'', NULL),
    (''d1000011-0000-0000-0000-000000000002'', ''2026-07-02T00:21:55.5220179Z'', N''Create users'', N''User Management'', N''Users.Create'', NULL),
    (''d1000011-0000-0000-0000-000000000003'', ''2026-07-02T00:21:55.5220179Z'', N''Edit users'', N''User Management'', N''Users.Edit'', NULL),
    (''d1000011-0000-0000-0000-000000000004'', ''2026-07-02T00:21:55.5220179Z'', N''Delete users'', N''User Management'', N''Users.Delete'', NULL),
    (''d1000012-0000-0000-0000-000000000001'', ''2026-07-02T00:21:55.5220179Z'', N''View roles & permissions'', N''Roles & Permissions'', N''Roles.View'', NULL),
    (''d1000012-0000-0000-0000-000000000002'', ''2026-07-02T00:21:55.5220179Z'', N''Create roles'', N''Roles & Permissions'', N''Roles.Create'', NULL),
    (''d1000012-0000-0000-0000-000000000003'', ''2026-07-02T00:21:55.5220179Z'', N''Edit roles'', N''Roles & Permissions'', N''Roles.Edit'', NULL),
    (''d1000012-0000-0000-0000-000000000004'', ''2026-07-02T00:21:55.5220179Z'', N''Delete roles'', N''Roles & Permissions'', N''Roles.Delete'', NULL),
    (''d1000013-0000-0000-0000-000000000001'', ''2026-07-02T00:21:55.5220179Z'', N''View devices'', N''Devices'', N''Devices.View'', NULL);
    INSERT INTO [Permissions] ([Id], [CreatedAt], [Description], [Module], [Name], [UpdatedAt])
    VALUES (''d1000013-0000-0000-0000-000000000002'', ''2026-07-02T00:21:55.5220179Z'', N''Manage devices'', N''Devices'', N''Devices.Manage'', NULL),
    (''d1000014-0000-0000-0000-000000000001'', ''2026-07-02T00:21:55.5220179Z'', N''Manage backups'', N''Backup'', N''Backup.Manage'', NULL),
    (''d1000015-0000-0000-0000-000000000001'', ''2026-07-02T00:21:55.5220179Z'', N''View notifications'', N''Notifications'', N''Notifications.View'', NULL),
    (''d1000015-0000-0000-0000-000000000002'', ''2026-07-02T00:21:55.5220179Z'', N''Send notifications'', N''Notifications'', N''Notifications.Send'', NULL),
    (''d1000016-0000-0000-0000-000000000001'', ''2026-07-02T00:21:55.5220179Z'', N''View memberships'', N''Memberships'', N''Memberships.View'', NULL),
    (''d1000016-0000-0000-0000-000000000002'', ''2026-07-02T00:21:55.5220179Z'', N''Manage memberships'', N''Memberships'', N''Memberships.Manage'', NULL)');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'CreatedAt', N'Description', N'Module', N'Name', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[Permissions]'))
        SET IDENTITY_INSERT [Permissions] OFF;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702002157_CreateWhatsAppTemplates'
)
BEGIN
    EXEC(N'UPDATE [Roles] SET [CreatedAt] = ''2026-07-02T00:21:55.2762086Z'', [IsActive] = CAST(1 AS bit)
    WHERE [Id] = ''a1b2c3d4-e5f6-7890-abcd-ef1234567890'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702002157_CreateWhatsAppTemplates'
)
BEGIN
    EXEC(N'UPDATE [Roles] SET [CreatedAt] = ''2026-07-02T00:21:55.2762086Z'', [IsActive] = CAST(1 AS bit)
    WHERE [Id] = ''b2c3d4e5-f6a7-8901-bcde-f12345678901'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702002157_CreateWhatsAppTemplates'
)
BEGIN
    EXEC(N'UPDATE [Roles] SET [CreatedAt] = ''2026-07-02T00:21:55.2762086Z'', [IsActive] = CAST(0 AS bit)
    WHERE [Id] = ''c3d4e5f6-a7b8-9012-cdef-123456789012'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702002157_CreateWhatsAppTemplates'
)
BEGIN
    EXEC(N'UPDATE [Settings] SET [CreatedAt] = ''2026-07-02T00:21:55.5274842Z''
    WHERE [Id] = ''a5b6c7d8-e9f0-1234-a567-890123456789'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702002157_CreateWhatsAppTemplates'
)
BEGIN
    EXEC(N'UPDATE [Settings] SET [CreatedAt] = ''2026-07-02T00:21:55.5274842Z''
    WHERE [Id] = ''b0c1d2e3-f4a5-6789-b012-345678901234'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702002157_CreateWhatsAppTemplates'
)
BEGIN
    EXEC(N'UPDATE [Settings] SET [CreatedAt] = ''2026-07-02T00:21:55.5274842Z''
    WHERE [Id] = ''b6c7d8e9-f0a1-2345-b678-901234567890'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702002157_CreateWhatsAppTemplates'
)
BEGIN
    EXEC(N'UPDATE [Settings] SET [CreatedAt] = ''2026-07-02T00:21:55.5274842Z''
    WHERE [Id] = ''c1d2e3f4-a5b6-7890-c123-456789012345'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702002157_CreateWhatsAppTemplates'
)
BEGIN
    EXEC(N'UPDATE [Settings] SET [CreatedAt] = ''2026-07-02T00:21:55.5274842Z''
    WHERE [Id] = ''c7d8e9f0-a1b2-3456-c789-012345678901'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702002157_CreateWhatsAppTemplates'
)
BEGIN
    EXEC(N'UPDATE [Settings] SET [CreatedAt] = ''2026-07-02T00:21:55.5274842Z''
    WHERE [Id] = ''d2e3f4a5-b6c7-8901-d234-567890123456'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702002157_CreateWhatsAppTemplates'
)
BEGIN
    EXEC(N'UPDATE [Settings] SET [CreatedAt] = ''2026-07-02T00:21:55.5274842Z''
    WHERE [Id] = ''d8e9f0a1-b2c3-4567-d890-123456789012'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702002157_CreateWhatsAppTemplates'
)
BEGIN
    EXEC(N'UPDATE [Settings] SET [CreatedAt] = ''2026-07-02T00:21:55.5274842Z''
    WHERE [Id] = ''e3f4a5b6-c7d8-9012-e345-678901234567'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702002157_CreateWhatsAppTemplates'
)
BEGIN
    EXEC(N'UPDATE [Settings] SET [CreatedAt] = ''2026-07-02T00:21:55.5274842Z''
    WHERE [Id] = ''e9f0a1b2-c3d4-5678-e901-234567890123'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702002157_CreateWhatsAppTemplates'
)
BEGIN
    EXEC(N'UPDATE [Settings] SET [CreatedAt] = ''2026-07-02T00:21:55.5274842Z''
    WHERE [Id] = ''f4a5b6c7-d8e9-0123-f456-789012345678'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702002157_CreateWhatsAppTemplates'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''$2a$11$P99M3nZQVBhPhpuVJaspOu8IrzY0YZlY.lIbDt4.PmFmllyCxt3Om''
    WHERE [Id] = ''d4e5f6a7-b8c9-0123-def4-567890123456'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702002157_CreateWhatsAppTemplates'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'CreatedAt', N'PermissionId', N'RoleId', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[RolePermissions]'))
        SET IDENTITY_INSERT [RolePermissions] ON;
    EXEC(N'INSERT INTO [RolePermissions] ([Id], [CreatedAt], [PermissionId], [RoleId], [UpdatedAt])
    VALUES (''005f9b64-4d0a-6c82-947e-e57cfe738452'', ''2026-07-02T00:21:55.5247369Z'', ''d1000012-0000-0000-0000-000000000003'', ''a1b2c3d4-e5f6-7890-abcd-ef1234567890'', NULL),
    (''05195fdb-cd30-4561-9168-bbeb6f886a69'', ''2026-07-02T00:21:55.5247369Z'', ''d1000006-0000-0000-0000-000000000003'', ''b2c3d4e5-f6a7-8901-bcde-f12345678901'', NULL),
    (''0a025850-9197-728d-2039-14fa13c954ce'', ''2026-07-02T00:21:55.5247369Z'', ''d1000006-0000-0000-0000-000000000004'', ''b2c3d4e5-f6a7-8901-bcde-f12345678901'', NULL),
    (''0e32d4f0-e31f-1692-6763-43d06de36c0e'', ''2026-07-02T00:21:55.5247369Z'', ''d1000012-0000-0000-0000-000000000002'', ''a1b2c3d4-e5f6-7890-abcd-ef1234567890'', NULL),
    (''1def5b70-20cb-7e7b-b398-51bae635cb4c'', ''2026-07-02T00:21:55.5247369Z'', ''d1000011-0000-0000-0000-000000000002'', ''a1b2c3d4-e5f6-7890-abcd-ef1234567890'', NULL),
    (''2babdd1d-2af8-85c1-be3d-93660af5d9ca'', ''2026-07-02T00:21:55.5247369Z'', ''d1000001-0000-0000-0000-000000000001'', ''a1b2c3d4-e5f6-7890-abcd-ef1234567890'', NULL),
    (''2cc74796-4a71-5342-2d08-a37e9154996d'', ''2026-07-02T00:21:55.5247369Z'', ''d1000002-0000-0000-0000-000000000001'', ''a1b2c3d4-e5f6-7890-abcd-ef1234567890'', NULL),
    (''32c5144e-c485-1183-98b8-403fbb277ebd'', ''2026-07-02T00:21:55.5247369Z'', ''d1000016-0000-0000-0000-000000000001'', ''b2c3d4e5-f6a7-8901-bcde-f12345678901'', NULL),
    (''35837f97-2623-ab10-0cbd-370671326f89'', ''2026-07-02T00:21:55.5247369Z'', ''d1000002-0000-0000-0000-000000000004'', ''a1b2c3d4-e5f6-7890-abcd-ef1234567890'', NULL),
    (''3c0e6bfe-9913-56af-20cb-cc7f4e36a4a6'', ''2026-07-02T00:21:55.5247369Z'', ''d1000015-0000-0000-0000-000000000002'', ''a1b2c3d4-e5f6-7890-abcd-ef1234567890'', NULL),
    (''430af60c-cfb8-d59a-e833-8cd110ff7f06'', ''2026-07-02T00:21:55.5247369Z'', ''d1000005-0000-0000-0000-000000000004'', ''b2c3d4e5-f6a7-8901-bcde-f12345678901'', NULL),
    (''43fe5635-7b4d-ab3d-a51e-3559aced41c5'', ''2026-07-02T00:21:55.5247369Z'', ''d1000005-0000-0000-0000-000000000001'', ''a1b2c3d4-e5f6-7890-abcd-ef1234567890'', NULL),
    (''45a0ab26-2f4b-7f21-a073-171f907faf77'', ''2026-07-02T00:21:55.5247369Z'', ''d1000002-0000-0000-0000-000000000002'', ''b2c3d4e5-f6a7-8901-bcde-f12345678901'', NULL),
    (''477c3b9c-bdb3-a703-0b56-f6516dcfc7e5'', ''2026-07-02T00:21:55.5247369Z'', ''d1000009-0000-0000-0000-000000000001'', ''b2c3d4e5-f6a7-8901-bcde-f12345678901'', NULL),
    (''48e05a34-1b30-1cdc-ca0d-974cb2c3ebd8'', ''2026-07-02T00:21:55.5247369Z'', ''d1000001-0000-0000-0000-000000000001'', ''b2c3d4e5-f6a7-8901-bcde-f12345678901'', NULL),
    (''4d02d95c-4aab-3930-8c9f-b46c21d1be4e'', ''2026-07-02T00:21:55.5247369Z'', ''d1000005-0000-0000-0000-000000000002'', ''a1b2c3d4-e5f6-7890-abcd-ef1234567890'', NULL),
    (''5334b704-3d25-6b96-517f-6443fb11c414'', ''2026-07-02T00:21:55.5247369Z'', ''d1000005-0000-0000-0000-000000000004'', ''a1b2c3d4-e5f6-7890-abcd-ef1234567890'', NULL),
    (''55690f65-12c3-9922-c4b1-47b34caedde2'', ''2026-07-02T00:21:55.5247369Z'', ''d1000002-0000-0000-0000-000000000003'', ''b2c3d4e5-f6a7-8901-bcde-f12345678901'', NULL),
    (''557952bd-50d0-d062-f35e-7fe6224bd482'', ''2026-07-02T00:21:55.5247369Z'', ''d1000002-0000-0000-0000-000000000003'', ''a1b2c3d4-e5f6-7890-abcd-ef1234567890'', NULL),
    (''566928b3-00dc-fb90-39b7-af77eb0f35b1'', ''2026-07-02T00:21:55.5247369Z'', ''d1000006-0000-0000-0000-000000000001'', ''b2c3d4e5-f6a7-8901-bcde-f12345678901'', NULL),
    (''5f9b5de3-e563-a2d9-672a-91816ba23dd6'', ''2026-07-02T00:21:55.5247369Z'', ''d1000006-0000-0000-0000-000000000001'', ''a1b2c3d4-e5f6-7890-abcd-ef1234567890'', NULL),
    (''63139b75-fbfc-2bcf-36d2-43bc185b9a05'', ''2026-07-02T00:21:55.5247369Z'', ''d1000003-0000-0000-0000-000000000002'', ''a1b2c3d4-e5f6-7890-abcd-ef1234567890'', NULL),
    (''6604e362-f7af-bb42-e9e1-448a76c8e188'', ''2026-07-02T00:21:55.5247369Z'', ''d1000008-0000-0000-0000-000000000002'', ''a1b2c3d4-e5f6-7890-abcd-ef1234567890'', NULL),
    (''6a745601-638c-730b-cda2-d7a67e9f5a16'', ''2026-07-02T00:21:55.5247369Z'', ''d1000005-0000-0000-0000-000000000007'', ''b2c3d4e5-f6a7-8901-bcde-f12345678901'', NULL),
    (''6b5ac608-95b1-cd3e-d869-0fdf79a09fd0'', ''2026-07-02T00:21:55.5247369Z'', ''d1000012-0000-0000-0000-000000000004'', ''a1b2c3d4-e5f6-7890-abcd-ef1234567890'', NULL),
    (''6b6ec0d1-eb14-8d88-e615-78e6fe6177bf'', ''2026-07-02T00:21:55.5247369Z'', ''d1000007-0000-0000-0000-000000000002'', ''a1b2c3d4-e5f6-7890-abcd-ef1234567890'', NULL),
    (''70ec0ef2-a390-97c2-76ef-1678f6cf8fbf'', ''2026-07-02T00:21:55.5247369Z'', ''d1000001-0000-0000-0000-000000000002'', ''a1b2c3d4-e5f6-7890-abcd-ef1234567890'', NULL),
    (''7297595c-0a29-eb0d-19be-c531ee673c5b'', ''2026-07-02T00:21:55.5247369Z'', ''d1000005-0000-0000-0000-000000000005'', ''b2c3d4e5-f6a7-8901-bcde-f12345678901'', NULL),
    (''72bf7e60-3016-f5ff-ec42-ddfc1e27acfc'', ''2026-07-02T00:21:55.5247369Z'', ''d1000012-0000-0000-0000-000000000001'', ''a1b2c3d4-e5f6-7890-abcd-ef1234567890'', NULL),
    (''7347301a-7584-8075-4aab-788b19078a62'', ''2026-07-02T00:21:55.5247369Z'', ''d1000002-0000-0000-0000-000000000002'', ''a1b2c3d4-e5f6-7890-abcd-ef1234567890'', NULL),
    (''78615f0f-b666-f010-e57b-dfb05c66f035'', ''2026-07-02T00:21:55.5247369Z'', ''d1000001-0000-0000-0000-000000000001'', ''c3d4e5f6-a7b8-9012-cdef-123456789012'', NULL),
    (''78b53d72-0ba7-6249-68a8-bedd284304f4'', ''2026-07-02T00:21:55.5247369Z'', ''d1000010-0000-0000-0000-000000000001'', ''a1b2c3d4-e5f6-7890-abcd-ef1234567890'', NULL),
    (''7916078f-5d85-86e1-8747-b5dd207f40b5'', ''2026-07-02T00:21:55.5247369Z'', ''d1000005-0000-0000-0000-000000000001'', ''b2c3d4e5-f6a7-8901-bcde-f12345678901'', NULL),
    (''82cdc680-60c7-ca13-1fba-0c1725b381fa'', ''2026-07-02T00:21:55.5247369Z'', ''d1000006-0000-0000-0000-000000000004'', ''a1b2c3d4-e5f6-7890-abcd-ef1234567890'', NULL),
    (''85bf1ab4-8b34-6fb0-9ad7-cbbd215d0949'', ''2026-07-02T00:21:55.5247369Z'', ''d1000009-0000-0000-0000-000000000002'', ''a1b2c3d4-e5f6-7890-abcd-ef1234567890'', NULL),
    (''8680abc3-7a3f-fc42-ec4b-282165955f1c'', ''2026-07-02T00:21:55.5247369Z'', ''d1000005-0000-0000-0000-000000000002'', ''b2c3d4e5-f6a7-8901-bcde-f12345678901'', NULL),
    (''86f4523e-677d-889c-8e00-7ee5a50d94c5'', ''2026-07-02T00:21:55.5247369Z'', ''d1000006-0000-0000-0000-000000000002'', ''a1b2c3d4-e5f6-7890-abcd-ef1234567890'', NULL),
    (''89d0bc79-33f4-2a4d-f951-da329c328c20'', ''2026-07-02T00:21:55.5247369Z'', ''d1000016-0000-0000-0000-000000000002'', ''a1b2c3d4-e5f6-7890-abcd-ef1234567890'', NULL),
    (''8ef92960-4f9f-70c2-58b7-a0fc32743d95'', ''2026-07-02T00:21:55.5247369Z'', ''d1000009-0000-0000-0000-000000000002'', ''b2c3d4e5-f6a7-8901-bcde-f12345678901'', NULL),
    (''904ee737-7848-bded-ec47-ed74c6b3b5e5'', ''2026-07-02T00:21:55.5247369Z'', ''d1000015-0000-0000-0000-000000000002'', ''b2c3d4e5-f6a7-8901-bcde-f12345678901'', NULL),
    (''9cbf51af-d816-7ca8-28cf-67d124fb62c0'', ''2026-07-02T00:21:55.5247369Z'', ''d1000005-0000-0000-0000-000000000003'', ''a1b2c3d4-e5f6-7890-abcd-ef1234567890'', NULL),
    (''a54e820b-654c-d289-f188-11d672a9a217'', ''2026-07-02T00:21:55.5247369Z'', ''d1000010-0000-0000-0000-000000000002'', ''a1b2c3d4-e5f6-7890-abcd-ef1234567890'', NULL);
    INSERT INTO [RolePermissions] ([Id], [CreatedAt], [PermissionId], [RoleId], [UpdatedAt])
    VALUES (''a5574f00-d065-186a-8630-9c6de44235eb'', ''2026-07-02T00:21:55.5247369Z'', ''d1000016-0000-0000-0000-000000000001'', ''a1b2c3d4-e5f6-7890-abcd-ef1234567890'', NULL),
    (''a9035ef4-18c2-3e13-d5cc-99fc5e63ed31'', ''2026-07-02T00:21:55.5247369Z'', ''d1000006-0000-0000-0000-000000000002'', ''b2c3d4e5-f6a7-8901-bcde-f12345678901'', NULL),
    (''ace9a66b-d17d-2b50-17b3-67eb40dcf41a'', ''2026-07-02T00:21:55.5247369Z'', ''d1000011-0000-0000-0000-000000000004'', ''a1b2c3d4-e5f6-7890-abcd-ef1234567890'', NULL),
    (''ad012924-5237-6205-38fe-d48cedc33d4d'', ''2026-07-02T00:21:55.5247369Z'', ''d1000003-0000-0000-0000-000000000004'', ''a1b2c3d4-e5f6-7890-abcd-ef1234567890'', NULL),
    (''b2f04998-b570-47d2-8236-0756a4e219e2'', ''2026-07-02T00:21:55.5247369Z'', ''d1000003-0000-0000-0000-000000000001'', ''a1b2c3d4-e5f6-7890-abcd-ef1234567890'', NULL),
    (''b4e93b8d-5dc3-24cf-da95-fb3403b3749e'', ''2026-07-02T00:21:55.5247369Z'', ''d1000008-0000-0000-0000-000000000001'', ''a1b2c3d4-e5f6-7890-abcd-ef1234567890'', NULL),
    (''b5e24ad4-8420-94b6-b8fc-9dc56e2a445a'', ''2026-07-02T00:21:55.5247369Z'', ''d1000004-0000-0000-0000-000000000003'', ''a1b2c3d4-e5f6-7890-abcd-ef1234567890'', NULL),
    (''b94ea0da-6b9a-ea4f-6ac8-08b4e88272fe'', ''2026-07-02T00:21:55.5247369Z'', ''d1000005-0000-0000-0000-000000000007'', ''a1b2c3d4-e5f6-7890-abcd-ef1234567890'', NULL),
    (''bbe26c6f-807b-dba2-eef3-33cd059b3296'', ''2026-07-02T00:21:55.5247369Z'', ''d1000004-0000-0000-0000-000000000001'', ''a1b2c3d4-e5f6-7890-abcd-ef1234567890'', NULL),
    (''bc79a3c2-f98e-9c5b-5c26-3072265353bc'', ''2026-07-02T00:21:55.5247369Z'', ''d1000014-0000-0000-0000-000000000001'', ''a1b2c3d4-e5f6-7890-abcd-ef1234567890'', NULL),
    (''bde31dac-221c-d02b-fca6-51fa576f9b89'', ''2026-07-02T00:21:55.5247369Z'', ''d1000008-0000-0000-0000-000000000001'', ''b2c3d4e5-f6a7-8901-bcde-f12345678901'', NULL),
    (''bf6ac31e-ca1d-b678-8e6e-3cc7b7dac946'', ''2026-07-02T00:21:55.5247369Z'', ''d1000015-0000-0000-0000-000000000001'', ''a1b2c3d4-e5f6-7890-abcd-ef1234567890'', NULL),
    (''bf70e113-e074-6b40-6534-fb4c1f9231d3'', ''2026-07-02T00:21:55.5247369Z'', ''d1000013-0000-0000-0000-000000000001'', ''a1b2c3d4-e5f6-7890-abcd-ef1234567890'', NULL),
    (''c175a900-6ce3-e698-94df-f33b4ed6bb50'', ''2026-07-02T00:21:55.5247369Z'', ''d1000007-0000-0000-0000-000000000001'', ''a1b2c3d4-e5f6-7890-abcd-ef1234567890'', NULL),
    (''c1a0d8cb-6b48-74ec-9919-d577f704c514'', ''2026-07-02T00:21:55.5247369Z'', ''d1000011-0000-0000-0000-000000000003'', ''a1b2c3d4-e5f6-7890-abcd-ef1234567890'', NULL),
    (''c24d281f-89aa-dd6c-c264-59d5b5ba0faa'', ''2026-07-02T00:21:55.5247369Z'', ''d1000011-0000-0000-0000-000000000001'', ''a1b2c3d4-e5f6-7890-abcd-ef1234567890'', NULL),
    (''ce25f18c-ed21-3f6b-8dd1-82406f135d61'', ''2026-07-02T00:21:55.5247369Z'', ''d1000005-0000-0000-0000-000000000005'', ''a1b2c3d4-e5f6-7890-abcd-ef1234567890'', NULL),
    (''ceadb402-0bb1-ea6e-7525-16b7a2ba8317'', ''2026-07-02T00:21:55.5247369Z'', ''d1000004-0000-0000-0000-000000000002'', ''a1b2c3d4-e5f6-7890-abcd-ef1234567890'', NULL),
    (''d138659a-966d-09d0-2309-f233f3f232d1'', ''2026-07-02T00:21:55.5247369Z'', ''d1000005-0000-0000-0000-000000000006'', ''b2c3d4e5-f6a7-8901-bcde-f12345678901'', NULL),
    (''d5c2ad14-559d-e8a6-0702-d8c9f26af634'', ''2026-07-02T00:21:55.5247369Z'', ''d1000005-0000-0000-0000-000000000006'', ''a1b2c3d4-e5f6-7890-abcd-ef1234567890'', NULL),
    (''ddaa2356-dbdd-cef7-9cde-69e9fe7387a8'', ''2026-07-02T00:21:55.5247369Z'', ''d1000003-0000-0000-0000-000000000003'', ''a1b2c3d4-e5f6-7890-abcd-ef1234567890'', NULL),
    (''e02b85cd-9dd3-5a0e-0700-cd4e3f2d9ed4'', ''2026-07-02T00:21:55.5247369Z'', ''d1000015-0000-0000-0000-000000000001'', ''c3d4e5f6-a7b8-9012-cdef-123456789012'', NULL),
    (''e2609822-210e-b891-0a3b-4be9978968e8'', ''2026-07-02T00:21:55.5247369Z'', ''d1000006-0000-0000-0000-000000000003'', ''a1b2c3d4-e5f6-7890-abcd-ef1234567890'', NULL),
    (''e296cb77-368f-3b74-5b92-300a6e222336'', ''2026-07-02T00:21:55.5247369Z'', ''d1000006-0000-0000-0000-000000000001'', ''c3d4e5f6-a7b8-9012-cdef-123456789012'', NULL),
    (''e2b9095b-3271-9fb3-b450-3c31cf89f018'', ''2026-07-02T00:21:55.5247369Z'', ''d1000013-0000-0000-0000-000000000002'', ''a1b2c3d4-e5f6-7890-abcd-ef1234567890'', NULL),
    (''e4d49503-81db-8555-9c3e-63285b5fb0e3'', ''2026-07-02T00:21:55.5247369Z'', ''d1000015-0000-0000-0000-000000000001'', ''b2c3d4e5-f6a7-8901-bcde-f12345678901'', NULL),
    (''e71d339d-af13-2cae-30f1-389d04db8597'', ''2026-07-02T00:21:55.5247369Z'', ''d1000002-0000-0000-0000-000000000001'', ''b2c3d4e5-f6a7-8901-bcde-f12345678901'', NULL),
    (''e891d17c-d8a0-4681-2e67-c2535102b824'', ''2026-07-02T00:21:55.5247369Z'', ''d1000004-0000-0000-0000-000000000004'', ''a1b2c3d4-e5f6-7890-abcd-ef1234567890'', NULL),
    (''f22816d7-77fb-13d5-6c4a-f0a724d75f2d'', ''2026-07-02T00:21:55.5247369Z'', ''d1000002-0000-0000-0000-000000000001'', ''c3d4e5f6-a7b8-9012-cdef-123456789012'', NULL),
    (''f71bec3e-d4d4-0866-0ed8-278392b16cc5'', ''2026-07-02T00:21:55.5247369Z'', ''d1000007-0000-0000-0000-000000000002'', ''b2c3d4e5-f6a7-8901-bcde-f12345678901'', NULL),
    (''f7b250ee-574b-ccf7-78da-b6b63b865ed6'', ''2026-07-02T00:21:55.5247369Z'', ''d1000007-0000-0000-0000-000000000001'', ''b2c3d4e5-f6a7-8901-bcde-f12345678901'', NULL),
    (''fbf2ce76-86d4-1963-ba14-10b9ff31b052'', ''2026-07-02T00:21:55.5247369Z'', ''d1000009-0000-0000-0000-000000000001'', ''a1b2c3d4-e5f6-7890-abcd-ef1234567890'', NULL),
    (''ff6fb9b4-3a44-22b2-9239-a335bc2aa23e'', ''2026-07-02T00:21:55.5247369Z'', ''d1000007-0000-0000-0000-000000000001'', ''c3d4e5f6-a7b8-9012-cdef-123456789012'', NULL)');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'CreatedAt', N'PermissionId', N'RoleId', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[RolePermissions]'))
        SET IDENTITY_INSERT [RolePermissions] OFF;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702002157_CreateWhatsAppTemplates'
)
BEGIN
    CREATE INDEX [IX_PermissionAuditLogs_UserId] ON [PermissionAuditLogs] ([UserId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702002157_CreateWhatsAppTemplates'
)
BEGIN
    CREATE UNIQUE INDEX [IX_WhatsAppTemplates_Name] ON [WhatsAppTemplates] ([Name]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702002157_CreateWhatsAppTemplates'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260702002157_CreateWhatsAppTemplates', N'10.0.9');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702004320_AddWhatsAppTemplateTriggerType'
)
BEGIN
    ALTER TABLE [WhatsAppTemplates] ADD [TriggerType] int NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702004320_AddWhatsAppTemplateTriggerType'
)
BEGIN
    EXEC(N'UPDATE [Permissions] SET [CreatedAt] = ''2026-07-02T00:43:18.4282081Z''
    WHERE [Id] = ''d1000001-0000-0000-0000-000000000001'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702004320_AddWhatsAppTemplateTriggerType'
)
BEGIN
    EXEC(N'UPDATE [Permissions] SET [CreatedAt] = ''2026-07-02T00:43:18.4282081Z''
    WHERE [Id] = ''d1000001-0000-0000-0000-000000000002'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702004320_AddWhatsAppTemplateTriggerType'
)
BEGIN
    EXEC(N'UPDATE [Permissions] SET [CreatedAt] = ''2026-07-02T00:43:18.4282081Z''
    WHERE [Id] = ''d1000002-0000-0000-0000-000000000001'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702004320_AddWhatsAppTemplateTriggerType'
)
BEGIN
    EXEC(N'UPDATE [Permissions] SET [CreatedAt] = ''2026-07-02T00:43:18.4282081Z''
    WHERE [Id] = ''d1000002-0000-0000-0000-000000000002'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702004320_AddWhatsAppTemplateTriggerType'
)
BEGIN
    EXEC(N'UPDATE [Permissions] SET [CreatedAt] = ''2026-07-02T00:43:18.4282081Z''
    WHERE [Id] = ''d1000002-0000-0000-0000-000000000003'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702004320_AddWhatsAppTemplateTriggerType'
)
BEGIN
    EXEC(N'UPDATE [Permissions] SET [CreatedAt] = ''2026-07-02T00:43:18.4282081Z''
    WHERE [Id] = ''d1000002-0000-0000-0000-000000000004'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702004320_AddWhatsAppTemplateTriggerType'
)
BEGIN
    EXEC(N'UPDATE [Permissions] SET [CreatedAt] = ''2026-07-02T00:43:18.4282081Z''
    WHERE [Id] = ''d1000003-0000-0000-0000-000000000001'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702004320_AddWhatsAppTemplateTriggerType'
)
BEGIN
    EXEC(N'UPDATE [Permissions] SET [CreatedAt] = ''2026-07-02T00:43:18.4282081Z''
    WHERE [Id] = ''d1000003-0000-0000-0000-000000000002'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702004320_AddWhatsAppTemplateTriggerType'
)
BEGIN
    EXEC(N'UPDATE [Permissions] SET [CreatedAt] = ''2026-07-02T00:43:18.4282081Z''
    WHERE [Id] = ''d1000003-0000-0000-0000-000000000003'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702004320_AddWhatsAppTemplateTriggerType'
)
BEGIN
    EXEC(N'UPDATE [Permissions] SET [CreatedAt] = ''2026-07-02T00:43:18.4282081Z''
    WHERE [Id] = ''d1000003-0000-0000-0000-000000000004'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702004320_AddWhatsAppTemplateTriggerType'
)
BEGIN
    EXEC(N'UPDATE [Permissions] SET [CreatedAt] = ''2026-07-02T00:43:18.4282081Z''
    WHERE [Id] = ''d1000004-0000-0000-0000-000000000001'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702004320_AddWhatsAppTemplateTriggerType'
)
BEGIN
    EXEC(N'UPDATE [Permissions] SET [CreatedAt] = ''2026-07-02T00:43:18.4282081Z''
    WHERE [Id] = ''d1000004-0000-0000-0000-000000000002'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702004320_AddWhatsAppTemplateTriggerType'
)
BEGIN
    EXEC(N'UPDATE [Permissions] SET [CreatedAt] = ''2026-07-02T00:43:18.4282081Z''
    WHERE [Id] = ''d1000004-0000-0000-0000-000000000003'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702004320_AddWhatsAppTemplateTriggerType'
)
BEGIN
    EXEC(N'UPDATE [Permissions] SET [CreatedAt] = ''2026-07-02T00:43:18.4282081Z''
    WHERE [Id] = ''d1000004-0000-0000-0000-000000000004'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702004320_AddWhatsAppTemplateTriggerType'
)
BEGIN
    EXEC(N'UPDATE [Permissions] SET [CreatedAt] = ''2026-07-02T00:43:18.4282081Z''
    WHERE [Id] = ''d1000005-0000-0000-0000-000000000001'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702004320_AddWhatsAppTemplateTriggerType'
)
BEGIN
    EXEC(N'UPDATE [Permissions] SET [CreatedAt] = ''2026-07-02T00:43:18.4282081Z''
    WHERE [Id] = ''d1000005-0000-0000-0000-000000000002'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702004320_AddWhatsAppTemplateTriggerType'
)
BEGIN
    EXEC(N'UPDATE [Permissions] SET [CreatedAt] = ''2026-07-02T00:43:18.4282081Z''
    WHERE [Id] = ''d1000005-0000-0000-0000-000000000003'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702004320_AddWhatsAppTemplateTriggerType'
)
BEGIN
    EXEC(N'UPDATE [Permissions] SET [CreatedAt] = ''2026-07-02T00:43:18.4282081Z''
    WHERE [Id] = ''d1000005-0000-0000-0000-000000000004'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702004320_AddWhatsAppTemplateTriggerType'
)
BEGIN
    EXEC(N'UPDATE [Permissions] SET [CreatedAt] = ''2026-07-02T00:43:18.4282081Z''
    WHERE [Id] = ''d1000005-0000-0000-0000-000000000005'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702004320_AddWhatsAppTemplateTriggerType'
)
BEGIN
    EXEC(N'UPDATE [Permissions] SET [CreatedAt] = ''2026-07-02T00:43:18.4282081Z''
    WHERE [Id] = ''d1000005-0000-0000-0000-000000000006'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702004320_AddWhatsAppTemplateTriggerType'
)
BEGIN
    EXEC(N'UPDATE [Permissions] SET [CreatedAt] = ''2026-07-02T00:43:18.4282081Z''
    WHERE [Id] = ''d1000005-0000-0000-0000-000000000007'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702004320_AddWhatsAppTemplateTriggerType'
)
BEGIN
    EXEC(N'UPDATE [Permissions] SET [CreatedAt] = ''2026-07-02T00:43:18.4282081Z''
    WHERE [Id] = ''d1000006-0000-0000-0000-000000000001'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702004320_AddWhatsAppTemplateTriggerType'
)
BEGIN
    EXEC(N'UPDATE [Permissions] SET [CreatedAt] = ''2026-07-02T00:43:18.4282081Z''
    WHERE [Id] = ''d1000006-0000-0000-0000-000000000002'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702004320_AddWhatsAppTemplateTriggerType'
)
BEGIN
    EXEC(N'UPDATE [Permissions] SET [CreatedAt] = ''2026-07-02T00:43:18.4282081Z''
    WHERE [Id] = ''d1000006-0000-0000-0000-000000000003'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702004320_AddWhatsAppTemplateTriggerType'
)
BEGIN
    EXEC(N'UPDATE [Permissions] SET [CreatedAt] = ''2026-07-02T00:43:18.4282081Z''
    WHERE [Id] = ''d1000006-0000-0000-0000-000000000004'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702004320_AddWhatsAppTemplateTriggerType'
)
BEGIN
    EXEC(N'UPDATE [Permissions] SET [CreatedAt] = ''2026-07-02T00:43:18.4282081Z''
    WHERE [Id] = ''d1000007-0000-0000-0000-000000000001'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702004320_AddWhatsAppTemplateTriggerType'
)
BEGIN
    EXEC(N'UPDATE [Permissions] SET [CreatedAt] = ''2026-07-02T00:43:18.4282081Z''
    WHERE [Id] = ''d1000007-0000-0000-0000-000000000002'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702004320_AddWhatsAppTemplateTriggerType'
)
BEGIN
    EXEC(N'UPDATE [Permissions] SET [CreatedAt] = ''2026-07-02T00:43:18.4282081Z''
    WHERE [Id] = ''d1000008-0000-0000-0000-000000000001'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702004320_AddWhatsAppTemplateTriggerType'
)
BEGIN
    EXEC(N'UPDATE [Permissions] SET [CreatedAt] = ''2026-07-02T00:43:18.4282081Z''
    WHERE [Id] = ''d1000008-0000-0000-0000-000000000002'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702004320_AddWhatsAppTemplateTriggerType'
)
BEGIN
    EXEC(N'UPDATE [Permissions] SET [CreatedAt] = ''2026-07-02T00:43:18.4282081Z''
    WHERE [Id] = ''d1000009-0000-0000-0000-000000000001'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702004320_AddWhatsAppTemplateTriggerType'
)
BEGIN
    EXEC(N'UPDATE [Permissions] SET [CreatedAt] = ''2026-07-02T00:43:18.4282081Z''
    WHERE [Id] = ''d1000009-0000-0000-0000-000000000002'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702004320_AddWhatsAppTemplateTriggerType'
)
BEGIN
    EXEC(N'UPDATE [Permissions] SET [CreatedAt] = ''2026-07-02T00:43:18.4282081Z''
    WHERE [Id] = ''d1000010-0000-0000-0000-000000000001'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702004320_AddWhatsAppTemplateTriggerType'
)
BEGIN
    EXEC(N'UPDATE [Permissions] SET [CreatedAt] = ''2026-07-02T00:43:18.4282081Z''
    WHERE [Id] = ''d1000010-0000-0000-0000-000000000002'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702004320_AddWhatsAppTemplateTriggerType'
)
BEGIN
    EXEC(N'UPDATE [Permissions] SET [CreatedAt] = ''2026-07-02T00:43:18.4282081Z''
    WHERE [Id] = ''d1000011-0000-0000-0000-000000000001'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702004320_AddWhatsAppTemplateTriggerType'
)
BEGIN
    EXEC(N'UPDATE [Permissions] SET [CreatedAt] = ''2026-07-02T00:43:18.4282081Z''
    WHERE [Id] = ''d1000011-0000-0000-0000-000000000002'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702004320_AddWhatsAppTemplateTriggerType'
)
BEGIN
    EXEC(N'UPDATE [Permissions] SET [CreatedAt] = ''2026-07-02T00:43:18.4282081Z''
    WHERE [Id] = ''d1000011-0000-0000-0000-000000000003'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702004320_AddWhatsAppTemplateTriggerType'
)
BEGIN
    EXEC(N'UPDATE [Permissions] SET [CreatedAt] = ''2026-07-02T00:43:18.4282081Z''
    WHERE [Id] = ''d1000011-0000-0000-0000-000000000004'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702004320_AddWhatsAppTemplateTriggerType'
)
BEGIN
    EXEC(N'UPDATE [Permissions] SET [CreatedAt] = ''2026-07-02T00:43:18.4282081Z''
    WHERE [Id] = ''d1000012-0000-0000-0000-000000000001'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702004320_AddWhatsAppTemplateTriggerType'
)
BEGIN
    EXEC(N'UPDATE [Permissions] SET [CreatedAt] = ''2026-07-02T00:43:18.4282081Z''
    WHERE [Id] = ''d1000012-0000-0000-0000-000000000002'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702004320_AddWhatsAppTemplateTriggerType'
)
BEGIN
    EXEC(N'UPDATE [Permissions] SET [CreatedAt] = ''2026-07-02T00:43:18.4282081Z''
    WHERE [Id] = ''d1000012-0000-0000-0000-000000000003'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702004320_AddWhatsAppTemplateTriggerType'
)
BEGIN
    EXEC(N'UPDATE [Permissions] SET [CreatedAt] = ''2026-07-02T00:43:18.4282081Z''
    WHERE [Id] = ''d1000012-0000-0000-0000-000000000004'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702004320_AddWhatsAppTemplateTriggerType'
)
BEGIN
    EXEC(N'UPDATE [Permissions] SET [CreatedAt] = ''2026-07-02T00:43:18.4282081Z''
    WHERE [Id] = ''d1000013-0000-0000-0000-000000000001'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702004320_AddWhatsAppTemplateTriggerType'
)
BEGIN
    EXEC(N'UPDATE [Permissions] SET [CreatedAt] = ''2026-07-02T00:43:18.4282081Z''
    WHERE [Id] = ''d1000013-0000-0000-0000-000000000002'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702004320_AddWhatsAppTemplateTriggerType'
)
BEGIN
    EXEC(N'UPDATE [Permissions] SET [CreatedAt] = ''2026-07-02T00:43:18.4282081Z''
    WHERE [Id] = ''d1000014-0000-0000-0000-000000000001'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702004320_AddWhatsAppTemplateTriggerType'
)
BEGIN
    EXEC(N'UPDATE [Permissions] SET [CreatedAt] = ''2026-07-02T00:43:18.4282081Z''
    WHERE [Id] = ''d1000015-0000-0000-0000-000000000001'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702004320_AddWhatsAppTemplateTriggerType'
)
BEGIN
    EXEC(N'UPDATE [Permissions] SET [CreatedAt] = ''2026-07-02T00:43:18.4282081Z''
    WHERE [Id] = ''d1000015-0000-0000-0000-000000000002'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702004320_AddWhatsAppTemplateTriggerType'
)
BEGIN
    EXEC(N'UPDATE [Permissions] SET [CreatedAt] = ''2026-07-02T00:43:18.4282081Z''
    WHERE [Id] = ''d1000016-0000-0000-0000-000000000001'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702004320_AddWhatsAppTemplateTriggerType'
)
BEGIN
    EXEC(N'UPDATE [Permissions] SET [CreatedAt] = ''2026-07-02T00:43:18.4282081Z''
    WHERE [Id] = ''d1000016-0000-0000-0000-000000000002'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702004320_AddWhatsAppTemplateTriggerType'
)
BEGIN
    EXEC(N'UPDATE [RolePermissions] SET [CreatedAt] = ''2026-07-02T00:43:18.4317219Z''
    WHERE [Id] = ''005f9b64-4d0a-6c82-947e-e57cfe738452'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702004320_AddWhatsAppTemplateTriggerType'
)
BEGIN
    EXEC(N'UPDATE [RolePermissions] SET [CreatedAt] = ''2026-07-02T00:43:18.4317219Z''
    WHERE [Id] = ''05195fdb-cd30-4561-9168-bbeb6f886a69'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702004320_AddWhatsAppTemplateTriggerType'
)
BEGIN
    EXEC(N'UPDATE [RolePermissions] SET [CreatedAt] = ''2026-07-02T00:43:18.4317219Z''
    WHERE [Id] = ''0a025850-9197-728d-2039-14fa13c954ce'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702004320_AddWhatsAppTemplateTriggerType'
)
BEGIN
    EXEC(N'UPDATE [RolePermissions] SET [CreatedAt] = ''2026-07-02T00:43:18.4317219Z''
    WHERE [Id] = ''0e32d4f0-e31f-1692-6763-43d06de36c0e'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702004320_AddWhatsAppTemplateTriggerType'
)
BEGIN
    EXEC(N'UPDATE [RolePermissions] SET [CreatedAt] = ''2026-07-02T00:43:18.4317219Z''
    WHERE [Id] = ''1def5b70-20cb-7e7b-b398-51bae635cb4c'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702004320_AddWhatsAppTemplateTriggerType'
)
BEGIN
    EXEC(N'UPDATE [RolePermissions] SET [CreatedAt] = ''2026-07-02T00:43:18.4317219Z''
    WHERE [Id] = ''2babdd1d-2af8-85c1-be3d-93660af5d9ca'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702004320_AddWhatsAppTemplateTriggerType'
)
BEGIN
    EXEC(N'UPDATE [RolePermissions] SET [CreatedAt] = ''2026-07-02T00:43:18.4317219Z''
    WHERE [Id] = ''2cc74796-4a71-5342-2d08-a37e9154996d'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702004320_AddWhatsAppTemplateTriggerType'
)
BEGIN
    EXEC(N'UPDATE [RolePermissions] SET [CreatedAt] = ''2026-07-02T00:43:18.4317219Z''
    WHERE [Id] = ''32c5144e-c485-1183-98b8-403fbb277ebd'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702004320_AddWhatsAppTemplateTriggerType'
)
BEGIN
    EXEC(N'UPDATE [RolePermissions] SET [CreatedAt] = ''2026-07-02T00:43:18.4317219Z''
    WHERE [Id] = ''35837f97-2623-ab10-0cbd-370671326f89'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702004320_AddWhatsAppTemplateTriggerType'
)
BEGIN
    EXEC(N'UPDATE [RolePermissions] SET [CreatedAt] = ''2026-07-02T00:43:18.4317219Z''
    WHERE [Id] = ''3c0e6bfe-9913-56af-20cb-cc7f4e36a4a6'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702004320_AddWhatsAppTemplateTriggerType'
)
BEGIN
    EXEC(N'UPDATE [RolePermissions] SET [CreatedAt] = ''2026-07-02T00:43:18.4317219Z''
    WHERE [Id] = ''430af60c-cfb8-d59a-e833-8cd110ff7f06'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702004320_AddWhatsAppTemplateTriggerType'
)
BEGIN
    EXEC(N'UPDATE [RolePermissions] SET [CreatedAt] = ''2026-07-02T00:43:18.4317219Z''
    WHERE [Id] = ''43fe5635-7b4d-ab3d-a51e-3559aced41c5'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702004320_AddWhatsAppTemplateTriggerType'
)
BEGIN
    EXEC(N'UPDATE [RolePermissions] SET [CreatedAt] = ''2026-07-02T00:43:18.4317219Z''
    WHERE [Id] = ''45a0ab26-2f4b-7f21-a073-171f907faf77'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702004320_AddWhatsAppTemplateTriggerType'
)
BEGIN
    EXEC(N'UPDATE [RolePermissions] SET [CreatedAt] = ''2026-07-02T00:43:18.4317219Z''
    WHERE [Id] = ''477c3b9c-bdb3-a703-0b56-f6516dcfc7e5'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702004320_AddWhatsAppTemplateTriggerType'
)
BEGIN
    EXEC(N'UPDATE [RolePermissions] SET [CreatedAt] = ''2026-07-02T00:43:18.4317219Z''
    WHERE [Id] = ''48e05a34-1b30-1cdc-ca0d-974cb2c3ebd8'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702004320_AddWhatsAppTemplateTriggerType'
)
BEGIN
    EXEC(N'UPDATE [RolePermissions] SET [CreatedAt] = ''2026-07-02T00:43:18.4317219Z''
    WHERE [Id] = ''4d02d95c-4aab-3930-8c9f-b46c21d1be4e'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702004320_AddWhatsAppTemplateTriggerType'
)
BEGIN
    EXEC(N'UPDATE [RolePermissions] SET [CreatedAt] = ''2026-07-02T00:43:18.4317219Z''
    WHERE [Id] = ''5334b704-3d25-6b96-517f-6443fb11c414'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702004320_AddWhatsAppTemplateTriggerType'
)
BEGIN
    EXEC(N'UPDATE [RolePermissions] SET [CreatedAt] = ''2026-07-02T00:43:18.4317219Z''
    WHERE [Id] = ''55690f65-12c3-9922-c4b1-47b34caedde2'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702004320_AddWhatsAppTemplateTriggerType'
)
BEGIN
    EXEC(N'UPDATE [RolePermissions] SET [CreatedAt] = ''2026-07-02T00:43:18.4317219Z''
    WHERE [Id] = ''557952bd-50d0-d062-f35e-7fe6224bd482'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702004320_AddWhatsAppTemplateTriggerType'
)
BEGIN
    EXEC(N'UPDATE [RolePermissions] SET [CreatedAt] = ''2026-07-02T00:43:18.4317219Z''
    WHERE [Id] = ''566928b3-00dc-fb90-39b7-af77eb0f35b1'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702004320_AddWhatsAppTemplateTriggerType'
)
BEGIN
    EXEC(N'UPDATE [RolePermissions] SET [CreatedAt] = ''2026-07-02T00:43:18.4317219Z''
    WHERE [Id] = ''5f9b5de3-e563-a2d9-672a-91816ba23dd6'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702004320_AddWhatsAppTemplateTriggerType'
)
BEGIN
    EXEC(N'UPDATE [RolePermissions] SET [CreatedAt] = ''2026-07-02T00:43:18.4317219Z''
    WHERE [Id] = ''63139b75-fbfc-2bcf-36d2-43bc185b9a05'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702004320_AddWhatsAppTemplateTriggerType'
)
BEGIN
    EXEC(N'UPDATE [RolePermissions] SET [CreatedAt] = ''2026-07-02T00:43:18.4317219Z''
    WHERE [Id] = ''6604e362-f7af-bb42-e9e1-448a76c8e188'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702004320_AddWhatsAppTemplateTriggerType'
)
BEGIN
    EXEC(N'UPDATE [RolePermissions] SET [CreatedAt] = ''2026-07-02T00:43:18.4317219Z''
    WHERE [Id] = ''6a745601-638c-730b-cda2-d7a67e9f5a16'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702004320_AddWhatsAppTemplateTriggerType'
)
BEGIN
    EXEC(N'UPDATE [RolePermissions] SET [CreatedAt] = ''2026-07-02T00:43:18.4317219Z''
    WHERE [Id] = ''6b5ac608-95b1-cd3e-d869-0fdf79a09fd0'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702004320_AddWhatsAppTemplateTriggerType'
)
BEGIN
    EXEC(N'UPDATE [RolePermissions] SET [CreatedAt] = ''2026-07-02T00:43:18.4317219Z''
    WHERE [Id] = ''6b6ec0d1-eb14-8d88-e615-78e6fe6177bf'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702004320_AddWhatsAppTemplateTriggerType'
)
BEGIN
    EXEC(N'UPDATE [RolePermissions] SET [CreatedAt] = ''2026-07-02T00:43:18.4317219Z''
    WHERE [Id] = ''70ec0ef2-a390-97c2-76ef-1678f6cf8fbf'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702004320_AddWhatsAppTemplateTriggerType'
)
BEGIN
    EXEC(N'UPDATE [RolePermissions] SET [CreatedAt] = ''2026-07-02T00:43:18.4317219Z''
    WHERE [Id] = ''7297595c-0a29-eb0d-19be-c531ee673c5b'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702004320_AddWhatsAppTemplateTriggerType'
)
BEGIN
    EXEC(N'UPDATE [RolePermissions] SET [CreatedAt] = ''2026-07-02T00:43:18.4317219Z''
    WHERE [Id] = ''72bf7e60-3016-f5ff-ec42-ddfc1e27acfc'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702004320_AddWhatsAppTemplateTriggerType'
)
BEGIN
    EXEC(N'UPDATE [RolePermissions] SET [CreatedAt] = ''2026-07-02T00:43:18.4317219Z''
    WHERE [Id] = ''7347301a-7584-8075-4aab-788b19078a62'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702004320_AddWhatsAppTemplateTriggerType'
)
BEGIN
    EXEC(N'UPDATE [RolePermissions] SET [CreatedAt] = ''2026-07-02T00:43:18.4317219Z''
    WHERE [Id] = ''78615f0f-b666-f010-e57b-dfb05c66f035'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702004320_AddWhatsAppTemplateTriggerType'
)
BEGIN
    EXEC(N'UPDATE [RolePermissions] SET [CreatedAt] = ''2026-07-02T00:43:18.4317219Z''
    WHERE [Id] = ''78b53d72-0ba7-6249-68a8-bedd284304f4'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702004320_AddWhatsAppTemplateTriggerType'
)
BEGIN
    EXEC(N'UPDATE [RolePermissions] SET [CreatedAt] = ''2026-07-02T00:43:18.4317219Z''
    WHERE [Id] = ''7916078f-5d85-86e1-8747-b5dd207f40b5'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702004320_AddWhatsAppTemplateTriggerType'
)
BEGIN
    EXEC(N'UPDATE [RolePermissions] SET [CreatedAt] = ''2026-07-02T00:43:18.4317219Z''
    WHERE [Id] = ''82cdc680-60c7-ca13-1fba-0c1725b381fa'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702004320_AddWhatsAppTemplateTriggerType'
)
BEGIN
    EXEC(N'UPDATE [RolePermissions] SET [CreatedAt] = ''2026-07-02T00:43:18.4317219Z''
    WHERE [Id] = ''85bf1ab4-8b34-6fb0-9ad7-cbbd215d0949'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702004320_AddWhatsAppTemplateTriggerType'
)
BEGIN
    EXEC(N'UPDATE [RolePermissions] SET [CreatedAt] = ''2026-07-02T00:43:18.4317219Z''
    WHERE [Id] = ''8680abc3-7a3f-fc42-ec4b-282165955f1c'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702004320_AddWhatsAppTemplateTriggerType'
)
BEGIN
    EXEC(N'UPDATE [RolePermissions] SET [CreatedAt] = ''2026-07-02T00:43:18.4317219Z''
    WHERE [Id] = ''86f4523e-677d-889c-8e00-7ee5a50d94c5'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702004320_AddWhatsAppTemplateTriggerType'
)
BEGIN
    EXEC(N'UPDATE [RolePermissions] SET [CreatedAt] = ''2026-07-02T00:43:18.4317219Z''
    WHERE [Id] = ''89d0bc79-33f4-2a4d-f951-da329c328c20'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702004320_AddWhatsAppTemplateTriggerType'
)
BEGIN
    EXEC(N'UPDATE [RolePermissions] SET [CreatedAt] = ''2026-07-02T00:43:18.4317219Z''
    WHERE [Id] = ''8ef92960-4f9f-70c2-58b7-a0fc32743d95'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702004320_AddWhatsAppTemplateTriggerType'
)
BEGIN
    EXEC(N'UPDATE [RolePermissions] SET [CreatedAt] = ''2026-07-02T00:43:18.4317219Z''
    WHERE [Id] = ''904ee737-7848-bded-ec47-ed74c6b3b5e5'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702004320_AddWhatsAppTemplateTriggerType'
)
BEGIN
    EXEC(N'UPDATE [RolePermissions] SET [CreatedAt] = ''2026-07-02T00:43:18.4317219Z''
    WHERE [Id] = ''9cbf51af-d816-7ca8-28cf-67d124fb62c0'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702004320_AddWhatsAppTemplateTriggerType'
)
BEGIN
    EXEC(N'UPDATE [RolePermissions] SET [CreatedAt] = ''2026-07-02T00:43:18.4317219Z''
    WHERE [Id] = ''a54e820b-654c-d289-f188-11d672a9a217'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702004320_AddWhatsAppTemplateTriggerType'
)
BEGIN
    EXEC(N'UPDATE [RolePermissions] SET [CreatedAt] = ''2026-07-02T00:43:18.4317219Z''
    WHERE [Id] = ''a5574f00-d065-186a-8630-9c6de44235eb'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702004320_AddWhatsAppTemplateTriggerType'
)
BEGIN
    EXEC(N'UPDATE [RolePermissions] SET [CreatedAt] = ''2026-07-02T00:43:18.4317219Z''
    WHERE [Id] = ''a9035ef4-18c2-3e13-d5cc-99fc5e63ed31'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702004320_AddWhatsAppTemplateTriggerType'
)
BEGIN
    EXEC(N'UPDATE [RolePermissions] SET [CreatedAt] = ''2026-07-02T00:43:18.4317219Z''
    WHERE [Id] = ''ace9a66b-d17d-2b50-17b3-67eb40dcf41a'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702004320_AddWhatsAppTemplateTriggerType'
)
BEGIN
    EXEC(N'UPDATE [RolePermissions] SET [CreatedAt] = ''2026-07-02T00:43:18.4317219Z''
    WHERE [Id] = ''ad012924-5237-6205-38fe-d48cedc33d4d'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702004320_AddWhatsAppTemplateTriggerType'
)
BEGIN
    EXEC(N'UPDATE [RolePermissions] SET [CreatedAt] = ''2026-07-02T00:43:18.4317219Z''
    WHERE [Id] = ''b2f04998-b570-47d2-8236-0756a4e219e2'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702004320_AddWhatsAppTemplateTriggerType'
)
BEGIN
    EXEC(N'UPDATE [RolePermissions] SET [CreatedAt] = ''2026-07-02T00:43:18.4317219Z''
    WHERE [Id] = ''b4e93b8d-5dc3-24cf-da95-fb3403b3749e'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702004320_AddWhatsAppTemplateTriggerType'
)
BEGIN
    EXEC(N'UPDATE [RolePermissions] SET [CreatedAt] = ''2026-07-02T00:43:18.4317219Z''
    WHERE [Id] = ''b5e24ad4-8420-94b6-b8fc-9dc56e2a445a'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702004320_AddWhatsAppTemplateTriggerType'
)
BEGIN
    EXEC(N'UPDATE [RolePermissions] SET [CreatedAt] = ''2026-07-02T00:43:18.4317219Z''
    WHERE [Id] = ''b94ea0da-6b9a-ea4f-6ac8-08b4e88272fe'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702004320_AddWhatsAppTemplateTriggerType'
)
BEGIN
    EXEC(N'UPDATE [RolePermissions] SET [CreatedAt] = ''2026-07-02T00:43:18.4317219Z''
    WHERE [Id] = ''bbe26c6f-807b-dba2-eef3-33cd059b3296'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702004320_AddWhatsAppTemplateTriggerType'
)
BEGIN
    EXEC(N'UPDATE [RolePermissions] SET [CreatedAt] = ''2026-07-02T00:43:18.4317219Z''
    WHERE [Id] = ''bc79a3c2-f98e-9c5b-5c26-3072265353bc'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702004320_AddWhatsAppTemplateTriggerType'
)
BEGIN
    EXEC(N'UPDATE [RolePermissions] SET [CreatedAt] = ''2026-07-02T00:43:18.4317219Z''
    WHERE [Id] = ''bde31dac-221c-d02b-fca6-51fa576f9b89'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702004320_AddWhatsAppTemplateTriggerType'
)
BEGIN
    EXEC(N'UPDATE [RolePermissions] SET [CreatedAt] = ''2026-07-02T00:43:18.4317219Z''
    WHERE [Id] = ''bf6ac31e-ca1d-b678-8e6e-3cc7b7dac946'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702004320_AddWhatsAppTemplateTriggerType'
)
BEGIN
    EXEC(N'UPDATE [RolePermissions] SET [CreatedAt] = ''2026-07-02T00:43:18.4317219Z''
    WHERE [Id] = ''bf70e113-e074-6b40-6534-fb4c1f9231d3'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702004320_AddWhatsAppTemplateTriggerType'
)
BEGIN
    EXEC(N'UPDATE [RolePermissions] SET [CreatedAt] = ''2026-07-02T00:43:18.4317219Z''
    WHERE [Id] = ''c175a900-6ce3-e698-94df-f33b4ed6bb50'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702004320_AddWhatsAppTemplateTriggerType'
)
BEGIN
    EXEC(N'UPDATE [RolePermissions] SET [CreatedAt] = ''2026-07-02T00:43:18.4317219Z''
    WHERE [Id] = ''c1a0d8cb-6b48-74ec-9919-d577f704c514'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702004320_AddWhatsAppTemplateTriggerType'
)
BEGIN
    EXEC(N'UPDATE [RolePermissions] SET [CreatedAt] = ''2026-07-02T00:43:18.4317219Z''
    WHERE [Id] = ''c24d281f-89aa-dd6c-c264-59d5b5ba0faa'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702004320_AddWhatsAppTemplateTriggerType'
)
BEGIN
    EXEC(N'UPDATE [RolePermissions] SET [CreatedAt] = ''2026-07-02T00:43:18.4317219Z''
    WHERE [Id] = ''ce25f18c-ed21-3f6b-8dd1-82406f135d61'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702004320_AddWhatsAppTemplateTriggerType'
)
BEGIN
    EXEC(N'UPDATE [RolePermissions] SET [CreatedAt] = ''2026-07-02T00:43:18.4317219Z''
    WHERE [Id] = ''ceadb402-0bb1-ea6e-7525-16b7a2ba8317'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702004320_AddWhatsAppTemplateTriggerType'
)
BEGIN
    EXEC(N'UPDATE [RolePermissions] SET [CreatedAt] = ''2026-07-02T00:43:18.4317219Z''
    WHERE [Id] = ''d138659a-966d-09d0-2309-f233f3f232d1'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702004320_AddWhatsAppTemplateTriggerType'
)
BEGIN
    EXEC(N'UPDATE [RolePermissions] SET [CreatedAt] = ''2026-07-02T00:43:18.4317219Z''
    WHERE [Id] = ''d5c2ad14-559d-e8a6-0702-d8c9f26af634'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702004320_AddWhatsAppTemplateTriggerType'
)
BEGIN
    EXEC(N'UPDATE [RolePermissions] SET [CreatedAt] = ''2026-07-02T00:43:18.4317219Z''
    WHERE [Id] = ''ddaa2356-dbdd-cef7-9cde-69e9fe7387a8'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702004320_AddWhatsAppTemplateTriggerType'
)
BEGIN
    EXEC(N'UPDATE [RolePermissions] SET [CreatedAt] = ''2026-07-02T00:43:18.4317219Z''
    WHERE [Id] = ''e02b85cd-9dd3-5a0e-0700-cd4e3f2d9ed4'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702004320_AddWhatsAppTemplateTriggerType'
)
BEGIN
    EXEC(N'UPDATE [RolePermissions] SET [CreatedAt] = ''2026-07-02T00:43:18.4317219Z''
    WHERE [Id] = ''e2609822-210e-b891-0a3b-4be9978968e8'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702004320_AddWhatsAppTemplateTriggerType'
)
BEGIN
    EXEC(N'UPDATE [RolePermissions] SET [CreatedAt] = ''2026-07-02T00:43:18.4317219Z''
    WHERE [Id] = ''e296cb77-368f-3b74-5b92-300a6e222336'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702004320_AddWhatsAppTemplateTriggerType'
)
BEGIN
    EXEC(N'UPDATE [RolePermissions] SET [CreatedAt] = ''2026-07-02T00:43:18.4317219Z''
    WHERE [Id] = ''e2b9095b-3271-9fb3-b450-3c31cf89f018'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702004320_AddWhatsAppTemplateTriggerType'
)
BEGIN
    EXEC(N'UPDATE [RolePermissions] SET [CreatedAt] = ''2026-07-02T00:43:18.4317219Z''
    WHERE [Id] = ''e4d49503-81db-8555-9c3e-63285b5fb0e3'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702004320_AddWhatsAppTemplateTriggerType'
)
BEGIN
    EXEC(N'UPDATE [RolePermissions] SET [CreatedAt] = ''2026-07-02T00:43:18.4317219Z''
    WHERE [Id] = ''e71d339d-af13-2cae-30f1-389d04db8597'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702004320_AddWhatsAppTemplateTriggerType'
)
BEGIN
    EXEC(N'UPDATE [RolePermissions] SET [CreatedAt] = ''2026-07-02T00:43:18.4317219Z''
    WHERE [Id] = ''e891d17c-d8a0-4681-2e67-c2535102b824'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702004320_AddWhatsAppTemplateTriggerType'
)
BEGIN
    EXEC(N'UPDATE [RolePermissions] SET [CreatedAt] = ''2026-07-02T00:43:18.4317219Z''
    WHERE [Id] = ''f22816d7-77fb-13d5-6c4a-f0a724d75f2d'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702004320_AddWhatsAppTemplateTriggerType'
)
BEGIN
    EXEC(N'UPDATE [RolePermissions] SET [CreatedAt] = ''2026-07-02T00:43:18.4317219Z''
    WHERE [Id] = ''f71bec3e-d4d4-0866-0ed8-278392b16cc5'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702004320_AddWhatsAppTemplateTriggerType'
)
BEGIN
    EXEC(N'UPDATE [RolePermissions] SET [CreatedAt] = ''2026-07-02T00:43:18.4317219Z''
    WHERE [Id] = ''f7b250ee-574b-ccf7-78da-b6b63b865ed6'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702004320_AddWhatsAppTemplateTriggerType'
)
BEGIN
    EXEC(N'UPDATE [RolePermissions] SET [CreatedAt] = ''2026-07-02T00:43:18.4317219Z''
    WHERE [Id] = ''fbf2ce76-86d4-1963-ba14-10b9ff31b052'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702004320_AddWhatsAppTemplateTriggerType'
)
BEGIN
    EXEC(N'UPDATE [RolePermissions] SET [CreatedAt] = ''2026-07-02T00:43:18.4317219Z''
    WHERE [Id] = ''ff6fb9b4-3a44-22b2-9239-a335bc2aa23e'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702004320_AddWhatsAppTemplateTriggerType'
)
BEGIN
    EXEC(N'UPDATE [Roles] SET [CreatedAt] = ''2026-07-02T00:43:17.9789946Z''
    WHERE [Id] = ''a1b2c3d4-e5f6-7890-abcd-ef1234567890'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702004320_AddWhatsAppTemplateTriggerType'
)
BEGIN
    EXEC(N'UPDATE [Roles] SET [CreatedAt] = ''2026-07-02T00:43:17.9789946Z''
    WHERE [Id] = ''b2c3d4e5-f6a7-8901-bcde-f12345678901'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702004320_AddWhatsAppTemplateTriggerType'
)
BEGIN
    EXEC(N'UPDATE [Roles] SET [CreatedAt] = ''2026-07-02T00:43:17.9789946Z''
    WHERE [Id] = ''c3d4e5f6-a7b8-9012-cdef-123456789012'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702004320_AddWhatsAppTemplateTriggerType'
)
BEGIN
    EXEC(N'UPDATE [Settings] SET [CreatedAt] = ''2026-07-02T00:43:18.4340159Z''
    WHERE [Id] = ''a5b6c7d8-e9f0-1234-a567-890123456789'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702004320_AddWhatsAppTemplateTriggerType'
)
BEGIN
    EXEC(N'UPDATE [Settings] SET [CreatedAt] = ''2026-07-02T00:43:18.4340159Z''
    WHERE [Id] = ''b0c1d2e3-f4a5-6789-b012-345678901234'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702004320_AddWhatsAppTemplateTriggerType'
)
BEGIN
    EXEC(N'UPDATE [Settings] SET [CreatedAt] = ''2026-07-02T00:43:18.4340159Z''
    WHERE [Id] = ''b6c7d8e9-f0a1-2345-b678-901234567890'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702004320_AddWhatsAppTemplateTriggerType'
)
BEGIN
    EXEC(N'UPDATE [Settings] SET [CreatedAt] = ''2026-07-02T00:43:18.4340159Z''
    WHERE [Id] = ''c1d2e3f4-a5b6-7890-c123-456789012345'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702004320_AddWhatsAppTemplateTriggerType'
)
BEGIN
    EXEC(N'UPDATE [Settings] SET [CreatedAt] = ''2026-07-02T00:43:18.4340159Z''
    WHERE [Id] = ''c7d8e9f0-a1b2-3456-c789-012345678901'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702004320_AddWhatsAppTemplateTriggerType'
)
BEGIN
    EXEC(N'UPDATE [Settings] SET [CreatedAt] = ''2026-07-02T00:43:18.4340159Z''
    WHERE [Id] = ''d2e3f4a5-b6c7-8901-d234-567890123456'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702004320_AddWhatsAppTemplateTriggerType'
)
BEGIN
    EXEC(N'UPDATE [Settings] SET [CreatedAt] = ''2026-07-02T00:43:18.4340159Z''
    WHERE [Id] = ''d8e9f0a1-b2c3-4567-d890-123456789012'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702004320_AddWhatsAppTemplateTriggerType'
)
BEGIN
    EXEC(N'UPDATE [Settings] SET [CreatedAt] = ''2026-07-02T00:43:18.4340159Z''
    WHERE [Id] = ''e3f4a5b6-c7d8-9012-e345-678901234567'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702004320_AddWhatsAppTemplateTriggerType'
)
BEGIN
    EXEC(N'UPDATE [Settings] SET [CreatedAt] = ''2026-07-02T00:43:18.4340159Z''
    WHERE [Id] = ''e9f0a1b2-c3d4-5678-e901-234567890123'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702004320_AddWhatsAppTemplateTriggerType'
)
BEGIN
    EXEC(N'UPDATE [Settings] SET [CreatedAt] = ''2026-07-02T00:43:18.4340159Z''
    WHERE [Id] = ''f4a5b6c7-d8e9-0123-f456-789012345678'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702004320_AddWhatsAppTemplateTriggerType'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''$2a$11$RGmZkrwZeCR9hNSlH/4L2.iSC8hTeVElGyhjqanaw7Zq/SOa1et2m''
    WHERE [Id] = ''d4e5f6a7-b8c9-0123-def4-567890123456'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260702004320_AddWhatsAppTemplateTriggerType'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260702004320_AddWhatsAppTemplateTriggerType', N'10.0.9');
END;

COMMIT;
GO

