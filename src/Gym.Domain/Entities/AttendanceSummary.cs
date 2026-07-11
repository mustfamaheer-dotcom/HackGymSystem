using Gym.Shared.Common;
using Gym.Shared.Enums;

namespace Gym.Domain.Entities;

public class AttendanceSummary : BaseEntity
{
    public Guid MemberId { get; set; }
    public DateTime Date { get; set; }
    public TimeSpan? CheckInTime { get; set; }
    public TimeSpan? CheckOutTime { get; set; }
    public double? WorkDurationMinutes { get; set; }
    public AttendanceStatus Status { get; set; } = AttendanceStatus.Absent;
    public int PresentDays { get; set; }
    public int LateDays { get; set; }
    public int AbsentDays { get; set; }
    public int EarlyLeaveDays { get; set; }
    public int HalfDays { get; set; }
    public int OnLeaveDays { get; set; }
    public double TotalWorkHours { get; set; }

    public Member Member { get; set; } = null!;

    private AttendanceSummary() { }

    public AttendanceSummary(Guid memberId, DateTime date)
    {
        MemberId = memberId;
        Date = date.Date;
    }

    public void SetCheckIn(TimeSpan time)
    {
        CheckInTime = time;
        MarkUpdated();
    }

    public void SetCheckOut(TimeSpan time)
    {
        CheckOutTime = time;
        MarkUpdated();
    }

    public void CalculateWorkDuration()
    {
        if (CheckInTime.HasValue && CheckOutTime.HasValue)
        {
            WorkDurationMinutes = (CheckOutTime.Value - CheckInTime.Value).TotalMinutes;
            TotalWorkHours = WorkDurationMinutes.Value / 60.0;
        }
        MarkUpdated();
    }

    public void SetStatus(AttendanceStatus status)
    {
        Status = status;
        MarkUpdated();
    }
}

public enum AttendanceStatus
{
    Present,
    Late,
    EarlyLeave,
    Absent,
    OnLeave,
    HalfDay
}
