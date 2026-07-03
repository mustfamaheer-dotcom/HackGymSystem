using AutoMapper;
using AutoMapper.QueryableExtensions;
using Gym.Application.Common.DTOs;
using Gym.Application.Leads.DTOs;
using Gym.Domain.Entities;
using Gym.Domain.Interfaces;
using Gym.Shared.Common;
using Gym.Shared.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gym.Application.Leads.Queries.GetAllLeads;

public record GetAllLeadsQuery(
    string? SearchTerm,
    LeadStatus? StatusFilter,
    Gender? GenderFilter,
    LeadSource? SourceFilter,
    Guid? PackageFilter,
    DateTime? DateFrom,
    DateTime? DateTo,
    DateTime? NextFollowUpFrom,
    DateTime? NextFollowUpTo,
    bool? HasFollowUp,
    string? SortBy,
    bool SortDescending,
    int Page = 1,
    int PageSize = 20) : IRequest<Result<PaginatedResult<LeadDto>>>;

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
            query = query.Where(l => l.Name.ToLower().Contains(s) || l.Phone.Contains(s) || (l.Email != null && l.Email.ToLower().Contains(s)) || (l.Notes != null && l.Notes.ToLower().Contains(s)));
        }

        if (request.StatusFilter.HasValue)
            query = query.Where(l => l.Status == request.StatusFilter.Value);

        if (request.GenderFilter.HasValue)
            query = query.Where(l => l.Gender == request.GenderFilter.Value);

        if (request.SourceFilter.HasValue)
            query = query.Where(l => l.Source == request.SourceFilter.Value);

        if (request.PackageFilter.HasValue)
            query = query.Where(l => l.InterestedPackageId == request.PackageFilter.Value);

        if (request.DateFrom.HasValue)
            query = query.Where(l => l.CreatedAt >= request.DateFrom.Value);

        if (request.DateTo.HasValue)
            query = query.Where(l => l.CreatedAt <= request.DateTo.Value);

        if (request.NextFollowUpFrom.HasValue)
            query = query.Where(l => l.NextFollowUpDate >= request.NextFollowUpFrom.Value);

        if (request.NextFollowUpTo.HasValue)
            query = query.Where(l => l.NextFollowUpDate <= request.NextFollowUpTo.Value);

        if (request.HasFollowUp.HasValue)
        {
            if (request.HasFollowUp.Value)
                query = query.Where(l => l.FollowUps.Any());
            else
                query = query.Where(l => !l.FollowUps.Any());
        }

        query = (request.SortBy?.ToLower()) switch
        {
            "name" => request.SortDescending
                ? query.OrderByDescending(l => l.Name)
                : query.OrderBy(l => l.Name),
            "phone" => request.SortDescending
                ? query.OrderByDescending(l => l.Phone)
                : query.OrderBy(l => l.Phone),
            "status" => request.SortDescending
                ? query.OrderByDescending(l => l.Status)
                : query.OrderBy(l => l.Status),
            "source" => request.SortDescending
                ? query.OrderByDescending(l => l.Source)
                : query.OrderBy(l => l.Source),
            "gender" => request.SortDescending
                ? query.OrderByDescending(l => l.Gender)
                : query.OrderBy(l => l.Gender),
            "nextfollowupdate" => request.SortDescending
                ? query.OrderByDescending(l => l.NextFollowUpDate)
                : query.OrderBy(l => l.NextFollowUpDate),
            "createdat" => request.SortDescending
                ? query.OrderByDescending(l => l.CreatedAt)
                : query.OrderBy(l => l.CreatedAt),
            _ => query.OrderByDescending(l => l.CreatedAt)
        };

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
