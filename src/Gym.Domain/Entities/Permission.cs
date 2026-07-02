using Gym.Shared.Common;

namespace Gym.Domain.Entities;

public class Permission : BaseEntity
{
    public string Name { get; set; }
    public string? Description { get; set; }
    public string Module { get; set; } = string.Empty;

    public ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();

    private Permission() { }

    public Permission(string name, string? description, string module = "")
    {
        Name = name;
        Description = description;
        Module = module;
    }
}
