using System.Text.RegularExpressions;
using AutoMapper;
using FluentValidation;
using Microsoft.Extensions.Localization;
using Gym.Application.Resources;
using Gym.Domain.Entities;
using Gym.Domain.Interfaces;
using Gym.Shared.Common;
using MediatR;

namespace Gym.Application.Devices.Commands.CreateDevice;

public record CreateDeviceCommand(
    string Name,
    string IPAddress,
    int Port,
    string? Model,
    string? SerialNumber) : IRequest<Result<Guid>>;

public class CreateDeviceCommandHandler : IRequestHandler<CreateDeviceCommand, Result<Guid>>
{
    private readonly IRepository<Device> _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateDeviceCommandHandler(IRepository<Device> repository, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<Guid>> Handle(CreateDeviceCommand request, CancellationToken cancellationToken)
    {
        var device = new Device(
            request.Name,
            request.IPAddress,
            request.Port,
            request.Model,
            request.SerialNumber);

        await _repository.AddAsync(device, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<Guid>.Success(device.Id);
    }
}

public class CreateDeviceCommandValidator : AbstractValidator<CreateDeviceCommand>
{
    private readonly IStringLocalizer<ApplicationResources> _localizer;

    public CreateDeviceCommandValidator(IStringLocalizer<ApplicationResources> localizer)
    {
        _localizer = localizer;
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.IPAddress)
            .NotEmpty()
            .Must(BeValidIpAddress).WithMessage(_localizer["Invalid IP address format"]);

        RuleFor(x => x.Port)
            .GreaterThan(0);
    }

    private static bool BeValidIpAddress(string ipAddress)
    {
        return Regex.IsMatch(ipAddress,
            @"^(\d{1,3}\.){3}\d{1,3}$");
    }
}
