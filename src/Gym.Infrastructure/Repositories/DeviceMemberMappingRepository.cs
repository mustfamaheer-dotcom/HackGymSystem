using Gym.Domain.Entities;
using Gym.Domain.Interfaces;
using Gym.Infrastructure.Data;
using Gym.Shared.Enums;
using Microsoft.EntityFrameworkCore;

namespace Gym.Infrastructure.Repositories;

public class DeviceMemberMappingRepository : IDeviceMemberMappingRepository
{
    private readonly GymDbContext _context;

    public DeviceMemberMappingRepository(GymDbContext context)
    {
        _context = context;
    }

    public async Task<DeviceMemberMapping?> GetByEnrollmentIdAsync(string enrollmentId, CancellationToken cancellationToken = default)
    {
        return await _context.Set<DeviceMemberMapping>()
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(m => m.DeviceEnrollmentId == enrollmentId && !m.IsDeleted, cancellationToken);
    }

    public async Task<DeviceMemberMapping?> GetByEnrollmentIdAsync(string enrollmentId, BiometricType type, CancellationToken cancellationToken = default)
    {
        return await _context.Set<DeviceMemberMapping>()
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(m => m.DeviceEnrollmentId == enrollmentId && m.BiometricType == type && !m.IsDeleted, cancellationToken);
    }

    public async Task<List<DeviceMemberMapping>> GetByMemberIdAsync(Guid memberId, CancellationToken cancellationToken = default)
    {
        return await _context.Set<DeviceMemberMapping>()
            .IgnoreQueryFilters()
            .Where(m => m.MemberId == memberId && !m.IsDeleted)
            .ToListAsync(cancellationToken);
    }

    public async Task<string> GetNextEnrollmentIdAsync(CancellationToken cancellationToken = default)
    {
        var maxId = await _context.Set<DeviceMemberMapping>()
            .IgnoreQueryFilters()
            .MaxAsync(m => (string?)m.DeviceEnrollmentId, cancellationToken);

        var nextNum = 1;
        if (!string.IsNullOrEmpty(maxId) && int.TryParse(maxId, out var parsed))
            nextNum = parsed + 1;

        return nextNum.ToString("D5");
    }

    public async Task SaveMappingAsync(DeviceMemberMapping mapping, CancellationToken cancellationToken = default)
    {
        var existing = await _context.Set<DeviceMemberMapping>()
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(m => m.Id == mapping.Id, cancellationToken);

        if (existing is not null)
            _context.Set<DeviceMemberMapping>().Update(mapping);
        else
            await _context.Set<DeviceMemberMapping>().AddAsync(mapping, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<DeviceMemberMapping?> GetActiveMappingAsync(Guid memberId, BiometricType type, CancellationToken cancellationToken = default)
    {
        return await _context.Set<DeviceMemberMapping>()
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(m => m.MemberId == memberId && m.BiometricType == type && !m.IsDeleted, cancellationToken);
    }

    public async Task<List<DeviceMemberMapping>> GetAllActiveMappingsAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Set<DeviceMemberMapping>()
            .IgnoreQueryFilters()
            .Where(m => !m.IsDeleted)
            .ToListAsync(cancellationToken);
    }
}
