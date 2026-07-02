using System.Security.Claims;
using Gym.Application.Common.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Gym.Infrastructure.Security;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid? UserId
    {
        get
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User?
                .FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return Guid.TryParse(userIdClaim, out var id) ? id : null;
        }
    }

    public string? Username
        => _httpContextAccessor.HttpContext?.User?
            .FindFirst(ClaimTypes.Name)?.Value;

    public string? Role
        => _httpContextAccessor.HttpContext?.User?
            .FindFirst(ClaimTypes.Role)?.Value;

    public bool IsAuthenticated
        => _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;

    public bool IsInRole(string role)
        => _httpContextAccessor.HttpContext?.User?
            .IsInRole(role) ?? false;

    public List<string> Permissions
    {
        get
        {
            var claims = _httpContextAccessor.HttpContext?.User?
                .FindAll("permission") ?? Enumerable.Empty<Claim>();
            return claims.Select(c => c.Value).ToList();
        }
    }

    public bool HasPermission(string permission)
    {
        return Permissions.Contains(permission, StringComparer.OrdinalIgnoreCase);
    }
}
