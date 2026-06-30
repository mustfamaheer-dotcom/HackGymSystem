using AutoMapper;
using FluentValidation;
using Gym.Application.Common.DTOs;
using Gym.Application.Common.Mappings;
using Gym.Domain.Entities;
using Gym.Domain.Interfaces;
using Gym.Shared.Common;
using MediatR;

namespace Gym.Application.Attendances.Commands.CheckIn;

public record CheckInCommand(Guid MemberId, bool IsManual = false, Guid? DeviceId = null) : IRequest<Result<Guid>>;

public class CheckInCommandHandler : IRequestHandler<CheckInCommand, Result<Guid>>
{
    private readonly IRepository<Attendance> _attendanceRepository;
    private readonly IRepository<Member> _memberRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CheckInCommandHandler(
        IRepository<Attendance> attendanceRepository,
        IRepository<Member> memberRepository,
        IUnitOfWork unitOfWork)
    {
        _attendanceRepository = attendanceRepository;
        _memberRepository = memberRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(CheckInCommand request, CancellationToken cancellationToken)
    {
        var memberExists = await _memberRepository.AnyAsync(m => m.Id == request.MemberId, cancellationToken);
        if (!memberExists)
            return Result<Guid>.Failure("Member not found");

        var now = DateTime.UtcNow;
        var attendance = new Attendance(request.MemberId, now.Date, now.TimeOfDay, request.IsManual);

        if (request.DeviceId.HasValue)
            attendance.AssignDevice(request.DeviceId.Value);

        await _attendanceRepository.AddAsync(attendance, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<Guid>.Success(attendance.Id);
    }
}

public class CheckInCommandValidator : AbstractValidator<CheckInCommand>
{
    public CheckInCommandValidator()
    {
        RuleFor(v => v.MemberId).NotEmpty();
    }
}
