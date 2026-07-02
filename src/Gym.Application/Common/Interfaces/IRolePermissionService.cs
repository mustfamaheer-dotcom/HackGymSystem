using Gym.Shared.Common;

namespace Gym.Application.Common.Interfaces;

public interface IRolePermissionService
{
    Task<Result<List<RoleListDto>>> GetAllRolesAsync(CancellationToken ct = default);
    Task<Result<RoleDetailDto>> GetRoleByIdAsync(Guid id, CancellationToken ct = default);
    Task<Result<Guid>> CreateRoleAsync(CreateRoleDto dto, CancellationToken ct = default);
    Task<Result> UpdateRoleAsync(UpdateRoleDto dto, CancellationToken ct = default);
    Task<Result> DeleteRoleAsync(Guid id, CancellationToken ct = default);
    Task<Result<List<PermissionGroupDto>>> GetAllPermissionsGroupedAsync(CancellationToken ct = default);
    Task<Result> UpdateRolePermissionsAsync(Guid roleId, List<Guid> permissionIds, CancellationToken ct = default);
}

public class RoleListDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsActive { get; set; }
    public bool IsSystem { get; set; }
    public int UserCount { get; set; }
    public int PermissionCount { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class RoleDetailDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsActive { get; set; }
    public bool IsSystem { get; set; }
    public List<Guid> PermissionIds { get; set; } = new();
    public DateTime CreatedAt { get; set; }
}

public class CreateRoleDto
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;
    public List<Guid> PermissionIds { get; set; } = new();
}

public class UpdateRoleDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsActive { get; set; }
    public List<Guid> PermissionIds { get; set; } = new();
}

public class PermissionGroupDto
{
    public string Module { get; set; } = string.Empty;
    public List<PermissionItemDto> Permissions { get; set; } = new();
}

public class PermissionItemDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string Code { get; set; } = string.Empty;
}
