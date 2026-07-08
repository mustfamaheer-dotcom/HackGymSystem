using Gym.Shared.Common;

namespace Gym.Domain.Entities;

public class DailySession : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public DateTime VisitDate { get; set; }

    private DailySession() { }

    public DailySession(string name, string phone, DateTime visitDate)
    {
        Name = name;
        Phone = phone;
        VisitDate = visitDate;
    }
}
