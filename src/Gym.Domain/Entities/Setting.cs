using Gym.Shared.Common;

namespace Gym.Domain.Entities;

public class Setting : BaseEntity
{
    public string Key { get; set; }
    public string Value { get; set; }
    public string? Group { get; set; }
    public string? Description { get; set; }
    public bool IsEncrypted { get; set; }

    private Setting() { }

    public Setting(string key, string value, string? group, string? description, bool isEncrypted = false)
    {
        Key = key;
        Value = value;
        Group = group;
        Description = description;
        IsEncrypted = isEncrypted;
    }

    public void UpdateValue(string value)
    {
        Value = value;
        MarkUpdated();
    }
}
