using Gym.Application.Common.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Gym.Infrastructure.Services;

public class OfferExpiryHostedService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<OfferExpiryHostedService> _logger;

    public OfferExpiryHostedService(IServiceProvider serviceProvider, ILogger<OfferExpiryHostedService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Offer expiry service started");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await ProcessExpiredOffers(stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing expired offers");
            }

            await Task.Delay(TimeSpan.FromDays(1), stoppingToken);
        }
    }

    private async Task ProcessExpiredOffers(CancellationToken stoppingToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var offerService = scope.ServiceProvider.GetRequiredService<IOfferService>();

        var count = await offerService.ExpireOffersAsync(stoppingToken);
        if (count > 0)
        {
            _logger.LogInformation("Expired {Count} offer(s)", count);
        }
    }
}
