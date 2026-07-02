using Gym.Shared.Common;
using Gym.Shared.Enums;

namespace Gym.Domain.Entities;

public class WhatsAppTemplate : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string MessageBody { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public NotificationType? TriggerType { get; set; }

    private WhatsAppTemplate() { }

    public WhatsAppTemplate(string name, string messageBody, NotificationType? triggerType = null)
    {
        Name = name;
        MessageBody = messageBody;
        TriggerType = triggerType;
    }

    public void Update(string name, string messageBody, bool isActive, NotificationType? triggerType = null)
    {
        Name = name;
        MessageBody = messageBody;
        IsActive = isActive;
        TriggerType = triggerType;
        MarkUpdated();
    }
}
