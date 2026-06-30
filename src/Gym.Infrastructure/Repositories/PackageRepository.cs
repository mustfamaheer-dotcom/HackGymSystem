using Gym.Domain.Entities;
using Gym.Domain.Interfaces;
using Gym.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Gym.Infrastructure.Repositories;

public class PackageRepository : Repository<Package>, IPackageRepository
{
    public PackageRepository(GymDbContext context) : base(context) { }

    public async Task<IReadOnlyList<Package>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        var search = searchTerm.ToLower();
        return await _dbSet
            .Where(p => p.PackageName.ToLower().Contains(search))
            .OrderBy(p => p.PackageName)
            .ToListAsync(cancellationToken);
    }

    public async Task ActivateAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var package = await _dbSet.FindAsync([id], cancellationToken);
        if (package is not null)
        {
            package.ToggleActive(true);
            _dbSet.Update(package);
        }
    }

    public async Task DeactivateAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var package = await _dbSet.FindAsync([id], cancellationToken);
        if (package is not null)
        {
            package.ToggleActive(false);
            _dbSet.Update(package);
        }
    }
}
