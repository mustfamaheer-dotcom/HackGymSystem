using Gym.Domain.Entities;
using Gym.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Gym.Infrastructure.Data.Seed;

public class SeedDataInitializer : IHostedService
{
    private static readonly Guid AdminUserId = Guid.Parse("D4E5F6A7-B8C9-0123-DEF4-567890123456");

    private readonly IServiceProvider _serviceProvider;
    private readonly IConfiguration _configuration;
    private readonly ILogger<SeedDataInitializer> _logger;

    public SeedDataInitializer(IServiceProvider serviceProvider, IConfiguration configuration, ILogger<SeedDataInitializer> logger)
    {
        _serviceProvider = serviceProvider;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
        var userRepo = unitOfWork.Repository<User>();
        var settingRepo = unitOfWork.Repository<Setting>();

        var configuredBackupPath = _configuration["Seed:BackupPath"];
        if (!string.IsNullOrEmpty(configuredBackupPath))
        {
            var setting = await settingRepo.Query().FirstOrDefaultAsync(s => s.Key == "BackupPath", cancellationToken);
            if (setting != null && setting.Value != configuredBackupPath)
            {
                setting.Value = configuredBackupPath;
                settingRepo.Update(setting);
                await unitOfWork.SaveChangesAsync(cancellationToken);
                _logger.LogInformation("Updated BackupPath setting to {BackupPath}", configuredBackupPath);
            }
        }

        var adminPassword = _configuration["Seed:AdminPassword"];
        if (!string.IsNullOrEmpty(adminPassword))
        {
            var admin = await userRepo.GetByIdAsync(AdminUserId, cancellationToken);
            if (admin == null)
            {
                var role = await unitOfWork.Repository<Role>().GetByIdAsync(Seed.SeedData.OwnerRoleId, cancellationToken);
                if (role != null)
                {
                    var newAdmin = new User("admin", BCrypt.Net.BCrypt.HashPassword(adminPassword, workFactor: 11), "System Administrator", "admin@gym.com", null, Seed.SeedData.OwnerRoleId)
                    {
                        Id = AdminUserId,
                        IsActive = true,
                        IsPasswordChangeRequired = true,
                        CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                    };
                    await userRepo.AddAsync(newAdmin, cancellationToken);
                    await unitOfWork.SaveChangesAsync(cancellationToken);
                    _logger.LogInformation("Created default admin user from Seed:AdminPassword configuration");
                }
            }
        }
        else
        {
            _logger.LogWarning("Seed:AdminPassword is not configured. Admin user will not be created automatically. Set Seed:AdminPassword via environment variable or user secrets.");
        }
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
