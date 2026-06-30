namespace Gym.Application.Dashboard.DTOs;

public class DetailedDashboardDto
{
    public MembersStatsDto Members { get; set; } = new();
    public MembershipsStatsDto Memberships { get; set; } = new();
    public AttendanceStatsDto Attendance { get; set; } = new();
    public PaymentsStatsDto Payments { get; set; } = new();
    public List<PlanDistributionDto> MembershipByPlan { get; set; } = new();
    public List<RecentActivityDto> RecentActivities { get; set; } = new();
    public List<DailyStatDto> DailyAttendanceTrend { get; set; } = new();
    public List<MonthlyStatDto> MonthlyRevenueTrend { get; set; } = new();
    public List<MonthlyStatDto> MonthlyNewMembersTrend { get; set; } = new();
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

public class PaymentsStatsDto
{
    public decimal TodayRevenue { get; set; }
    public decimal WeeklyRevenue { get; set; }
    public decimal MonthlyRevenue { get; set; }
    public decimal YearlyRevenue { get; set; }
    public decimal TotalRevenue { get; set; }
    public int TodayCount { get; set; }
    public int ThisMonthCount { get; set; }
    public int CashCount { get; set; }
    public int VisaCount { get; set; }
    public int InstapayCount { get; set; }
    public int WalletCount { get; set; }
    public int OverdueCount { get; set; }
    public decimal AvgTransactionValue => ThisMonthCount > 0 ? Math.Round(MonthlyRevenue / ThisMonthCount, 2) : 0;
    public double CashPercent => GetPaymentMethodPercent(CashCount);
    public double VisaPercent => GetPaymentMethodPercent(VisaCount);
    public double InstapayPercent => GetPaymentMethodPercent(InstapayCount);
    public double WalletPercent => GetPaymentMethodPercent(WalletCount);
    private int TotalPaymentMethods => CashCount + VisaCount + InstapayCount + WalletCount;
    private double GetPaymentMethodPercent(int count) => TotalPaymentMethods > 0 ? Math.Round(count * 100.0 / TotalPaymentMethods, 1) : 0;
}

public class PlanDistributionDto
{
    public string PlanName { get; set; } = string.Empty;
    public int Count { get; set; }
    public double Percent { get; set; }
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


