using System.Net;
using System.Text.Json;
using FluentValidation;
using Gym.API;
using Gym.Application.Common.DTOs;
using Microsoft.Extensions.Localization;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Gym.API.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ValidationException ex)
        {
            _logger.LogWarning("Validation failed: {Errors}", string.Join("; ", ex.Errors.Select(e => e.ErrorMessage)));
            var localizer = context.RequestServices.GetService<IStringLocalizer<SharedResources>>();
            var message = localizer?["Validation failed"].Value ?? "Validation failed";
            await HandleExceptionAsync(context, HttpStatusCode.BadRequest, message, ex.Errors.Select(e => e.ErrorMessage).ToArray());
        }
        catch (UnauthorizedAccessException)
        {
            var localizer = context.RequestServices.GetService<IStringLocalizer<SharedResources>>();
            var message = localizer?["Unauthorized"].Value ?? "Unauthorized";
            await HandleExceptionAsync(context, HttpStatusCode.Unauthorized, message);
        }
        catch (KeyNotFoundException)
        {
            var localizer = context.RequestServices.GetService<IStringLocalizer<SharedResources>>();
            var message = localizer?["Resource not found"].Value ?? "Resource not found";
            await HandleExceptionAsync(context, HttpStatusCode.NotFound, message);
        }
        catch (ArgumentException)
        {
            var localizer = context.RequestServices.GetService<IStringLocalizer<SharedResources>>();
            var message = localizer?["Invalid request"].Value ?? "Invalid request";
            await HandleExceptionAsync(context, HttpStatusCode.BadRequest, message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception occurred");

            if (context.Request.Path.StartsWithSegments("/api"))
            {
                var localizer = context.RequestServices.GetService<IStringLocalizer<SharedResources>>();
                var message = localizer?["An error occurred. Please try again later."].Value ?? "An error occurred. Please try again later.";
                await HandleExceptionAsync(context, HttpStatusCode.InternalServerError, message);
            }
            else
            {
                // Surface the error to the user via TempData (consumed by _Notifications.cshtml)
                // so silent redirect-on-exception can never hide the root cause again.
                var tempDataFactory = context.RequestServices.GetService<ITempDataDictionaryFactory>();
                if (tempDataFactory is not null)
                {
                    var tempData = tempDataFactory.GetTempData(context);
                    tempData["Error"] = "An unexpected error occurred. Please try again. If the problem persists, contact the system administrator.";
                    tempData.Save();
                }

                context.Response.StatusCode = 302;
                var referer = context.Request.Headers.Referer.ToString();
                context.Response.Headers.Location = string.IsNullOrEmpty(referer) ? "/" : referer;
            }
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, HttpStatusCode statusCode, string message, params string[] errors)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        var response = ApiResponse.Fail(message, errors);
        var json = JsonSerializer.Serialize(response, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        await context.Response.WriteAsync(json);
    }
}
