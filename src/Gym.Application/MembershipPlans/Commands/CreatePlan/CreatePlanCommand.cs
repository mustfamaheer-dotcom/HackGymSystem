using AutoMapper;
using FluentValidation;
using Gym.Domain.Entities;
using Gym.Domain.Interfaces;
using Gym.Shared.Common;
using MediatR;

namespace Gym.Application.MembershipPlans.Commands.CreatePlan;

public record CreatePlanCommand(
    string Name,
    decimal Price,
    int DurationDays,
    int? MaxVisits,
    int? FreezeDays,
    string? Description) : IRequest<Result<Guid>>;

public class CreatePlanCommandHandler : IRequestHandler<CreatePlanCommand, Result<Guid>>
{
    private readonly IRepository<MembershipPlan> _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreatePlanCommandHandler(IRepository<MembershipPlan> repository, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<Guid>> Handle(CreatePlanCommand request, CancellationToken cancellationToken)
    {
        var plan = new MembershipPlan(
            request.Name,
            request.Price,
            request.DurationDays,
            request.MaxVisits,
            request.FreezeDays,
            request.Description);

        await _repository.AddAsync(plan, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<Guid>.Success(plan.Id);
    }
}

public class CreatePlanCommandValidator : AbstractValidator<CreatePlanCommand>
{
    public CreatePlanCommandValidator()
    {
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
