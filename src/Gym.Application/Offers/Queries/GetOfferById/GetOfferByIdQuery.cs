using AutoMapper;
using Gym.Application.Offers.DTOs;
using Gym.Domain.Entities;
using Gym.Domain.Interfaces;
using Gym.Shared.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gym.Application.Offers.Queries.GetOfferById;

public record GetOfferByIdQuery(Guid Id) : IRequest<Result<OfferDto>>;

public class GetOfferByIdQueryHandler : IRequestHandler<GetOfferByIdQuery, Result<OfferDto>>
{
    private readonly IRepository<Offer> _repository;
    private readonly IMapper _mapper;

    public GetOfferByIdQueryHandler(IRepository<Offer> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<Result<OfferDto>> Handle(GetOfferByIdQuery request, CancellationToken cancellationToken)
    {
        var offer = await _repository.Query()
            .Include(o => o.LinkedPackage)
            .FirstOrDefaultAsync(o => o.Id == request.Id, cancellationToken);

        if (offer is null)
            return Result<OfferDto>.Failure("Offer not found");

        return Result<OfferDto>.Success(_mapper.Map<OfferDto>(offer));
    }
}
