using AutoMapper;
using FluentValidation;
using Microsoft.Extensions.Localization;
using Gym.Application.Resources;
using Gym.Domain.Entities;
using Gym.Domain.Interfaces;
using Gym.Shared.Common;
using MediatR;

namespace Gym.Application.MembershipPlans.Commands.DeletePlan;

public record DeletePlanCommand(Guid Id) : IRequest<Result>;

public class DeletePlanCommandHandler : IRequestHandler<DeletePlanCommand, Result>
{
    private readonly IRepository<MembershipPlan> _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IStringLocalizer<ApplicationResources> _localizer;

    public DeletePlanCommandHandler(IRepository<MembershipPlan> repository, IUnitOfWork unitOfWork, IMapper mapper, IStringLocalizer<ApplicationResources> localizer)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _localizer = localizer;
    }

    public async Task<Result> Handle(DeletePlanCommand request, CancellationToken cancellationToken)
    {
        var plan = await _repository.GetByIdAsync(request.Id, cancellationToken);

        if (plan is null)
            return Result.Failure(_localizer["Plan not found"]);

        _repository.Delete(plan);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}

public class DeletePlanCommandValidator : AbstractValidator<DeletePlanCommand>
{
    public DeletePlanCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
