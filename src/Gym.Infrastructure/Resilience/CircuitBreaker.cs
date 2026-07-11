using Microsoft.Extensions.Logging;

namespace Gym.Infrastructure.Resilience;

public class CircuitBreaker
{
    private int _failureCount;
    private DateTime? _lastFailureTime;
    private readonly int _failureThreshold;
    private readonly TimeSpan _recoveryTime;
    private readonly object _lock = new();
    private readonly ILogger<CircuitBreaker>? _logger;

    public CircuitBreaker(int failureThreshold = 5, int recoveryTimeSeconds = 60, ILogger<CircuitBreaker>? logger = null)
    {
        _failureThreshold = failureThreshold;
        _recoveryTime = TimeSpan.FromSeconds(recoveryTimeSeconds);
        _logger = logger;
    }

    public bool IsOpen
    {
        get
        {
            lock (_lock)
            {
                if (_failureCount < _failureThreshold) return false;
                if (_lastFailureTime.HasValue && DateTime.UtcNow - _lastFailureTime > _recoveryTime)
                {
                    _logger?.LogInformation("Circuit breaker transitioning to HALF-OPEN state");
                    _failureCount = 0;
                    return false;
                }
                return true;
            }
        }
    }

    public void RecordSuccess()
    {
        lock (_lock)
        {
            if (_failureCount > 0)
                _logger?.LogInformation("Circuit breaker RESET after successful operation");
            _failureCount = 0;
        }
    }

    public void RecordFailure()
    {
        lock (_lock)
        {
            _failureCount++;
            _lastFailureTime = DateTime.UtcNow;
            _logger?.LogWarning(
                "Circuit breaker recorded failure: {Count}/{Threshold}",
                _failureCount, _failureThreshold);

            if (_failureCount >= _failureThreshold)
                _logger?.LogError("Circuit breaker OPENED after {Threshold} consecutive failures", _failureThreshold);
        }
    }

    public CircuitBreakerState GetState()
    {
        lock (_lock)
        {
            if (_failureCount < _failureThreshold)
                return CircuitBreakerState.Closed;
            if (_lastFailureTime.HasValue && DateTime.UtcNow - _lastFailureTime > _recoveryTime)
                return CircuitBreakerState.HalfOpen;
            return CircuitBreakerState.Open;
        }
    }
}

public enum CircuitBreakerState
{
    Closed,
    Open,
    HalfOpen
}
