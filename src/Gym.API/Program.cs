using System.Globalization;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.RateLimiting;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using System.Diagnostics;
using Gym.API.Hubs;
using Gym.API.Middleware;
using Gym.API;
using Gym.API.Services;
using Gym.API.WebSockets;
using Gym.Application;
using Gym.Application.Common.Interfaces;
using Gym.Infrastructure;
using Gym.Infrastructure.Data;
using Gym.Infrastructure.Security;
using Gym.Infrastructure.Services;
using Gym.Infrastructure.Services.ZKTeco;
using Hangfire;
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
    .WriteTo.File("logs/attendance-.log", rollingInterval: RollingInterval.Day, restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information)
    .WriteTo.File("logs/device-.log", rollingInterval: RollingInterval.Day, restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information)
    .CreateLogger();

// Resolve JWT secret from env var first (more secure), then fall back to appsettings.json.
// In production, set the environment variable JWT__Secret to a strong random value (>= 32 chars).
var jwtSecret = Environment.GetEnvironmentVariable("JWT__Secret")
                ?? builder.Configuration["Jwt:Secret"]
                ?? throw new InvalidOperationException("JWT secret is not configured. Set the JWT__Secret environment variable or Jwt:Secret in appsettings.json.");

if (jwtSecret.Length < 32)
{
    throw new InvalidOperationException($"JWT secret must be at least 32 characters. Current length: {jwtSecret.Length}.");
}

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

// (jwtSecret is already resolved from env var + fallback above)

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new()
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret)),
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
builder.Services.AddSingleton<BridgeWebSocketHandler>();
builder.Services.AddScoped<AttendancePushService>();
builder.Services.AddScoped<ReceiptPdfService>();
builder.Services.AddHostedService<Gym.Infrastructure.Data.Seed.SeedDataInitializer>();
builder.Services.AddHealthChecks().AddDbContextCheck<Gym.Infrastructure.Data.GymDbContext>();
builder.Services.AddMemoryCache();
builder.Services.AddScoped<ICacheService, CacheService>();
builder.Services.Configure<Gym.Infrastructure.Resilience.DeviceConnectionManagerOptions>(
    builder.Configuration.GetSection("DeviceConnectionManager"));

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
builder.Services.Configure<ZKTecoBridgeOptions>(builder.Configuration.GetSection("ZKTecoBridge"));

// Hangfire temporarily disabled: no SQLite/Memory storage package available offline.
// Re-enable once Hangfire.MemoryStorage or Hangfire.SQLite can be restored.
// builder.Services.AddHangfire(config =>
//     config.UseMemoryStorage());
// builder.Services.AddHangfireServer();

builder.Services.AddScoped<Gym.Application.Jobs.SubscriptionRenewalReminderJob>();
builder.Services.AddScoped<Gym.Application.Jobs.SubscriptionExpiryJob>();
builder.Services.AddScoped<Gym.Application.Jobs.LeadFollowUpJob>();

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

builder.Services.AddHostedService<SystemHealthMonitor>();

        QuestPDF.Settings.License = LicenseType.Community;

var app = builder.Build();

Gym.Application.Common.DTOs.PaginationRequest.DefaultPageSize = app.Configuration.GetValue<int?>("Pagination:DefaultPageSize") ?? 20;

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<GymDbContext>();
    try
    {
        await db.Database.EnsureCreatedAsync();
        Log.Information("SQLite database and schema ensured");
    }
    catch (Exception ex)
    {
        Log.Warning(ex, "Database initialization failed");
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.MapOpenApi();
}

// Ensure SQLite database schema exists (code-first, no migrations)
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<GymDbContext>();
    try
    {
        await db.Database.EnsureCreatedAsync();
        Log.Information("SQLite database schema ensured (second pass)");
    }
    catch (Exception ex)
    {
        Log.Warning(ex, "Database initialization (second pass) failed");
    }
}

app.UseMiddleware<ExceptionMiddleware>();

app.UseRateLimiter();

app.UseCors("AllowFrontend");

app.UseRequestLocalization();

app.UseWebSockets(new WebSocketOptions
{
    KeepAliveInterval = TimeSpan.FromSeconds(30)
});

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
app.Map("/ws/bridge", async (HttpContext context, BridgeWebSocketHandler handler) =>
{
    await handler.HandleAsync(context, context.RequestAborted);
});

// Hangfire dashboard and recurring jobs temporarily disabled (no storage available offline).
// app.UseHangfireDashboard("/hangfire", new DashboardOptions
// {
//     Authorization = new[] { new HangfireAuthorizationFilter() }
// });
//
// RecurringJob.AddOrUpdate<Gym.Application.Jobs.SubscriptionRenewalReminderJob>("subscription-renewal-reminders",
//     job => job.ExecuteAsync(CancellationToken.None), Cron.Daily(9));
// RecurringJob.AddOrUpdate<Gym.Application.Jobs.SubscriptionExpiryJob>("subscription-expiry",
//     job => job.ExecuteAsync(CancellationToken.None), Cron.Daily(0));
// RecurringJob.AddOrUpdate<Gym.Application.Jobs.LeadFollowUpJob>("lead-follow-up",
//     job => job.ExecuteAsync(CancellationToken.None), Cron.Daily(14));

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

