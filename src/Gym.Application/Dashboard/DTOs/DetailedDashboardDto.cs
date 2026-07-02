namespace Gym.Application.Dashboard.DTOs;

public class DetailedDashboardDto
{
    public MembersStatsDto Members { get; set; } = new();
    public MembershipsStatsDto Memberships { get; set; } = new();
    public AttendanceStatsDto Attendance { get; set; } = new();
    public SubscriptionStatsDto Subscriptions { get; set; } = new();
    public TopPackageDto TopRevenuePackage { get; set; } = new();
    public List<PlanDistributionDto> MembershipByPlan { get; set; } = new();
    public List<SubscriptionRevenueByPlanDto> SubscriptionRevenueByPlan { get; set; } = new();
    public List<RecentActivityDto> RecentActivities { get; set; } = new();
    public List<DailyStatDto> DailyAttendanceTrend { get; set; } = new();
    public List<MonthlyStatDto> MonthlyNewMembersTrend { get; set; } = new();
    public List<SubscriptionDailyRevenueDto> DailySubscriptionRevenue { get; set; } = new();
    public List<SubscriptionMonthlyRevenueDto> MonthlySubscriptionRevenue { get; set; } = new();
    public List<RecentActivityDto> RecentSubscriptionActivity { get; set; } = new();
    public List<AovTrendDto> AovTrend { get; set; } = new();
    public List<RenewalRateByPlanDto> RenewalRateByPlan { get; set; } = new();
    public List<TopSpenderDto> TopSpenders { get; set; } = new();
    public List<OverduePaymentDto> OverduePayments { get; set; } = new();
    public PaymentDelayStatsDto PaymentDelay { get; set; } = new();
    public List<FreezeImpactDto> FreezeImpact { get; set; } = new();
    public double OverallRenewalRate { get; set; }
}

public class TopPackageDto
{
    public string PlanName { get; set; } = string.Empty;
    public decimal TotalPaid { get; set; }
    public int SubCount { get; set; }
    public double PercentOfRevenue { get; set; }
}

public class AovTrendDto
{
    public string Label { get; set; } = string.Empty;
    public decimal Aov { get; set; }
    public int SubCount { get; set; }
    public decimal Revenue { get; set; }
}

public class RenewalRateByPlanDto
{
    public string PlanName { get; set; } = string.Empty;
    public int TotalSubscriptions { get; set; }
    public int RenewedCount { get; set; }
    public double RenewalRate { get; set; }
    public int ActiveCount { get; set; }
}

public class TopSpenderDto
{
    public string MemberName { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public decimal TotalPaid { get; set; }
    public string TopPlan { get; set; } = string.Empty;
    public int SubscriptionCount { get; set; }
}

public class OverduePaymentDto
{
    public string MemberName { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string ReceiptNumber { get; set; } = string.Empty;
    public decimal RemainingBalance { get; set; }
    public decimal TotalValue { get; set; }
    public DateTime ExpirationDate { get; set; }
    public string PlanName { get; set; } = string.Empty;
}

public class PaymentDelayStatsDto
{
    public double AverageDelayDays { get; set; }
    public List<PaymentDelayTrendDto> MonthlyTrend { get; set; } = new();
}

public class PaymentDelayTrendDto
{
    public string Label { get; set; } = string.Empty;
    public double AvgDays { get; set; }
    public int PaymentCount { get; set; }
}

public class FreezeImpactDto
{
    public string Label { get; set; } = string.Empty;
    public int FreezeCount { get; set; }
    public decimal Revenue { get; set; }
    public int ExpectedSubscriptions { get; set; }
}

public class MembersStatsDto
{
    public int TotalMembers { get; set; }
    public int ActiveMembers { get; set; }
    public int NewThisMonth { get; set; }
    public int MaleCount { get; set; }
    public int FemaleCount { get; set; }
    public int ExpiredSubscriptions { get; set; }
    public int ExpiringThisWeek { get; set; }
    public double MalePercent => TotalMembers > 0 ? Math.Round(MaleCount * 100.0 / TotalMembers, 1) : 0;
    public double FemalePercent => TotalMembers > 0 ? Math.Round(FemaleCount * 100.0 / TotalMembers, 1) : 0;
}

public class MembershipsStatsDto
{
    public int Total { get; set; }
    public int Active { get; set; }
    public int Frozen { get; set; }
    public int Expired { get; set; }
    public int Cancelled { get; set; }
    public double ActivePercent => Total > 0 ? Math.Round(Active * 100.0 / Total, 1) : 0;
}

public class AttendanceStatsDto
{
    public int TodayTotal { get; set; }
    public int ThisWeekTotal { get; set; }
    public int ThisMonthTotal { get; set; }
    public int CurrentlyCheckedIn { get; set; }
    public double AvgDailyThisMonth { get; set; }
}

public class SubscriptionStatsDto
{
    public int TotalSubscriptions { get; set; }
    public int ActiveSubscriptions { get; set; }
    public int FrozenSubscriptions { get; set; }
    public int ExpiredSubscriptions { get; set; }
    public int RenewedSubscriptions { get; set; }
    public decimal TotalRevenue { get; set; }
    public decimal RevenueThisMonth { get; set; }
    public decimal RevenueThisWeek { get; set; }
    public decimal TodayRevenue { get; set; }
    public decimal TotalOutstanding { get; set; }
    public decimal AverageSubscriptionValue { get; set; }
    public int ExpiringNext7Days { get; set; }
    public int ExpiringNext30Days { get; set; }
    public int SubscriptionsWithOffers { get; set; }
    public int FreezesThisMonth { get; set; }
    public double ActivePercent => TotalSubscriptions > 0 ? Math.Round(ActiveSubscriptions * 100.0 / TotalSubscriptions, 1) : 0;
    public decimal AvgRevenuePerSubscription => TotalSubscriptions > 0 ? Math.Round(TotalRevenue / TotalSubscriptions, 2) : 0;
}

public class SubscriptionRevenueByPlanDto
{
    public string PlanName { get; set; } = string.Empty;
    public int Count { get; set; }
    public decimal TotalPaid { get; set; }
    public decimal TotalOutstanding { get; set; }
    public double Percent { get; set; }
}

public class SubscriptionDailyRevenueDto
{
    public string Label { get; set; } = string.Empty;
    public int SubscriptionCount { get; set; }
    public decimal Revenue { get; set; }
}

public class SubscriptionMonthlyRevenueDto
{
    public string Label { get; set; } = string.Empty;
    public decimal Revenue { get; set; }
    public int NewSubscriptions { get; set; }
}

public class PlanDistributionDto
{
    public string PlanName { get; set; } = string.Empty;
    public int Count { get; set; }
    public double Percent { get; set; }
}

public class RecentActivityDto
{
    public string Type { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
}

public class DailyStatDto
{
    public string Label { get; set; } = string.Empty;
    public int Count { get; set; }
}

public class MonthlyStatDto
{
    public string Label { get; set; } = string.Empty;
    public decimal Value { get; set; }
}
