using Gym.API.Resources;
using Gym.Application.Common.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace Gym.API.Controllers;

[AllowAnonymous]
public class AccountController : Controller
{
    private readonly IAuthService _authService;
    private readonly IStringLocalizer<SharedResources> _localizer;

    public AccountController(IAuthService authService, IStringLocalizer<SharedResources> localizer)
    {
        _authService = authService;
        _localizer = localizer;
    }

    [HttpGet]
    public IActionResult Login()
    {
        if (User.Identity?.IsAuthenticated == true)
            return RedirectToAction("Index", "HomeMvc");

        ViewData["Title"] = _localizer["Login"];
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(string username, string password, CancellationToken cancellationToken)
    {
        ViewData["Title"] = _localizer["Login"];

        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
        {
            ViewBag.Error = _localizer["Username and password are required"];
            return View();
        }

        var result = await _authService.LoginAsync(username, password, cancellationToken);

        if (result.IsFailure)
        {
            ViewBag.Error = result.Message ?? _localizer["Login failed"];
            return View();
        }

        var response = result.Data!;

        Response.Cookies.Append("accessToken", response.AccessToken, new CookieOptions
        {
            HttpOnly = true,
            SameSite = SameSiteMode.Lax,
            Secure = false,
            MaxAge = TimeSpan.FromDays(7)
        });

        return RedirectToAction("Index", "HomeMvc");
    }

    [HttpPost("logout")]
    [Authorize]
    public async Task<IActionResult> Logout(CancellationToken cancellationToken)
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (userId is not null && Guid.TryParse(userId, out var id))
            await _authService.LogoutAsync(id, cancellationToken);

        Response.Cookies.Delete("accessToken");
        return RedirectToAction("Login");
    }

    [HttpPost]
    public IActionResult SetLanguage(string culture, string returnUrl = "/")
    {
        if (culture != "ar" && culture != "en")
            culture = "ar";

        Response.Cookies.Append(
            CookieRequestCultureProvider.DefaultCookieName,
            CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture, culture)),
            new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1), IsEssential = true }
        );

        if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            return LocalRedirect(returnUrl);

        return RedirectToAction("Index", "HomeMvc");
    }
}
