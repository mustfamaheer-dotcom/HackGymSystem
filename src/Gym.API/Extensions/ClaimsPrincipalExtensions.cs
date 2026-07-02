using System.Security.Claims;

namespace Gym.API.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static bool HasPermission(this ClaimsPrincipal user, string permission)
    {
        return user.HasClaim(c =>
            c.Type == "permission" &&
            c.Value.Equals(permission, StringComparison.OrdinalIgnoreCase));
    }
}
