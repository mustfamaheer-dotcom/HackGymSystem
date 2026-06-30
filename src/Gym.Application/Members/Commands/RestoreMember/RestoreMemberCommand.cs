using FluentValidation;
using Gym.Domain.Interfaces;
using Gym.Shared.Common;
using MediatR;

namespace Gym.Application.Members.Commands.RestoreMember;

public record RestoreMemberCommand(Guid Id) : IRequest<Result>;

public class RestoreMemberCommandHandler : IRequestHandler<RestoreMemberCommand, Result>
{
    private readonly IMemberRepository _memberRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RestoreMemberCommandHandler(IMemberRepository memberRepository, IUnitOfWork unitOfWork)
    {
        _memberRepository = memberRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(RestoreMemberCommand request, CancellationToken cancellationToken)
    {
        var member = await _memberRepository.GetByIdAsync(request.Id, true, cancellationToken);
        if (member is null)
            return Result.Failure("Member not found");

        member.Restore();
        _memberRepository.Update(member);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success("Member restored successfully");
    }
}

public class RestoreMemberCommandValidator : AbstractValidator<RestoreMemberCommand>
{
    public RestoreMemberCommandValidator()
    {
        RuleFor(v => v.Id)
            .NotEmpty().WithMessage("Member ID is required");
    }
}
