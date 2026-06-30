using AutoMapper;
using FluentValidation;
using Gym.Domain.Entities;
using Gym.Domain.Interfaces;
using Gym.Shared.Common;
using MediatR;

namespace Gym.Application.MembershipPlans.Commands.UpdatePlan;

public record UpdatePlanCommand(
    Guid Id,
    string Name,
    decimal Price,
    int DurationDays,
    int? MaxVisits,
    int? FreezeDays,
    string? Description) : IRequest<Result>;

public class UpdatePlanCommandHandler : IRequestHandler<UpdatePlanCommand, Result>
{
    private readonly IRepository<MembershipPlan> _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdatePlanCommandHandler(IRepository<MembershipPlan> repository, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result> Handle(UpdatePlanCommand request, CancellationToken cancellationToken)
    {
        var plan = await _repository.GetByIdAsync(request.Id, cancellationToken);

        if (plan is null)
            return Result.Failure("Plan not found");

        plan.Update(
            request.Name,
            request.Price,
            request.DurationDays,
            request.MaxVisits,
            request.FreezeDays,
            request.Description);

        _repository.Update(plan);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}

public class UpdatePlanCommandValidator : AbstractValidator<UpdatePlanCommand>
{
    public UpdatePlanCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();

        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Price)
            .GreaterThan(0);

        RuleFor(x => x.DurationDays)
            .GreaterThan(0);

        RuleFor(x => x.MaxVisits)
            .GreaterThanOrEqualTo(0)
            .When(x => x.MaxVisits.HasValue);

        RuleFor(x => x.FreezeDays)
            .GreaterThanOrEqualTo(0)
            .When(x => x.FreezeDays.HasValue);
    }
}
