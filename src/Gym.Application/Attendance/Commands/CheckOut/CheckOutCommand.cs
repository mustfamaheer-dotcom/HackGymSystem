using FluentValidation;
using Microsoft.Extensions.Localization;
using Gym.Application.Resources;
using Gym.Domain.Entities;
using Gym.Domain.Interfaces;
using Gym.Shared.Common;
using MediatR;

namespace Gym.Application.Attendances.Commands.CheckOut;

public record CheckOutCommand(Guid AttendanceId, Guid? DeviceId = null) : IRequest<Result>;

public class CheckOutCommandHandler : IRequestHandler<CheckOutCommand, Result>
{
    private readonly IRepository<Attendance> _attendanceRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IStringLocalizer<ApplicationResources> _localizer;

    public CheckOutCommandHandler(IRepository<Attendance> attendanceRepository, IUnitOfWork unitOfWork, IStringLocalizer<ApplicationResources> localizer)
    {
        _attendanceRepository = attendanceRepository;
        _unitOfWork = unitOfWork;
        _localizer = localizer;
    }

    public async Task<Result> Handle(CheckOutCommand request, CancellationToken cancellationToken)
    {
        var attendance = await _attendanceRepository.GetByIdAsync(request.AttendanceId, cancellationToken);
        if (attendance == null)
            return Result.Failure(_localizer["Attendance not found"]);

        attendance.SetCheckOut(DateTime.UtcNow);

        if (request.DeviceId.HasValue)
            attendance.AssignDevice(request.DeviceId.Value);

        _attendanceRepository.Update(attendance);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}

public class CheckOutCommandValidator : AbstractValidator<CheckOutCommand>
{
    public CheckOutCommandValidator()
    {
        RuleFor(v => v.AttendanceId).NotEmpty();
    }
}
