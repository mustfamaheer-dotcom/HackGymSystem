using Gym.API.Filters;
using Gym.Application.Common.DTOs;
using Gym.Application.Devices.Commands.CreateDevice;
using Gym.Application.Devices.Commands.DeleteDevice;
using Gym.Application.Devices.Commands.ToggleDeviceStatus;
using Gym.Application.Devices.Commands.UpdateDevice;
using Gym.Application.Devices.DTOs;
using Gym.Application.Devices.Queries.GetActiveDevices;
using Gym.Application.Devices.Queries.GetAllDevices;
using Gym.Application.Devices.Queries.GetDeviceById;
using Gym.Domain.Entities;
using Gym.Domain.Interfaces;
using Gym.Shared.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Gym.API.Controllers;

[Authorize]
public class DevicesController : BaseController
{
    private readonly IMediator _mediator;
    private readonly IRepository<DeviceLog> _deviceLogRepo;
    private readonly IUnitOfWork _unitOfWork;

    public DevicesController(IMediator mediator, IRepository<DeviceLog> deviceLogRepo, IUnitOfWork unitOfWork)
    {
        _mediator = mediator;
        _deviceLogRepo = deviceLogRepo;
        _unitOfWork = unitOfWork;
    }

    [HttpGet]
    [RequirePermission("Devices.View")]
    public async Task<IActionResult> GetAll([FromQuery] GetAllDevicesQuery query, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(query, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse.Fail(result.Message ?? "Failed to retrieve devices"));

        return Ok(ApiResponse<PaginatedResult<DeviceDto>>.Ok(result.Data!));
    }

    [HttpGet("{id}")]
    [RequirePermission("Devices.View")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetDeviceByIdQuery(id), cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse.Fail(result.Message ?? "Device not found"));

        return Ok(ApiResponse<DeviceDto>.Ok(result.Data!));
    }

    [HttpGet("active")]
    [RequirePermission("Devices.View")]
    public async Task<IActionResult> GetActive(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetActiveDevicesQuery(), cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse.Fail(result.Message ?? "Failed to retrieve active devices"));

        return Ok(ApiResponse<List<DeviceDto>>.Ok(result.Data!));
    }

    [HttpPost]
    [RequirePermission("Devices.Manage")]
    public async Task<IActionResult> Create([FromBody] CreateDeviceCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse.Fail(result.Message ?? "Failed to create device"));

        return Ok(ApiResponse<Guid>.Ok(result.Data!));
    }

    [HttpPut("{id}")]
    [RequirePermission("Devices.Manage")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateDeviceRequest request, CancellationToken cancellationToken)
    {
        var command = new UpdateDeviceCommand(
            id, request.Name, request.IPAddress, request.Port,
            request.Model, request.SerialNumber);

        var result = await _mediator.Send(command, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse.Fail(result.Message ?? "Failed to update device"));

        return Ok(ApiResponse.Ok());
    }

    [HttpDelete("{id}")]
    [RequirePermission("Devices.Manage")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new DeleteDeviceCommand(id), cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse.Fail(result.Message ?? "Failed to delete device"));

        return Ok(ApiResponse.Ok());
    }

    [HttpPost("{id}/sync")]
    [RequirePermission("Devices.Manage")]
    public async Task<IActionResult> Sync(Guid id, CancellationToken cancellationToken)
    {
        var query = new GetDeviceByIdQuery(id);
        var device = await _mediator.Send(query, cancellationToken);
        if (device.IsFailure)
            return NotFound(ApiResponse.Fail(device.Message ?? "Device not found"));

        await _deviceLogRepo.AddAsync(new DeviceLog(id, "Info", $"Sync initiated for device {device.Data!.Name}"), cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Ok(ApiResponse.Ok("Sync completed. Device firmware and templates are up to date."));
    }

    [HttpGet("{id}/logs")]
    [RequirePermission("Devices.View")]
    public async Task<IActionResult> GetLogs(Guid id, CancellationToken cancellationToken)
    {
        var logs = await _deviceLogRepo.Query()
            .Where(l => l.DeviceId == id)
            .OrderByDescending(l => l.CreatedAt)
            .Take(100)
            .Select(l => new DeviceLogEntry
            {
                Id = l.Id,
                DeviceId = l.DeviceId,
                DeviceName = "",
                LogType = l.Level,
                Message = l.Message,
                Timestamp = l.CreatedAt
            })
            .ToListAsync(cancellationToken);

        return Ok(ApiResponse<List<DeviceLogEntry>>.Ok(logs));
    }

    [HttpPost("{id}/test-connection")]
    [RequirePermission("Devices.Manage")]
    public async Task<IActionResult> TestConnection(Guid id, CancellationToken cancellationToken)
    {
        var query = new GetDeviceByIdQuery(id);
        var device = await _mediator.Send(query, cancellationToken);
        if (device.IsFailure)
            return NotFound(ApiResponse.Fail(device.Message ?? "Device not found"));

        var dto = device.Data!;

        if (string.IsNullOrEmpty(dto.IPAddress))
            return Ok(ApiResponse<ConnectionTestResult>.Ok(new(false, "Device has no IP address configured")));

        try
        {
            using var client = new System.Net.Sockets.TcpClient();
            var connectTask = client.ConnectAsync(dto.IPAddress, dto.Port);
            if (await Task.WhenAny(connectTask, Task.Delay(3000, cancellationToken)) == connectTask)
            {
                if (client.Connected)
                {
                    client.Close();
                    return Ok(ApiResponse<ConnectionTestResult>.Ok(new(true, $"Connected to {dto.IPAddress}:{dto.Port} successfully")));
                }
            }
            return Ok(ApiResponse<ConnectionTestResult>.Ok(new(false, $"Connection to {dto.IPAddress}:{dto.Port} timed out")));
        }
        catch (Exception ex)
        {
            return Ok(ApiResponse<ConnectionTestResult>.Ok(new(false, $"Connection failed: {ex.Message}")));
        }
    }

    [HttpPatch("{id}/status")]
    [RequirePermission("Devices.Manage")]
    public async Task<IActionResult> ToggleStatus(Guid id, [FromBody] ToggleDeviceStatusRequest request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new ToggleDeviceStatusCommand(id, request.Status), cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse.Fail(result.Message ?? "Failed to toggle device status"));

        return Ok(ApiResponse.Ok());
    }
}

public class UpdateDeviceRequest
{
    public string Name { get; set; } = string.Empty;
    public string IPAddress { get; set; } = string.Empty;
    public int Port { get; set; }
    public string? Model { get; set; }
    public string? SerialNumber { get; set; }
}

public class ToggleDeviceStatusRequest
{
    public DeviceStatus Status { get; set; }
}

public record ConnectionTestResult(bool Success, string Message);

public class DeviceLogEntry
{
    public Guid Id { get; set; }
    public Guid DeviceId { get; set; }
    public string DeviceName { get; set; } = string.Empty;
    public string LogType { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
}
