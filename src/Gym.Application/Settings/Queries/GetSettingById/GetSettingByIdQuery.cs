using AutoMapper;
using AutoMapper.QueryableExtensions;
using Gym.Application.Settings.DTOs;
using Gym.Domain.Entities;
using Gym.Domain.Interfaces;
using Gym.Shared.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gym.Application.Settings.Queries.GetSettingById;

public record GetSettingByIdQuery(Guid Id) : IRequest<Result<SettingDto>>;

public class GetSettingByIdQueryHandler : IRequestHandler<GetSettingByIdQuery, Result<SettingDto>>
{
    private readonly IRepository<Setting> _repository;
    private readonly IMapper _mapper;

    public GetSettingByIdQueryHandler(IRepository<Setting> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<Result<SettingDto>> Handle(GetSettingByIdQuery request, CancellationToken cancellationToken)
    {
        var setting = await _repository.Query()
            .ProjectTo<SettingDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(s => s.Id == request.Id, cancellationToken);

        if (setting == null)
            return Result<SettingDto>.Failure("Setting not found");

        return Result<SettingDto>.Success(setting);
    }
}
