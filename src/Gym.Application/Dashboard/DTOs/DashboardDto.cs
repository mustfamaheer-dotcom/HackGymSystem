namespace Gym.Application.Dashboard.DTOs;

public class DashboardDto
{
    public int TotalMembers { get; set; }
    public int ActiveMembers { get; set; }
    public int ActiveMemberships { get; set; }
    public int TodayCheckIns { get; set; }
    public decimal TodayRevenue { get; set; }
    public decimal MonthlyRevenue { get; set; }
    public int ExpiringMemberships { get; set; }
    public int OverduePayments { get; set; }
    public List<RecentActivityDto> RecentActivities { get; set; } = new();
    public List<MembershipStatsDto> MembershipByPlan { get; set; } = new();
}

public class RecentActivityDto
{
    public string Type { get; set; }
    public string Description { get; set; }
    public DateTime Timestamp { get; set; }
}

public class MembershipStatsDto
{
    public string PlanName { get; set; }
    public int Count { get; set; }
}
