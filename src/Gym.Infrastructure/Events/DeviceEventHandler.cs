using Gym.Application.Common.Events;
using Gym.Domain.Entities;
using Gym.Domain.Events;
using Gym.Domain.Interfaces;
using Gym.Shared.Enums;
using Microsoft.Extensions.Logging;

namespace Gym.Infrastructure.Events;

public class DeviceEventHandler :
    IEventHandler<DeviceConnectedEvent>,
    IEventHandler<DeviceDisconnectedEvent>,
    IEventHandler<DeviceStatusChangedEvent>
{
    private readonly IRepository<Device> _deviceRepo;
    private readonly ILogger<DeviceEventHandler> _logger;

    public DeviceEventHandler(IRepository<Device> deviceRepo, ILogger<DeviceEventHandler> logger)
    {
        _deviceRepo = deviceRepo;
        _logger = logger;
    }

    public async Task HandleAsync(DeviceConnectedEvent @event, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation(
            "[DEVICE] Connected: {Serial} at {IP}",
            @event.DeviceSerial, @event.IPAddress);

        var device = await _deviceRepo.GetByIdAsync(@event.DeviceId, cancellationToken);
        if (device != null)
        {
            device.MarkOnline();
            _deviceRepo.Update(device);
        }
    }

    public async Task HandleAsync(DeviceDisconnectedEvent @event, CancellationToken cancellationToken = default)
    {
        _logger.LogWarning(
            "[DEVICE] Disconnected: {Serial} - {Reason}",
            @event.DeviceSerial, @event.Reason);

        var device = await _deviceRepo.GetByIdAsync(@event.DeviceId, cancellationToken);
        if (device != null)
        {
            device.MarkOffline();
            _deviceRepo.Update(device);
        }
    }

    public async Task HandleAsync(DeviceStatusChangedEvent @event, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation(
            "[DEVICE] Status changed: {Serial} {OldStatus} -> {NewStatus}",
            @event.DeviceSerial, @event.OldStatus, @event.NewStatus);

        var device = await _deviceRepo.GetByIdAsync(@event.DeviceId, cancellationToken);
        if (device != null)
        {
            switch (@event.NewStatus)
            {
                case DeviceStatus.Online:
                    device.MarkOnline();
                    break;
                case DeviceStatus.Offline:
                    device.MarkOffline();
                    break;
                case DeviceStatus.Error:
                    device.MarkError();
                    break;
            }
            _deviceRepo.Update(device);
        }
    }
}
