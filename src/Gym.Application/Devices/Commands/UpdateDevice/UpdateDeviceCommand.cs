using System.Text.RegularExpressions;
using AutoMapper;
using FluentValidation;
using Gym.Domain.Entities;
using Gym.Domain.Interfaces;
using Gym.Shared.Common;
using MediatR;

namespace Gym.Application.Devices.Commands.UpdateDevice;

public record UpdateDeviceCommand(
    Guid Id,
    string Name,
    string IPAddress,
    int Port,
    string? Model,
    string? SerialNumber) : IRequest<Result>;

public class UpdateDeviceCommandHandler : IRequestHandler<UpdateDeviceCommand, Result>
{
    private readonly IRepository<Device> _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateDeviceCommandHandler(IRepository<Device> repository, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result> Handle(UpdateDeviceCommand request, CancellationToken cancellationToken)
    {
        var device = await _repository.GetByIdAsync(request.Id, cancellationToken);

        if (device is null)
            return Result.Failure("Device not found");

        device.Name = request.Name;
        device.IPAddress = request.IPAddress;
        device.Port = request.Port;
        device.Model = request.Model;
        device.SerialNumber = request.SerialNumber;
        device.MarkUpdated();

        _repository.Update(device);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}

public class UpdateDeviceCommandValidator : AbstractValidator<UpdateDeviceCommand>
{
    public UpdateDeviceCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();

        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.IPAddress)
            .NotEmpty()
            .Must(BeValidIpAddress).WithMessage("Invalid IP address format");

        RuleFor(x => x.Port)
            .GreaterThan(0);
    }

    private static bool BeValidIpAddress(string ipAddress)
    {
        return Regex.IsMatch(ipAddress,
            @"^(\d{1,3}\.){3}\d{1,3}$");
    }
}
