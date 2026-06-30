using Gym.Shared.Common;

namespace Gym.Domain.Entities;

public class Permission : BaseEntity
{
    public string Name { get; set; }
    public string? Description { get; set; }

    public ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();

    private Permission() { }

    public Permission(string name, string? description)
    {
        Name = name;
        Description = description;
    }
}
