using AutoMapper;
using AutoMapper.QueryableExtensions;
using Gym.Application.Common.DTOs;
using Gym.Application.DailySessions.DTOs;
using Gym.Domain.Entities;
using Gym.Domain.Interfaces;
using Gym.Shared.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gym.Application.DailySessions.Queries.GetAllDailySessions;

public record GetAllDailySessionsQuery(
    string? SearchTerm,
    DateTime? DateFrom,
    DateTime? DateTo,
    int Page = 1,
    int PageSize = 20) : IRequest<Result<PaginatedResult<DailySessionDto>>>;

public class GetAllDailySessionsQueryHandler : IRequestHandler<GetAllDailySessionsQuery, Result<PaginatedResult<DailySessionDto>>>
{
    private readonly IRepository<DailySession> _repository;
    private readonly IMapper _mapper;

    public GetAllDailySessionsQueryHandler(IRepository<DailySession> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<Result<PaginatedResult<DailySessionDto>>> Handle(GetAllDailySessionsQuery request, CancellationToken cancellationToken)
    {
        var query = _repository.Query().AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            var s = request.SearchTerm.ToLower();
            query = query.Where(d => d.Name.ToLower().Contains(s) || d.Phone.Contains(s));
        }

        if (request.DateFrom.HasValue)
            query = query.Where(d => d.VisitDate >= request.DateFrom.Value);

        if (request.DateTo.HasValue)
            query = query.Where(d => d.VisitDate <= request.DateTo.Value);

        query = query.OrderByDescending(d => d.VisitDate).ThenByDescending(d => d.CreatedAt);

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ProjectTo<DailySessionDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        return Result<PaginatedResult<DailySessionDto>>.Success(new PaginatedResult<DailySessionDto>
        {
            Items = items,
            TotalCount = totalCount,
            Page = request.Page,
            PageSize = request.PageSize
        });
    }
}
