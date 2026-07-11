using Gym.Domain.Entities;
using Gym.Domain.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace Gym.Infrastructure.Caching;

public interface ITrackMembersCache
{
    Task<IReadOnlyList<Member>> GetActiveMembersAsync(Func<CancellationToken, Task<IReadOnlyList<Member>>> loader, CancellationToken ct = default);
    Task<Member?> GetMemberByIdAsync(Guid memberId, Func<CancellationToken, Task<Member?>> loader, CancellationToken ct = default);
    Task<IReadOnlyList<Device>> GetActiveDevicesAsync(Func<CancellationToken, Task<IReadOnlyList<Device>>> loader, CancellationToken ct = default);
    Task<int> GetTodayCheckInCountAsync(Func<CancellationToken, Task<int>> loader, CancellationToken ct = default);
    void InvalidateMemberCache();
    void InvalidateDeviceCache();
    void InvalidateAttendanceCache();
    void InvalidateAll();
}

public class TrackMembersCache : ITrackMembersCache
{
    private readonly IMemoryCache _cache;
    private const string ActiveMembersKey = "TrackMembers_ActiveMembers";
    private const string ActiveDevicesKey = "TrackMembers_ActiveDevices";
    private const string TodayCheckInKey = "TrackMembers_TodayCheckIn";

    private static readonly MemoryCacheEntryOptions MembersCacheOptions = new()
    {
        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5),
        SlidingExpiration = TimeSpan.FromMinutes(2),
        Priority = CacheItemPriority.Normal
    };

    private static readonly MemoryCacheEntryOptions DeviceCacheOptions = new()
    {
        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10),
        SlidingExpiration = TimeSpan.FromMinutes(5),
        Priority = CacheItemPriority.Normal
    };

    private static readonly MemoryCacheEntryOptions ShortCacheOptions = new()
    {
        AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30),
        SlidingExpiration = TimeSpan.FromSeconds(15),
        Priority = CacheItemPriority.Low
    };

    public TrackMembersCache(IMemoryCache cache)
    {
        _cache = cache;
    }

    public async Task<IReadOnlyList<Member>> GetActiveMembersAsync(
        Func<CancellationToken, Task<IReadOnlyList<Member>>> loader,
        CancellationToken ct = default)
    {
        if (_cache.TryGetValue(ActiveMembersKey, out IReadOnlyList<Member>? cached) && cached != null)
            return cached;

        var members = await loader(ct);
        _cache.Set(ActiveMembersKey, members, MembersCacheOptions);
        return members;
    }

    public async Task<Member?> GetMemberByIdAsync(
        Guid memberId,
        Func<CancellationToken, Task<Member?>> loader,
        CancellationToken ct = default)
    {
        var key = $"TrackMembers_Member_{memberId}";
        if (_cache.TryGetValue(key, out Member? cached))
            return cached;

        var member = await loader(ct);
        _cache.Set(key, member, MembersCacheOptions);
        return member;
    }

    public async Task<IReadOnlyList<Device>> GetActiveDevicesAsync(
        Func<CancellationToken, Task<IReadOnlyList<Device>>> loader,
        CancellationToken ct = default)
    {
        if (_cache.TryGetValue(ActiveDevicesKey, out IReadOnlyList<Device>? cached) && cached != null)
            return cached;

        var devices = await loader(ct);
        _cache.Set(ActiveDevicesKey, devices, DeviceCacheOptions);
        return devices;
    }

    public async Task<int> GetTodayCheckInCountAsync(
        Func<CancellationToken, Task<int>> loader,
        CancellationToken ct = default)
    {
        if (_cache.TryGetValue(TodayCheckInKey, out int cached))
            return cached;

        var count = await loader(ct);
        _cache.Set(TodayCheckInKey, count, ShortCacheOptions);
        return count;
    }

    public void InvalidateMemberCache()
    {
        _cache.Remove(ActiveMembersKey);
    }

    public void InvalidateDeviceCache()
    {
        _cache.Remove(ActiveDevicesKey);
    }

    public void InvalidateAttendanceCache()
    {
        _cache.Remove(TodayCheckInKey);
    }

    public void InvalidateAll()
    {
        _cache.Remove(ActiveMembersKey);
        _cache.Remove(ActiveDevicesKey);
        _cache.Remove(TodayCheckInKey);
    }
}
