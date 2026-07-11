using Gym.Domain.Entities;
using Gym.Domain.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace Gym.Infrastructure.Caching;

public interface IMemberLookupCache
{
    Task<Member?> GetByIdAsync(Guid memberId, CancellationToken ct = default);
    Task<Member?> GetByCodeAsync(string code, CancellationToken ct = default);
    void Invalidate(Guid memberId);
    void InvalidateAll();
}

public class MemberLookupCache : IMemberLookupCache
{
    private readonly IMemoryCache _cache;
    private readonly IRepository<Member> _memberRepo;
    private readonly ILogger<MemberLookupCache> _logger;

    private static readonly MemoryCacheEntryOptions CacheOptions = new()
    {
        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10),
        SlidingExpiration = TimeSpan.FromMinutes(5),
        Priority = CacheItemPriority.Normal
    };

    public MemberLookupCache(IMemoryCache cache, IRepository<Member> memberRepo, ILogger<MemberLookupCache> logger)
    {
        _cache = cache;
        _memberRepo = memberRepo;
        _logger = logger;
    }

    public async Task<Member?> GetByIdAsync(Guid memberId, CancellationToken ct = default)
    {
        var key = $"MemberLookup_{memberId}";
        if (_cache.TryGetValue(key, out Member? cached))
            return cached;

        var member = await _memberRepo.GetByIdAsync(memberId, ct);
        if (member != null)
            _cache.Set(key, member, CacheOptions);

        return member;
    }

    public async Task<Member?> GetByCodeAsync(string code, CancellationToken ct = default)
    {
        var key = $"MemberLookup_Code_{code}";
        if (_cache.TryGetValue(key, out Member? cached))
            return cached;

        if (!int.TryParse(code, out var codeInt))
            return null;

        var member = await _memberRepo.FirstOrDefaultAsync(m => m.Code == codeInt, ct);
        if (member != null)
            _cache.Set(key, member, CacheOptions);

        return member;
    }

    public void Invalidate(Guid memberId)
    {
        _cache.Remove($"MemberLookup_{memberId}");
    }

    public void InvalidateAll()
    {
        _cache.Remove("MemberLookup_");
    }
}
