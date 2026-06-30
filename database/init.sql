-- =============================================
-- Gym Management System - Database Initialization
-- SQL Server Script
-- =============================================

-- Create database if not exists
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'GymManagementDb')
BEGIN
    CREATE DATABASE GymManagementDb;
END
GO

USE GymManagementDb;
GO

-- =============================================
-- TABLES
-- =============================================

-- Roles
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Roles')
BEGIN
    CREATE TABLE Roles (
        Id              UNIQUEIDENTIFIER NOT NULL PRIMARY KEY DEFAULT NEWID(),
        Name            NVARCHAR(50)     NOT NULL,
        Description     NVARCHAR(500)    NULL,
        IsSystem        BIT              NOT NULL DEFAULT 0,
        CreatedAt       DATETIME2        NOT NULL DEFAULT GETUTCDATE(),
        UpdatedAt       DATETIME2        NULL,
        CONSTRAINT UQ_Roles_Name UNIQUE (Name)
    );
END
GO

-- Permissions
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Permissions')
BEGIN
    CREATE TABLE Permissions (
        Id              UNIQUEIDENTIFIER NOT NULL PRIMARY KEY DEFAULT NEWID(),
        Name            NVARCHAR(100)    NOT NULL,
        Description     NVARCHAR(500)    NULL,
        CreatedAt       DATETIME2        NOT NULL DEFAULT GETUTCDATE(),
        UpdatedAt       DATETIME2        NULL,
        CONSTRAINT UQ_Permissions_Name UNIQUE (Name)
    );
END
GO

-- RolePermissions (many-to-many)
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'RolePermissions')
BEGIN
    CREATE TABLE RolePermissions (
        Id              UNIQUEIDENTIFIER NOT NULL PRIMARY KEY DEFAULT NEWID(),
        RoleId          UNIQUEIDENTIFIER NOT NULL,
        PermissionId    UNIQUEIDENTIFIER NOT NULL,
        CreatedAt       DATETIME2        NOT NULL DEFAULT GETUTCDATE(),
        UpdatedAt       DATETIME2        NULL,
        CONSTRAINT FK_RolePermissions_Roles FOREIGN KEY (RoleId) REFERENCES Roles(Id) ON DELETE CASCADE,
        CONSTRAINT FK_RolePermissions_Permissions FOREIGN KEY (PermissionId) REFERENCES Permissions(Id) ON DELETE CASCADE,
        CONSTRAINT UQ_RolePermissions_Role_Permission UNIQUE (RoleId, PermissionId)
    );
END
GO

-- Users
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Users')
BEGIN
    CREATE TABLE Users (
        Id                  UNIQUEIDENTIFIER NOT NULL PRIMARY KEY DEFAULT NEWID(),
        Username            NVARCHAR(50)     NOT NULL,
        PasswordHash        NVARCHAR(500)    NOT NULL,
        FullName            NVARCHAR(200)    NOT NULL,
        Email               NVARCHAR(200)    NOT NULL,
        Phone               NVARCHAR(20)     NULL,
        RoleId              UNIQUEIDENTIFIER NOT NULL,
        IsActive            BIT              NOT NULL DEFAULT 1,
        RefreshToken        NVARCHAR(500)    NULL,
        RefreshTokenExpiry  DATETIME2        NULL,
        LastLoginAt         DATETIME2        NULL,
        CreatedAt           DATETIME2        NOT NULL DEFAULT GETUTCDATE(),
        UpdatedAt           DATETIME2        NULL,
        CONSTRAINT FK_Users_Roles FOREIGN KEY (RoleId) REFERENCES Roles(Id) ON DELETE RESTRICT,
        CONSTRAINT UQ_Users_Username UNIQUE (Username),
        CONSTRAINT UQ_Users_Email UNIQUE (Email)
    );
END
GO

