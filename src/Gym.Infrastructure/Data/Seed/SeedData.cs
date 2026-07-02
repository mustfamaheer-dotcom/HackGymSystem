using System.Security.Cryptography;
using Gym.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Gym.Infrastructure.Data.Seed;

public static class SeedData
{
    public static readonly Guid OwnerRoleId = Guid.Parse("A1B2C3D4-E5F6-7890-ABCD-EF1234567890");
    public static readonly Guid ReceptionistRoleId = Guid.Parse("B2C3D4E5-F6A7-8901-BCDE-F12345678901");
    public static readonly Guid TrainerRoleId = Guid.Parse("C3D4E5F6-A7B8-9012-CDEF-123456789012");

    private static readonly Guid PermDashboardView = Guid.Parse("D1000001-0000-0000-0000-000000000001");
    private static readonly Guid PermRevenueView = Guid.Parse("D1000001-0000-0000-0000-000000000002");
    private static readonly Guid PermMembersView = Guid.Parse("D1000002-0000-0000-0000-000000000001");
    private static readonly Guid PermMembersCreate = Guid.Parse("D1000002-0000-0000-0000-000000000002");
    private static readonly Guid PermMembersEdit = Guid.Parse("D1000002-0000-0000-0000-000000000003");
    private static readonly Guid PermMembersDelete = Guid.Parse("D1000002-0000-0000-0000-000000000004");
    private static readonly Guid PermPlansView = Guid.Parse("D1000003-0000-0000-0000-000000000001");
    private static readonly Guid PermPlansCreate = Guid.Parse("D1000003-0000-0000-0000-000000000002");
    private static readonly Guid PermPlansEdit = Guid.Parse("D1000003-0000-0000-0000-000000000003");
    private static readonly Guid PermPlansDelete = Guid.Parse("D1000003-0000-0000-0000-000000000004");
    private static readonly Guid PermOffersView = Guid.Parse("D1000004-0000-0000-0000-000000000001");
    private static readonly Guid PermOffersCreate = Guid.Parse("D1000004-0000-0000-0000-000000000002");
    private static readonly Guid PermOffersEdit = Guid.Parse("D1000004-0000-0000-0000-000000000003");
    private static readonly Guid PermOffersDelete = Guid.Parse("D1000004-0000-0000-0000-000000000004");
    private static readonly Guid PermSubsView = Guid.Parse("D1000005-0000-0000-0000-000000000001");
    private static readonly Guid PermSubsCreate = Guid.Parse("D1000005-0000-0000-0000-000000000002");
    private static readonly Guid PermSubsEdit = Guid.Parse("D1000005-0000-0000-0000-000000000003");
    private static readonly Guid PermSubsFreeze = Guid.Parse("D1000005-0000-0000-0000-000000000004");
    private static readonly Guid PermSubsUnfreeze = Guid.Parse("D1000005-0000-0000-0000-000000000005");
    private static readonly Guid PermSubsRenew = Guid.Parse("D1000005-0000-0000-0000-000000000006");
    private static readonly Guid PermSubsPaymentHistory = Guid.Parse("D1000005-0000-0000-0000-000000000007");
    private static readonly Guid PermLeadsView = Guid.Parse("D1000006-0000-0000-0000-000000000001");
    private static readonly Guid PermLeadsCreate = Guid.Parse("D1000006-0000-0000-0000-000000000002");
    private static readonly Guid PermLeadsEdit = Guid.Parse("D1000006-0000-0000-0000-000000000003");
    private static readonly Guid PermLeadsConvert = Guid.Parse("D1000006-0000-0000-0000-000000000004");
    private static readonly Guid PermAttendanceView = Guid.Parse("D1000007-0000-0000-0000-000000000001");
    private static readonly Guid PermAttendanceExport = Guid.Parse("D1000007-0000-0000-0000-000000000002");
    private static readonly Guid PermWhatsAppSend = Guid.Parse("D1000008-0000-0000-0000-000000000001");
    private static readonly Guid PermWhatsAppBroadcast = Guid.Parse("D1000008-0000-0000-0000-000000000002");
    private static readonly Guid PermImportExportImport = Guid.Parse("D1000009-0000-0000-0000-000000000001");
    private static readonly Guid PermImportExportExport = Guid.Parse("D1000009-0000-0000-0000-000000000002");
    private static readonly Guid PermSettingsView = Guid.Parse("D1000010-0000-0000-0000-000000000001");
    private static readonly Guid PermSettingsEdit = Guid.Parse("D1000010-0000-0000-0000-000000000002");
    private static readonly Guid PermUsersView = Guid.Parse("D1000011-0000-0000-0000-000000000001");
    private static readonly Guid PermUsersCreate = Guid.Parse("D1000011-0000-0000-0000-000000000002");
    private static readonly Guid PermUsersEdit = Guid.Parse("D1000011-0000-0000-0000-000000000003");
    private static readonly Guid PermUsersDelete = Guid.Parse("D1000011-0000-0000-0000-000000000004");
    private static readonly Guid PermRolesView = Guid.Parse("D1000012-0000-0000-0000-000000000001");
    private static readonly Guid PermRolesCreate = Guid.Parse("D1000012-0000-0000-0000-000000000002");
    private static readonly Guid PermRolesEdit = Guid.Parse("D1000012-0000-0000-0000-000000000003");
    private static readonly Guid PermRolesDelete = Guid.Parse("D1000012-0000-0000-0000-000000000004");
    private static readonly Guid PermDevicesView = Guid.Parse("D1000013-0000-0000-0000-000000000001");
    private static readonly Guid PermDevicesManage = Guid.Parse("D1000013-0000-0000-0000-000000000002");
    private static readonly Guid PermBackupManage = Guid.Parse("D1000014-0000-0000-0000-000000000001");

