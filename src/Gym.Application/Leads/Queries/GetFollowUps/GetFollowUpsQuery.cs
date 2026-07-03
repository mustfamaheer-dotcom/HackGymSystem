using AutoMapper;
using AutoMapper.QueryableExtensions;
using Gym.Application.Leads.DTOs;
using Gym.Domain.Entities;
using Gym.Domain.Interfaces;
using Gym.Shared.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gym.Application.Leads.Queries.GetFollowUps;

public record GetFollowUpsQuery(Guid LeadId) : IRequest<Result<List<LeadFollowUpDto>>>;

public class GetFollowUpsQueryHandler : IRequestHandler<GetFollowUpsQuery, Result<List<LeadFollowUpDto>>>
{
    private readonly IRepository<LeadFollowUp> _repository;
    private readonly IMapper _mapper;

    public GetFollowUpsQueryHandler(IRepository<LeadFollowUp> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<Result<List<LeadFollowUpDto>>> Handle(GetFollowUpsQuery request, CancellationToken cancellationToken)
    {
        var items = await _repository.Query()
            .Where(f => f.LeadId == request.LeadId)
            .OrderByDescending(f => f.CreatedAt)
            .ProjectTo<LeadFollowUpDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        return Result<List<LeadFollowUpDto>>.Success(items);
    }
}