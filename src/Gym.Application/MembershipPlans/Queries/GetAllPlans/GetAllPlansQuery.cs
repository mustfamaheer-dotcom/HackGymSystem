using AutoMapper;
using AutoMapper.QueryableExtensions;
using Gym.Application.Common.DTOs;
using Gym.Application.MembershipPlans.DTOs;
using Gym.Domain.Entities;
using Gym.Domain.Interfaces;
using Gym.Shared.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gym.Application.MembershipPlans.Queries.GetAllPlans;

public record GetAllPlansQuery : PaginationRequest, IRequest<Result<PaginatedResult<PlanDto>>>;

public class GetAllPlansQueryHandler : IRequestHandler<GetAllPlansQuery, Result<PaginatedResult<PlanDto>>>
{
    private readonly IRepository<MembershipPlan> _repository;
    private readonly IMapper _mapper;

    public GetAllPlansQueryHandler(IRepository<MembershipPlan> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<Result<PaginatedResult<PlanDto>>> Handle(GetAllPlansQuery request, CancellationToken cancellationToken)
    {
        var query = _repository.Query();

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            query = query.Where(p => p.Name.Contains(request.SearchTerm));

        query = (request.SortBy?.ToLower()) switch
        {
            "name" => request.SortDescending ? query.OrderByDescending(p => p.Name) : query.OrderBy(p => p.Name),
            "price" => request.SortDescending ? query.OrderByDescending(p => p.Price) : query.OrderBy(p => p.Price),
            "createdat" => request.SortDescending ? query.OrderByDescending(p => p.CreatedAt) : query.OrderBy(p => p.CreatedAt),
            _ => query.OrderBy(p => p.Name)
        };

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ProjectTo<PlanDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        var result = new PaginatedResult<PlanDto>
        {
            Items = items,
            TotalCount = totalCount,
            Page = request.Page,
            PageSize = request.PageSize
        };

        return Result<PaginatedResult<PlanDto>>.Success(result);
    }
}
