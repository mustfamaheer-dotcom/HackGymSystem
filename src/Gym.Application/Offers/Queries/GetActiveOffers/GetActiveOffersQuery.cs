using AutoMapper;
using Gym.Application.Offers.DTOs;
using Gym.Domain.Entities;
using Gym.Domain.Interfaces;
using Gym.Shared.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gym.Application.Offers.Queries.GetActiveOffers;

public record GetActiveOffersQuery : IRequest<Result<List<OfferDto>>>;

public class GetActiveOffersQueryHandler : IRequestHandler<GetActiveOffersQuery, Result<List<OfferDto>>>
{
    private readonly IRepository<Offer> _repository;
    private readonly IMapper _mapper;

    public GetActiveOffersQueryHandler(IRepository<Offer> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<Result<List<OfferDto>>> Handle(GetActiveOffersQuery request, CancellationToken cancellationToken)
    {
        var offers = await _repository.FindAsync(o => o.IsActive, cancellationToken);
        var dtos = _mapper.Map<List<OfferDto>>(offers.ToList());
        return Result<List<OfferDto>>.Success(dtos);
    }
}
