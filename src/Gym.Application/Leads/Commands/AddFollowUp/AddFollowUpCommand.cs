using FluentValidation;
using Microsoft.Extensions.Localization;
using Gym.Application;
using Gym.Domain.Entities;
using Gym.Domain.Interfaces;
using Gym.Shared.Common;
using MediatR;

namespace Gym.Application.Leads.Commands.AddFollowUp;

public record AddFollowUpCommand(Guid LeadId, string Notes) : IRequest<Result<Guid>>;

public class AddFollowUpCommandHandler : IRequestHandler<AddFollowUpCommand, Result<Guid>>
{
    private readonly IRepository<LeadFollowUp> _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IStringLocalizer<ApplicationResources> _localizer;

    public AddFollowUpCommandHandler(IRepository<LeadFollowUp> repository, IUnitOfWork unitOfWork, IStringLocalizer<ApplicationResources> localizer)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _localizer = localizer;
    }

    public async Task<Result<Guid>> Handle(AddFollowUpCommand request, CancellationToken cancellationToken)
    {
        var followUp = new LeadFollowUp(request.LeadId, request.Notes);
        await _repository.AddAsync(followUp, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result<Guid>.Success(followUp.Id, _localizer["Follow-up added successfully"]);
    }
}