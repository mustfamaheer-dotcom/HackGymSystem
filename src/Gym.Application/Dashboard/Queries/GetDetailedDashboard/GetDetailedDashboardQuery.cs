using Gym.Application.Dashboard.DTOs;
using Gym.Domain.Entities;
using Gym.Domain.Interfaces;
using Gym.Shared.Common;
using Gym.Shared.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gym.Application.Dashboard.Queries.GetDetailedDashboard;

public record GetDetailedDashboardQuery : IRequest<Result<DetailedDashboardDto>>;

public class GetDetailedDashboardQueryHandler : IRequestHandler<GetDetailedDashboardQuery, Result<DetailedDashboardDto>>
{
    private readonly IRepository<Member> _memberRepo;
    private readonly IRepository<Membership> _membershipRepo;
    private readonly IRepository<Attendance> _attendanceRepo;
    private readonly IRepository<MembershipPlan> _planRepo;

    public GetDetailedDashboardQueryHandler(
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

    public async Task<Result<DetailedDashboardDto>> Handle(GetDetailedDashboardQuery request, CancellationToken cancellationToken)
    {
        var now = DateTime.UtcNow;
        var today = now.Date;
        var weekStart = today.AddDays(-(int)today.DayOfWeek);
        var monthStart = new DateTime(now.Year, now.Month, 1, 0, 0, 0, DateTimeKind.Utc);

        var memberQuery = _memberRepo.Query();
        var membershipQuery = _membershipRepo.Query();
        var attendanceQuery = _attendanceRepo.Query();

        var totalMembers = await memberQuery.CountAsync(cancellationToken);
        var activeMembers = await memberQuery.CountAsync(m => !m.IsDeleted, cancellationToken);
        var newThisMonth = await memberQuery.CountAsync(m => m.CreatedAt >= monthStart, cancellationToken);
        var maleCount = await memberQuery.CountAsync(m => m.Gender == Gender.Male, cancellationToken);
        var femaleCount = await memberQuery.CountAsync(m => m.Gender == Gender.Female, cancellationToken);

        var totalMemberships = await membershipQuery.CountAsync(cancellationToken);
        var activeMemberships = await membershipQuery.CountAsync(m => m.Status == MembershipStatus.Active, cancellationToken);
        var frozenMemberships = await membershipQuery.CountAsync(m => m.Status == MembershipStatus.Frozen, cancellationToken);
        var expiredMemberships = await membershipQuery.CountAsync(m => m.Status == MembershipStatus.Expired, cancellationToken);
        var cancelledMemberships = await membershipQuery.CountAsync(m => m.Status == MembershipStatus.Cancelled, cancellationToken);

        var expiringThisWeek = await membershipQuery
            .CountAsync(m => m.EndDate <= now.AddDays(7) && m.EndDate > now && m.Status == MembershipStatus.Active, cancellationToken);

        var todayAttendance = await attendanceQuery.CountAsync(a => a.Date == today, cancellationToken);
        var weekAttendance = await attendanceQuery.CountAsync(a => a.Date >= weekStart, cancellationToken);
        var monthAttendance = await attendanceQuery.CountAsync(a => a.Date >= monthStart, cancellationToken);
        var currentlyCheckedIn = await attendanceQuery
            .CountAsync(a => a.Date == today && a.CheckOut == null, cancellationToken);

        var avgDailyThisMonth = Math.Round((double)monthAttendance / Math.Max(now.Day, 1), 1);

        var membershipByPlan = await membershipQuery
            .Include(m => m.Plan)
            .Where(m => m.Status == MembershipStatus.Active)
            .GroupBy(m => new { m.PlanId, m.Plan.Name })
            .Select(g => new PlanDistributionDto
            {
                PlanName = g.Key.Name,
                Count = g.Count()
            })
            .ToListAsync(cancellationToken);

        var totalPlanMembers = membershipByPlan.Sum(p => p.Count);
        foreach (var plan in membershipByPlan)
            plan.Percent = totalPlanMembers > 0 ? Math.Round(plan.Count * 100.0 / totalPlanMembers, 1) : 0;

        var recentActivities = await attendanceQuery
            .Include(a => a.Member)
            .OrderByDescending(a => a.Date)
            .ThenByDescending(a => a.Time)
            .Take(15)
            .Select(a => new RecentActivityDto
            {
                Type = a.IsManual ? "manual" : "checkin",
                Description = $"{a.Member.FullName} - {(a.CheckOut == null ? "Checked in" : "Checked out")}",
                Timestamp = a.Date
            })
            .ToListAsync(cancellationToken);

        var last7Days = Enumerable.Range(0, 7).Select(i => today.AddDays(-6 + i)).ToList();
        var dailyAttendanceData = await attendanceQuery
            .Where(a => a.Date >= last7Days[0])
            .GroupBy(a => a.Date)
            .Select(g => new { Date = g.Key, Count = g.Count() })
            .ToListAsync(cancellationToken);
        var dailyAttendanceTrend = last7Days.Select(d => new DailyStatDto
        {
            Label = d.ToString("ddd"),
            Count = dailyAttendanceData.FirstOrDefault(x => x.Date == d)?.Count ?? 0
        }).ToList();

        var last6Months = Enumerable.Range(0, 6)
            .Select(i => new DateTime(now.Year, now.Month, 1).AddMonths(-i))
            .Reverse()
            .ToList();
        var sixMonthsAgo = last6Months[0];

        var monthlyMembersData = await memberQuery
            .Where(m => m.CreatedAt >= sixMonthsAgo)
            .GroupBy(m => new { m.CreatedAt.Year, m.CreatedAt.Month })
            .Select(g => new { g.Key.Year, g.Key.Month, Count = g.Count() })
            .ToListAsync(cancellationToken);
        var monthlyNewMembersTrend = last6Months.Select(d => new MonthlyStatDto
        {
            Label = d.ToString("MMM"),
            Value = monthlyMembersData.FirstOrDefault(x => x.Year == d.Year && x.Month == d.Month)?.Count ?? 0
        }).ToList();

        var dashboard = new DetailedDashboardDto
        {
            Members = new MembersStatsDto
            {
                TotalMembers = totalMembers,
                ActiveMembers = activeMembers,
                NewThisMonth = newThisMonth,
                MaleCount = maleCount,
                FemaleCount = femaleCount,
                ExpiredSubscriptions = expiredMemberships,
                ExpiringThisWeek = expiringThisWeek
            },
            Memberships = new MembershipsStatsDto
            {
                Total = totalMemberships,
                Active = activeMemberships,
                Frozen = frozenMemberships,
                Expired = expiredMemberships,
                Cancelled = cancelledMemberships
            },
            Attendance = new AttendanceStatsDto
            {
                TodayTotal = todayAttendance,
                ThisWeekTotal = weekAttendance,
                ThisMonthTotal = monthAttendance,
                CurrentlyCheckedIn = currentlyCheckedIn,
                AvgDailyThisMonth = avgDailyThisMonth
            },
            MembershipByPlan = membershipByPlan,
            RecentActivities = recentActivities,
            DailyAttendanceTrend = dailyAttendanceTrend,
            MonthlyNewMembersTrend = monthlyNewMembersTrend
        };

        return Result<DetailedDashboardDto>.Success(dashboard);
    }
}
