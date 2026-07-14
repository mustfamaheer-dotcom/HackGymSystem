using FluentValidation;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Gym.Application;
using Gym.Application.Common.Events;
using Gym.Domain.Entities;
using Gym.Domain.Events;
using Gym.Domain.Interfaces;
using Gym.Shared.Common;
using MediatR;

namespace Gym.Application.Attendances.Commands.CheckIn;

public record CheckInCommand(Guid MemberId, bool IsManual = false, Guid? DeviceId = null, DateTime? DeviceTimestamp = null) : IRequest<Result<Guid>>;

public class CheckInCommandHandler : IRequestHandler<CheckInCommand, Result<Guid>>
{
    private readonly IRepository<Attendance> _attendanceRepository;
    private readonly IRepository<Member> _memberRepository;
    private readonly IRepository<AttendanceSummary> _summaryRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEventPublisher _eventPublisher;
    private readonly IStringLocalizer<ApplicationResources> _localizer;
    private readonly ILogger<CheckInCommandHandler> _logger;

    public CheckInCommandHandler(
        IRepository<Attendance> attendanceRepository,
        IRepository<Member> memberRepository,
        IRepository<AttendanceSummary> summaryRepository,
        IUnitOfWork unitOfWork,
        IEventPublisher eventPublisher,
        IStringLocalizer<ApplicationResources> localizer,
        ILogger<CheckInCommandHandler> logger)
    {
        _attendanceRepository = attendanceRepository;
        _memberRepository = memberRepository;
        _summaryRepository = summaryRepository;
        _unitOfWork = unitOfWork;
        _eventPublisher = eventPublisher;
        _localizer = localizer;
        _logger = logger;
    }

    public async Task<Result<Guid>> Handle(CheckInCommand request, CancellationToken cancellationToken)
    {
        var member = await _memberRepository.GetByIdAsync(request.MemberId, cancellationToken);
        if (member == null)
            return Result<Guid>.Failure(_localizer["Member not found"]);

        var checkInTime = request.DeviceTimestamp ?? DateTime.UtcNow;

        var existingToday = await _attendanceRepository.FirstOrDefaultAsync(
            a => a.MemberId == request.MemberId && a.CheckIn.Date == checkInTime.Date, cancellationToken);
        if (existingToday != null)
        {
            _logger.LogInformation("Duplicate check-in ignored for MemberId={MemberId}, existing AttendanceId={AttId}", request.MemberId, existingToday.Id);
            return Result<Guid>.Success(existingToday.Id);
        }

        var attendance = new Attendance(request.MemberId, checkInTime, request.IsManual);

        if (request.DeviceId.HasValue)
            attendance.AssignDevice(request.DeviceId.Value);

        await _attendanceRepository.AddAsync(attendance, cancellationToken);

        await UpdateAttendanceSummaryAsync(request.MemberId, checkInTime, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var status = CalculateAttendanceStatus(checkInTime);
        await _eventPublisher.PublishAsync(new AttendanceRecordedEvent
        {
            MemberId = request.MemberId,
            MemberName = member.FullName,
            Status = status.ToString(),
            AttendanceTimestamp = checkInTime,
            Method = request.IsManual ? "Manual" : "Device",
            IsManual = request.IsManual,
            Direction = "CheckIn"
        }, cancellationToken);

        return Result<Guid>.Success(attendance.Id);
    }

    private async Task UpdateAttendanceSummaryAsync(Guid memberId, DateTime checkInTime, CancellationToken cancellationToken)
    {
        var today = checkInTime.Date;
        var summary = await _summaryRepository.FirstOrDefaultAsync(
            s => s.MemberId == memberId && s.Date == today, cancellationToken);

        if (summary == null)
        {
            summary = new AttendanceSummary(memberId, today);
            summary.SetCheckIn(checkInTime.TimeOfDay);
            summary.SetStatus(CalculateAttendanceStatus(checkInTime));
            await _summaryRepository.AddAsync(summary, cancellationToken);
        }
        else
        {
            summary.SetCheckIn(checkInTime.TimeOfDay);
            summary.SetStatus(CalculateAttendanceStatus(checkInTime));
            _summaryRepository.Update(summary);
        }
    }

    private static AttendanceStatus CalculateAttendanceStatus(DateTime checkInTime)
    {
        var lateThreshold = new DateTime(checkInTime.Year, checkInTime.Month, checkInTime.Day, 9, 15, 0);
        return checkInTime > lateThreshold
            ? AttendanceStatus.Late
            : AttendanceStatus.Present;
    }
}

public class CheckInCommandValidator : AbstractValidator<CheckInCommand>
{
    public CheckInCommandValidator()
    {
        RuleFor(v => v.MemberId).NotEmpty();
    }
}
