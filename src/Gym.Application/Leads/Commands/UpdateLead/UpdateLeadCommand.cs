using FluentValidation;
using Microsoft.Extensions.Localization;
using Gym.Application.Resources;
using Gym.Domain.Entities;
using Gym.Domain.Interfaces;
using Gym.Shared.Common;
using Gym.Shared.Enums;
using MediatR;

namespace Gym.Application.Leads.Commands.UpdateLead;

public record UpdateLeadCommand(
    Guid Id,
    string Name,
    string Phone,
    LeadSource Source,
    Guid? InterestedPackageId,
    LeadStatus Status,
    DateTime? NextFollowUpDate,
    string? Notes,
    string? Email = null,
    Gender? Gender = null) : IRequest<Result<Guid>>;

public class UpdateLeadCommandHandler : IRequestHandler<UpdateLeadCommand, Result<Guid>>
{
    private readonly IRepository<Lead> _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IStringLocalizer<ApplicationResources> _localizer;

    public UpdateLeadCommandHandler(IRepository<Lead> repository, IUnitOfWork unitOfWork, IStringLocalizer<ApplicationResources> localizer)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _localizer = localizer;
    }

    public async Task<Result<Guid>> Handle(UpdateLeadCommand request, CancellationToken cancellationToken)
    {
        var lead = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (lead == null)
            return Result<Guid>.Failure(_localizer["Lead not found"]);

        lead.Update(request.Name, request.Phone, request.Source, request.InterestedPackageId, request.Status, request.NextFollowUpDate, request.Notes, request.Email, request.Gender);
        _repository.Update(lead);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result<Guid>.Success(lead.Id, _localizer["Lead updated successfully"]);
    }
}