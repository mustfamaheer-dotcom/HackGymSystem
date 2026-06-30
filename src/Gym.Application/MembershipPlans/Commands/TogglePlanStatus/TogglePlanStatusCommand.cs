using AutoMapper;
using FluentValidation;
using Gym.Domain.Entities;
using Gym.Domain.Interfaces;
using Gym.Shared.Common;
using MediatR;

namespace Gym.Application.MembershipPlans.Commands.TogglePlanStatus;

public record TogglePlanStatusCommand(Guid Id, bool IsActive) : IRequest<Result>;

public class TogglePlanStatusCommandHandler : IRequestHandler<TogglePlanStatusCommand, Result>
{
    private readonly IRepository<MembershipPlan> _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public TogglePlanStatusCommandHandler(IRepository<MembershipPlan> repository, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result> Handle(TogglePlanStatusCommand request, CancellationToken cancellationToken)
    {
        var plan = await _repository.GetByIdAsync(request.Id, cancellationToken);

        if (plan is null)
            return Result.Failure("Plan not found");

        plan.ToggleActive(request.IsActive);
        _repository.Update(plan);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}

public class TogglePlanStatusCommandValidator : AbstractValidator<TogglePlanStatusCommand>
{
    public TogglePlanStatusCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