-- Members
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Members')
BEGIN
    CREATE TABLE Members (
        Id                      UNIQUEIDENTIFIER NOT NULL PRIMARY KEY DEFAULT NEWID(),
        Code                    NVARCHAR(50)     NOT NULL,
        FullName                NVARCHAR(200)    NOT NULL,
        Phone                   NVARCHAR(20)     NOT NULL,
        WhatsAppNumber          NVARCHAR(20)     NULL,
        Email                   NVARCHAR(200)    NULL,
        Gender                  NVARCHAR(10)     NULL,
        DateOfBirth             DATETIME2        NULL,
        Address                 NVARCHAR(500)    NULL,
        PhotoPath               NVARCHAR(500)    NULL,
        EmergencyContactName    NVARCHAR(200)    NULL,
        EmergencyContactPhone   NVARCHAR(20)     NULL,
        JoinDate                DATETIME2        NOT NULL DEFAULT GETUTCDATE(),
        Status                  NVARCHAR(20)     NULL DEFAULT 'Active',
        Notes                   NVARCHAR(2000)   NULL,
        FingerprintId           INT              NULL,
        FaceId                  INT              NULL,
        DeviceUserId            INT              NULL,
        IsActive                BIT              NOT NULL DEFAULT 1,
        CreatedAt               DATETIME2        NOT NULL DEFAULT GETUTCDATE(),
        UpdatedAt               DATETIME2        NULL,
        CONSTRAINT UQ_Members_Code UNIQUE (Code)
    );
END
GO

-- MembershipPlans
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'MembershipPlans')
BEGIN
    CREATE TABLE MembershipPlans (
        Id              UNIQUEIDENTIFIER NOT NULL PRIMARY KEY DEFAULT NEWID(),
        Name            NVARCHAR(200)    NOT NULL,
        Price           DECIMAL(18,2)    NOT NULL DEFAULT 0,
        DurationDays    INT              NOT NULL DEFAULT 0,
        MaxVisits       INT              NULL,
        FreezeDays      INT              NULL,
        Description     NVARCHAR(1000)   NULL,
        IsActive        BIT              NOT NULL DEFAULT 1,
        CreatedAt       DATETIME2        NOT NULL DEFAULT GETUTCDATE(),
        UpdatedAt       DATETIME2        NULL
    );
END
GO

-- Memberships
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Memberships')
BEGIN
    CREATE TABLE Memberships (
        Id                  UNIQUEIDENTIFIER NOT NULL PRIMARY KEY DEFAULT NEWID(),
        MemberId            UNIQUEIDENTIFIER NOT NULL,
        PlanId              UNIQUEIDENTIFIER NOT NULL,
        StartDate           DATETIME2        NOT NULL,
        EndDate             DATETIME2        NOT NULL,
        RemainingDays       INT              NOT NULL DEFAULT 0,
        RemainingVisits     INT              NOT NULL DEFAULT 0,
        Status              NVARCHAR(20)     NULL DEFAULT 'Active',
        FreezeStartDate     DATETIME2        NULL,
        FreezeEndDate       DATETIME2        NULL,
        Notes               NVARCHAR(1000)   NULL,
        CreatedAt           DATETIME2        NOT NULL DEFAULT GETUTCDATE(),
        UpdatedAt           DATETIME2        NULL,
        CONSTRAINT FK_Memberships_Members FOREIGN KEY (MemberId) REFERENCES Members(Id) ON DELETE RESTRICT,
        CONSTRAINT FK_Memberships_Plans FOREIGN KEY (PlanId) REFERENCES MembershipPlans(Id) ON DELETE RESTRICT
    );
    CREATE INDEX IX_Memberships_MemberId_Status ON Memberships (MemberId, Status);
END
GO

-- Attendance
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Attendance')
BEGIN
    CREATE TABLE Attendance (
        Id              UNIQUEIDENTIFIER NOT NULL PRIMARY KEY DEFAULT NEWID(),
        MemberId        UNIQUEIDENTIFIER NOT NULL,
        DeviceId        UNIQUEIDENTIFIER NULL,
        Date            DATETIME2        NOT NULL,
        Time            TIME             NOT NULL,
        CheckIn         DATETIME2        NULL,
        CheckOut        DATETIME2        NULL,
        IsManual        BIT              NOT NULL DEFAULT 0,
        SyncStatus      NVARCHAR(20)     NULL DEFAULT 'Synced',
        CreatedAt       DATETIME2        NOT NULL DEFAULT GETUTCDATE(),
        UpdatedAt       DATETIME2        NULL,
        CONSTRAINT FK_Attendance_Members FOREIGN KEY (MemberId) REFERENCES Members(Id) ON DELETE RESTRICT,
        CONSTRAINT FK_Attendance_Devices FOREIGN KEY (DeviceId) REFERENCES Devices(Id) ON DELETE SET NULL
    );
    CREATE INDEX IX_Attendance_MemberId_Date ON Attendance (MemberId, Date);
    CREATE INDEX IX_Attendance_DeviceId_Date ON Attendance (DeviceId, Date);
