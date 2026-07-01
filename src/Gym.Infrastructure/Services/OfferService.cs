using AutoMapper;
using AutoMapper.QueryableExtensions;
using Gym.Application.Common.DTOs;
using Gym.Application.Common.Interfaces;
using Gym.Application.Offers.DTOs;
using Gym.Domain.Entities;
using Gym.Domain.Interfaces;
using Gym.Shared.Common;
using Gym.Shared.Enums;
using Microsoft.EntityFrameworkCore;

namespace Gym.Infrastructure.Services;

public class OfferService : IOfferService
{
    private readonly IRepository<Offer> _offerRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public OfferService(IRepository<Offer> offerRepository, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _offerRepository = offerRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<PaginatedResult<OfferDto>>> GetAllAsync(int page, int pageSize, string? searchTerm, CancellationToken cancellationToken = default)
    {
        IQueryable<Offer> query = _offerRepository.Query().Include(o => o.LinkedPackage);

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            var s = searchTerm.ToLower();
            query = query.Where(o => o.OfferTitle.ToLower().Contains(s));
        }

        query = query.OrderByDescending(o => o.CreatedAt);

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ProjectTo<OfferDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        return Result<PaginatedResult<OfferDto>>.Success(new PaginatedResult<OfferDto>
        {
            Items = items,
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize
        });
    }

    public async Task<Result<OfferDto>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var offer = await _offerRepository.Query()
            .Include(o => o.LinkedPackage)
            .FirstOrDefaultAsync(o => o.Id == id, cancellationToken);

        if (offer is null)
            return Result<OfferDto>.Failure("Offer not found");

