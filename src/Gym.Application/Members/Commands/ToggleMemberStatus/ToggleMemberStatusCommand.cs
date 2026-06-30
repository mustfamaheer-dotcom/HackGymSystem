using FluentValidation;
using Gym.Domain.Entities;
using Gym.Domain.Interfaces;
using Gym.Shared.Common;
using MediatR;

namespace Gym.Application.Members.Commands.ToggleMemberStatus;

public record ToggleMemberStatusCommand(Guid Id, bool IsDeleted) : IRequest<Result>;

public class ToggleMemberStatusCommandHandler : IRequestHandler<ToggleMemberStatusCommand, Result>
{
    private readonly IRepository<Member> _repository;
    private readonly IUnitOfWork _unitOfWork;

    public ToggleMemberStatusCommandHandler(IRepository<Member> repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(ToggleMemberStatusCommand request, CancellationToken cancellationToken)
    {
        var member = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (member is null)
            return Result.Failure("Member not found");

        if (request.IsDeleted)
            member.SoftDelete();
        else
            member.Restore();

        _repository.Update(member);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success("Member status updated successfully");
    }
}

public class ToggleMemberStatusCommandValidator : AbstractValidator<ToggleMemberStatusCommand>
{
    public ToggleMemberStatusCommandValidator()
    {
        RuleFor(v => v.Id)
            .NotEmpty().WithMessage("Member ID is required");
    }
}
