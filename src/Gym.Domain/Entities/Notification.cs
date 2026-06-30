using Gym.Shared.Common;
using Gym.Shared.Enums;

namespace Gym.Domain.Entities;

public class Notification : BaseEntity
{
    public Guid? MemberId { get; set; }
    public NotificationType Type { get; set; }
    public NotificationChannel Channel { get; set; }
    public string Subject { get; set; }
    public string Message { get; set; }
    public NotificationStatus Status { get; set; } = NotificationStatus.Pending;
    public DateTime? ScheduledDate { get; set; }
    public DateTime? SentDate { get; set; }
    public bool IsPending { get; set; } = true;
    public int RetryCount { get; set; }
    public string? LastError { get; set; }

    public Member? Member { get; set; }

    private Notification() { }

    public Notification(Guid? memberId, NotificationType type, NotificationChannel channel,
        string subject, string message, DateTime? scheduledDate = null)
    {
        MemberId = memberId;
        Type = type;
        Channel = channel;
        Subject = subject;
        Message = message;
        ScheduledDate = scheduledDate ?? DateTime.UtcNow;
    }

    public void MarkSent()
    {
        Status = NotificationStatus.Sent;
        SentDate = DateTime.UtcNow;
        IsPending = false;
        MarkUpdated();
    }

    public void MarkFailed(string error)
    {
        Status = NotificationStatus.Failed;
        LastError = error;
        RetryCount++;
        MarkUpdated();
    }

    public void MarkPending()
    {
        Status = NotificationStatus.Pending;
        IsPending = true;
        MarkUpdated();
    }
}