END
GO

-- Payments
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Payments')
BEGIN
    CREATE TABLE Payments (
        Id                  UNIQUEIDENTIFIER NOT NULL PRIMARY KEY DEFAULT NEWID(),
        MemberId            UNIQUEIDENTIFIER NOT NULL,
        Amount              DECIMAL(18,2)    NOT NULL DEFAULT 0,
        DiscountAmount      DECIMAL(18,2)    NOT NULL DEFAULT 0,
        TotalAmount         DECIMAL(18,2)    NOT NULL DEFAULT 0,
        PaymentDate         DATETIME2        NOT NULL DEFAULT GETUTCDATE(),
        PaymentMethod       NVARCHAR(20)     NULL,
        ReferenceNumber     NVARCHAR(100)    NULL,
        EmployeeId          UNIQUEIDENTIFIER NULL,
        Notes               NVARCHAR(1000)   NULL,
        ReceiptNumber       NVARCHAR(50)     NOT NULL,
        CreatedAt           DATETIME2        NOT NULL DEFAULT GETUTCDATE(),
        UpdatedAt           DATETIME2        NULL,
        CONSTRAINT FK_Payments_Members FOREIGN KEY (MemberId) REFERENCES Members(Id) ON DELETE RESTRICT,
        CONSTRAINT FK_Payments_Users FOREIGN KEY (EmployeeId) REFERENCES Users(Id) ON DELETE SET NULL,
        CONSTRAINT UQ_Payments_ReceiptNumber UNIQUE (ReceiptNumber)
    );
    CREATE INDEX IX_Payments_PaymentDate ON Payments (PaymentDate);
END
GO

-- Offers
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Offers')
BEGIN
    CREATE TABLE Offers (
        Id              UNIQUEIDENTIFIER NOT NULL PRIMARY KEY DEFAULT NEWID(),
        Title           NVARCHAR(200)    NOT NULL,
        Description     NVARCHAR(2000)   NULL,
        DiscountType    NVARCHAR(20)     NULL,
        DiscountValue   DECIMAL(18,2)    NOT NULL DEFAULT 0,
        StartDate       DATETIME2        NOT NULL,
        EndDate         DATETIME2        NOT NULL,
        IsActive        BIT              NOT NULL DEFAULT 1,
        CreatedAt       DATETIME2        NOT NULL DEFAULT GETUTCDATE(),
        UpdatedAt       DATETIME2        NULL
    );
END
GO

-- Notifications
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Notifications')
BEGIN
    CREATE TABLE Notifications (
        Id              UNIQUEIDENTIFIER NOT NULL PRIMARY KEY DEFAULT NEWID(),
        MemberId        UNIQUEIDENTIFIER NULL,
        Type            NVARCHAR(30)     NULL,
        Channel         NVARCHAR(20)     NULL,
        Subject         NVARCHAR(500)    NOT NULL,
        Message         NVARCHAR(4000)   NOT NULL,
        Status          NVARCHAR(20)     NULL DEFAULT 'Pending',
        ScheduledDate   DATETIME2        NULL,
        SentDate        DATETIME2        NULL,
        IsPending       BIT              NOT NULL DEFAULT 1,
        RetryCount      INT              NOT NULL DEFAULT 0,
        LastError       NVARCHAR(1000)   NULL,
        CreatedAt       DATETIME2        NOT NULL DEFAULT GETUTCDATE(),
        UpdatedAt       DATETIME2        NULL,
        CONSTRAINT FK_Notifications_Members FOREIGN KEY (MemberId) REFERENCES Members(Id) ON DELETE SET NULL
    );
    CREATE INDEX IX_Notifications_Status_ScheduledDate ON Notifications (Status, ScheduledDate);
