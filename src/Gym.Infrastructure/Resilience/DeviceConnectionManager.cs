using Gym.Application.Common.Interfaces;
using Gym.Domain.Events;
using Gym.Infrastructure.Resilience;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Gym.Infrastructure.Resilience;

public class DeviceConnectionManager : IDisposable
{
    private readonly IZKTecoBridgeClient _bridgeClient;
    private readonly DeviceConnectionManagerOptions _options;
    private readonly ILogger<DeviceConnectionManager> _logger;
    private readonly CircuitBreaker _circuitBreaker;
    private bool _isConnected;

    public DeviceConnectionManager(
        IZKTecoBridgeClient bridgeClient,
        IOptions<DeviceConnectionManagerOptions> options,
        ILogger<DeviceConnectionManager> logger,
        ILogger<CircuitBreaker> circuitBreakerLogger)
    {
        _bridgeClient = bridgeClient;
        _options = options.Value;
        _logger = logger;
        _circuitBreaker = new CircuitBreaker(
            _options.FailureThreshold,
            _options.RecoveryTimeSeconds,
            circuitBreakerLogger);
    }

    public bool IsConnected => _isConnected;
    public CircuitBreakerState CircuitState => _circuitBreaker.GetState();

    public async Task<bool> ConnectAsync(CancellationToken cancellationToken = default)
    {
        if (_circuitBreaker.IsOpen)
        {
            _logger.LogWarning("Circuit breaker OPEN. Skipping connection attempt.");
            return false;
        }

        try
        {
            var health = await _bridgeClient.CheckHealthAsync(cancellationToken);
            _isConnected = health.IsConnected;

            if (_isConnected)
            {
                _circuitBreaker.RecordSuccess();
                _logger.LogInformation("Connected to ZKTeco bridge. Enrolled users: {Count}",
                    health.EnrolledUserCount);
            }
            else
            {
                _circuitBreaker.RecordFailure();
            }

            return _isConnected;
        }
        catch (Exception ex)
        {
            _isConnected = false;
            _circuitBreaker.RecordFailure();
            _logger.LogError(ex, "Failed to connect to ZKTeco bridge");
            return false;
        }
    }

    public async Task<T?> ExecuteWithCircuitBreakerAsync<T>(
        Func<CancellationToken, Task<T>> operation,
        CancellationToken cancellationToken = default)
    {
        if (_circuitBreaker.IsOpen)
        {
            _logger.LogWarning("Circuit breaker OPEN. Skipping operation.");
            return default;
        }

        try
        {
            var result = await operation(cancellationToken);
            _circuitBreaker.RecordSuccess();
            _isConnected = true;
            return result;
        }
        catch (Exception ex)
        {
            _circuitBreaker.RecordFailure();
            _isConnected = false;
            _logger.LogError(ex, "Operation failed through circuit breaker");
            throw;
        }
    }

    public void Dispose()
    {
        _isConnected = false;
    }
}

public class DeviceConnectionManagerOptions
{
    public int FailureThreshold { get; set; } = 5;
    public int RecoveryTimeSeconds { get; set; } = 60;
}
