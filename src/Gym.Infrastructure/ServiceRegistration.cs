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
        services.AddDbContext<GymDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                sqlOptions =>
                {
                    sqlOptions.MigrationsAssembly(typeof(GymDbContext).Assembly.FullName);
                    sqlOptions.EnableRetryOnFailure(3);
                    sqlOptions.CommandTimeout(60);
                })
                );

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
