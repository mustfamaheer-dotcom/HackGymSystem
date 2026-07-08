using Gym.Application.Dashboard.DTOs;
using Gym.Domain.Entities;
using Gym.Domain.Interfaces;
using Gym.Shared.Common;
using Gym.Shared.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gym.Application.Dashboard.Queries.GetDetailedDashboard;

public record GetDetailedDashboardQuery(int? Year = null, int? Month = null, DateTime? From = null, DateTime? To = null) : IRequest<Result<DetailedDashboardDto>>;

public class GetDetailedDashboardQueryHandler : IRequestHandler<GetDetailedDashboardQuery, Result<DetailedDashboardDto>>
{
    private readonly IRepository<Member> _memberRepo;
    private readonly IRepository<Attendance> _attendanceRepo;
    private readonly IRepository<MembershipPlan> _planRepo;
    private readonly IRepository<Domain.Entities.Subscription> _subscriptionRepo;
    private readonly IRepository<SubscriptionPayment> _paymentRepo;
    private readonly IRepository<SubscriptionFreezeHistory> _freezeRepo;
    private readonly IRepository<Offer> _offerRepo;

    public GetDetailedDashboardQueryHandler(
        IRepository<Member> memberRepo,
        IRepository<Attendance> attendanceRepo,
        IRepository<MembershipPlan> planRepo,
        IRepository<Domain.Entities.Subscription> subscriptionRepo,
        IRepository<SubscriptionPayment> paymentRepo,
        IRepository<SubscriptionFreezeHistory> freezeRepo,
        IRepository<Offer> offerRepo)
    {
        _memberRepo = memberRepo;
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

        // Determine filter boundaries
        DateTime? filterFrom = null;
        DateTime? filterTo = null;

        if (request.From.HasValue && request.To.HasValue)
        {
            filterFrom = request.From.Value.Date;
            filterTo = request.To.Value.Date.AddDays(1).AddTicks(-1);
        }
        else if (request.Year.HasValue && request.Month.HasValue)
        {
            filterFrom = new DateTime(request.Year.Value, request.Month.Value, 1, 0, 0, 0, DateTimeKind.Utc);
            filterTo = filterFrom.Value.AddMonths(1).AddTicks(-1);
        }
        else if (request.Year.HasValue)
        {
            filterFrom = new DateTime(request.Year.Value, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            filterTo = filterFrom.Value.AddYears(1).AddTicks(-1);
        }

        var hasFilter = filterFrom.HasValue && filterTo.HasValue;

        // Query builder — always unfiltered for absolute stats
        var memberQueryAll = _memberRepo.Query();
        var attendanceQueryAll = _attendanceRepo.Query();
        var paymentQueryAll = _paymentRepo.Query();
        var freezeQueryAll = _freezeRepo.Query();
        var subscriptionQueryAll = _subscriptionRepo.Query();

        // Period-scoped queries for "new / revenue / activity this period"
        IQueryable<Member> memberQueryPeriod = memberQueryAll;
        IQueryable<Attendance> attendanceQueryPeriod = attendanceQueryAll;
        IQueryable<SubscriptionPayment> paymentQueryPeriod = paymentQueryAll;
        IQueryable<SubscriptionFreezeHistory> freezeQueryPeriod = freezeQueryAll;
        IQueryable<Domain.Entities.Subscription> subscriptionQueryPeriod = subscriptionQueryAll;

        if (hasFilter)
        {
            memberQueryPeriod = memberQueryAll.Where(m => m.CreatedAt >= filterFrom && m.CreatedAt <= filterTo);
            attendanceQueryPeriod = attendanceQueryAll.Where(a => a.CheckIn >= filterFrom && a.CheckIn <= filterTo);
            paymentQueryPeriod = paymentQueryAll.Where(p => p.CreatedAt >= filterFrom && p.CreatedAt <= filterTo);
            freezeQueryPeriod = freezeQueryAll.Where(f => f.CreatedAt >= filterFrom && f.CreatedAt <= filterTo);
            subscriptionQueryPeriod = subscriptionQueryAll.Where(s => s.CreatedAt >= filterFrom && s.CreatedAt <= filterTo);
        }

        // ===== ABSOLUTE STATS (always unfiltered) =====
        var totalMembers = await memberQueryAll.CountAsync(cancellationToken);
        var activeMembers = await memberQueryAll.CountAsync(m => !m.IsDeleted, cancellationToken);
        var maleCount = await memberQueryAll.CountAsync(m => m.Gender == Gender.Male, cancellationToken);
        var femaleCount = await memberQueryAll.CountAsync(m => m.Gender == Gender.Female, cancellationToken);

        // ===== CURRENT-STATE METRICS (always real-time) =====
        var todayAttendance = await attendanceQueryAll.CountAsync(a => a.CheckIn.Date == today, cancellationToken);
        var currentlyCheckedIn = await attendanceQueryAll
            .CountAsync(a => a.CheckIn.Date == today && a.CheckOut == null, cancellationToken);

        // ===== PERIOD METRICS (respect filter; default to current month) =====
        var periodStart = hasFilter ? filterFrom!.Value : new DateTime(now.Year, now.Month, 1, 0, 0, 0, DateTimeKind.Utc);
        var periodEnd = hasFilter ? filterTo!.Value : periodStart.AddMonths(1).AddTicks(-1);

        var periodAttendanceQuery = hasFilter ? attendanceQueryPeriod : attendanceQueryAll.Where(a => a.CheckIn >= periodStart && a.CheckIn <= periodEnd);
        var periodPaymentQuery = hasFilter ? paymentQueryPeriod : paymentQueryAll.Where(p => p.CreatedAt >= periodStart && p.CreatedAt <= periodEnd);
        var periodFreezeQuery = hasFilter ? freezeQueryPeriod : freezeQueryAll.Where(f => f.CreatedAt >= periodStart && f.CreatedAt <= periodEnd);
        var periodMemberQuery = hasFilter ? memberQueryPeriod : memberQueryAll.Where(m => m.CreatedAt >= periodStart && m.CreatedAt <= periodEnd);
        var periodSubscriptionQuery = hasFilter ? subscriptionQueryPeriod : subscriptionQueryAll.Where(s => s.CreatedAt >= periodStart && s.CreatedAt <= periodEnd);

        var newThisPeriod = await periodMemberQuery.CountAsync(cancellationToken);
        var periodAttendanceCount = await periodAttendanceQuery.CountAsync(cancellationToken);
        var periodDays = (int)((periodEnd - periodStart).TotalDays) + 1;
        var avgDailyPeriod = Math.Round((double)periodAttendanceCount / Math.Max(periodDays, 1), 1);

        var weekAttendance = await attendanceQueryAll.CountAsync(a => a.CheckIn >= weekStart, cancellationToken);

        // ===== SUBSCRIPTION STATS =====
        var totalSubscriptions = await subscriptionQueryAll.CountAsync(cancellationToken);
        var activeSubscriptions = await subscriptionQueryAll.CountAsync(s => s.Status == SubscriptionStatus.Active, cancellationToken);
        var frozenSubscriptions = await subscriptionQueryAll.CountAsync(s => s.Status == SubscriptionStatus.Frozen, cancellationToken);
        var expiredSubscriptions = await subscriptionQueryAll.CountAsync(s => s.Status == SubscriptionStatus.Expired, cancellationToken);

        var expiringThisWeek = await subscriptionQueryAll
            .CountAsync(s => s.ExpirationDate <= now.AddDays(7) && s.ExpirationDate > now && s.Status == SubscriptionStatus.Active, cancellationToken);

        var subsByPlan = await subscriptionQueryAll
            .Where(s => s.Status == SubscriptionStatus.Active)
            .GroupBy(s => s.Plan.Name)
            .Select(g => new PlanDistributionDto
            {
                PlanName = g.Key,
                Count = g.Count()
            })
            .ToListAsync(cancellationToken);

        var membershipByPlan = subsByPlan;
        var totalPlanMembers = membershipByPlan.Sum(p => p.Count);
        foreach (var plan in membershipByPlan)
            plan.Percent = totalPlanMembers > 0 ? Math.Round(plan.Count * 100.0 / totalPlanMembers, 1) : 0;

        // ===== RECENT ACTIVITY =====
        var recentActivities = await attendanceQueryAll
            .Include(a => a.Member)
            .OrderByDescending(a => a.CheckIn)
            .Take(15)
            .Select(a => new RecentActivityDto
            {
                Type = a.IsManual ? "manual" : "checkin",
                Description = $"{a.Member.FullName} - {(a.CheckOut == null ? "Checked in" : "Checked out")}",
                Timestamp = a.CheckIn
            })
            .ToListAsync(cancellationToken);

        // ===== TRENDS (always last 7 days / last 6 months) =====
        var last7Days = Enumerable.Range(0, 7).Select(i => today.AddDays(-6 + i)).ToList();
        var dailyAttendanceData = await attendanceQueryAll
            .Where(a => a.CheckIn >= last7Days[0])
            .GroupBy(a => a.CheckIn.Date)
            .Select(g => new { Date = g.Key, Count = g.Count() })
            .ToListAsync(cancellationToken);
        var dailyAttendanceTrend = last7Days.Select(d => new DailyStatDto
        {
            Label = d.ToString("ddd"),
            Count = dailyAttendanceData.FirstOrDefault(x => x.Date == d)?.Count ?? 0
        }).ToList();

        var last6Months = Enumerable.Range(0, 6).Select(i => new DateTime(now.Year, now.Month, 1).AddMonths(-i)).Reverse().ToList();
        var sixMonthsAgo = last6Months[0];

        var monthlyMembersData = await memberQueryAll
            .Where(m => m.CreatedAt >= sixMonthsAgo)
            .GroupBy(m => new { m.CreatedAt.Year, m.CreatedAt.Month })
            .Select(g => new { g.Key.Year, g.Key.Month, Count = g.Count() })
            .ToListAsync(cancellationToken);
        var monthlyNewMembersTrend = last6Months.Select(d => new MonthlyStatDto
        {
            Label = d.ToString("MMM"),
            Value = monthlyMembersData.FirstOrDefault(x => x.Year == d.Year && x.Month == d.Month)?.Count ?? 0
        }).ToList();

        // ===== SUBSCRIPTION / REVENUE DETAILS =====
        var subWithPlan = subscriptionQueryAll
            .Include(s => s.Plan)
            .Include(s => s.Offer)
            .Include(s => s.Member);

        var subWithPlanAndMember = subscriptionQueryAll
            .Include(s => s.Plan)
            .Include(s => s.Member);

        var totalSubs = await subscriptionQueryAll.CountAsync(cancellationToken);
        var activeSubs = await subscriptionQueryAll.CountAsync(s => s.Status == SubscriptionStatus.Active, cancellationToken);
        var frozenSubs = await subscriptionQueryAll.CountAsync(s => s.Status == SubscriptionStatus.Frozen, cancellationToken);
        var expiredSubs = await subscriptionQueryAll.CountAsync(s => s.Status == SubscriptionStatus.Expired, cancellationToken);
        var renewedSubs = await subscriptionQueryAll.CountAsync(s => s.Status == SubscriptionStatus.Renewed, cancellationToken);

        var totalRevenue = await paymentQueryAll.SumAsync(p => (decimal?)p.Amount, cancellationToken) ?? 0;
        var revenueThisPeriod = await periodPaymentQuery.SumAsync(p => (decimal?)p.Amount, cancellationToken) ?? 0;
        var revenueThisWeek = await paymentQueryAll
            .Where(p => p.CreatedAt >= weekStart)
            .SumAsync(p => (decimal?)p.Amount, cancellationToken) ?? 0;
        var todayRevenue = await paymentQueryAll
            .Where(p => p.CreatedAt >= today)
            .SumAsync(p => (decimal?)p.Amount, cancellationToken) ?? 0;

        var totalOutstanding = await subscriptionQueryAll
            .SumAsync(s => (decimal?)s.RemainingBalance, cancellationToken) ?? 0;

        var avgSubValue = totalSubs > 0
            ? Math.Round(await subscriptionQueryAll.SumAsync(s => (decimal?)s.TotalSubscriptionValue, cancellationToken) ?? 0 / totalSubs, 2)
            : 0;

        var expiring7Days = await subscriptionQueryAll
            .CountAsync(s => s.ExpirationDate <= now.AddDays(7) && s.ExpirationDate > now && s.Status == SubscriptionStatus.Active, cancellationToken);
        var expiring30Days = await subscriptionQueryAll
            .CountAsync(s => s.ExpirationDate <= now.AddDays(30) && s.ExpirationDate > now && s.Status == SubscriptionStatus.Active, cancellationToken);
        var subsWithOffers = await subscriptionQueryAll.CountAsync(s => s.OfferId != null, cancellationToken);
        var freezesThisPeriod = await periodFreezeQuery.CountAsync(cancellationToken);

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
        var dailyPaymentData = await paymentQueryAll
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
        var monthlyPaymentData = await paymentQueryAll
            .Where(p => p.CreatedAt >= sixMonthsAgo)
            .GroupBy(p => new { p.CreatedAt.Year, p.CreatedAt.Month })
            .Select(g => new { g.Key.Year, g.Key.Month, Revenue = g.Sum(p => p.Amount), Payments = g.Count() })
            .ToListAsync(cancellationToken);

        var monthlySubCounts = await subscriptionQueryAll
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

        // --- (1) Top Revenue Package ---
        var topPackage = revenueByPlan.OrderByDescending(r => r.TotalPaid).FirstOrDefault();
        var topPackageDto = new TopPackageDto();
        if (topPackage != null)
        {
            topPackageDto.PlanName = topPackage.PlanName;
            topPackageDto.TotalPaid = topPackage.TotalPaid;
            topPackageDto.SubCount = topPackage.Count;
            topPackageDto.PercentOfRevenue = totalPaidAllPlans > 0
                ? Math.Round((double)(topPackage.TotalPaid / totalPaidAllPlans) * 100, 1)
                : 0;
        }

        // --- (2) AOV Trend ---
        var aovTrend = last6Months.Select(d =>
        {
            var monthData = monthlyPaymentData.FirstOrDefault(x => x.Year == d.Year && x.Month == d.Month);
            var subData = monthlySubCounts.FirstOrDefault(x => x.Year == d.Year && x.Month == d.Month);
            var subCount = subData?.Count ?? 0;
            var rev = monthData?.Revenue ?? 0;
            return new AovTrendDto
            {
                Label = d.ToString("MMM"),
                Revenue = rev,
                SubCount = subCount,
                Aov = subCount > 0 ? Math.Round(rev / subCount, 2) : 0
            };
        }).ToList();

        // --- (3) Renewal Rate by Plan ---
        var renewalByPlan = await subWithPlanAndMember
            .GroupBy(s => new { s.PlanId, s.Plan.Name })
            .Select(g => new RenewalRateByPlanDto
            {
                PlanName = g.Key.Name,
                TotalSubscriptions = g.Count(),
                RenewedCount = g.Count(s => s.Status == SubscriptionStatus.Renewed),
                ActiveCount = g.Count(s => s.Status == SubscriptionStatus.Active)
            })
            .ToListAsync(cancellationToken);

        var totalRenewable = renewalByPlan.Sum(r => r.TotalSubscriptions);
        var totalRenewed = renewalByPlan.Sum(r => r.RenewedCount);
        var overallRenewalRate = totalRenewable > 0 ? Math.Round((double)totalRenewed / totalRenewable * 100, 1) : 0;

        foreach (var r in renewalByPlan)
            r.RenewalRate = r.TotalSubscriptions > 0 ? Math.Round((double)r.RenewedCount / r.TotalSubscriptions * 100, 1) : 0;

        // --- (4) Top Spenders ---
        var memberPayments = await paymentQueryAll
            .Join(subscriptionQueryAll.Include(s => s.Plan), p => p.SubscriptionId, s => s.Id, (p, s) => new { p, s })
            .Join(memberQueryAll, ps => ps.s.MemberId, m => m.Id, (ps, m) => new { ps.p, ps.s, m })
            .GroupBy(x => new { x.m.Id, x.m.FullName, x.m.PhoneNumber })
            .Select(g => new
            {
                MemberId = g.Key.Id,
                g.Key.FullName,
                Phone = g.Key.PhoneNumber,
                TotalPaid = g.Sum(x => x.p.Amount),
                SubscriptionCount = g.Select(x => x.p.SubscriptionId).Distinct().Count(),
                TopPlan = g.Select(x => x.s.Plan.Name).FirstOrDefault()
            })
            .OrderByDescending(x => x.TotalPaid)
            .ToListAsync(cancellationToken);

        var topSpenderCount = Math.Max(1, (int)Math.Ceiling(memberPayments.Count * 0.1));
        var topSpenders = memberPayments.Take(topSpenderCount).Select(x => new TopSpenderDto
        {
            MemberName = x.FullName,
            Phone = x.Phone,
            TotalPaid = x.TotalPaid,
            SubscriptionCount = x.SubscriptionCount,
            TopPlan = x.TopPlan ?? ""
        }).ToList();

        // --- (5) Overdue Payments ---
        var overduePaymentsData = await subWithPlanAndMember
            .Where(s => s.RemainingBalance > 0 && s.Status == SubscriptionStatus.Active)
            .OrderByDescending(s => s.RemainingBalance)
            .Take(20)
            .Select(s => new OverduePaymentDto
            {
                MemberName = s.Member.FullName,
                Phone = s.Member.PhoneNumber,
                ReceiptNumber = s.ReceiptNumber,
                RemainingBalance = s.RemainingBalance,
                TotalValue = s.TotalSubscriptionValue,
                ExpirationDate = s.ExpirationDate,
                PlanName = s.Plan.Name
            })
            .ToListAsync(cancellationToken);

        // --- (6) Payment Delay ---
        var paymentDelays = await paymentQueryAll
            .Include(p => p.Subscription)
            .Where(p => p.CreatedAt >= sixMonthsAgo)
            .Select(p => new
            {
                PaymentDate = p.CreatedAt,
                SubStart = p.Subscription.CreatedAt,
                Year = p.CreatedAt.Year,
                Month = p.CreatedAt.Month
            })
            .ToListAsync(cancellationToken);

        var delayDays = paymentDelays
            .Where(p => p.PaymentDate >= p.SubStart)
            .Select(p => (p.PaymentDate - p.SubStart).TotalDays)
            .ToList();

        var avgDelayDays = delayDays.Count > 0 ? Math.Round(delayDays.Average(), 1) : 0;

        var delayMonthly = paymentDelays
            .Where(p => p.PaymentDate >= p.SubStart)
            .GroupBy(p => new { p.Year, p.Month })
            .Select(g => new
            {
                g.Key.Year,
                g.Key.Month,
                AvgDays = Math.Round(g.Average(x => (x.PaymentDate - x.SubStart).TotalDays), 1),
                PaymentCount = g.Count()
            })
            .ToList();

        var delayTrend = last6Months.Select(d => new PaymentDelayTrendDto
        {
            Label = d.ToString("MMM"),
            AvgDays = delayMonthly.FirstOrDefault(x => x.Year == d.Year && x.Month == d.Month)?.AvgDays ?? 0,
            PaymentCount = delayMonthly.FirstOrDefault(x => x.Year == d.Year && x.Month == d.Month)?.PaymentCount ?? 0
        }).ToList();

        var paymentDelay = new PaymentDelayStatsDto
        {
            AverageDelayDays = avgDelayDays,
            MonthlyTrend = delayTrend
        };

        // --- (7) Freeze Impact ---
        var freezeMonthly = await freezeQueryAll
            .Where(f => f.CreatedAt >= sixMonthsAgo)
            .GroupBy(f => new { f.CreatedAt.Year, f.CreatedAt.Month })
            .Select(g => new { g.Key.Year, g.Key.Month, Count = g.Count() })
            .ToListAsync(cancellationToken);

        var freezeImpact = last6Months.Select(d =>
        {
            var freezeData = freezeMonthly.FirstOrDefault(x => x.Year == d.Year && x.Month == d.Month);
            var revData = monthlyPaymentData.FirstOrDefault(x => x.Year == d.Year && x.Month == d.Month);
            var subData = monthlySubCounts.FirstOrDefault(x => x.Year == d.Year && x.Month == d.Month);
            return new FreezeImpactDto
            {
                Label = d.ToString("MMM"),
                FreezeCount = freezeData?.Count ?? 0,
                Revenue = revData?.Revenue ?? 0,
                ExpectedSubscriptions = subData?.Count ?? 0
            };
        }).ToList();

        var recentSubscriptionActivity = await subscriptionQueryAll
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
                NewThisMonth = newThisPeriod,
                MaleCount = maleCount,
                FemaleCount = femaleCount,
                ExpiredSubscriptions = expiredSubscriptions,
                ExpiringThisWeek = expiringThisWeek
            },
            Memberships = new MembershipsStatsDto
            {
                Total = totalSubscriptions,
                Active = activeSubscriptions,
                Frozen = frozenSubscriptions,
                Expired = expiredSubscriptions,
                Cancelled = 0
            },
            Attendance = new AttendanceStatsDto
            {
                TodayTotal = todayAttendance,
                ThisWeekTotal = weekAttendance,
                ThisMonthTotal = periodAttendanceCount,
                CurrentlyCheckedIn = currentlyCheckedIn,
                AvgDailyThisMonth = avgDailyPeriod
            },
            Subscriptions = new SubscriptionStatsDto
            {
                TotalSubscriptions = totalSubs,
                ActiveSubscriptions = activeSubs,
                FrozenSubscriptions = frozenSubs,
                ExpiredSubscriptions = expiredSubs,
                RenewedSubscriptions = renewedSubs,
                TotalRevenue = totalRevenue,
                RevenueThisMonth = revenueThisPeriod,
                RevenueThisWeek = revenueThisWeek,
                TodayRevenue = todayRevenue,
                TotalOutstanding = totalOutstanding,
                AverageSubscriptionValue = avgSubValue,
                ExpiringNext7Days = expiring7Days,
                ExpiringNext30Days = expiring30Days,
                SubscriptionsWithOffers = subsWithOffers,
                FreezesThisMonth = freezesThisPeriod
            },
            TopRevenuePackage = topPackageDto,
            MembershipByPlan = membershipByPlan,
            SubscriptionRevenueByPlan = revenueByPlan,
            RecentActivities = recentActivities,
            DailyAttendanceTrend = dailyAttendanceTrend,
            MonthlyNewMembersTrend = monthlyNewMembersTrend,
            DailySubscriptionRevenue = dailySubscriptionRevenue,
            MonthlySubscriptionRevenue = monthlySubscriptionRevenue,
            RecentSubscriptionActivity = recentSubscriptionActivity,
            AovTrend = aovTrend,
            RenewalRateByPlan = renewalByPlan,
            TopSpenders = topSpenders,
            OverduePayments = overduePaymentsData,
            PaymentDelay = paymentDelay,
            FreezeImpact = freezeImpact,
            OverallRenewalRate = overallRenewalRate
        };

        return Result<DetailedDashboardDto>.Success(dashboard);
    }
}
