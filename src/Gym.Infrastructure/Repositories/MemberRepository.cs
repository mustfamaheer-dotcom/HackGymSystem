using Gym.Domain.Entities;
using Gym.Domain.Interfaces;
using Gym.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Gym.Infrastructure.Repositories;

public class MemberRepository : Repository<Member>, IMemberRepository
{
    public MemberRepository(GymDbContext context) : base(context) { }

    public async Task<Member?> GetByIdAsync(Guid id, bool includeDeleted, CancellationToken cancellationToken = default)
    {
        var query = _dbSet.AsQueryable();

        if (includeDeleted)
            query = query.IgnoreQueryFilters();

        return await query
            .Include(m => m.Package)
            .FirstOrDefaultAsync(m => m.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<Member>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        var search = searchTerm.ToLower();
        return await _dbSet
            .Include(m => m.Package)
            .Where(m =>
                m.FullName.ToLower().Contains(search) ||
                m.PhoneNumber.Contains(search) ||
                m.ReceiptNumber.ToLower().Contains(search) ||
                m.NationalId.Contains(search))
            .OrderBy(m => m.FullName)
            .ToListAsync(cancellationToken);
    }

    public async Task SoftDeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var member = await _dbSet.FindAsync([id], cancellationToken);
        if (member is not null)
        {
            member.SoftDelete();
            _dbSet.Update(member);
        }
    }

    public async Task RestoreAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var member = await _dbSet
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(m => m.Id == id, cancellationToken);

        if (member is not null)
        {
            member.Restore();
            _dbSet.Update(member);
        }
    }

    public async Task<IReadOnlyList<Member>> GetExpiringMembersAsync(int withinDays, CancellationToken cancellationToken = default)
    {
        var cutoff = DateTime.UtcNow.AddDays(withinDays);
        return await _dbSet
            .Include(m => m.Package)
            .Where(m => m.SubscriptionEndDate <= cutoff && m.SubscriptionEndDate >= DateTime.UtcNow)
            .OrderBy(m => m.SubscriptionEndDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Member>> GetMembersWithOutstandingBalanceAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(m => m.Package)
            .Where(m => m.PaidAmount < m.SubscriptionPrice && m.SubscriptionPrice > 0)
            .OrderByDescending(m => m.RemainingAmount)
            .ToListAsync(cancellationToken);
    }
}