        return Result<OfferDto>.Success(_mapper.Map<OfferDto>(offer));
    }

    public async Task<Result<Guid>> CreateAsync(CreateOfferDto dto, CancellationToken cancellationToken = default)
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(dto.OfferTitle))
            errors.Add("Offer title is required");

        if (dto.EndDate <= dto.StartDate)
            errors.Add("End date must be after start date");

        if (dto.OfferType == OfferType.BonusDuration)
        {
            if ((dto.BonusMonths == null || dto.BonusMonths == 0) && (dto.BonusDays == null || dto.BonusDays == 0))
                errors.Add("Bonus months or bonus days must be provided");
            if (dto.OfferPrice.HasValue)
                errors.Add("Offer price must be null for bonus duration offers");
        }

        if (dto.OfferType == OfferType.FixedPrice)
        {
            if (dto.OfferPrice == null || dto.OfferPrice <= 0)
                errors.Add("Offer price is required and must be positive");
            if (dto.BonusMonths.HasValue || dto.BonusDays.HasValue)
                errors.Add("Bonus months/days must be null for fixed price offers");
        }

        if (dto.OfferType == OfferType.ExtraFreeze && (dto.ExtraFreezeDays == null || dto.ExtraFreezeDays <= 0))
            errors.Add("Extra freeze days is required and must be positive");

        if (errors.Count > 0)
            return Result<Guid>.Failure(errors.ToArray());

        var offer = new Offer(
            dto.OfferTitle, dto.OfferType, dto.StartDate, dto.EndDate,
            dto.LinkedPackageId, dto.BonusMonths, dto.BonusDays,
            dto.OfferPrice, dto.ExtraFreezeDays, dto.Description);

        await _offerRepository.AddAsync(offer, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<Guid>.Success(offer.Id, "Offer created successfully");
    }

    public async Task<Result> UpdateAsync(UpdateOfferDto dto, CancellationToken cancellationToken = default)
    {
        var offer = await _offerRepository.GetByIdAsync(dto.Id, cancellationToken);
        if (offer is null)
            return Result.Failure("Offer not found");

        offer.Update(
            dto.OfferTitle, dto.OfferType, dto.StartDate, dto.EndDate,
            dto.LinkedPackageId, dto.BonusMonths, dto.BonusDays,
            dto.OfferPrice, dto.ExtraFreezeDays, dto.Description);

        _offerRepository.Update(offer);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success("Offer updated successfully");
    }

    public async Task<Result> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var offer = await _offerRepository.GetByIdAsync(id, cancellationToken);
        if (offer is null)
            return Result.Failure("Offer not found");

        _offerRepository.Delete(offer);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success("Offer deleted successfully");
    }

    public async Task<Result> ToggleActiveAsync(Guid id, bool isActive, CancellationToken cancellationToken = default)
    {
        var offer = await _offerRepository.GetByIdAsync(id, cancellationToken);
        if (offer is null)
            return Result.Failure("Offer not found");

        offer.ToggleActive(isActive);
        _offerRepository.Update(offer);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(isActive ? "Offer activated" : "Offer deactivated");
    }

    public async Task<Result<List<OfferDto>>> GetActiveOffersAsync(CancellationToken cancellationToken = default)
    {
        var offers = await _offerRepository.Query()
            .Include(o => o.LinkedPackage)
            .Where(o => o.IsActive && o.EndDate >= DateTime.UtcNow)
            .OrderBy(o => o.OfferTitle)
            .ToListAsync(cancellationToken);

        return Result<List<OfferDto>>.Success(_mapper.Map<List<OfferDto>>(offers));
    }

    public async Task<Result<List<OfferDto>>> GetExpiredOffersAsync(CancellationToken cancellationToken = default)
    {
        var offers = await _offerRepository.Query()
            .Include(o => o.LinkedPackage)
            .Where(o => !o.IsActive || o.EndDate < DateTime.UtcNow)
            .OrderByDescending(o => o.EndDate)
            .ToListAsync(cancellationToken);

        return Result<List<OfferDto>>.Success(_mapper.Map<List<OfferDto>>(offers));
    }

    public async Task<Result<List<OfferDto>>> GetOffersByPackageAsync(Guid packageId, CancellationToken cancellationToken = default)
    {
        var offers = await _offerRepository.Query()
            .Include(o => o.LinkedPackage)
            .Where(o => o.LinkedPackageId == packageId || o.LinkedPackageId == null)
            .OrderBy(o => o.OfferTitle)
            .ToListAsync(cancellationToken);

        return Result<List<OfferDto>>.Success(_mapper.Map<List<OfferDto>>(offers));
    }

    public async Task<Result<AppliedOfferDto>> ApplyOfferAsync(Guid offerId, Guid? packageId, decimal? packagePrice,
        int? packageDurationMonths, CancellationToken cancellationToken = default)
    {
        var offer = await _offerRepository.Query()
            .Include(o => o.LinkedPackage)
            .FirstOrDefaultAsync(o => o.Id == offerId, cancellationToken);

        if (offer is null)
            return Result<AppliedOfferDto>.Failure("Offer not found");

        if (!offer.IsActive || offer.EndDate < DateTime.UtcNow)
            return Result<AppliedOfferDto>.Failure("Offer has expired and cannot be applied");

        var result = new AppliedOfferDto
        {
            OfferId = offer.Id,
            OfferTitle = offer.OfferTitle,
            OfferType = offer.OfferType,
            OriginalDurationMonths = packageDurationMonths ?? 0,
            OriginalPrice = packagePrice ?? 0
        };

        if (offer.OfferType == OfferType.BonusDuration)
        {
            result.FinalDurationMonths = (packageDurationMonths ?? 0) + (offer.BonusMonths ?? 0);
            result.BonusDays = offer.BonusDays;
            result.FinalPrice = packagePrice ?? 0;
            result.Description = $"Duration: {result.FinalDurationMonths} months ({(packageDurationMonths ?? 0)} base + {offer.BonusMonths ?? 0} bonus)";
        }
        else if (offer.OfferType == OfferType.FixedPrice)
        {
            result.FinalDurationMonths = packageDurationMonths ?? 0;
            result.FinalPrice = offer.OfferPrice ?? 0;
            result.Description = $"Fixed price: {offer.OfferPrice:N2} EGP (original: {packagePrice:N2} EGP)";
        }
        else if (offer.OfferType == OfferType.ExtraFreeze)
        {
            result.FinalDurationMonths = packageDurationMonths ?? 0;
            result.FinalPrice = packagePrice ?? 0;
            result.ExtraFreezeDays = offer.ExtraFreezeDays;
            result.Description = $"Extra freeze days: +{offer.ExtraFreezeDays} days";
        }
        else if (offer.OfferType == OfferType.FreeRegistration)
        {
            result.FinalDurationMonths = packageDurationMonths ?? 0;
            result.FinalPrice = 0;
            result.Description = "Free registration - price set to 0";
        }
        else
        {
            result.FinalDurationMonths = packageDurationMonths ?? 0;
            result.FinalPrice = packagePrice ?? 0;
            result.Description = offer.Description ?? "Custom offer applied";
        }

        return Result<AppliedOfferDto>.Success(result);
    }

    public async Task<int> ExpireOffersAsync(CancellationToken cancellationToken = default)
    {
        var expired = await _offerRepository.FindAsync(o => o.IsActive && o.EndDate < DateTime.UtcNow, cancellationToken);
        var count = 0;

        foreach (var offer in expired)
        {
            offer.ToggleActive(false);
            _offerRepository.Update(offer);
            count++;
        }

        if (count > 0)
            await _unitOfWork.SaveChangesAsync(cancellationToken);

        return count;
    }
}