END
GO

-- Devices
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Devices')
BEGIN
    CREATE TABLE Devices (
        Id                  UNIQUEIDENTIFIER NOT NULL PRIMARY KEY DEFAULT NEWID(),
        Name                NVARCHAR(200)    NOT NULL,
        IPAddress           NVARCHAR(50)     NOT NULL,
        Port                INT              NOT NULL DEFAULT 4370,
        Model               NVARCHAR(100)    NULL,
        SerialNumber        NVARCHAR(100)    NULL,
        FirmwareVersion     NVARCHAR(50)     NULL,
        IsActive            BIT              NOT NULL DEFAULT 1,
        Status              NVARCHAR(20)     NULL DEFAULT 'Offline',
        LastConnectedAt     DATETIME2        NULL,
        CreatedAt           DATETIME2        NOT NULL DEFAULT GETUTCDATE(),
        UpdatedAt           DATETIME2        NULL
    );
END
GO

-- DeviceLogs
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'DeviceLogs')
BEGIN
    CREATE TABLE DeviceLogs (
        Id              UNIQUEIDENTIFIER NOT NULL PRIMARY KEY DEFAULT NEWID(),
        DeviceId        UNIQUEIDENTIFIER NOT NULL,
        Level           NVARCHAR(20)     NOT NULL,
        Message         NVARCHAR(2000)   NOT NULL,
        Details         NVARCHAR(MAX)    NULL,
        CreatedAt       DATETIME2        NOT NULL DEFAULT GETUTCDATE(),
        UpdatedAt       DATETIME2        NULL,
        CONSTRAINT FK_DeviceLogs_Devices FOREIGN KEY (DeviceId) REFERENCES Devices(Id) ON DELETE CASCADE
    );
    CREATE INDEX IX_DeviceLogs_CreatedAt ON DeviceLogs (CreatedAt);
END
GO

-- Attendance

-- Settings
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Settings')
BEGIN
    CREATE TABLE Settings (
        Id              UNIQUEIDENTIFIER NOT NULL PRIMARY KEY DEFAULT NEWID(),
        Key             NVARCHAR(100)    NOT NULL,
        Value           NVARCHAR(2000)   NOT NULL,
        [Group]         NVARCHAR(100)    NULL,
        Description     NVARCHAR(500)    NULL,
        IsEncrypted     BIT              NOT NULL DEFAULT 0,
        CreatedAt       DATETIME2        NOT NULL DEFAULT GETUTCDATE(),
        UpdatedAt       DATETIME2        NULL,
        CONSTRAINT UQ_Settings_Key UNIQUE ([Key])
    );
END
GO

-- AuditLogs
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'AuditLogs')
BEGIN
    CREATE TABLE AuditLogs (
        Id              UNIQUEIDENTIFIER NOT NULL PRIMARY KEY DEFAULT NEWID(),
        UserId          UNIQUEIDENTIFIER NULL,
        Action          NVARCHAR(100)    NOT NULL,
        EntityType      NVARCHAR(100)    NOT NULL,
        EntityId        NVARCHAR(50)     NULL,
        OldValues       NVARCHAR(MAX)    NULL,
        NewValues       NVARCHAR(MAX)    NULL,
        IpAddress       NVARCHAR(50)     NULL,
        CreatedAt       DATETIME2        NOT NULL DEFAULT GETUTCDATE(),
        UpdatedAt       DATETIME2        NULL,
        CONSTRAINT FK_AuditLogs_Users FOREIGN KEY (UserId) REFERENCES Users(Id) ON DELETE SET NULL
    );
    CREATE INDEX IX_AuditLogs_CreatedAt ON AuditLogs (CreatedAt);
    CREATE INDEX IX_AuditLogs_EntityType_EntityId ON AuditLogs (EntityType, EntityId);
END
GO

