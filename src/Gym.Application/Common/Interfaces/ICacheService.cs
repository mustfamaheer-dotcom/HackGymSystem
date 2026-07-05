namespace Gym.Application.Common.Interfaces;

public interface ICacheService
{
    Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default) where T : class;
    Task SetAsync<T>(string key, T value, TimeSpan? expiry = null, CancellationToken cancellationToken = default) where T : class;
    Task RemoveAsync(string key, CancellationToken cancellationToken = default);
}

public static class CacheKeys
{
    public const string Plans = "cache:plans";
    public const string Offers = "cache:offers";
    public const string Settings = "cache:settings";
    public static readonly TimeSpan DefaultTtl = TimeSpan.FromMinutes(10);
}
