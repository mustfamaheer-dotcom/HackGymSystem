using Gym.Shared.Common;

namespace Gym.Domain.Entities;

public class Role : BaseEntity
{
    public string Name { get; set; }
    public string? Description { get; set; }
    public bool IsSystem { get; set; }
    public bool IsActive { get; set; } = true;

    public ICollection<User> Users { get; set; } = new List<User>();
    public ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();

    private Role() { }

    public Role(string name, string? description, bool isSystem = false)
    {
        Name = name;
        Description = description;
        IsSystem = isSystem;
    }
}
