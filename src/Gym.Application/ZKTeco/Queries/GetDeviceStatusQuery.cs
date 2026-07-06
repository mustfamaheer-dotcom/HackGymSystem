using Gym.Application.Common.Interfaces;
using Gym.Application.ZKTeco.DTOs;
using Gym.Shared.Common;
using MediatR;

namespace Gym.Application.ZKTeco.Queries;

public record GetDeviceStatusQuery : IRequest<Result<DeviceStatusDto>>;

public class GetDeviceStatusQueryHandler : IRequestHandler<GetDeviceStatusQuery, Result<DeviceStatusDto>>
{
    private readonly IZKTecoBridgeClient _bridge;

    public GetDeviceStatusQueryHandler(IZKTecoBridgeClient bridge)
    {
        _bridge = bridge;
    }

    public async Task<Result<DeviceStatusDto>> Handle(GetDeviceStatusQuery request, CancellationToken cancellationToken)
    {
        var health = await _bridge.CheckHealthAsync(cancellationToken);

        return Result<DeviceStatusDto>.Success(new DeviceStatusDto
        {
            IsConnected = health.IsConnected,
            EnrolledUserCount = health.EnrolledUserCount,
            FreeMemory = health.FreeMemory,
            FirmwareVersion = health.FirmwareVersion,
            UptimeMs = health.UptimeMs
        });
    }
}
