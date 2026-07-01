using AutoMapper;
using AutoMapper.QueryableExtensions;
using Gym.Application.Common.DTOs;
using Gym.Application.Offers.DTOs;
using Gym.Domain.Entities;
using Gym.Domain.Interfaces;
using Gym.Shared.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gym.Application.Offers.Queries.GetAllOffers;

public record GetAllOffersQuery : PaginationRequest, IRequest<Result<PaginatedResult<OfferDto>>>;

public class GetAllOffersQueryHandler : IRequestHandler<GetAllOffersQuery, Result<PaginatedResult<OfferDto>>>
{
    private readonly IRepository<Offer> _repository;
    private readonly IMapper _mapper;

    public GetAllOffersQueryHandler(IRepository<Offer> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<Result<PaginatedResult<OfferDto>>> Handle(GetAllOffersQuery request, CancellationToken cancellationToken)
    {
        IQueryable<Offer> query = _repository.Query()
            .Include(o => o.LinkedPackage);

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            var s = request.SearchTerm.ToLower();
            query = query.Where(o => o.OfferTitle.ToLower().Contains(s));
        }

        query = (request.SortBy?.ToLower()) switch
        {
            "offertitle" => request.SortDescending ? query.OrderByDescending(o => o.OfferTitle) : query.OrderBy(o => o.OfferTitle),
            "startdate" => request.SortDescending ? query.OrderByDescending(o => o.StartDate) : query.OrderBy(o => o.StartDate),
            "enddate" => request.SortDescending ? query.OrderByDescending(o => o.EndDate) : query.OrderBy(o => o.EndDate),
            "createdat" => request.SortDescending ? query.OrderByDescending(o => o.CreatedAt) : query.OrderBy(o => o.CreatedAt),
            _ => query.OrderByDescending(o => o.CreatedAt)
        };

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ProjectTo<OfferDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        return Result<PaginatedResult<OfferDto>>.Success(new PaginatedResult<OfferDto>
        {
            Items = items,
            TotalCount = totalCount,
            Page = request.Page,
            PageSize = request.PageSize
        });
    }
}
