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
    string? Notes) : IRequest<Result<Guid>>;

public class UpdateLeadCommandHandler : IRequestHandler<UpdateLeadCommand, Result<Guid>>
{
    private readonly IRepository<Lead> _repository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateLeadCommandHandler(IRepository<Lead> repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(UpdateLeadCommand request, CancellationToken cancellationToken)
    {
        var lead = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (lead == null)
            return Result<Guid>.Failure("Lead not found");

        lead.Update(request.Name, request.Phone, request.Source, request.InterestedPackageId, request.Status, request.NextFollowUpDate, request.Notes);
        _repository.Update(lead);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result<Guid>.Success(lead.Id, "Lead updated successfully");
    }
}