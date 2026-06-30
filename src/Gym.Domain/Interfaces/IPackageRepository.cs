using Gym.Domain.Entities;

namespace Gym.Domain.Interfaces;

public interface IPackageRepository : IRepository<Package>
{
    Task<IReadOnlyList<Package>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default);
    Task ActivateAsync(Guid id, CancellationToken cancellationToken = default);
    Task DeactivateAsync(Guid id, CancellationToken cancellationToken = default);
}
