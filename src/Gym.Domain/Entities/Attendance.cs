using Gym.Shared.Common;
using Gym.Shared.Enums;

namespace Gym.Domain.Entities;

public class Attendance : BaseEntity
{
    public Guid MemberId { get; set; }
    public Guid? DeviceId { get; set; }
    public DateTime Date { get; set; }
    public TimeSpan Time { get; set; }
    public DateTime? CheckIn { get; set; }
    public DateTime? CheckOut { get; set; }
    public bool IsManual { get; set; }
    public AttendanceSyncStatus SyncStatus { get; set; } = AttendanceSyncStatus.Synced;

    public Member Member { get; set; } = null!;
    public Device? Device { get; set; }

    private Attendance() { }

    public Attendance(Guid memberId, DateTime date, TimeSpan time, bool isManual = false)
    {
        MemberId = memberId;
        Date = date;
        Time = time;
        IsManual = isManual;
        CheckIn = date.Date + time;
    }

    public void SetCheckOut(DateTime checkOut)
    {
        CheckOut = checkOut;
        MarkUpdated();
    }

    public void MarkSynced(AttendanceSyncStatus status)
    {
        SyncStatus = status;
    }

    public void AssignDevice(Guid deviceId)
    {
        DeviceId = deviceId;
    }
}
