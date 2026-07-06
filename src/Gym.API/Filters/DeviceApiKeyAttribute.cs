using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Gym.API.Filters;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class DeviceApiKeyAttribute : Attribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var config = context.HttpContext.RequestServices.GetRequiredService<IConfiguration>();
        var apiKey = config["ZKTecoBridge:ApiKey"];
        if (string.IsNullOrEmpty(apiKey))
        {
            context.Result = new UnauthorizedObjectResult(new { error = "ZKTecoBridge API key not configured" });
            return;
        }

        if (!context.HttpContext.Request.Headers.TryGetValue("X-API-Key", out var headerKey) || headerKey != apiKey)
        {
            context.Result = new UnauthorizedObjectResult(new { error = "Invalid or missing API key" });
        }
    }
}
