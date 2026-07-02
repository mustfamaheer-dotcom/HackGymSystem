using Gym.Application.Common.Interfaces;
using Gym.Domain.Interfaces;
using Gym.Infrastructure.Data;
using Gym.Infrastructure.Repositories;
using Gym.Infrastructure.Security;
using Gym.Infrastructure.Services;
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
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

        return services;
    }
}
