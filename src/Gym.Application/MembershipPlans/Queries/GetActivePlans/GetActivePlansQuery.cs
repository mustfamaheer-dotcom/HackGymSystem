using AutoMapper;
using Gym.Application.MembershipPlans.DTOs;
using Gym.Domain.Entities;
using Gym.Domain.Interfaces;
using Gym.Shared.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gym.Application.MembershipPlans.Queries.GetActivePlans;

public record GetActivePlansQuery : IRequest<Result<List<PlanDto>>>;

public class GetActivePlansQueryHandler : IRequestHandler<GetActivePlansQuery, Result<List<PlanDto>>>
{
    private readonly IRepository<MembershipPlan> _repository;
    private readonly IMapper _mapper;

    public GetActivePlansQueryHandler(IRepository<MembershipPlan> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<Result<List<PlanDto>>> Handle(GetActivePlansQuery request, CancellationToken cancellationToken)
    {
        var plans = await _repository.FindAsync(p => p.IsActive, cancellationToken);
        var dtos = _mapper.Map<List<PlanDto>>(plans.ToList());
        return Result<List<PlanDto>>.Success(dtos);
    }
}
