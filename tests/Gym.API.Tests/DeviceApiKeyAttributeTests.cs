using Gym.API.Filters;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Moq;

namespace Gym.API.Tests;

public class DeviceApiKeyAttributeTests
{
    private readonly DeviceApiKeyAttribute _attribute = new();
    private readonly Mock<IConfiguration> _configMock = new();

    private AuthorizationFilterContext CreateContext()
    {
        var httpContext = new DefaultHttpContext();
        var servicesMock = new Mock<IServiceProvider>();
        servicesMock.Setup(s => s.GetService(typeof(IConfiguration))).Returns(_configMock.Object);
        httpContext.RequestServices = servicesMock.Object;

        return new AuthorizationFilterContext(
            new ActionContext(httpContext, new RouteData(), new ActionDescriptor()),
            new List<IFilterMetadata>());
    }

    [Fact]
    public void OnAuthorization_NoApiKeyConfigured_Returns401()
    {
        _configMock.Setup(c => c["ZKTecoBridge:ApiKey"]).Returns((string?)null);
        var ctx = CreateContext();

        _attribute.OnAuthorization(ctx);

        ctx.Result.Should().BeOfType<UnauthorizedObjectResult>();
    }

    [Fact]
    public void OnAuthorization_WrongApiKey_Returns401()
    {
        _configMock.Setup(c => c["ZKTecoBridge:ApiKey"]).Returns("correct-key");
        var ctx = CreateContext();
        ctx.HttpContext.Request.Headers["X-API-Key"] = "wrong-key";

        _attribute.OnAuthorization(ctx);

        ctx.Result.Should().BeOfType<UnauthorizedObjectResult>();
    }

    [Fact]
    public void OnAuthorization_ValidApiKey_NoResult()
    {
        _configMock.Setup(c => c["ZKTecoBridge:ApiKey"]).Returns("correct-key");
        var ctx = CreateContext();
        ctx.HttpContext.Request.Headers["X-API-Key"] = "correct-key";

        _attribute.OnAuthorization(ctx);

        ctx.Result.Should().BeNull();
    }
}
