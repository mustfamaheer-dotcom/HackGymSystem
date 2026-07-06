using Gym.Domain.Entities;
using Gym.Shared.Enums;

namespace Gym.Domain.Interfaces;

public interface IDeviceMemberMappingRepository
{
    Task<DeviceMemberMapping?> GetByEnrollmentIdAsync(string enrollmentId, CancellationToken cancellationToken = default);
    Task<DeviceMemberMapping?> GetByEnrollmentIdAsync(string enrollmentId, BiometricType type, CancellationToken cancellationToken = default);
    Task<List<DeviceMemberMapping>> GetByMemberIdAsync(Guid memberId, CancellationToken cancellationToken = default);
    Task<string> GetNextEnrollmentIdAsync(CancellationToken cancellationToken = default);
    Task SaveMappingAsync(DeviceMemberMapping mapping, CancellationToken cancellationToken = default);
    Task<DeviceMemberMapping?> GetActiveMappingAsync(Guid memberId, BiometricType type, CancellationToken cancellationToken = default);
    Task<List<DeviceMemberMapping>> GetAllActiveMappingsAsync(CancellationToken cancellationToken = default);
}
