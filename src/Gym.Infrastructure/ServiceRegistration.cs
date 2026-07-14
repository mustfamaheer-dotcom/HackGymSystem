using System.IO;
using System;
using Gym.Application.Common.Events;
using Gym.Application.Common.Interfaces;
using Gym.Domain.Interfaces;
using Gym.Infrastructure.Caching;
using Gym.Infrastructure.Data;
using Gym.Infrastructure.Events;
using Gym.Infrastructure.Repositories;
using Gym.Infrastructure.Resilience;
using Gym.Infrastructure.Security;
using Gym.Infrastructure.Services;
using Gym.Infrastructure.Services.ZKTeco;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Gym.Infrastructure;

public static class ServiceRegistration
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        GymDbContext.UseSqliteRowVersionWorkaround = true;
var connString = configuration.GetConnectionString("DefaultConnection");
if (!string.IsNullOrWhiteSpace(connString))
{
    // Extract the Data Source path and resolve relative paths to absolute
    var parts = connString.Split(';')
        .Select(p => p.Split('=', 2))
        .Where(p => p.Length == 2 && p[0].Trim().Equals("Data Source", StringComparison.OrdinalIgnoreCase))
        .Select(p => p[1].Trim())
        .ToList();
    if (parts.Count > 0 && !string.IsNullOrWhiteSpace(parts[0]) && !Path.IsPathRooted(parts[0]))
    {
        // Relative path: resolve to the application base directory (next to the binary)
        var appDir = AppContext.BaseDirectory;
        var absoluteDbPath = Path.GetFullPath(Path.Combine(appDir, parts[0]));
        connString = $"Data Source={absoluteDbPath};Cache=Shared;";
    }
}
else
{
    // No connection string: create the DB in the application base directory
    var appDir = AppContext.BaseDirectory;
    var dbPath = Path.Combine(appDir, "GymDb.db");
    connString = $"Data Source={dbPath};Cache=Shared;";
}
services.AddDbContext<GymDbContext>(options =>
    options.UseSqlite(
        connString,
        sqliteOptions =>
        {
            sqliteOptions.MigrationsAssembly(typeof(GymDbContext).Assembly.FullName);
        }));

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped<IMemberRepository, MemberRepository>();

        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IMemberService, MemberService>();
        services.AddScoped<IExcelImportService, ExcelImportService>();
        services.AddScoped<IOfferService, OfferService>();
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddScoped<IRolePermissionService, RolePermissionService>();
        services.AddScoped<IDeviceMemberMappingRepository, DeviceMemberMappingRepository>();
        services.AddScoped<ICaptchaService, CaptchaService>();
        services.AddScoped<ISyncAuditService, SyncAuditService>();
        services.AddScoped<IWhatsAppService, WhatsAppService>();

        services.AddHttpClient("ZKTecoBridge", client =>
        {
            client.Timeout = TimeSpan.FromSeconds(30);
        });
        // Named client for SystemHealthMonitor — short timeout, reused
        // instance (avoids socket exhaustion from `new HttpClient()` per
        // check cycle). The monitor uses this in CheckApiEndpoints.
        services.AddHttpClient("SystemHealthMonitor", client =>
        {
            client.Timeout = TimeSpan.FromSeconds(5);
        });
        services.AddScoped<IZKTecoBridgeClient, ZKTecoBridgeGrpcClient>();
        services.Configure<ZKTecoBridgeOptions>(configuration.GetSection("ZKTecoBridge"));

        services.AddHostedService<BackgroundJobs.PeriodicReconciliationWorker>();

        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

        // Event Bus
        services.AddSingleton<IEventBus, InMemoryEventBus>();
        services.AddScoped<IEventPublisher, EventPublisher>();
        services.AddScoped<Events.AttendanceEventHandler>();
        services.AddScoped<Events.DeviceEventHandler>();

        // Caching
        services.AddMemoryCache();
        services.AddScoped<ITrackMembersCache, TrackMembersCache>();
        services.AddScoped<IMemberLookupCache, MemberLookupCache>();

        // Specialized Repositories
        services.AddScoped<IDeviceTrackingRepository, DeviceTrackingRepository>();
        services.AddScoped<IAttendanceTrackingRepository, AttendanceTrackingRepository>();
        services.AddScoped<IAttendanceSummaryTrackingRepository, AttendanceSummaryTrackingRepository>();

        // Resilience
        services.Configure<DeviceConnectionManagerOptions>(configuration.GetSection("DeviceConnectionManager"));
        services.AddScoped<DeviceConnectionManager>();

        return services;
    }
}
