using Gym.API.Hubs;
using Gym.API.WebSockets;
using Gym.Application.Attendances.Commands.CheckIn;
using Gym.Application.Attendances.Commands.CheckOut;
using Gym.Application.Common.Interfaces;
using Gym.Domain.Entities;
using Gym.Domain.Interfaces;
using Gym.Shared.Enums;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace Gym.API.Services;

public class AttendancePushResult
{
    public bool Success { get; set; }
    public string? Error { get; set; }
    public Guid? AttendanceId { get; set; }
    public string? Type { get; set; }
}

public class AttendancePushService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IHubContext<AttendanceHub> _hubContext;
    private readonly ILogger<AttendancePushService> _logger;

    public AttendancePushService(
        IServiceProvider serviceProvider,
        IHubContext<AttendanceHub> hubContext,
        ILogger<AttendancePushService> logger)
    {
        _serviceProvider = serviceProvider;
        _hubContext = hubContext;
        _logger = logger;
    }

    public async Task<AttendancePushResult> ProcessAttendanceAsync(
        AttendancePushPayload request, CancellationToken ct)
    {
        _logger.LogInformation("Processing attendance push: EnrollmentId={EnrollmentId}, Direction={Direction}, Timestamp={Timestamp}",
            request.EnrollmentId, request.Direction, request.Timestamp);

        using var scope = _serviceProvider.CreateScope();
        var sp = scope.ServiceProvider;
        var mediator = sp.GetRequiredService<IMediator>();
        var mappingRepo = sp.GetRequiredService<IDeviceMemberMappingRepository>();
        var subscriptionRepo = sp.GetRequiredService<IRepository<Subscription>>();
        var memberRepo = sp.GetRequiredService<IRepository<Member>>();
        var deviceRepo = sp.GetRequiredService<IRepository<Device>>();
        var attendanceRepo = sp.GetRequiredService<IRepository<Attendance>>();

        var mapping = await mappingRepo.GetByEnrollmentIdAsync(request.EnrollmentId, ct);
        if (mapping == null)
        {
            _logger.LogWarning("No member mapping found for EnrollmentId={EnrollmentId} — attendance rejected", request.EnrollmentId);
            return new AttendancePushResult { Success = false, Error = $"No member mapping for '{request.EnrollmentId}'" };
        }

        _logger.LogInformation("Mapping found: EnrollmentId={EnrollmentId} -> MemberId={MemberId}", request.EnrollmentId, mapping.MemberId);

        var hasActiveSub = await subscriptionRepo.AnyAsync(
            s => s.MemberId == mapping.MemberId
              && s.Status == SubscriptionStatus.Active
              && s.ExpirationDate > DateTime.UtcNow, ct);

        if (!hasActiveSub)
        {
            _logger.LogWarning("No active subscription for MemberId={MemberId} — attendance rejected", mapping.MemberId);
            return new AttendancePushResult { Success = false, Error = "No active subscription" };
        }

        var config = sp.GetRequiredService<Microsoft.Extensions.Options.IOptions<ZKTecoSettings>>().Value;
        var device = await deviceRepo.FirstOrDefaultAsync(d => d.IPAddress == config.DeviceIp, ct);
        if (device == null)
        {
            device = new Device("ZKMB2000", config.DeviceIp, config.DevicePort, "ZKMB2000", "");
            device.MarkOnline();
            await deviceRepo.AddAsync(device, ct);
        }

        if (request.Direction == 0)
        {
            _logger.LogInformation("Processing CHECK-IN for MemberId={MemberId}", mapping.MemberId);
            var result = await mediator.Send(new CheckInCommand(mapping.MemberId, false, device?.Id, request.Timestamp), ct);
            if (result.IsFailure)
            {
                _logger.LogWarning("Check-in failed for MemberId={MemberId}: {Error}", mapping.MemberId, result.Message);
                return new AttendancePushResult { Success = false, Error = result.Message };
            }

            _logger.LogInformation("Check-in succeeded for MemberId={MemberId}, AttendanceId={AttendanceId}", mapping.MemberId, result.Data);

            var member = await memberRepo.Query()
                .Include(m => m.Package)
                .FirstOrDefaultAsync(m => m.Id == mapping.MemberId, ct);

            await _hubContext.Clients.All.SendAsync("AttendancePushed", new
            {
                memberId = mapping.MemberId,
                memberName = member?.FullName ?? "",
                imagePath = member?.ImagePath ?? "",
                packageName = member?.Package?.Name ?? "",
                phoneNumber = member?.PhoneNumber ?? "",
                timestamp = request.Timestamp,
                type = "check-in",
                attendanceId = result.Data!
            }, ct);

            return new AttendancePushResult { Success = true, AttendanceId = result.Data, Type = "check-in" };
        }
        else
        {
            _logger.LogInformation("Processing CHECK-OUT for MemberId={MemberId}", mapping.MemberId);
            var existing = await attendanceRepo.FirstOrDefaultAsync(
                a => a.MemberId == mapping.MemberId && a.CheckIn.Date == request.Timestamp.Date && a.CheckOut == null, ct);
            if (existing == null)
            {
                _logger.LogWarning("No active check-in found for MemberId={MemberId} on {Date}", mapping.MemberId, request.Timestamp.Date);
                return new AttendancePushResult { Success = false, Error = "No active check-in found" };
            }

            var result = await mediator.Send(new CheckOutCommand(existing.Id, device?.Id, request.Timestamp), ct);
            if (result.IsFailure)
            {
                _logger.LogWarning("Check-out failed for MemberId={MemberId}: {Error}", mapping.MemberId, result.Message);
                return new AttendancePushResult { Success = false, Error = result.Message };
            }

            _logger.LogInformation("Check-out succeeded for MemberId={MemberId}, AttendanceId={AttendanceId}", mapping.MemberId, existing.Id);

            await _hubContext.Clients.All.SendAsync("AttendancePushed", new
            {
                memberId = mapping.MemberId,
                timestamp = request.Timestamp,
                type = "check-out",
                attendanceId = existing.Id
            }, ct);

            return new AttendancePushResult { Success = true, AttendanceId = existing.Id, Type = "check-out" };
        }
    }
}
