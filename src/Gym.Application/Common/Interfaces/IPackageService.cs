using Gym.Application.Packages.DTOs;
using Gym.Shared.Common;

namespace Gym.Application.Common.Interfaces;

public interface IPackageService
{
    Task<Result<List<PackageDto>>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Result<List<PackageDto>>> GetActiveAsync(CancellationToken cancellationToken = default);
    Task<Result<PackageDto>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Result<List<PackageDto>>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default);
    Task<Result<Guid>> CreateAsync(string packageName, int durationMonths, decimal price, int? freePeriodMonths, int? maxFreezeDays, CancellationToken cancellationToken = default);
    Task<Result> UpdateAsync(Guid id, string packageName, int durationMonths, decimal price, int? freePeriodMonths, int? maxFreezeDays, CancellationToken cancellationToken = default);
    Task<Result> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Result> ActivateAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Result> DeactivateAsync(Guid id, CancellationToken cancellationToken = default);
}
