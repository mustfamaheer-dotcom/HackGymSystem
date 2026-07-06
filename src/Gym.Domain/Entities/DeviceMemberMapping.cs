using Gym.Shared.Common;
using Gym.Shared.Enums;

namespace Gym.Domain.Entities;

public class DeviceMemberMapping : BaseEntity
{
    public Guid MemberId { get; set; }
    public string DeviceEnrollmentId { get; set; } = string.Empty;
    public BiometricType BiometricType { get; set; }
    public int? FingerIndex { get; set; }
    public DateTime EnrolledAt { get; set; } = DateTime.UtcNow;
    public bool IsDeleted { get; set; }

    public Member Member { get; set; } = null!;

    private DeviceMemberMapping() { }

    public DeviceMemberMapping(Guid memberId, string deviceEnrollmentId, BiometricType biometricType, int? fingerIndex = null)
    {
        MemberId = memberId;
        DeviceEnrollmentId = deviceEnrollmentId;
        BiometricType = biometricType;
        FingerIndex = fingerIndex;
    }

    public void SoftDelete()
    {
        IsDeleted = true;
        MarkUpdated();
    }
}
