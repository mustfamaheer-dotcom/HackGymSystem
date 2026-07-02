using Gym.Shared.Common;

namespace Gym.Domain.Entities;

public class PermissionAuditLog : BaseEntity
{
    public Guid? UserId { get; set; }
    public string Action { get; set; }
    public string RoleName { get; set; }
    public string? OldPermissions { get; set; }
    public string? NewPermissions { get; set; }
    public string? IpAddress { get; set; }

    public User? User { get; set; }

    private PermissionAuditLog() { }

    public PermissionAuditLog(Guid? userId, string action, string roleName,
        string? oldPermissions, string? newPermissions, string? ipAddress)
    {
        UserId = userId;
        Action = action;
        RoleName = roleName;
        OldPermissions = oldPermissions;
        NewPermissions = newPermissions;
        IpAddress = ipAddress;
    }
}
