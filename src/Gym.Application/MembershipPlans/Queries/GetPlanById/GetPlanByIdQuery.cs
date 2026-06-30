using AutoMapper;
using Gym.Application.MembershipPlans.DTOs;
using Gym.Domain.Entities;
using Gym.Domain.Interfaces;
using Gym.Shared.Common;
using MediatR;

namespace Gym.Application.MembershipPlans.Queries.GetPlanById;

public record GetPlanByIdQuery(Guid Id) : IRequest<Result<PlanDto>>;

public class GetPlanByIdQueryHandler : IRequestHandler<GetPlanByIdQuery, Result<PlanDto>>
{
    private readonly IRepository<MembershipPlan> _repository;
    private readonly IMapper _mapper;

    public GetPlanByIdQueryHandler(IRepository<MembershipPlan> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<Result<PlanDto>> Handle(GetPlanByIdQuery request, CancellationToken cancellationToken)
    {
        var plan = await _repository.GetByIdAsync(request.Id, cancellationToken);

        if (plan is null)
            return Result<PlanDto>.Failure("Plan not found");

        var dto = _mapper.Map<PlanDto>(plan);
        return Result<PlanDto>.Success(dto);
    }
}
