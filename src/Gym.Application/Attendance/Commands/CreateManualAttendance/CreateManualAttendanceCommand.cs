using FluentValidation;
using Gym.Domain.Entities;
using Gym.Domain.Interfaces;
using Gym.Shared.Common;
using MediatR;

namespace Gym.Application.Attendances.Commands.CreateManualAttendance;

public record CreateManualAttendanceCommand(
    Guid MemberId,
    DateTime Date,
    TimeSpan Time,
    DateTime? CheckIn = null,
    DateTime? CheckOut = null,
    Guid? DeviceId = null) : IRequest<Result<Guid>>;

public class CreateManualAttendanceCommandHandler : IRequestHandler<CreateManualAttendanceCommand, Result<Guid>>
{
    private readonly IRepository<Attendance> _attendanceRepository;
    private readonly IRepository<Member> _memberRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateManualAttendanceCommandHandler(
        IRepository<Attendance> attendanceRepository,
        IRepository<Member> memberRepository,
        IUnitOfWork unitOfWork)
    {
        _attendanceRepository = attendanceRepository;
        _memberRepository = memberRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(CreateManualAttendanceCommand request, CancellationToken cancellationToken)
    {
        var memberExists = await _memberRepository.AnyAsync(m => m.Id == request.MemberId, cancellationToken);
        if (!memberExists)
            return Result<Guid>.Failure("Member not found");

        var attendance = new Attendance(request.MemberId, request.Date, request.Time, true);

        if (request.CheckIn.HasValue)
            attendance.CheckIn = request.CheckIn.Value;
        if (request.CheckOut.HasValue)
            attendance.CheckOut = request.CheckOut.Value;
        if (request.DeviceId.HasValue)
            attendance.AssignDevice(request.DeviceId.Value);

        await _attendanceRepository.AddAsync(attendance, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<Guid>.Success(attendance.Id);
    }
}

public class CreateManualAttendanceCommandValidator : AbstractValidator<CreateManualAttendanceCommand>
{
    public CreateManualAttendanceCommandValidator()
    {
        RuleFor(v => v.MemberId).NotEmpty();
        RuleFor(v => v.Date).NotEmpty();
        RuleFor(v => v.Time).NotEmpty();
    }
}
