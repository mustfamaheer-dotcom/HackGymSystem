using AutoMapper;
using FluentValidation;
using Gym.Domain.Entities;
using Gym.Domain.Interfaces;
using Gym.Shared.Common;
using MediatR;

namespace Gym.Application.Devices.Commands.DeleteDevice;

public record DeleteDeviceCommand(Guid Id) : IRequest<Result>;

public class DeleteDeviceCommandHandler : IRequestHandler<DeleteDeviceCommand, Result>
{
    private readonly IRepository<Device> _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public DeleteDeviceCommandHandler(IRepository<Device> repository, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result> Handle(DeleteDeviceCommand request, CancellationToken cancellationToken)
    {
        var device = await _repository.GetByIdAsync(request.Id, cancellationToken);

        if (device is null)
            return Result.Failure("Device not found");

        _repository.Delete(device);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}

public class DeleteDeviceCommandValidator : AbstractValidator<DeleteDeviceCommand>
{
    public DeleteDeviceCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
