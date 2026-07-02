using Gym.Application.Dashboard.DTOs;
using Gym.Domain.Entities;
using Gym.Domain.Interfaces;
using Gym.Shared.Common;
using Gym.Shared.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gym.Application.Dashboard.Queries.GetDashboard;

public record GetDashboardQuery : IRequest<Result<DashboardDto>>;

public class GetDashboardQueryHandler : IRequestHandler<GetDashboardQuery, Result<DashboardDto>>
{
    private readonly IRepository<Member> _memberRepo;
    private readonly IRepository<Domain.Entities.Subscription> _subscriptionRepo;
    private readonly IRepository<Attendance> _attendanceRepo;
    private readonly IRepository<MembershipPlan> _planRepo;

    public GetDashboardQueryHandler(
        IRepository<Member> memberRepo,
        IRepository<Domain.Entities.Subscription> subscriptionRepo,
        IRepository<Attendance> attendanceRepo,
        IRepository<MembershipPlan> planRepo)
    {
        _memberRepo = memberRepo;
        _subscriptionRepo = subscriptionRepo;
        _attendanceRepo = attendanceRepo;
        _planRepo = planRepo;
    }

    public async Task<Result<DashboardDto>> Handle(GetDashboardQuery request, CancellationToken cancellationToken)
    {
        var now = DateTime.UtcNow;
        var today = now.Date;

        var totalMembers = await _memberRepo.Query().CountAsync(cancellationToken);

        var activeMembers = await _memberRepo.Query()
            .CountAsync(m => !m.IsDeleted, cancellationToken);

        var activeSubs = await _subscriptionRepo.Query()
            .CountAsync(s => s.Status == SubscriptionStatus.Active, cancellationToken);

        var todayCheckIns = await _attendanceRepo.Query()
            .CountAsync(a => a.Date == today, cancellationToken);

        var expiringSubs = await _subscriptionRepo.Query()
            .CountAsync(s => s.ExpirationDate <= now.AddDays(7) && s.ExpirationDate > now && s.Status == SubscriptionStatus.Active, cancellationToken);

        var subsByPlan = await _subscriptionRepo.Query()
            .Include(s => s.Plan)
            .GroupBy(s => new { s.PlanId, s.Plan.Name })
            .Select(g => new MembershipStatsDto
            {
                PlanName = g.Key.Name,
                Count = g.Count()
            })
            .ToListAsync(cancellationToken);

        var dashboard = new DashboardDto
        {
            TotalMembers = totalMembers,
            ActiveMembers = activeMembers,
            ActiveMemberships = activeSubs,
            TodayCheckIns = todayCheckIns,
            ExpiringMemberships = expiringSubs,
            MembershipByPlan = subsByPlan
        };

        return Result<DashboardDto>.Success(dashboard);
    }
}
