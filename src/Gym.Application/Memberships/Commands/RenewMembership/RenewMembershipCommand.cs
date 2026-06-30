using AutoMapper;
using FluentValidation;
using Gym.Domain.Entities;
using Gym.Domain.Interfaces;
using Gym.Shared.Common;
using MediatR;

namespace Gym.Application.Memberships.Commands.RenewMembership;

public record RenewMembershipCommand(
    Guid Id,
    DateTime NewEndDate,
    int AdditionalDays,
    int? AdditionalVisits = null) : IRequest<Result>;

public class RenewMembershipCommandHandler : IRequestHandler<RenewMembershipCommand, Result>
{
    private readonly IRepository<Membership> _membershipRepo;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public RenewMembershipCommandHandler(
        IRepository<Membership> membershipRepo,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _membershipRepo = membershipRepo;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result> Handle(RenewMembershipCommand request, CancellationToken cancellationToken)
    {
        var membership = await _membershipRepo.GetByIdAsync(request.Id, cancellationToken);
        if (membership is null)
            return Result.Failure("Membership not found");

        if (request.NewEndDate <= membership.StartDate)
            return Result.Failure("New end date must be after the membership start date");

        membership.Renew(request.NewEndDate, request.AdditionalDays, request.AdditionalVisits);
        _membershipRepo.Update(membership);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success("Membership renewed successfully");
    }
}

public class RenewMembershipCommandValidator : AbstractValidator<RenewMembershipCommand>
{
    public RenewMembershipCommandValidator()
    {
        RuleFor(v => v.Id)
            .NotEmpty().WithMessage("Membership ID is required");

        RuleFor(v => v.NewEndDate)
            .NotEmpty().WithMessage("New end date is required")
            .GreaterThan(DateTime.UtcNow).WithMessage("New end date must be in the future");

        RuleFor(v => v.AdditionalDays)
            .GreaterThan(0).WithMessage("Additional days must be greater than zero");

        RuleFor(v => v.AdditionalVisits)
            .GreaterThanOrEqualTo(0).When(v => v.AdditionalVisits.HasValue)
            .WithMessage("Additional visits must be zero or greater");
    }
}