    private static readonly List<(Guid Id, string Name, string Description, string Module)> AllPermissions = new()
    {
        (PermDashboardView, "Dashboard.View", "View main dashboard", "Dashboard"),
        (PermRevenueView, "Dashboard.Revenue", "View revenue dashboard", "Dashboard"),
        (PermMembersView, "Members.View", "View members list", "Members"),
        (PermMembersCreate, "Members.Create", "Create new members", "Members"),
        (PermMembersEdit, "Members.Edit", "Edit existing members", "Members"),
        (PermMembersDelete, "Members.Delete", "Delete members", "Members"),
        (PermPlansView, "Plans.View", "View plans", "Plans"),
        (PermPlansCreate, "Plans.Create", "Create plans", "Plans"),
        (PermPlansEdit, "Plans.Edit", "Edit plans", "Plans"),
        (PermPlansDelete, "Plans.Delete", "Delete plans", "Plans"),
        (PermOffersView, "Offers.View", "View offers", "Offers"),
        (PermOffersCreate, "Offers.Create", "Create offers", "Offers"),
        (PermOffersEdit, "Offers.Edit", "Edit offers", "Offers"),
        (PermOffersDelete, "Offers.Delete", "Delete offers", "Offers"),
        (PermSubsView, "Subscriptions.View", "View subscriptions", "Subscriptions"),
        (PermSubsCreate, "Subscriptions.Create", "Create subscriptions", "Subscriptions"),
        (PermSubsEdit, "Subscriptions.Edit", "Edit subscriptions", "Subscriptions"),
        (PermSubsFreeze, "Subscriptions.Freeze", "Freeze subscriptions", "Subscriptions"),
        (PermSubsUnfreeze, "Subscriptions.Unfreeze", "Unfreeze subscriptions", "Subscriptions"),
        (PermSubsRenew, "Subscriptions.Renew", "Renew subscriptions", "Subscriptions"),
        (PermSubsPaymentHistory, "Subscriptions.PaymentHistory", "View payment history", "Subscriptions"),
        (PermLeadsView, "Leads.View", "View leads", "Leads"),
        (PermLeadsCreate, "Leads.Create", "Create leads", "Leads"),
        (PermLeadsEdit, "Leads.Edit", "Edit leads", "Leads"),
        (PermLeadsConvert, "Leads.Convert", "Convert leads to members", "Leads"),
        (PermAttendanceView, "Attendance.View", "View attendance", "Attendance"),
        (PermAttendanceExport, "Attendance.Export", "Export attendance", "Attendance"),
        (PermWhatsAppSend, "WhatsApp.Send", "Send WhatsApp messages", "WhatsApp"),
        (PermWhatsAppBroadcast, "WhatsApp.Broadcast", "Manual broadcast via WhatsApp", "WhatsApp"),
        (PermImportExportImport, "ImportExport.Import", "Import data", "Import/Export"),
        (PermImportExportExport, "ImportExport.Export", "Export data", "Import/Export"),
        (PermSettingsView, "Settings.View", "View settings", "Settings"),
        (PermSettingsEdit, "Settings.Edit", "Edit settings", "Settings"),
        (PermUsersView, "Users.View", "View users", "User Management"),
        (PermUsersCreate, "Users.Create", "Create users", "User Management"),
        (PermUsersEdit, "Users.Edit", "Edit users", "User Management"),
        (PermUsersDelete, "Users.Delete", "Delete users", "User Management"),
        (PermRolesView, "Roles.View", "View roles & permissions", "Roles & Permissions"),
        (PermRolesCreate, "Roles.Create", "Create roles", "Roles & Permissions"),
        (PermRolesEdit, "Roles.Edit", "Edit roles", "Roles & Permissions"),
        (PermRolesDelete, "Roles.Delete", "Delete roles", "Roles & Permissions"),
        (PermDevicesView, "Devices.View", "View devices", "Devices"),
        (PermDevicesManage, "Devices.Manage", "Manage devices", "Devices"),
        (PermBackupManage, "Backup.Manage", "Manage backups", "Backup")
    };

