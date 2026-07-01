using Gym.Application.Common.DTOs;
using Gym.Application.Offers.DTOs;
using Gym.Shared.Common;

namespace Gym.Application.Common.Interfaces;

public interface IOfferService
{
    Task<Result<PaginatedResult<OfferDto>>> GetAllAsync(int page, int pageSize, string? searchTerm, CancellationToken cancellationToken = default);
    Task<Result<OfferDto>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Result<Guid>> CreateAsync(CreateOfferDto dto, CancellationToken cancellationToken = default);
    Task<Result> UpdateAsync(UpdateOfferDto dto, CancellationToken cancellationToken = default);
    Task<Result> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Result> ToggleActiveAsync(Guid id, bool isActive, CancellationToken cancellationToken = default);
    Task<Result<List<OfferDto>>> GetActiveOffersAsync(CancellationToken cancellationToken = default);
    Task<Result<List<OfferDto>>> GetExpiredOffersAsync(CancellationToken cancellationToken = default);
    Task<Result<List<OfferDto>>> GetOffersByPackageAsync(Guid packageId, CancellationToken cancellationToken = default);
    Task<Result<AppliedOfferDto>> ApplyOfferAsync(Guid offerId, Guid? packageId, decimal? packagePrice, int? packageDurationMonths, CancellationToken cancellationToken = default);
    Task<int> ExpireOffersAsync(CancellationToken cancellationToken = default);
}