-- BackupLogs
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'BackupLogs')
BEGIN
    CREATE TABLE BackupLogs (
        Id              UNIQUEIDENTIFIER NOT NULL PRIMARY KEY DEFAULT NEWID(),
        FileName        NVARCHAR(500)    NOT NULL,
        FilePath        NVARCHAR(1000)   NOT NULL,
        Size            BIGINT           NULL,
        Status          NVARCHAR(20)     NOT NULL,
        ErrorMessage    NVARCHAR(2000)   NULL,
        BackupDate      DATETIME2        NOT NULL DEFAULT GETUTCDATE(),
        CreatedAt       DATETIME2        NOT NULL DEFAULT GETUTCDATE(),
        UpdatedAt       DATETIME2        NULL
    );
    CREATE INDEX IX_BackupLogs_BackupDate ON BackupLogs (BackupDate);
END
GO

-- =============================================
-- SEED DATA
-- =============================================

-- Roles
IF NOT EXISTS (SELECT * FROM Roles WHERE Name = 'Owner')
BEGIN
    INSERT INTO Roles (Id, Name, Description, IsSystem, CreatedAt)
    VALUES
        ('A1B2C3D4-E5F6-7890-ABCD-EF1234567890', 'Owner',        'Full system access',   1, GETUTCDATE()),
        ('B2C3D4E5-F6A7-8901-BCDE-F12345678901', 'Receptionist', 'Front desk operations', 1, GETUTCDATE()),
        ('C3D4E5F6-A7B8-9012-CDEF-123456789012', 'Trainer',      'Trainer limited access',1, GETUTCDATE());
END
GO

-- Admin User (password: Admin@123 -- BCrypt hash)
IF NOT EXISTS (SELECT * FROM Users WHERE Username = 'admin')
BEGIN
    INSERT INTO Users (Id, Username, PasswordHash, FullName, Email, Phone, RoleId, IsActive, CreatedAt)
    VALUES (
        'D4E5F6A7-B8C9-0123-DEF4-567890123456',
        'admin',
        '$2a$11$1brDlyE5gMcS/ugJbDUMRO.snjzcwW9lH.BQXxHaf5gfdzItjRgim',
        'System Administrator',
        'admin@gym.com',
        NULL,
        'A1B2C3D4-E5F6-7890-ABCD-EF1234567890',
        1,
        GETUTCDATE()
    );
END
GO

-- Permissions
IF NOT EXISTS (SELECT * FROM Permissions WHERE Name = 'members.read')
BEGIN
    INSERT INTO Permissions (Id, Name, Description, CreatedAt) VALUES
        ('E5F6A7B8-C9D0-1234-EF56-789012345678', 'members.read',     'View members',             GETUTCDATE()),
        ('F6A7B8C9-D0E1-2345-F678-901234567890', 'members.write',    'Create/Edit members',      GETUTCDATE()),
        ('A7B8C9D0-E1F2-3456-A789-012345678901', 'members.delete',   'Delete members',           GETUTCDATE()),
        ('B8C9D0E1-F2A3-4567-B890-123456789012', 'plans.read',       'View plans',               GETUTCDATE()),
        ('C9D0E1F2-A3B4-5678-C901-234567890123', 'plans.write',      'Create/Edit plans',        GETUTCDATE()),
        ('D0E1F2A3-B4C5-6789-D012-345678901234', 'attendance.read',  'View attendance',          GETUTCDATE()),
        ('E1F2A3B4-C5D6-7890-E123-456789012345', 'attendance.write', 'Record attendance',        GETUTCDATE()),
        ('F2A3B4C5-D6E7-8901-F234-567890123456', 'payments.read',    'View payments',            GETUTCDATE()),
        ('A3B4C5D6-E7F8-9012-A345-678901234567', 'payments.write',   'Process payments',         GETUTCDATE()),
        ('B4C5D6E7-F8A9-0123-B456-789012345678', 'reports.read',     'View reports',             GETUTCDATE()),
        ('C5D6E7F8-A9B0-1234-C567-890123456789', 'settings.read',    'View settings',            GETUTCDATE()),
        ('D6E7F8A9-B0C1-2345-D678-901234567890', 'settings.write',   'Manage settings',          GETUTCDATE()),
        ('E7F8A9B0-C1D2-3456-E789-012345678901', 'devices.manage',   'Manage devices',           GETUTCDATE()),
        ('F8A9B0C1-D2E3-4567-F890-123456789012', 'backup.manage',    'Manage backups',           GETUTCDATE()),
        ('A9B0C1D2-E3F4-5678-A901-234567890123', 'offers.manage',    'Manage offers',            GETUTCDATE());
