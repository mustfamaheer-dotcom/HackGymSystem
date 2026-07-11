using Gym.Domain.Entities;
using Gym.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Gym.Infrastructure.Repositories;

public interface IDeviceTrackingRepository
{
    Task<Device?> GetByIdAsync(Guid deviceId, CancellationToken ct = default);
    Task<Device?> GetByIpAsync(string ipAddress, CancellationToken ct = default);
    Task<IReadOnlyList<Device>> GetActiveDevicesAsync(CancellationToken ct = default);
    Task<int> GetOnlineCountAsync(CancellationToken ct = default);
    Task UpdateStatusAsync(Guid deviceId, Shared.Enums.DeviceStatus status, CancellationToken ct = default);
}

public class DeviceTrackingRepository : IDeviceTrackingRepository
{
    private readonly Data.GymDbContext _context;
    private readonly ILogger<DeviceTrackingRepository> _logger;

    public DeviceTrackingRepository(Data.GymDbContext context, ILogger<DeviceTrackingRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Device?> GetByIdAsync(Guid deviceId, CancellationToken ct = default)
    {
        return await _context.Devices.FirstOrDefaultAsync(d => d.Id == deviceId, ct);
    }

    public async Task<Device?> GetByIpAsync(string ipAddress, CancellationToken ct = default)
    {
        return await _context.Devices.FirstOrDefaultAsync(d => d.IPAddress == ipAddress, ct);
    }

    public async Task<IReadOnlyList<Device>> GetActiveDevicesAsync(CancellationToken ct = default)
    {
        return await _context.Devices
            .Where(d => d.IsActive)
            .OrderBy(d => d.Name)
            .ToListAsync(ct);
    }

    public async Task<int> GetOnlineCountAsync(CancellationToken ct = default)
    {
        return await _context.Devices
            .CountAsync(d => d.IsActive && d.Status == Shared.Enums.DeviceStatus.Online, ct);
    }

    public async Task UpdateStatusAsync(Guid deviceId, Shared.Enums.DeviceStatus status, CancellationToken ct = default)
    {
        var device = await _context.Devices.FirstOrDefaultAsync(d => d.Id == deviceId, ct);
        if (device == null) return;

        switch (status)
        {
            case Shared.Enums.DeviceStatus.Online:
                device.MarkOnline();
                break;
            case Shared.Enums.DeviceStatus.Offline:
                device.MarkOffline();
                break;
            case Shared.Enums.DeviceStatus.Error:
                device.MarkError();
                break;
        }

        await _context.SaveChangesAsync(ct);
    }
}

public interface IAttendanceTrackingRepository
{
    Task<int> GetTodayCheckInCountAsync(CancellationToken ct = default);
    Task<int> GetTodayLateCountAsync(CancellationToken ct = default);
    Task<IReadOnlyList<Attendance>> GetTodayAttendancesAsync(int limit = 100, CancellationToken ct = default);
    Task<IReadOnlyList<Attendance>> GetMemberAttendancesAsync(Guid memberId, int limit = 50, CancellationToken ct = default);
    Task<int> GetCheckedInMembersTodayAsync(CancellationToken ct = default);
}

public class AttendanceTrackingRepository : IAttendanceTrackingRepository
{
    private readonly Data.GymDbContext _context;
    private readonly ILogger<AttendanceTrackingRepository> _logger;

    public AttendanceTrackingRepository(Data.GymDbContext context, ILogger<AttendanceTrackingRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<int> GetTodayCheckInCountAsync(CancellationToken ct = default)
    {
        var today = DateTime.UtcNow.Date;
        return await _context.Attendances
            .CountAsync(a => a.CheckIn.Date == today, ct);
    }

    public async Task<int> GetTodayLateCountAsync(CancellationToken ct = default)
    {
        var today = DateTime.UtcNow.Date;
        var lateThreshold = new DateTime(today.Year, today.Month, today.Day, 9, 15, 0, DateTimeKind.Utc);
        return await _context.Attendances
            .CountAsync(a => a.CheckIn.Date == today && a.CheckIn > lateThreshold, ct);
    }

    public async Task<IReadOnlyList<Attendance>> GetTodayAttendancesAsync(int limit = 100, CancellationToken ct = default)
    {
        var today = DateTime.UtcNow.Date;
        return await _context.Attendances
            .Include(a => a.Member)
            .Include(a => a.Device)
            .Where(a => a.CheckIn.Date == today)
            .OrderByDescending(a => a.CheckIn)
            .Take(limit)
            .ToListAsync(ct);
    }

    public async Task<IReadOnlyList<Attendance>> GetMemberAttendancesAsync(Guid memberId, int limit = 50, CancellationToken ct = default)
    {
        return await _context.Attendances
            .Include(a => a.Member)
            .Include(a => a.Device)
            .Where(a => a.MemberId == memberId)
            .OrderByDescending(a => a.CheckIn)
            .Take(limit)
            .ToListAsync(ct);
    }

    public async Task<int> GetCheckedInMembersTodayAsync(CancellationToken ct = default)
    {
        var today = DateTime.UtcNow.Date;
        return await _context.Attendances
            .Where(a => a.CheckIn.Date == today)
            .Select(a => a.MemberId)
            .Distinct()
            .CountAsync(ct);
    }
}

public interface IAttendanceSummaryTrackingRepository
{
    Task<int> GetTodayAbsentCountAsync(int totalMembers, CancellationToken ct = default);
    Task<int> GetTodayOnLeaveCountAsync(CancellationToken ct = default);
    Task<AttendanceSummary?> GetTodaySummaryAsync(Guid memberId, CancellationToken ct = default);
}

public class AttendanceSummaryTrackingRepository : IAttendanceSummaryTrackingRepository
{
    private readonly Data.GymDbContext _context;
    private readonly ILogger<AttendanceSummaryTrackingRepository> _logger;

    public AttendanceSummaryTrackingRepository(Data.GymDbContext context, ILogger<AttendanceSummaryTrackingRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<int> GetTodayAbsentCountAsync(int totalMembers, CancellationToken ct = default)
    {
        var today = DateTime.UtcNow.Date;
        var checkedIn = await _context.Attendances
            .Where(a => a.CheckIn.Date == today)
            .Select(a => a.MemberId)
            .Distinct()
            .CountAsync(ct);
        return Math.Max(0, totalMembers - checkedIn);
    }

    public async Task<int> GetTodayOnLeaveCountAsync(CancellationToken ct = default)
    {
        var today = DateTime.UtcNow.Date;
        return await _context.AttendanceSummaries
            .CountAsync(s => s.Date == today && s.Status == Domain.Entities.AttendanceStatus.OnLeave, ct);
    }

    public async Task<AttendanceSummary?> GetTodaySummaryAsync(Guid memberId, CancellationToken ct = default)
    {
        var today = DateTime.UtcNow.Date;
        return await _context.AttendanceSummaries
            .FirstOrDefaultAsync(s => s.MemberId == memberId && s.Date == today, ct);
    }
}
