using AutoMapper;
using Gym.Application.Offers.DTOs;
using Gym.Domain.Entities;
using Gym.Domain.Interfaces;
using Gym.Shared.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gym.Application.Offers.Queries.GetExpiredOffers;

public record GetExpiredOffersQuery : IRequest<Result<List<OfferDto>>>;

public class GetExpiredOffersQueryHandler : IRequestHandler<GetExpiredOffersQuery, Result<List<OfferDto>>>
{
    private readonly IRepository<Offer> _repository;
    private readonly IMapper _mapper;

    public GetExpiredOffersQueryHandler(IRepository<Offer> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<Result<List<OfferDto>>> Handle(GetExpiredOffersQuery request, CancellationToken cancellationToken)
    {
        var offers = await _repository.Query()
            .Include(o => o.LinkedPackage)
            .Where(o => !o.IsActive || o.EndDate < DateTime.UtcNow)
            .OrderByDescending(o => o.EndDate)
            .ToListAsync(cancellationToken);

        return Result<List<OfferDto>>.Success(_mapper.Map<List<OfferDto>>(offers));
    }
}
