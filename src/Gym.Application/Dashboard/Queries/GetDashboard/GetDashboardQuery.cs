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
    private readonly IRepository<Payment> _paymentRepo;
    private readonly IRepository<MembershipPlan> _planRepo;

    public GetDashboardQueryHandler(
        IRepository<Member> memberRepo,
        IRepository<Membership> membershipRepo,
        IRepository<Attendance> attendanceRepo,
        IRepository<Payment> paymentRepo,
        IRepository<MembershipPlan> planRepo)
    {
        _memberRepo = memberRepo;
        _membershipRepo = membershipRepo;
        _attendanceRepo = attendanceRepo;
        _paymentRepo = paymentRepo;
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

        var todayRevenue = await _paymentRepo.Query()
            .Where(p => p.PaymentDate.Date == today)
            .SumAsync(p => (decimal?)p.TotalAmount, cancellationToken) ?? 0;

        var monthlyRevenue = await _paymentRepo.Query()
            .Where(p => p.PaymentDate.Month == now.Month && p.PaymentDate.Year == now.Year)
            .SumAsync(p => (decimal?)p.TotalAmount, cancellationToken) ?? 0;

        var expiringMemberships = await _membershipRepo.Query()
            .CountAsync(m => m.EndDate <= now.AddDays(7) && m.EndDate > now && m.Status == MembershipStatus.Active, cancellationToken);

        var recentActivities = await _attendanceRepo.Query()
            .Include(a => a.Member)
            .OrderByDescending(a => a.Date)
            .Take(10)
            .Select(a => new RecentActivityDto
            {
                Type = "checkin",
                Description = a.Member.FullName,
                Timestamp = a.Date
            })
            .ToListAsync(cancellationToken);

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
            TodayRevenue = todayRevenue,
            MonthlyRevenue = monthlyRevenue,
            ExpiringMemberships = expiringMemberships,
            OverduePayments = 0,
            RecentActivities = recentActivities,
            MembershipByPlan = membershipByPlan
        };

        return Result<DashboardDto>.Success(dashboard);
    }
}
