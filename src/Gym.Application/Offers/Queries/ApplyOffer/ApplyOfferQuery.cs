using Gym.Application.Offers.DTOs;
using Gym.Domain.Entities;
using Gym.Domain.Interfaces;
using Gym.Shared.Common;
using Gym.Shared.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gym.Application.Offers.Queries.ApplyOffer;

public record ApplyOfferQuery(Guid OfferId, Guid? PackageId = null, decimal? PackagePrice = null, int? PackageDurationMonths = null) : IRequest<Result<AppliedOfferDto>>;

public class ApplyOfferQueryHandler : IRequestHandler<ApplyOfferQuery, Result<AppliedOfferDto>>
{
    private readonly IRepository<Offer> _repository;

    public ApplyOfferQueryHandler(IRepository<Offer> repository)
    {
        _repository = repository;
    }

    public async Task<Result<AppliedOfferDto>> Handle(ApplyOfferQuery request, CancellationToken cancellationToken)
    {
        var offer = await _repository.Query()
            .Include(o => o.LinkedPackage)
            .FirstOrDefaultAsync(o => o.Id == request.OfferId, cancellationToken);

        if (offer is null)
            return Result<AppliedOfferDto>.Failure("Offer not found");

        if (!offer.IsActive || offer.EndDate < DateTime.UtcNow)
            return Result<AppliedOfferDto>.Failure("Offer has expired and cannot be applied");

        var result = new AppliedOfferDto
        {
            OfferId = offer.Id,
            OfferTitle = offer.OfferTitle,
            OfferType = offer.OfferType,
            OriginalDurationMonths = request.PackageDurationMonths ?? 0,
            OriginalPrice = request.PackagePrice ?? 0
        };

        if (offer.OfferType == OfferType.BonusDuration)
        {
            result.FinalDurationMonths = (request.PackageDurationMonths ?? 0) + (offer.BonusMonths ?? 0);
            result.BonusDays = offer.BonusDays;
            result.FinalPrice = request.PackagePrice ?? 0;
            result.Description = $"Duration: {result.FinalDurationMonths} months ({(request.PackageDurationMonths ?? 0)} base + {offer.BonusMonths ?? 0} bonus)";
        }
        else if (offer.OfferType == OfferType.FixedPrice)
        {
            result.FinalDurationMonths = request.PackageDurationMonths ?? 0;
            result.FinalPrice = offer.OfferPrice ?? 0;
            result.Description = $"Fixed price: {offer.OfferPrice:N2} EGP (original: {request.PackagePrice:N2} EGP)";
        }
        else if (offer.OfferType == OfferType.ExtraFreeze)
        {
            result.FinalDurationMonths = request.PackageDurationMonths ?? 0;
            result.FinalPrice = request.PackagePrice ?? 0;
            result.ExtraFreezeDays = offer.ExtraFreezeDays;
            result.Description = $"Extra freeze days: +{offer.ExtraFreezeDays} days";
        }
        else if (offer.OfferType == OfferType.FreeRegistration)
        {
            result.FinalDurationMonths = request.PackageDurationMonths ?? 0;
            result.FinalPrice = 0;
            result.Description = "Free registration - price set to 0";
        }
        else
        {
            result.FinalDurationMonths = request.PackageDurationMonths ?? 0;
            result.FinalPrice = request.PackagePrice ?? 0;
            result.Description = offer.Description ?? "Custom offer applied";
        }

        return Result<AppliedOfferDto>.Success(result);
    }
}
