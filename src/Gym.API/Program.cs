using System.Globalization;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.RateLimiting;
using Gym.API.Hubs;
using Gym.API.Middleware;
using Gym.API;
using Gym.API.Services;
using Gym.Application;
using Gym.Application.Common.Interfaces;
using Gym.Infrastructure;
using Gym.Infrastructure.Data;
using Gym.Infrastructure.Security;
using Gym.Infrastructure.Services;
using Gym.Infrastructure.Services.ZKTeco;
using QuestPDF.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("logs/gym-api-.log", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add(new Microsoft.AspNetCore.Mvc.AutoValidateAntiforgeryTokenAttribute());
})
    .AddViewLocalization()
    .AddDataAnnotationsLocalization()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });
builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer((document, context, cancellationToken) =>
    {
        document.Info = new()
        {
            Title = "Gym Management API",
            Version = "v1",
            Description = "REST API for Gym Management System"
        };
        return Task.CompletedTask;
    });
});

var jwtSecret = builder.Configuration["Jwt:Secret"];
if (string.IsNullOrEmpty(jwtSecret) || jwtSecret.Length < 32)
{
    throw new InvalidOperationException(
        "JWT secret is not configured or is too short (minimum 32 characters). "
        + "Set Jwt:Secret via environment variable or appsettings.Development.json.");
}

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new()
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSecret)),
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidateAudience = true,
            ValidAudience = builder.Configuration["Jwt:Audience"],
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };

        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                context.Token = context.Request.Cookies["accessToken"];
                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:5173", "http://localhost:3000")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCultures = new[]
    {
        new CultureInfo("ar"),
        new CultureInfo("en")
    };

    options.DefaultRequestCulture = new RequestCulture("ar", "ar");
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;
    options.RequestCultureProviders.Insert(0, new CookieRequestCultureProvider());
});

builder.Services.AddSignalR();
builder.Services.AddScoped<ReceiptPdfService>();
builder.Services.AddHostedService<Gym.Infrastructure.Data.Seed.SeedDataInitializer>();
builder.Services.AddHealthChecks().AddDbContextCheck<Gym.Infrastructure.Data.GymDbContext>();
builder.Services.AddMemoryCache();
builder.Services.AddScoped<ICacheService, CacheService>();

builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

    options.AddFixedWindowLimiter("Login", config =>
    {
        config.PermitLimit = 5;
        config.Window = TimeSpan.FromMinutes(1);
        config.QueueLimit = 0;
    });

    options.AddFixedWindowLimiter("Api", config =>
    {
        config.PermitLimit = 100;
        config.Window = TimeSpan.FromMinutes(1);
        config.QueueLimit = 0;
    });
});

builder.Services.Configure<ZKTecoSettings>(builder.Configuration.GetSection("ZKTeco"));
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

// Application-layer service registrations (composition root)
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IMemberService, MemberService>();
builder.Services.AddScoped<IExcelImportService, ExcelImportService>();
builder.Services.AddScoped<IOfferService, OfferService>();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
builder.Services.AddScoped<IRolePermissionService, RolePermissionService>();

QuestPDF.Settings.License = LicenseType.Community;

var app = builder.Build();

Gym.Application.Common.DTOs.PaginationRequest.DefaultPageSize = app.Configuration.GetValue<int?>("Pagination:DefaultPageSize") ?? 20;

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<GymDbContext>();
    try
    {
        var pending = await db.Database.GetPendingMigrationsAsync();
        if (pending.Any())
        {
            await db.Database.MigrateAsync();
            Log.Information("Applied {Count} pending migration(s)", pending.Count());
        }
        else
        {
            Log.Information("No pending migrations");
        }
    }
    catch (Exception ex)
    {
        Log.Warning(ex, "Database migration failed, attempting to create database");
        try
        {
            if (!await db.Database.CanConnectAsync())
                await db.Database.EnsureCreatedAsync();
        }
        catch (Exception createEx)
        {
            Log.Warning(createEx, "Database creation also failed, continuing with existing database");
        }
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.MapOpenApi();
}

app.UseMiddleware<ExceptionMiddleware>();

app.UseRateLimiter();

app.UseCors("AllowFrontend");

app.UseRequestLocalization();

app.UseAuthentication();
app.UseAuthorization();

// Root redirect — catches / before routing middleware
app.Use(async (context, next) =>
{
    if (context.Request.Path == "/")
    {
        context.Response.Redirect("/Account/Login");
        return;
    }
    await next();
});

app.UseStaticFiles();

app.MapControllers();
app.MapHealthChecks("/health");
app.MapHub<AttendanceHub>("/hubs/attendance");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

try
{
    Log.Information("Starting Gym Management API");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}

