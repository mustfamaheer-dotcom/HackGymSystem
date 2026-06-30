using Gym.Domain.Entities;

namespace Gym.Domain.Interfaces;

public interface IMemberRepository : IRepository<Member>
{
    Task<Member?> GetByIdAsync(Guid id, bool includeDeleted, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Member>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default);
    Task SoftDeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task RestoreAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Member>> GetExpiringMembersAsync(int withinDays, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Member>> GetMembersWithOutstandingBalanceAsync(CancellationToken cancellationToken = default);
}
