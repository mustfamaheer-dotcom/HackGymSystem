using FluentValidation;
using Microsoft.Extensions.Localization;
using Gym.Application;
using Gym.Application.Common.Events;
using Gym.Domain.Entities;
using Gym.Domain.Events;
using Gym.Domain.Interfaces;
using Gym.Shared.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gym.Application.Attendances.Commands.CheckOut;

public record CheckOutCommand(Guid AttendanceId, Guid? DeviceId = null, DateTime? DeviceTimestamp = null) : IRequest<Result>;

public class CheckOutCommandHandler : IRequestHandler<CheckOutCommand, Result>
{
    private readonly IRepository<Attendance> _attendanceRepository;
    private readonly IRepository<Member> _memberRepository;
    private readonly IRepository<AttendanceSummary> _summaryRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEventPublisher _eventPublisher;
    private readonly IStringLocalizer<ApplicationResources> _localizer;

    public CheckOutCommandHandler(
        IRepository<Attendance> attendanceRepository,
        IRepository<Member> memberRepository,
        IRepository<AttendanceSummary> summaryRepository,
        IUnitOfWork unitOfWork,
        IEventPublisher eventPublisher,
        IStringLocalizer<ApplicationResources> localizer)
    {
        _attendanceRepository = attendanceRepository;
        _memberRepository = memberRepository;
        _summaryRepository = summaryRepository;
        _unitOfWork = unitOfWork;
        _eventPublisher = eventPublisher;
        _localizer = localizer;
    }

    public async Task<Result> Handle(CheckOutCommand request, CancellationToken cancellationToken)
    {
        var attendance = await _attendanceRepository.GetByIdAsync(request.AttendanceId, cancellationToken);
        if (attendance == null)
            return Result.Failure(_localizer["Attendance not found"]);

        var member = await _memberRepository.GetByIdAsync(attendance.MemberId, cancellationToken);

        var checkOutTime = request.DeviceTimestamp ?? DateTime.UtcNow;
        attendance.SetCheckOut(checkOutTime);

        if (request.DeviceId.HasValue)
            attendance.AssignDevice(request.DeviceId.Value);

        _attendanceRepository.Update(attendance);

        await UpdateAttendanceSummaryAsync(attendance.MemberId, attendance.CheckIn, checkOutTime, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var workDuration = checkOutTime - attendance.CheckIn;
        await _eventPublisher.PublishAsync(new AttendanceRecordedEvent
        {
            MemberId = attendance.MemberId,
            MemberName = member?.FullName ?? "Unknown",
            Status = workDuration.TotalMinutes < 240 ? "HalfDay" : "Present",
            AttendanceTimestamp = checkOutTime,
            Method = attendance.IsManual ? "Manual" : "Device",
            IsManual = attendance.IsManual,
            Direction = "CheckOut"
        }, cancellationToken);

        return Result.Success();
    }

    private async Task UpdateAttendanceSummaryAsync(Guid memberId, DateTime checkIn, DateTime checkOut, CancellationToken cancellationToken)
    {
        var today = checkIn.Date;
        var summary = await _summaryRepository.FirstOrDefaultAsync(
            s => s.MemberId == memberId && s.Date == today, cancellationToken);

        if (summary != null)
        {
            summary.SetCheckOut(checkOut.TimeOfDay);
            summary.CalculateWorkDuration();

            if (summary.WorkDurationMinutes.HasValue && summary.WorkDurationMinutes.Value < 240)
            {
                summary.SetStatus(AttendanceStatus.HalfDay);
            }

            _summaryRepository.Update(summary);
        }
    }
}

public class CheckOutCommandValidator : AbstractValidator<CheckOutCommand>
{
    public CheckOutCommandValidator()
    {
        RuleFor(v => v.AttendanceId).NotEmpty();
    }
}
