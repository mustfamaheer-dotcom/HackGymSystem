using AutoMapper;
using FluentValidation;
using Microsoft.Extensions.Localization;
using Gym.Application.Resources;
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
    private readonly IStringLocalizer<ApplicationResources> _localizer;

    public TogglePlanStatusCommandHandler(IRepository<MembershipPlan> repository, IUnitOfWork unitOfWork, IMapper mapper, IStringLocalizer<ApplicationResources> localizer)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _localizer = localizer;
    }

    public async Task<Result> Handle(TogglePlanStatusCommand request, CancellationToken cancellationToken)
    {
        var plan = await _repository.GetByIdAsync(request.Id, cancellationToken);

        if (plan is null)
            return Result.Failure(_localizer["Plan not found"]);

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
