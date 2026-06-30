using AutoMapper;
using Gym.Application.Devices.DTOs;
using Gym.Domain.Entities;
using Gym.Domain.Interfaces;
using Gym.Shared.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gym.Application.Devices.Queries.GetActiveDevices;

public record GetActiveDevicesQuery : IRequest<Result<List<DeviceDto>>>;

public class GetActiveDevicesQueryHandler : IRequestHandler<GetActiveDevicesQuery, Result<List<DeviceDto>>>
{
    private readonly IRepository<Device> _repository;
    private readonly IMapper _mapper;

    public GetActiveDevicesQueryHandler(IRepository<Device> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<Result<List<DeviceDto>>> Handle(GetActiveDevicesQuery request, CancellationToken cancellationToken)
    {
        var devices = await _repository.FindAsync(d => d.IsActive, cancellationToken);
        var dtos = _mapper.Map<List<DeviceDto>>(devices.ToList());
        return Result<List<DeviceDto>>.Success(dtos);
    }
}
