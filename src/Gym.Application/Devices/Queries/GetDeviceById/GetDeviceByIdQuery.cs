using AutoMapper;
using Gym.Application.Devices.DTOs;
using Gym.Domain.Entities;
using Gym.Domain.Interfaces;
using Gym.Shared.Common;
using MediatR;

namespace Gym.Application.Devices.Queries.GetDeviceById;

public record GetDeviceByIdQuery(Guid Id) : IRequest<Result<DeviceDto>>;

public class GetDeviceByIdQueryHandler : IRequestHandler<GetDeviceByIdQuery, Result<DeviceDto>>
{
    private readonly IRepository<Device> _repository;
    private readonly IMapper _mapper;

    public GetDeviceByIdQueryHandler(IRepository<Device> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<Result<DeviceDto>> Handle(GetDeviceByIdQuery request, CancellationToken cancellationToken)
    {
        var device = await _repository.GetByIdAsync(request.Id, cancellationToken);

        if (device is null)
            return Result<DeviceDto>.Failure("Device not found");

        var dto = _mapper.Map<DeviceDto>(device);
        return Result<DeviceDto>.Success(dto);
    }
}