END
GO

-- Owner gets all permissions
IF NOT EXISTS (SELECT * FROM RolePermissions WHERE RoleId = 'A1B2C3D4-E5F6-7890-ABCD-EF1234567890')
BEGIN
    INSERT INTO RolePermissions (Id, RoleId, PermissionId, CreatedAt)
    SELECT NEWID(), 'A1B2C3D4-E5F6-7890-ABCD-EF1234567890', Id, GETUTCDATE() FROM Permissions;
END
GO

-- Receptionist gets read + write permissions
IF NOT EXISTS (SELECT * FROM RolePermissions WHERE RoleId = 'B2C3D4E5-F6A7-8901-BCDE-F12345678901')
BEGIN
    INSERT INTO RolePermissions (Id, RoleId, PermissionId, CreatedAt)
    SELECT NEWID(), 'B2C3D4E5-F6A7-8901-BCDE-F12345678901', Id, GETUTCDATE()
    FROM Permissions
    WHERE Name IN (
        'members.read', 'plans.read', 'attendance.read', 'attendance.write',
        'payments.read', 'payments.write'
    );
END
GO

-- Trainer gets read-only permissions
IF NOT EXISTS (SELECT * FROM RolePermissions WHERE RoleId = 'C3D4E5F6-A7B8-9012-CDEF-123456789012')
BEGIN
    INSERT INTO RolePermissions (Id, RoleId, PermissionId, CreatedAt)
    SELECT NEWID(), 'C3D4E5F6-A7B8-9012-CDEF-123456789012', Id, GETUTCDATE()
    FROM Permissions
    WHERE Name IN ('members.read', 'attendance.read', 'reports.read');
END
GO

-- Settings
IF NOT EXISTS (SELECT * FROM Settings WHERE [Key] = 'GymName')
BEGIN
    INSERT INTO Settings (Id, [Key], Value, [Group], Description, IsEncrypted, CreatedAt) VALUES
        ('B0C1D2E3-F4A5-6789-B012-345678901234', 'GymName',          'My Gym',           'General',      'Gym display name',                        0, GETUTCDATE()),
        ('C1D2E3F4-A5B6-7890-C123-456789012345', 'DeviceIP',         '192.168.1.201',    'Device',       'ZKTeco MB2000 IP address',                0, GETUTCDATE()),
        ('D2E3F4A5-B6C7-8901-D234-567890123456', 'DevicePort',       '4370',             'Device',       'ZKTeco MB2000 port',                      0, GETUTCDATE()),
        ('E3F4A5B6-C7D8-9012-E345-678901234567', 'BackupPath',       'C:\Backups\GymManagement', 'Backup', 'Database backup location',             0, GETUTCDATE()),
        ('F4A5B6C7-D8E9-0123-F456-789012345678', 'WorkingHoursStart','08:00',            'General',      'Gym opening time',                        0, GETUTCDATE()),
        ('A5B6C7D8-E9F0-1234-A567-890123456789', 'WorkingHoursEnd',  '22:00',            'General',      'Gym closing time',                        0, GETUTCDATE()),
        ('B6C7D8E9-F0A1-2345-B678-901234567890', 'ReminderDays',     '7,3,1',            'Notifications','Membership expiry reminder days',         0, GETUTCDATE()),
        ('C7D8E9F0-A1B2-3456-C789-012345678901', 'WhatsAppEnabled',  'false',            'Notifications','Enable WhatsApp notifications',           0, GETUTCDATE()),
        ('D8E9F0A1-B2C3-4567-D890-123456789012', 'SMSEnabled',       'false',            'Notifications','Enable SMS notifications',               0, GETUTCDATE()),
        ('E9F0A1B2-C3D4-5678-E901-234567890123', 'DefaultCurrency',  'EGP',              'General',      'Default currency symbol',                 0, GETUTCDATE());
END
GO

PRINT 'GymManagementDb initialization completed successfully.';
GO
