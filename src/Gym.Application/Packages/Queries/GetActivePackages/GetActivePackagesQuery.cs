using AutoMapper;
using Gym.Application.Packages.DTOs;
using Gym.Domain.Entities;
using Gym.Domain.Interfaces;
using Gym.Shared.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gym.Application.Packages.Queries.GetActivePackages;

public record GetActivePackagesQuery : IRequest<Result<List<PackageDto>>>;

public class GetActivePackagesQueryHandler : IRequestHandler<GetActivePackagesQuery, Result<List<PackageDto>>>
{
    private readonly IRepository<Package> _repository;
    private readonly IMapper _mapper;

    public GetActivePackagesQueryHandler(IRepository<Package> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<Result<List<PackageDto>>> Handle(GetActivePackagesQuery request, CancellationToken cancellationToken)
    {
        var packages = await _repository.Query()
            .Where(p => p.IsActive)
            .OrderBy(p => p.PackageName)
            .ToListAsync(cancellationToken);

        var dtos = _mapper.Map<List<PackageDto>>(packages);
        return Result<List<PackageDto>>.Success(dtos);
    }
}
