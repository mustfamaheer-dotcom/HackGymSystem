using AutoMapper;
using Gym.Application.Offers.DTOs;
using Gym.Domain.Entities;
using Gym.Domain.Interfaces;
using Gym.Shared.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gym.Application.Offers.Queries.GetOffersByPackage;

public record GetOffersByPackageQuery(Guid PackageId) : IRequest<Result<List<OfferDto>>>;

public class GetOffersByPackageQueryHandler : IRequestHandler<GetOffersByPackageQuery, Result<List<OfferDto>>>
{
    private readonly IRepository<Offer> _repository;
    private readonly IMapper _mapper;

    public GetOffersByPackageQueryHandler(IRepository<Offer> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<Result<List<OfferDto>>> Handle(GetOffersByPackageQuery request, CancellationToken cancellationToken)
    {
        var offers = await _repository.Query()
            .Include(o => o.LinkedPackage)
            .Where(o => o.LinkedPackageId == request.PackageId || o.LinkedPackageId == null)
            .OrderBy(o => o.OfferTitle)
            .ToListAsync(cancellationToken);

        return Result<List<OfferDto>>.Success(_mapper.Map<List<OfferDto>>(offers));
    }
}
