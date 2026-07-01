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
    private readonly IRepository<Membership> _membershipRepo;
    private readonly IRepository<Attendance> _attendanceRepo;
    private readonly IRepository<MembershipPlan> _planRepo;

    public GetDashboardQueryHandler(
        IRepository<Member> memberRepo,
        IRepository<Membership> membershipRepo,
        IRepository<Attendance> attendanceRepo,
        IRepository<MembershipPlan> planRepo)
    {
        _memberRepo = memberRepo;
        _membershipRepo = membershipRepo;
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

        var activeMemberships = await _membershipRepo.Query()
            .CountAsync(m => m.Status == MembershipStatus.Active, cancellationToken);

        var todayCheckIns = await _attendanceRepo.Query()
            .CountAsync(a => a.Date == today, cancellationToken);

        var expiringMemberships = await _membershipRepo.Query()
            .CountAsync(m => m.EndDate <= now.AddDays(7) && m.EndDate > now && m.Status == MembershipStatus.Active, cancellationToken);

        var membershipByPlan = await _membershipRepo.Query()
            .Include(m => m.Plan)
            .GroupBy(m => new { m.PlanId, m.Plan.Name })
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
            ActiveMemberships = activeMemberships,
            TodayCheckIns = todayCheckIns,
            ExpiringMemberships = expiringMemberships,
            MembershipByPlan = membershipByPlan
        };

        return Result<DashboardDto>.Success(dashboard);
    }
}
