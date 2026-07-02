using FluentValidation;
using Microsoft.Extensions.Localization;
using Gym.Application.Resources;
using Gym.Domain.Entities;
using Gym.Domain.Interfaces;
using Gym.Shared.Common;
using MediatR;

namespace Gym.Application.Members.Commands.DeleteMember;

public record DeleteMemberCommand(Guid Id) : IRequest<Result>;

public class DeleteMemberCommandHandler : IRequestHandler<DeleteMemberCommand, Result>
{
    private readonly IRepository<Member> _repository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteMemberCommandHandler(IRepository<Member> repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(DeleteMemberCommand request, CancellationToken cancellationToken)
    {
        var member = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (member is null)
            return Result.Failure("Member not found");

        member.SoftDelete();
        _repository.Update(member);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success("Member deleted successfully");
    }
}

    public class DeleteMemberCommandValidator : AbstractValidator<DeleteMemberCommand>
    {
        private readonly IStringLocalizer<ApplicationResources> _localizer;

        public DeleteMemberCommandValidator(IStringLocalizer<ApplicationResources> localizer)
        {
            _localizer = localizer;
            RuleFor(v => v.Id)
                .NotEmpty().WithMessage(_localizer["Member ID is required"]);
        }
    }
