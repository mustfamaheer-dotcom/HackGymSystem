using AutoMapper;
using FluentValidation;
using Gym.Domain.Entities;
using Gym.Domain.Interfaces;
using Gym.Shared.Common;
using Gym.Shared.Enums;
using MediatR;

namespace Gym.Application.Devices.Commands.ToggleDeviceStatus;

public record ToggleDeviceStatusCommand(Guid Id, DeviceStatus Status) : IRequest<Result>;

public class ToggleDeviceStatusCommandHandler : IRequestHandler<ToggleDeviceStatusCommand, Result>
{
    private readonly IRepository<Device> _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ToggleDeviceStatusCommandHandler(IRepository<Device> repository, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result> Handle(ToggleDeviceStatusCommand request, CancellationToken cancellationToken)
    {
        var device = await _repository.GetByIdAsync(request.Id, cancellationToken);

        if (device is null)
            return Result.Failure("Device not found");

        device.Status = request.Status;
        device.MarkUpdated();
        _repository.Update(device);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}

public class ToggleDeviceStatusCommandValidator : AbstractValidator<ToggleDeviceStatusCommand>
{
    public ToggleDeviceStatusCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Status).IsInEnum();
    }
}
