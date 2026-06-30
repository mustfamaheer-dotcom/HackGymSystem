using AutoMapper;
using Gym.Application.Packages.DTOs;
using Gym.Domain.Entities;
using Gym.Domain.Interfaces;
using Gym.Shared.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gym.Application.Packages.Queries.GetPackageById;

public record GetPackageByIdQuery(Guid Id) : IRequest<Result<PackageDto>>;

public class GetPackageByIdQueryHandler : IRequestHandler<GetPackageByIdQuery, Result<PackageDto>>
{
    private readonly IRepository<Package> _repository;
    private readonly IMapper _mapper;

    public GetPackageByIdQueryHandler(IRepository<Package> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<Result<PackageDto>> Handle(GetPackageByIdQuery request, CancellationToken cancellationToken)
    {
        var package = await _repository.Query()
            .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

        if (package is null)
            return Result<PackageDto>.Failure("Package not found");

        var dto = _mapper.Map<PackageDto>(package);
        return Result<PackageDto>.Success(dto);
    }
}
