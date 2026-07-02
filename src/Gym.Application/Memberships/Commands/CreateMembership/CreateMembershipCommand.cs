using AutoMapper;
using FluentValidation;
using Microsoft.Extensions.Localization;
using Gym.Application.Resources;
using Gym.Domain.Entities;
using Gym.Domain.Interfaces;
using Gym.Shared.Common;
using MediatR;

namespace Gym.Application.Memberships.Commands.CreateMembership;

public record CreateMembershipCommand(
    Guid MemberId,
    Guid PlanId,
    DateTime StartDate,
    string? Notes = null) : IRequest<Result<Guid>>;

public class CreateMembershipCommandHandler : IRequestHandler<CreateMembershipCommand, Result<Guid>>
{
    private readonly IRepository<Membership> _membershipRepo;
    private readonly IRepository<Member> _memberRepo;
    private readonly IRepository<MembershipPlan> _planRepo;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateMembershipCommandHandler(
        IRepository<Membership> membershipRepo,
        IRepository<Member> memberRepo,
        IRepository<MembershipPlan> planRepo,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _membershipRepo = membershipRepo;
        _memberRepo = memberRepo;
        _planRepo = planRepo;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<Guid>> Handle(CreateMembershipCommand request, CancellationToken cancellationToken)
    {
        var memberExists = await _memberRepo.AnyAsync(m => m.Id == request.MemberId, cancellationToken);
        if (!memberExists)
            return Result<Guid>.Failure("Member not found");

        var plan = await _planRepo.FirstOrDefaultAsync(p => p.Id == request.PlanId, cancellationToken);
        if (plan == null)
            return Result<Guid>.Failure("Membership plan not found");

        var endDate = request.StartDate.AddDays(plan.DurationDays);
        var remainingDays = plan.DurationDays;
        var remainingVisits = plan.MaxVisits ?? 0;

        var membership = new Membership(
            request.MemberId,
            request.PlanId,
            request.StartDate,
            endDate,
            remainingDays,
            remainingVisits)
        {
            Notes = request.Notes
        };

        await _membershipRepo.AddAsync(membership, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<Guid>.Success(membership.Id, "Membership created successfully");
    }
}

public class CreateMembershipCommandValidator : AbstractValidator<CreateMembershipCommand>
{
    private readonly IStringLocalizer<ApplicationResources> _localizer;

    public CreateMembershipCommandValidator(IStringLocalizer<ApplicationResources> localizer)
    {
        _localizer = localizer;
        RuleFor(v => v.MemberId)
            .NotEmpty().WithMessage(_localizer["Member is required"]);

        RuleFor(v => v.PlanId)
            .NotEmpty().WithMessage(_localizer["Plan is required"]);

        RuleFor(v => v.StartDate)
            .NotEmpty().WithMessage(_localizer["Start date is required"]);
    }
}
