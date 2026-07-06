using Gym.Application.ZKTeco.Commands;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Gym.Infrastructure.BackgroundJobs;

public class PeriodicReconciliationWorker : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<PeriodicReconciliationWorker> _logger;

    public PeriodicReconciliationWorker(IServiceScopeFactory scopeFactory, ILogger<PeriodicReconciliationWorker> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Periodic reconciliation worker starting");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await Task.Delay(TimeSpan.FromMinutes(20), stoppingToken);

                using var scope = _scopeFactory.CreateScope();
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                var result = await mediator.Send(new ReconcileUsersCommand(), stoppingToken);

                if (result.IsFailure)
                    _logger.LogWarning("Reconciliation failed: {Message}", result.Message);
                else
                    _logger.LogInformation("Reconciliation completed: {Checked} users, {Fixed} fixes",
                        result.Data!.UsersChecked, result.Data.DiscrepanciesFixed);
            }
            catch (OperationCanceledException)
            {
                break;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in periodic reconciliation");
            }
        }
    }
}
