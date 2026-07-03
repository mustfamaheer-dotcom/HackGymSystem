using Gym.Domain.Entities;
using Gym.Domain.Interfaces;
using Gym.Shared.Common;
using Gym.Shared.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gym.Application.Leads.Queries.GetLeadStats;

public record GetLeadStatsQuery : IRequest<Result<LeadStatsDto>>;

public class LeadStatsDto
{
    public int TotalLeads { get; set; }
    public int NewThisWeek { get; set; }
    public int FollowUpsDueToday { get; set; }
    public int ConvertedCount { get; set; }
    public double ConversionRate { get; set; }
}

public class GetLeadStatsQueryHandler : IRequestHandler<GetLeadStatsQuery, Result<LeadStatsDto>>
{
    private readonly IRepository<Lead> _repository;

    public GetLeadStatsQueryHandler(IRepository<Lead> repository)
    {
        _repository = repository;
    }

    public async Task<Result<LeadStatsDto>> Handle(GetLeadStatsQuery request, CancellationToken cancellationToken)
    {
        var now = DateTime.UtcNow;
        var weekStart = now.AddDays(-(int)now.DayOfWeek);
        var todayStart = now.Date;

        var baseQuery = _repository.Query().IgnoreQueryFilters();

        var total = await baseQuery.CountAsync(cancellationToken);
        var newThisWeek = await baseQuery.CountAsync(l => l.CreatedAt >= weekStart, cancellationToken);
        var followUpsDue = await baseQuery.CountAsync(l => l.NextFollowUpDate != null && l.NextFollowUpDate.Value <= now, cancellationToken);
        var converted = await baseQuery.CountAsync(l => l.Status == LeadStatus.Converted, cancellationToken);
        var conversionRate = total > 0 ? Math.Round((double)converted / total * 100, 1) : 0;

        return Result<LeadStatsDto>.Success(new LeadStatsDto
        {
            TotalLeads = total,
            NewThisWeek = newThisWeek,
            FollowUpsDueToday = followUpsDue,
            ConvertedCount = converted,
            ConversionRate = conversionRate
        });
    }
}
