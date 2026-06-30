using AutoMapper;
using AutoMapper.QueryableExtensions;
using Gym.Application.Common.DTOs;
using Gym.Application.Devices.DTOs;
using Gym.Domain.Entities;
using Gym.Domain.Interfaces;
using Gym.Shared.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gym.Application.Devices.Queries.GetAllDevices;

public record GetAllDevicesQuery : PaginationRequest, IRequest<Result<PaginatedResult<DeviceDto>>>;

public class GetAllDevicesQueryHandler : IRequestHandler<GetAllDevicesQuery, Result<PaginatedResult<DeviceDto>>>
{
    private readonly IRepository<Device> _repository;
    private readonly IMapper _mapper;

    public GetAllDevicesQueryHandler(IRepository<Device> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<Result<PaginatedResult<DeviceDto>>> Handle(GetAllDevicesQuery request, CancellationToken cancellationToken)
    {
        var query = _repository.Query();

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            query = query.Where(d => d.Name.Contains(request.SearchTerm));

        query = (request.SortBy?.ToLower()) switch
        {
            "name" => request.SortDescending ? query.OrderByDescending(d => d.Name) : query.OrderBy(d => d.Name),
            "status" => request.SortDescending ? query.OrderByDescending(d => d.Status) : query.OrderBy(d => d.Status),
            "lastconnectedat" => request.SortDescending ? query.OrderByDescending(d => d.LastConnectedAt) : query.OrderBy(d => d.LastConnectedAt),
            "createdat" => request.SortDescending ? query.OrderByDescending(d => d.CreatedAt) : query.OrderBy(d => d.CreatedAt),
            _ => query.OrderBy(d => d.Name)
        };

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ProjectTo<DeviceDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        var result = new PaginatedResult<DeviceDto>
        {
            Items = items,
            TotalCount = totalCount,
            Page = request.Page,
            PageSize = request.PageSize
        };

        return Result<PaginatedResult<DeviceDto>>.Success(result);
    }
}