    public static void Seed(ModelBuilder builder)
    {
        SeedRoles(builder);
        SeedUsers(builder);
        SeedPermissions(builder);
        SeedRolePermissions(builder);
        SeedSettings(builder);
    }

    private static void SeedRoles(ModelBuilder builder)
    {
        var now = DateTime.UtcNow;
        builder.Entity<Role>().HasData(
            new { Id = OwnerRoleId, Name = "Owner", Description = "Full system access", IsSystem = true, IsActive = true, CreatedAt = now, UpdatedAt = (DateTime?)null },
            new { Id = ReceptionistRoleId, Name = "Receptionist", Description = "Front desk operations", IsSystem = true, IsActive = true, CreatedAt = now, UpdatedAt = (DateTime?)null },
            new { Id = TrainerRoleId, Name = "Trainer", Description = "Trainer limited access", IsSystem = true, IsActive = false, CreatedAt = now, UpdatedAt = (DateTime?)null }
        );
    }

    private static void SeedUsers(ModelBuilder builder)
    {
        builder.Entity<User>().HasData(
            new
            {
                Id = Guid.Parse("D4E5F6A7-B8C9-0123-DEF4-567890123456"),
                Username = "admin",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123"),
                FullName = "System Administrator",
                Email = "admin@gym.com",
                Phone = (string?)null,
                RoleId = OwnerRoleId,
                IsActive = true,
                CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                UpdatedAt = (DateTime?)null,
                RefreshToken = (string?)null,
                RefreshTokenExpiry = (DateTime?)null,
                LastLoginAt = (DateTime?)null
            }
        );
    }

    private static void SeedPermissions(ModelBuilder builder)
    {
        var now = DateTime.UtcNow;
        var data = AllPermissions.Select(p => new
        {
            Id = p.Id,
            Name = p.Name,
            Description = p.Description,
            Module = p.Module,
            CreatedAt = now,
            UpdatedAt = (DateTime?)null
        }).ToArray();

        builder.Entity<Permission>().HasData(data);
    }

