using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace Gym.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[EnableRateLimiting("Api")]
public abstract class BaseController : ControllerBase
{
    protected Guid? CurrentUserId =>
        Guid.TryParse(User?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value, out var id)
            ? id : null;

    protected string CurrentUserRole =>
        User?.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value ?? string.Empty;
}
