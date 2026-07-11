using Gym.Application.Common.Events;
using Gym.Domain.Events;
using Microsoft.Extensions.Logging;

namespace Gym.Infrastructure.Events;

public class AttendanceEventHandler :
    IEventHandler<AttendanceRecordedEvent>,
    IEventHandler<AttendanceSyncedEvent>
{
    private readonly ILogger<AttendanceEventHandler> _logger;

    public AttendanceEventHandler(ILogger<AttendanceEventHandler> logger)
    {
        _logger = logger;
    }

    public Task HandleAsync(AttendanceRecordedEvent @event, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation(
            "[ATTENDANCE] {MemberName} {Status} at {Timestamp:HH:mm:ss} via {Method} ({Direction})",
            @event.MemberName, @event.Status, @event.AttendanceTimestamp, @event.Method, @event.Direction);

        return Task.CompletedTask;
    }

    public Task HandleAsync(AttendanceSyncedEvent @event, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation(
            "[SYNC] Device {Serial}: {Success} inserted, {Failed} skipped (total {Total})",
            @event.DeviceSerial, @event.SuccessCount, @event.FailedCount, @event.RecordsCount);

        return Task.CompletedTask;
    }
}
