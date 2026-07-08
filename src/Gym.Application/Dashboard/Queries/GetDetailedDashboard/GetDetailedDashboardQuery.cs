using Gym.Application.Dashboard.DTOs;
using Gym.Domain.Entities;
using Gym.Domain.Interfaces;
using Gym.Shared.Common;
using Gym.Shared.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gym.Application.Dashboard.Queries.GetDetailedDashboard;

public record GetDetailedDashboardQuery(int? Year = null, int? Month = null) : IRequest<Result<DetailedDashboardDto>>;

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

        var isAllTime = !request.Year.HasValue || !request.Month.HasValue;
        var filterYear = request.Year ?? now.Year;
        var filterMonth = request.Month ?? now.Month;
        var selectedMonthStart = new DateTime(filterYear, filterMonth, 1, 0, 0, 0, DateTimeKind.Utc);
        var selectedMonthEnd = selectedMonthStart.AddMonths(1).AddTicks(-1);
        var monthStart = isAllTime ? new DateTime(now.Year, now.Month, 1, 0, 0, 0, DateTimeKind.Utc) : selectedMonthStart;
        var monthEnd = isAllTime ? selectedMonthEnd.AddMonths(1) : selectedMonthEnd;

        var memberQuery = _memberRepo.Query();
        var attendanceQuery = _attendanceRepo.Query();
        var subscriptionQuery = _subscriptionRepo.Query();
        var paymentQuery = _paymentRepo.Query();
        var freezeQuery = _freezeRepo.Query();

        // --- Apply month filter on member / attendance / payment / freeze queries when selected ---
        if (!isAllTime)
        {
            memberQuery = memberQuery.Where(m => m.CreatedAt >= monthStart && m.CreatedAt <= monthEnd);
            attendanceQuery = attendanceQuery.Where(a => a.CheckIn >= monthStart && a.CheckIn <= monthEnd);
            paymentQuery = paymentQuery.Where(p => p.CreatedAt >= monthStart && p.CreatedAt <= monthEnd);
            freezeQuery = freezeQuery.Where(f => f.CreatedAt >= monthStart && f.CreatedAt <= monthEnd);
        }

        // --- Existing Stats (Members, Subscriptions, Attendance) ---
        var totalMembers = await _memberRepo.Query().CountAsync(cancellationToken);
        var activeMembers = await _memberRepo.Query().CountAsync(m => !m.IsDeleted, cancellationToken);
        var newThisMonth = isAllTime
            ? await memberQuery.CountAsync(cancellationToken)
            : await _memberRepo.Query().CountAsync(m => m.CreatedAt >= new DateTime(now.Year, now.Month, 1, 0, 0, 0, DateTimeKind.Utc), cancellationToken);
        var maleCount = await _memberRepo.Query().CountAsync(m => m.Gender == Gender.Male, cancellationToken);
        var femaleCount = await _memberRepo.Query().CountAsync(m => m.Gender == Gender.Female, cancellationToken);

        var totalSubscriptions = await subscriptionQuery.CountAsync(cancellationToken);
        var activeSubscriptions = await subscriptionQuery.CountAsync(s => s.Status == SubscriptionStatus.Active, cancellationToken);
        var frozenSubscriptions = await subscriptionQuery.CountAsync(s => s.Status == SubscriptionStatus.Frozen, cancellationToken);
        var expiredSubscriptions = await subscriptionQuery.CountAsync(s => s.Status == SubscriptionStatus.Expired, cancellationToken);
        var cancelledSubscriptions = 0;

        var expiringThisWeek = await subscriptionQuery
            .CountAsync(s => s.ExpirationDate <= now.AddDays(7) && s.ExpirationDate > now && s.Status == SubscriptionStatus.Active, cancellationToken);

        var todayAttendance = await attendanceQuery.CountAsync(a => a.CheckIn.Date == today, cancellationToken);
        var weekAttendance = isAllTime ? await attendanceQuery.CountAsync(a => a.CheckIn >= weekStart, cancellationToken) : 0;
        var monthAttendance = await attendanceQuery.CountAsync(cancellationToken);
        var currentlyCheckedIn = await _attendanceRepo.Query()
            .CountAsync(a => a.CheckIn.Date == today && a.CheckOut == null, cancellationToken);

        var daysInSelectedMonth = DateTime.DaysInMonth(filterYear, filterMonth);
        var avgDailyThisMonth = Math.Round((double)monthAttendance / Math.Max(daysInSelectedMonth, 1), 1);

        var subsByPlan = await subscriptionQuery
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

        var recentActivities = await attendanceQuery
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

        var last7Days = isAllTime
            ? Enumerable.Range(0, 7).Select(i => today.AddDays(-6 + i)).ToList()
            : Enumerable.Range(0, 7).Select(i => selectedMonthStart.AddDays(i)).ToList();
        var dailyAttendanceData = await attendanceQuery
            .Where(a => a.CheckIn >= last7Days[0])
            .GroupBy(a => a.CheckIn.Date)
            .Select(g => new { Date = g.Key, Count = g.Count() })
            .ToListAsync(cancellationToken);
        var dailyAttendanceTrend = last7Days.Select(d => new DailyStatDto
        {
            Label = isAllTime ? d.ToString("ddd") : d.ToString("dd"),
            Count = dailyAttendanceData.FirstOrDefault(x => x.Date == d)?.Count ?? 0
        }).ToList();

        var last6Months = Enumerable.Range(0, 6)
            .Select(i => new DateTime(filterYear, filterMonth, 1).AddMonths(-i))
            .Reverse()
            .ToList();
        var sixMonthsAgo = last6Months[0];

        var monthlyMembersData = await _memberRepo.Query()
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

        var totalSubs = await subscriptionQuery.CountAsync(cancellationToken);
        var activeSubs = await subscriptionQuery.CountAsync(s => s.Status == SubscriptionStatus.Active, cancellationToken);
        var frozenSubs = await subscriptionQuery.CountAsync(s => s.Status == SubscriptionStatus.Frozen, cancellationToken);
        var expiredSubs = await subscriptionQuery.CountAsync(s => s.Status == SubscriptionStatus.Expired, cancellationToken);
        var renewedSubs = await subscriptionQuery.CountAsync(s => s.Status == SubscriptionStatus.Renewed, cancellationToken);

        var totalRevenue = await _paymentRepo.Query().SumAsync(p => (decimal?)p.Amount, cancellationToken) ?? 0;
        var revenueThisMonth = await paymentQuery
            .SumAsync(p => (decimal?)p.Amount, cancellationToken) ?? 0;
        var revenueThisWeek = await _paymentRepo.Query()
            .Where(p => p.CreatedAt >= weekStart)
            .SumAsync(p => (decimal?)p.Amount, cancellationToken) ?? 0;
        var todayRevenue = await _paymentRepo.Query()
            .Where(p => p.CreatedAt >= today)
            .SumAsync(p => (decimal?)p.Amount, cancellationToken) ?? 0;

        var totalOutstanding = await subscriptionQuery
            .SumAsync(s => (decimal?)s.RemainingBalance, cancellationToken) ?? 0;

        var avgSubValue = totalSubs > 0
            ? Math.Round(await subscriptionQuery.SumAsync(s => (decimal?)s.TotalSubscriptionValue, cancellationToken) ?? 0 / totalSubs, 2)
            : 0;

        var expiring7Days = await subscriptionQuery
            .CountAsync(s => s.ExpirationDate <= now.AddDays(7) && s.ExpirationDate > now
                && s.Status == SubscriptionStatus.Active, cancellationToken);
        var expiring30Days = await subscriptionQuery
            .CountAsync(s => s.ExpirationDate <= now.AddDays(30) && s.ExpirationDate > now
                && s.Status == SubscriptionStatus.Active, cancellationToken);
        var subsWithOffers = await subscriptionQuery.CountAsync(s => s.OfferId != null, cancellationToken);
        var freezesThisMonth = await freezeQuery
            .CountAsync(cancellationToken);

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
            Label = isAllTime ? d.ToString("ddd") : d.ToString("dd"),
            Revenue = dailyPaymentData.FirstOrDefault(x => x.Date == d)?.Revenue ?? 0,
            SubscriptionCount = dailyPaymentData.FirstOrDefault(x => x.Date == d)?.Count ?? 0
        }).ToList();

        // Monthly revenue trend (last 6 months)
        var monthlyPaymentData = await _paymentRepo.Query()
            .Where(p => p.CreatedAt >= sixMonthsAgo)
            .GroupBy(p => new { p.CreatedAt.Year, p.CreatedAt.Month })
            .Select(g => new { g.Key.Year, g.Key.Month, Revenue = g.Sum(p => p.Amount), Payments = g.Count() })
            .ToListAsync(cancellationToken);

        var monthlySubCounts = await subscriptionQuery
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
        var freezeMonthly = await _freezeRepo.Query()
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
