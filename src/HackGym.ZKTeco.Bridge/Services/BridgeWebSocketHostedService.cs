using Microsoft.Extensions.Hosting;

namespace HackGym.ZKTeco.Bridge.Services;

public class BridgeWebSocketHostedService : BackgroundService
{
    private readonly BridgeWebSocketClient _wsClient;
    private readonly ILogger<BridgeWebSocketHostedService> _logger;
    private int _reconnectDelayMs = 1000;
    private const int MaxReconnectDelayMs = 30000;

    public BridgeWebSocketHostedService(
        BridgeWebSocketClient wsClient,
        ILogger<BridgeWebSocketHostedService> logger)
    {
        _wsClient = wsClient;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Bridge WebSocket hosted service starting");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await _wsClient.ConnectAsync(stoppingToken);
                _reconnectDelayMs = 1000;

                _logger.LogInformation("Bridge WebSocket connected to Gym API");

                // Wait until the API (or network) drops the connection.
                await WaitForDisconnectAsync(stoppingToken);
            }
            catch (OperationCanceledException) { break; }
            catch (Exception ex)
            {
                // The API is typically not up yet during startup (connection refused /
                // name resolution). Back off exponentially and avoid flooding the logs:
                // log a concise info line per attempt, full exception detail only at debug.
                _logger.LogInformation(
                    "Bridge WebSocket not connected to Gym API yet — retrying in {Delay}ms (exponential backoff)",
                    _reconnectDelayMs);
                _logger.LogDebug(ex, "WebSocket connect attempt failed");
            }

            if (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(_reconnectDelayMs, stoppingToken);
                _reconnectDelayMs = Math.Min(_reconnectDelayMs * 2, MaxReconnectDelayMs);
            }
        }
    }

    private async Task WaitForDisconnectAsync(CancellationToken ct)
    {
        while (!ct.IsCancellationRequested && _wsClient.IsConnected)
        {
            await Task.Delay(1000, ct);
        }
    }
}
