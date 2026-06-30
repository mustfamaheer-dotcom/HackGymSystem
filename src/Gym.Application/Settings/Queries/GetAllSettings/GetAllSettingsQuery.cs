using AutoMapper;
using AutoMapper.QueryableExtensions;
using Gym.Application.Settings.DTOs;
using Gym.Domain.Entities;
using Gym.Domain.Interfaces;
using Gym.Shared.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gym.Application.Settings.Queries.GetAllSettings;

public record GetAllSettingsQuery : IRequest<Result<List<SettingDto>>>;

public class GetAllSettingsQueryHandler : IRequestHandler<GetAllSettingsQuery, Result<List<SettingDto>>>
{
    private readonly IRepository<Setting> _repository;
    private readonly IMapper _mapper;

    public GetAllSettingsQueryHandler(IRepository<Setting> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<Result<List<SettingDto>>> Handle(GetAllSettingsQuery request, CancellationToken cancellationToken)
    {
        var settings = await _repository.Query()
            .OrderBy(s => s.Group)
            .ThenBy(s => s.Key)
            .ProjectTo<SettingDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        return Result<List<SettingDto>>.Success(settings);
    }
}
