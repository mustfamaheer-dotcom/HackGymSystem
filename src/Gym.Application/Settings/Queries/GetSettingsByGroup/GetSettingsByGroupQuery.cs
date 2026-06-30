using AutoMapper;
using AutoMapper.QueryableExtensions;
using Gym.Application.Settings.DTOs;
using Gym.Domain.Entities;
using Gym.Domain.Interfaces;
using Gym.Shared.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gym.Application.Settings.Queries.GetSettingsByGroup;

public record GetSettingsByGroupQuery(string Group) : IRequest<Result<List<SettingDto>>>;

public class GetSettingsByGroupQueryHandler : IRequestHandler<GetSettingsByGroupQuery, Result<List<SettingDto>>>
{
    private readonly IRepository<Setting> _repository;
    private readonly IMapper _mapper;

    public GetSettingsByGroupQueryHandler(IRepository<Setting> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<Result<List<SettingDto>>> Handle(GetSettingsByGroupQuery request, CancellationToken cancellationToken)
    {
        var settings = await _repository.Query()
            .Where(s => s.Group == request.Group)
            .OrderBy(s => s.Key)
            .ProjectTo<SettingDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        return Result<List<SettingDto>>.Success(settings);
    }
}