    private static void SeedRolePermissions(ModelBuilder builder)
    {
        var now = DateTime.UtcNow;
        var rolePermissions = new List<dynamic>();

        foreach (var (permId, _, _, _) in AllPermissions)
        {
            rolePermissions.Add(new { Id = DeterministicId(OwnerRoleId, permId), RoleId = OwnerRoleId, PermissionId = permId, CreatedAt = now, UpdatedAt = (DateTime?)null });
        }

        var receptionistPermissions = new HashSet<Guid>
        {
            PermDashboardView,
            PermMembersView,
            PermMembersCreate,
            PermMembersEdit,
            PermSubsView,
            PermSubsCreate,
            PermSubsFreeze,
            PermSubsUnfreeze,
            PermSubsRenew,
            PermSubsPaymentHistory,
            PermLeadsView,
            PermLeadsCreate,
            PermLeadsEdit,
            PermLeadsConvert,
            PermAttendanceView,
            PermAttendanceExport,
            PermWhatsAppSend,
            PermImportExportImport,
            PermImportExportExport
        };

        foreach (var permId in receptionistPermissions)
        {
            rolePermissions.Add(new { Id = DeterministicId(ReceptionistRoleId, permId), RoleId = ReceptionistRoleId, PermissionId = permId, CreatedAt = now, UpdatedAt = (DateTime?)null });
        }

        var trainerPermissions = new HashSet<Guid>
        {
            PermDashboardView,
            PermMembersView,
            PermAttendanceView,
            PermLeadsView
        };

        foreach (var permId in trainerPermissions)
        {
            rolePermissions.Add(new { Id = DeterministicId(TrainerRoleId, permId), RoleId = TrainerRoleId, PermissionId = permId, CreatedAt = now, UpdatedAt = (DateTime?)null });
        }

        builder.Entity<RolePermission>().HasData(rolePermissions.ToArray());
    }

    private static Guid DeterministicId(Guid roleId, Guid permissionId)
    {
        Span<byte> bytes = stackalloc byte[32];
        roleId.TryWriteBytes(bytes);
        permissionId.TryWriteBytes(bytes[16..]);
        return new Guid(SHA256.HashData(bytes)[..16]);
    }

    private static void SeedSettings(ModelBuilder builder)
    {
        var now = DateTime.UtcNow;
        builder.Entity<Setting>().HasData(
            new { Id = Guid.Parse("B0C1D2E3-F4A5-6789-B012-345678901234"), Key = "GymName", Value = "My Gym", Group = "General", Description = "Gym display name", IsEncrypted = false, CreatedAt = now, UpdatedAt = (DateTime?)null },
            new { Id = Guid.Parse("C1D2E3F4-A5B6-7890-C123-456789012345"), Key = "DeviceIP", Value = "192.168.1.201", Group = "Device", Description = "ZKTeco MB2000 IP address", IsEncrypted = false, CreatedAt = now, UpdatedAt = (DateTime?)null },
            new { Id = Guid.Parse("D2E3F4A5-B6C7-8901-D234-567890123456"), Key = "DevicePort", Value = "4370", Group = "Device", Description = "ZKTeco MB2000 port", IsEncrypted = false, CreatedAt = now, UpdatedAt = (DateTime?)null },
            new { Id = Guid.Parse("E3F4A5B6-C7D8-9012-E345-678901234567"), Key = "BackupPath", Value = "C:\\Backups\\GymManagement", Group = "Backup", Description = "Database backup location", IsEncrypted = false, CreatedAt = now, UpdatedAt = (DateTime?)null },
            new { Id = Guid.Parse("F4A5B6C7-D8E9-0123-F456-789012345678"), Key = "WorkingHoursStart", Value = "08:00", Group = "General", Description = "Gym opening time", IsEncrypted = false, CreatedAt = now, UpdatedAt = (DateTime?)null },
            new { Id = Guid.Parse("A5B6C7D8-E9F0-1234-A567-890123456789"), Key = "WorkingHoursEnd", Value = "22:00", Group = "General", Description = "Gym closing time", IsEncrypted = false, CreatedAt = now, UpdatedAt = (DateTime?)null },

            new { Id = Guid.Parse("E9F0A1B2-C3D4-5678-E901-234567890123"), Key = "DefaultCurrency", Value = "EGP", Group = "General", Description = "Default currency symbol", IsEncrypted = false, CreatedAt = now, UpdatedAt = (DateTime?)null }
        );
    }
}
