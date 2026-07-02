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
        var monthStart = new DateTime(now.Year, now.Month, 1, 0, 0, 0, DateTimeKind.Utc);

        var memberQuery = _memberRepo.Query();
        var attendanceQuery = _attendanceRepo.Query();
        var subscriptionQuery = _subscriptionRepo.Query();
        var paymentQuery = _paymentRepo.Query();
        var freezeQuery = _freezeRepo.Query();

        // --- Existing Stats (Members, Subscriptions, Attendance) ---
        var totalMembers = await memberQuery.CountAsync(cancellationToken);
        var activeMembers = await memberQuery.CountAsync(m => !m.IsDeleted, cancellationToken);
        var newThisMonth = await memberQuery.CountAsync(m => m.CreatedAt >= monthStart, cancellationToken);
        var maleCount = await memberQuery.CountAsync(m => m.Gender == Gender.Male, cancellationToken);
        var femaleCount = await memberQuery.CountAsync(m => m.Gender == Gender.Female, cancellationToken);

        var totalSubsList = await subscriptionQuery.ToListAsync(cancellationToken);
        var totalSubscriptions = totalSubsList.Count;
        var activeSubscriptions = totalSubsList.Count(s => s.Status == SubscriptionStatus.Active);
        var frozenSubscriptions = totalSubsList.Count(s => s.Status == SubscriptionStatus.Frozen);
        var expiredSubscriptions = totalSubsList.Count(s => s.Status == SubscriptionStatus.Expired);
        var cancelledSubscriptions = 0;

        var expiringThisWeek = totalSubsList
            .Count(s => s.ExpirationDate <= now.AddDays(7) && s.ExpirationDate > now && s.Status == SubscriptionStatus.Active);

        var todayAttendance = await attendanceQuery.CountAsync(a => a.Date == today, cancellationToken);
        var weekAttendance = await attendanceQuery.CountAsync(a => a.Date >= weekStart, cancellationToken);
        var monthAttendance = await attendanceQuery.CountAsync(a => a.Date >= monthStart, cancellationToken);
        var currentlyCheckedIn = await attendanceQuery
            .CountAsync(a => a.Date == today && a.CheckOut == null, cancellationToken);

        var avgDailyThisMonth = Math.Round((double)monthAttendance / Math.Max(now.Day, 1), 1);

        var activeSubsWithPlan = totalSubsList.Where(s => s.Status == SubscriptionStatus.Active).ToList();
        var subsByPlan = activeSubsWithPlan
            .GroupBy(s => s.PlanId)
            .Select(g => new
            {
                PlanId = g.Key,
                Count = g.Count(),
                PlanName = g.First().Plan?.Name ?? ""
            })
            .ToList();

        var membershipByPlan = subsByPlan.Select(s => new PlanDistributionDto
        {
            PlanName = s.PlanName,
            Count = s.Count
        }).ToList();

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

        // --- Subscription base queries ---
        var subWithPlan = subscriptionQuery
            .Include(s => s.Plan)
            .Include(s => s.Offer)
            .Include(s => s.Member);

        var subWithPlanAndMember = subscriptionQuery
            .Include(s => s.Plan)
            .Include(s => s.Member);

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

        // --- (2) AOV Trend (Average Order Value per month) ---
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

        // --- (4) Top Spenders (top 10% by total paid) ---
        var memberPayments = await paymentQuery
            .Join(subscriptionQuery.Include(s => s.Plan), p => p.SubscriptionId, s => s.Id, (p, s) => new { p, s })
            .Join(_memberRepo.Query(), ps => ps.s.MemberId, m => m.Id, (ps, m) => new { ps.p, ps.s, m })
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
        var paymentDelays = await paymentQuery
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
        var freezeMonthly = await freezeQuery
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
                ExpiredSubscriptions = expiredSubscriptions,
                ExpiringThisWeek = expiringThisWeek
            },
            Memberships = new MembershipsStatsDto
            {
                Total = totalSubscriptions,
                Active = activeSubscriptions,
                Frozen = frozenSubscriptions,
                Expired = expiredSubscriptions,
                Cancelled = cancelledSubscriptions
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
