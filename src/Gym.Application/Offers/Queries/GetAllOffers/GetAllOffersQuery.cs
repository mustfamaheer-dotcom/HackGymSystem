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
        var query = _repository.Query();

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            query = query.Where(o => o.Title.Contains(request.SearchTerm));

        query = (request.SortBy?.ToLower()) switch
        {
            "title" => request.SortDescending ? query.OrderByDescending(o => o.Title) : query.OrderBy(o => o.Title),
            "startdate" => request.SortDescending ? query.OrderByDescending(o => o.StartDate) : query.OrderBy(o => o.StartDate),
            "enddate" => request.SortDescending ? query.OrderByDescending(o => o.EndDate) : query.OrderBy(o => o.EndDate),
            "discountvalue" => request.SortDescending ? query.OrderByDescending(o => o.DiscountValue) : query.OrderBy(o => o.DiscountValue),
            "createdat" => request.SortDescending ? query.OrderByDescending(o => o.CreatedAt) : query.OrderBy(o => o.CreatedAt),
            _ => query.OrderBy(o => o.Title)
        };

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ProjectTo<OfferDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        var result = new PaginatedResult<OfferDto>
        {
            Items = items,
            TotalCount = totalCount,
            Page = request.Page,
            PageSize = request.PageSize
        };

        return Result<PaginatedResult<OfferDto>>.Success(result);
    }
}
