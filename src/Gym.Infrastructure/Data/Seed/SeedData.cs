using System.Security.Cryptography;
using Gym.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Gym.Infrastructure.Data.Seed;

public static class SeedData
{
    private static readonly Guid OwnerRoleId = Guid.Parse("A1B2C3D4-E5F6-7890-ABCD-EF1234567890");
    private static readonly Guid ReceptionistRoleId = Guid.Parse("B2C3D4E5-F6A7-8901-BCDE-F12345678901");
    private static readonly Guid TrainerRoleId = Guid.Parse("C3D4E5F6-A7B8-9012-CDEF-123456789012");

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
        builder.Entity<Role>().HasData(
            new { Id = OwnerRoleId, Name = "Owner", Description = "Full system access", IsSystem = true, CreatedAt = DateTime.UtcNow },
            new { Id = ReceptionistRoleId, Name = "Receptionist", Description = "Front desk operations", IsSystem = true, CreatedAt = DateTime.UtcNow },
            new { Id = TrainerRoleId, Name = "Trainer", Description = "Trainer limited access", IsSystem = true, CreatedAt = DateTime.UtcNow }
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

    private static readonly Guid UsersManagePermissionId = Guid.Parse("D1E2F3A4-B5C6-7890-D123-456789012345");

    private static void SeedPermissions(ModelBuilder builder)
    {
        builder.Entity<Permission>().HasData(
            new { Id = Guid.Parse("E5F6A7B8-C9D0-1234-EF56-789012345678"), Name = "members.read", Description = "View members", CreatedAt = DateTime.UtcNow, UpdatedAt = (DateTime?)null },
            new { Id = Guid.Parse("F6A7B8C9-D0E1-2345-F678-901234567890"), Name = "members.write", Description = "Create/Edit members", CreatedAt = DateTime.UtcNow, UpdatedAt = (DateTime?)null },
            new { Id = Guid.Parse("A7B8C9D0-E1F2-3456-A789-012345678901"), Name = "members.delete", Description = "Delete members", CreatedAt = DateTime.UtcNow, UpdatedAt = (DateTime?)null },
            new { Id = Guid.Parse("B8C9D0E1-F2A3-4567-B890-123456789012"), Name = "plans.read", Description = "View plans", CreatedAt = DateTime.UtcNow, UpdatedAt = (DateTime?)null },
            new { Id = Guid.Parse("C9D0E1F2-A3B4-5678-C901-234567890123"), Name = "plans.write", Description = "Create/Edit plans", CreatedAt = DateTime.UtcNow, UpdatedAt = (DateTime?)null },
            new { Id = Guid.Parse("D0E1F2A3-B4C5-6789-D012-345678901234"), Name = "attendance.read", Description = "View attendance", CreatedAt = DateTime.UtcNow, UpdatedAt = (DateTime?)null },
            new { Id = Guid.Parse("E1F2A3B4-C5D6-7890-E123-456789012345"), Name = "attendance.write", Description = "Record attendance", CreatedAt = DateTime.UtcNow, UpdatedAt = (DateTime?)null },
            new { Id = Guid.Parse("F2A3B4C5-D6E7-8901-F234-567890123456"), Name = "payments.read", Description = "View payments", CreatedAt = DateTime.UtcNow, UpdatedAt = (DateTime?)null },
            new { Id = Guid.Parse("A3B4C5D6-E7F8-9012-A345-678901234567"), Name = "payments.write", Description = "Process payments", CreatedAt = DateTime.UtcNow, UpdatedAt = (DateTime?)null },
            new { Id = Guid.Parse("B4C5D6E7-F8A9-0123-B456-789012345678"), Name = "reports.read", Description = "View reports", CreatedAt = DateTime.UtcNow, UpdatedAt = (DateTime?)null },
            new { Id = Guid.Parse("C5D6E7F8-A9B0-1234-C567-890123456789"), Name = "settings.read", Description = "View settings", CreatedAt = DateTime.UtcNow, UpdatedAt = (DateTime?)null },
            new { Id = Guid.Parse("D6E7F8A9-B0C1-2345-D678-901234567890"), Name = "settings.write", Description = "Manage settings", CreatedAt = DateTime.UtcNow, UpdatedAt = (DateTime?)null },
            new { Id = Guid.Parse("E7F8A9B0-C1D2-3456-E789-012345678901"), Name = "devices.manage", Description = "Manage devices", CreatedAt = DateTime.UtcNow, UpdatedAt = (DateTime?)null },
            new { Id = Guid.Parse("F8A9B0C1-D2E3-4567-F890-123456789012"), Name = "backup.manage", Description = "Manage backups", CreatedAt = DateTime.UtcNow, UpdatedAt = (DateTime?)null },
            new { Id = Guid.Parse("A9B0C1D2-E3F4-5678-A901-234567890123"), Name = "offers.manage", Description = "Manage offers", CreatedAt = DateTime.UtcNow, UpdatedAt = (DateTime?)null },
            new { Id = UsersManagePermissionId, Name = "users.manage", Description = "Manage users", CreatedAt = DateTime.UtcNow, UpdatedAt = (DateTime?)null }
        );
    }

    private static void SeedRolePermissions(ModelBuilder builder)
    {
        var allPermissionIds = new List<Guid>
        {
            Guid.Parse("E5F6A7B8-C9D0-1234-EF56-789012345678"),
            Guid.Parse("F6A7B8C9-D0E1-2345-F678-901234567890"),
            Guid.Parse("A7B8C9D0-E1F2-3456-A789-012345678901"),
            Guid.Parse("B8C9D0E1-F2A3-4567-B890-123456789012"),
            Guid.Parse("C9D0E1F2-A3B4-5678-C901-234567890123"),
            Guid.Parse("D0E1F2A3-B4C5-6789-D012-345678901234"),
            Guid.Parse("E1F2A3B4-C5D6-7890-E123-456789012345"),
            Guid.Parse("F2A3B4C5-D6E7-8901-F234-567890123456"),
            Guid.Parse("A3B4C5D6-E7F8-9012-A345-678901234567"),
            Guid.Parse("B4C5D6E7-F8A9-0123-B456-789012345678"),
            Guid.Parse("C5D6E7F8-A9B0-1234-C567-890123456789"),
            Guid.Parse("D6E7F8A9-B0C1-2345-D678-901234567890"),
            Guid.Parse("E7F8A9B0-C1D2-3456-E789-012345678901"),
            Guid.Parse("F8A9B0C1-D2E3-4567-F890-123456789012"),
            Guid.Parse("A9B0C1D2-E3F4-5678-A901-234567890123"),
            UsersManagePermissionId
        };

        var receptionistReadPermissions = new List<Guid>
        {
            Guid.Parse("E5F6A7B8-C9D0-1234-EF56-789012345678"),
            Guid.Parse("B8C9D0E1-F2A3-4567-B890-123456789012"),
            Guid.Parse("D0E1F2A3-B4C5-6789-D012-345678901234"),
            Guid.Parse("E1F2A3B4-C5D6-7890-E123-456789012345"),
            Guid.Parse("F2A3B4C5-D6E7-8901-F234-567890123456"),
            Guid.Parse("A3B4C5D6-E7F8-9012-A345-678901234567")
        };

        var trainerReadPermissions = new List<Guid>
        {
            Guid.Parse("E5F6A7B8-C9D0-1234-EF56-789012345678"),
            Guid.Parse("D0E1F2A3-B4C5-6789-D012-345678901234"),
            Guid.Parse("B4C5D6E7-F8A9-0123-B456-789012345678")
        };

        var rolePermissions = new List<dynamic>();
        var now = DateTime.UtcNow;

        foreach (var permId in allPermissionIds)
        {
            rolePermissions.Add(new { Id = DeterministicId(OwnerRoleId, permId), RoleId = OwnerRoleId, PermissionId = permId, CreatedAt = now, UpdatedAt = (DateTime?)null });
        }

        foreach (var permId in receptionistReadPermissions)
        {
            rolePermissions.Add(new { Id = DeterministicId(ReceptionistRoleId, permId), RoleId = ReceptionistRoleId, PermissionId = permId, CreatedAt = now, UpdatedAt = (DateTime?)null });
        }

        foreach (var permId in trainerReadPermissions)
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
            new { Id = Guid.Parse("B6C7D8E9-F0A1-2345-B678-901234567890"), Key = "ReminderDays", Value = "7,3,1", Group = "Notifications", Description = "Membership expiry reminder days", IsEncrypted = false, CreatedAt = now, UpdatedAt = (DateTime?)null },
            new { Id = Guid.Parse("C7D8E9F0-A1B2-3456-C789-012345678901"), Key = "WhatsAppEnabled", Value = "false", Group = "Notifications", Description = "Enable WhatsApp notifications", IsEncrypted = false, CreatedAt = now, UpdatedAt = (DateTime?)null },
            new { Id = Guid.Parse("D8E9F0A1-B2C3-4567-D890-123456789012"), Key = "SMSEnabled", Value = "false", Group = "Notifications", Description = "Enable SMS notifications", IsEncrypted = false, CreatedAt = now, UpdatedAt = (DateTime?)null },
            new { Id = Guid.Parse("E9F0A1B2-C3D4-5678-E901-234567890123"), Key = "DefaultCurrency", Value = "EGP", Group = "General", Description = "Default currency symbol", IsEncrypted = false, CreatedAt = now, UpdatedAt = (DateTime?)null }
        );
    }
}
