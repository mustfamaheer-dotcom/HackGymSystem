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
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
