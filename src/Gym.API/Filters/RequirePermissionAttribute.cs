using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Gym.API.Filters;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
public class RequirePermissionAttribute : Attribute, IAuthorizationFilter
{
    public string Permission { get; }

    public RequirePermissionAttribute(string permission)
    {
        Permission = permission;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var user = context.HttpContext.User;
        if (user?.Identity?.IsAuthenticated != true)
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        var hasClaim = user.HasClaim(c =>
            c.Type == "permission" &&
            c.Value.Equals(Permission, StringComparison.OrdinalIgnoreCase));

        if (!hasClaim)
        {
            if (context.HttpContext.Request.Headers["Accept"].ToString().Contains("application/json"))
            {
                context.Result = new JsonResult(new { success = false, message = "Access denied. You do not have permission to perform this action." })
                {
                    StatusCode = 403
                };
            }
            else
            {
                context.Result = new RedirectToPageResult("/AccessDenied");
            }
        }
    }
}
