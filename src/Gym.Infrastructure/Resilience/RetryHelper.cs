using Microsoft.Extensions.Logging;

namespace Gym.Infrastructure.Resilience;

public static class RetryHelper
{
    public static async Task<T> ExecuteWithRetryAsync<T>(
        Func<Task<T>> operation,
        int maxRetries = 3,
        int delayMs = 1000,
        ILogger? logger = null)
    {
        int attempt = 0;
        while (true)
        {
            try
            {
                return await operation();
            }
            catch (Exception ex)
            {
                attempt++;
                if (attempt >= maxRetries)
                {
                    logger?.LogError(ex, "Operation failed after {MaxRetries} attempts", maxRetries);
                    throw;
                }

                logger?.LogWarning(ex,
                    "Attempt {Attempt}/{MaxRetries} failed. Retrying in {DelayMs}ms...",
                    attempt, maxRetries, delayMs * attempt);
                await Task.Delay(delayMs * attempt);
            }
        }
    }

    public static async Task ExecuteWithRetryAsync(
        Func<Task> operation,
        int maxRetries = 3,
        int delayMs = 1000,
        ILogger? logger = null)
    {
        await ExecuteWithRetryAsync(async () =>
        {
            await operation();
            return true;
        }, maxRetries, delayMs, logger);
    }
}
