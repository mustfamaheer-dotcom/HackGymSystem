using AutoMapper;
using AutoMapper.QueryableExtensions;
using Gym.Application.Settings.DTOs;
using Gym.Domain.Entities;
using Gym.Domain.Interfaces;
using Gym.Shared.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gym.Application.Settings.Queries.GetSettingByKey;

public record GetSettingByKeyQuery(string Key) : IRequest<Result<SettingDto>>;

public class GetSettingByKeyQueryHandler : IRequestHandler<GetSettingByKeyQuery, Result<SettingDto>>
{
    private readonly IRepository<Setting> _repository;
    private readonly IMapper _mapper;

    public GetSettingByKeyQueryHandler(IRepository<Setting> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<Result<SettingDto>> Handle(GetSettingByKeyQuery request, CancellationToken cancellationToken)
    {
        var setting = await _repository.Query()
            .ProjectTo<SettingDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(s => s.Key == request.Key, cancellationToken);

        if (setting == null)
            return Result<SettingDto>.Failure("Setting not found");

        return Result<SettingDto>.Success(setting);
    }
}
