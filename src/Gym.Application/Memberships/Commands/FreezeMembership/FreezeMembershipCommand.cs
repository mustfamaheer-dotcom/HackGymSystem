using AutoMapper;
using FluentValidation;
using Gym.Domain.Entities;
using Gym.Domain.Interfaces;
using Gym.Shared.Common;
using MediatR;

namespace Gym.Application.Memberships.Commands.FreezeMembership;

public record FreezeMembershipCommand(Guid Id, int FreezeDays) : IRequest<Result>;

public class FreezeMembershipCommandHandler : IRequestHandler<FreezeMembershipCommand, Result>
{
    private readonly IRepository<Membership> _membershipRepo;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public FreezeMembershipCommandHandler(
        IRepository<Membership> membershipRepo,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _membershipRepo = membershipRepo;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result> Handle(FreezeMembershipCommand request, CancellationToken cancellationToken)
    {
        var membership = await _membershipRepo.GetByIdAsync(request.Id, cancellationToken);
        if (membership is null)
            return Result.Failure("Membership not found");

        membership.Freeze(request.FreezeDays);
        _membershipRepo.Update(membership);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success("Membership frozen successfully");
    }
}

public class FreezeMembershipCommandValidator : AbstractValidator<FreezeMembershipCommand>
{
    public FreezeMembershipCommandValidator()
    {
        RuleFor(v => v.Id)
            .NotEmpty().WithMessage("Membership ID is required");

        RuleFor(v => v.FreezeDays)
            .GreaterThan(0).WithMessage("Freeze days must be greater than zero")
            .LessThanOrEqualTo(90).WithMessage("Freeze days must not exceed 90");
    }
}
