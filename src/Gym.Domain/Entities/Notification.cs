using Gym.Shared.Common;

namespace Gym.Domain.Entities;

public class Notification : BaseEntity
{
    public Guid MemberId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
    public bool IsRead { get; set; }
    public DateTime? ReadAt { get; set; }

    public Member Member { get; set; } = null!;

    private Notification() { }

    public Notification(Guid memberId, string title, string body)
    {
        MemberId = memberId;
        Title = title;
        Body = body;
    }

    public void MarkRead()
    {
        IsRead = true;
        ReadAt = DateTime.UtcNow;
        MarkUpdated();
    }
}
