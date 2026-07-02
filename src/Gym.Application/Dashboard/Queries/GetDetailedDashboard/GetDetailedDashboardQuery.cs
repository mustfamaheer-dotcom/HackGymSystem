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
    private readonly IRepository<Domain.Entities.Subscription> _subscriptionRepo;
    private readonly IRepository<SubscriptionPayment> _paymentRepo;
    private readonly IRepository<SubscriptionFreezeHistory> _freezeRepo;
    private readonly IRepository<Offer> _offerRepo;

    public GetDetailedDashboardQueryHandler(
        IRepository<Member> memberRepo,
        IRepository<Membership> membershipRepo,
        IRepository<Attendance> attendanceRepo,
        IRepository<MembershipPlan> planRepo,
        IRepository<Domain.Entities.Subscription> subscriptionRepo,
        IRepository<SubscriptionPayment> paymentRepo,
        IRepository<SubscriptionFreezeHistory> freezeRepo,
        IRepository<Offer> offerRepo)
    {
        _memberRepo = memberRepo;
        _membershipRepo = membershipRepo;
        _attendanceRepo = attendanceRepo;
        _planRepo = planRepo;
        _subscriptionRepo = subscriptionRepo;
        _paymentRepo = paymentRepo;
        _freezeRepo = freezeRepo;
        _offerRepo = offerRepo;
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
        var subscriptionQuery = _subscriptionRepo.Query();
        var paymentQuery = _paymentRepo.Query();
        var freezeQuery = _freezeRepo.Query();

        // --- Existing Stats (Members, Memberships, Attendance) ---
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

        // --- Subscription Stats ---
        var subWithPlan = subscriptionQuery
            .Include(s => s.Plan)
            .Include(s => s.Offer);

        var totalSubs = await subWithPlan.CountAsync(cancellationToken);
        var activeSubs = await subWithPlan.CountAsync(s => s.Status == SubscriptionStatus.Active, cancellationToken);
        var frozenSubs = await subWithPlan.CountAsync(s => s.Status == SubscriptionStatus.Frozen, cancellationToken);
        var expiredSubs = await subWithPlan.CountAsync(s => s.Status == SubscriptionStatus.Expired, cancellationToken);
        var renewedSubs = await subWithPlan.CountAsync(s => s.Status == SubscriptionStatus.Renewed, cancellationToken);

        var totalRevenue = await paymentQuery.SumAsync(p => (decimal?)p.Amount, cancellationToken) ?? 0;
        var revenueThisMonth = await paymentQuery
            .Where(p => p.CreatedAt >= monthStart)
            .SumAsync(p => (decimal?)p.Amount, cancellationToken) ?? 0;
        var revenueThisWeek = await paymentQuery
            .Where(p => p.CreatedAt >= weekStart)
            .SumAsync(p => (decimal?)p.Amount, cancellationToken) ?? 0;
        var todayRevenue = await paymentQuery
            .Where(p => p.CreatedAt >= today)
            .SumAsync(p => (decimal?)p.Amount, cancellationToken) ?? 0;

        var totalOutstanding = await subWithPlan
            .SumAsync(s => (decimal?)s.RemainingBalance, cancellationToken) ?? 0;

        var avgSubValue = totalSubs > 0
            ? Math.Round(await subWithPlan.SumAsync(s => (decimal?)s.TotalSubscriptionValue, cancellationToken) ?? 0 / totalSubs, 2)
            : 0;

        var expiring7Days = await subWithPlan
            .CountAsync(s => s.ExpirationDate <= now.AddDays(7) && s.ExpirationDate > now
                && s.Status == SubscriptionStatus.Active, cancellationToken);
        var expiring30Days = await subWithPlan
            .CountAsync(s => s.ExpirationDate <= now.AddDays(30) && s.ExpirationDate > now
                && s.Status == SubscriptionStatus.Active, cancellationToken);
        var subsWithOffers = await subWithPlan.CountAsync(s => s.OfferId != null, cancellationToken);
        var freezesThisMonth = await freezeQuery
            .CountAsync(f => f.CreatedAt >= monthStart, cancellationToken);

        // Revenue by Plan
        var revenueByPlan = await subWithPlan
            .GroupBy(s => new { s.PlanId, s.Plan.Name })
            .Select(g => new SubscriptionRevenueByPlanDto
            {
                PlanName = g.Key.Name,
                Count = g.Count(),
                TotalPaid = g.Sum(s => s.AmountPaid),
                TotalOutstanding = g.Sum(s => s.RemainingBalance)
            })
            .ToListAsync(cancellationToken);

        var totalPaidAllPlans = revenueByPlan.Sum(r => r.TotalPaid);
        foreach (var r in revenueByPlan)
            r.Percent = totalPaidAllPlans > 0 ? Math.Round((double)(r.TotalPaid / totalPaidAllPlans) * 100, 1) : 0;

        // Daily revenue trend (last 7 days)
        var dailyPaymentData = await paymentQuery
            .Where(p => p.CreatedAt >= last7Days[0])
            .GroupBy(p => new { Date = p.CreatedAt.Date })
            .Select(g => new { g.Key.Date, Revenue = g.Sum(p => p.Amount), Count = g.Count() })
            .ToListAsync(cancellationToken);
        var dailySubscriptionRevenue = last7Days.Select(d => new SubscriptionDailyRevenueDto
        {
            Label = d.ToString("ddd"),
            Revenue = dailyPaymentData.FirstOrDefault(x => x.Date == d)?.Revenue ?? 0,
            SubscriptionCount = dailyPaymentData.FirstOrDefault(x => x.Date == d)?.Count ?? 0
        }).ToList();

        // Monthly revenue trend (last 6 months)
        var monthlyPaymentData = await paymentQuery
            .Where(p => p.CreatedAt >= sixMonthsAgo)
            .GroupBy(p => new { p.CreatedAt.Year, p.CreatedAt.Month })
            .Select(g => new { g.Key.Year, g.Key.Month, Revenue = g.Sum(p => p.Amount), Payments = g.Count() })
            .ToListAsync(cancellationToken);

        var monthlySubCounts = await subWithPlan
            .Where(s => s.CreatedAt >= sixMonthsAgo)
            .GroupBy(s => new { s.CreatedAt.Year, s.CreatedAt.Month })
            .Select(g => new { g.Key.Year, g.Key.Month, Count = g.Count() })
            .ToListAsync(cancellationToken);

        var monthlySubscriptionRevenue = last6Months.Select(d => new SubscriptionMonthlyRevenueDto
        {
            Label = d.ToString("MMM"),
            Revenue = monthlyPaymentData.FirstOrDefault(x => x.Year == d.Year && x.Month == d.Month)?.Revenue ?? 0,
            NewSubscriptions = monthlySubCounts.FirstOrDefault(x => x.Year == d.Year && x.Month == d.Month)?.Count ?? 0
        }).ToList();

        // Recent subscription activity
        var recentSubscriptionActivity = await subscriptionQuery
            .Include(s => s.Member)
            .Include(s => s.Plan)
            .OrderByDescending(s => s.CreatedAt)
            .Take(15)
            .Select(s => new RecentActivityDto
            {
                Type = s.OfferId != null ? "subscription_offer" : "subscription",
                Description = s.Member.FullName + " - " + s.ReceiptNumber + " (" + s.Plan.Name + ")",
                Timestamp = s.CreatedAt
            })
            .ToListAsync(cancellationToken);

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
            Subscriptions = new SubscriptionStatsDto
            {
                TotalSubscriptions = totalSubs,
                ActiveSubscriptions = activeSubs,
                FrozenSubscriptions = frozenSubs,
                ExpiredSubscriptions = expiredSubs,
                RenewedSubscriptions = renewedSubs,
                TotalRevenue = totalRevenue,
                RevenueThisMonth = revenueThisMonth,
                RevenueThisWeek = revenueThisWeek,
                TodayRevenue = todayRevenue,
                TotalOutstanding = totalOutstanding,
                AverageSubscriptionValue = avgSubValue,
                ExpiringNext7Days = expiring7Days,
                ExpiringNext30Days = expiring30Days,
                SubscriptionsWithOffers = subsWithOffers,
                FreezesThisMonth = freezesThisMonth
            },
            MembershipByPlan = membershipByPlan,
            SubscriptionRevenueByPlan = revenueByPlan,
            RecentActivities = recentActivities,
            DailyAttendanceTrend = dailyAttendanceTrend,
            MonthlyNewMembersTrend = monthlyNewMembersTrend,
            DailySubscriptionRevenue = dailySubscriptionRevenue,
            MonthlySubscriptionRevenue = monthlySubscriptionRevenue,
            RecentSubscriptionActivity = recentSubscriptionActivity
        };

        return Result<DetailedDashboardDto>.Success(dashboard);
    }
}
