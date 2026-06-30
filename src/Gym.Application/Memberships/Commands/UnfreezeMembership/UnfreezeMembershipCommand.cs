using AutoMapper;
using Gym.Domain.Entities;
using Gym.Domain.Interfaces;
using Gym.Shared.Common;
using MediatR;

namespace Gym.Application.Memberships.Commands.UnfreezeMembership;

public record UnfreezeMembershipCommand(Guid Id) : IRequest<Result>;

public class UnfreezeMembershipCommandHandler : IRequestHandler<UnfreezeMembershipCommand, Result>
{
    private readonly IRepository<Membership> _membershipRepo;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UnfreezeMembershipCommandHandler(
        IRepository<Membership> membershipRepo,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _membershipRepo = membershipRepo;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result> Handle(UnfreezeMembershipCommand request, CancellationToken cancellationToken)
    {
        var membership = await _membershipRepo.GetByIdAsync(request.Id, cancellationToken);
        if (membership is null)
            return Result.Failure("Membership not found");

        membership.Unfreeze();
        _membershipRepo.Update(membership);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success("Membership unfrozen successfully");
    }
}
