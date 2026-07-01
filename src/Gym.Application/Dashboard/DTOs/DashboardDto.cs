namespace Gym.Application.Dashboard.DTOs;

public class DashboardDto
{
    public int TotalMembers { get; set; }
    public int ActiveMembers { get; set; }
    public int ActiveMemberships { get; set; }
    public int TodayCheckIns { get; set; }
    public int ExpiringMemberships { get; set; }
    public List<MembershipStatsDto> MembershipByPlan { get; set; } = new();
}

public class MembershipStatsDto
{
    public string PlanName { get; set; }
    public int Count { get; set; }
}
