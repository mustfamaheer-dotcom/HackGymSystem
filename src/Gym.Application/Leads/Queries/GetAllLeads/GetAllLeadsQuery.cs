using AutoMapper;
using AutoMapper.QueryableExtensions;
using Gym.Application.Leads.DTOs;
using Gym.Domain.Entities;
using Gym.Domain.Interfaces;
using Gym.Shared.Common;
using Gym.Application.Common.DTOs;
using Gym.Shared.Common;
using Gym.Shared.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gym.Application.Leads.Queries.GetAllLeads;

public record GetAllLeadsQuery(string? SearchTerm, LeadStatus? StatusFilter, int Page = 1, int PageSize = 20) : IRequest<Result<PaginatedResult<LeadDto>>>;

public class GetAllLeadsQueryHandler : IRequestHandler<GetAllLeadsQuery, Result<PaginatedResult<LeadDto>>>
{
    private readonly IRepository<Lead> _repository;
    private readonly IMapper _mapper;

    public GetAllLeadsQueryHandler(IRepository<Lead> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<Result<PaginatedResult<LeadDto>>> Handle(GetAllLeadsQuery request, CancellationToken cancellationToken)
    {
        var query = _repository.Query()
            .Include(l => l.InterestedPackage)
            .IgnoreQueryFilters()
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            var s = request.SearchTerm.ToLower();
            query = query.Where(l => l.Name.ToLower().Contains(s) || l.Phone.Contains(s));
        }

        if (request.StatusFilter.HasValue)
            query = query.Where(l => l.Status == request.StatusFilter.Value);

        query = query.OrderByDescending(l => l.CreatedAt);

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ProjectTo<LeadDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        return Result<PaginatedResult<LeadDto>>.Success(new PaginatedResult<LeadDto>
        {
            Items = items,
            TotalCount = totalCount,
            Page = request.Page,
            PageSize = request.PageSize
        });
    }
}