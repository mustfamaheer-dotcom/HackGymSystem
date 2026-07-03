using Gym.Domain.Entities;
using Gym.Domain.Interfaces;
using Gym.Shared.Common;
using Gym.Shared.Enums;
using MediatR;

namespace Gym.Application.Leads.Commands.CreateLead;

public record CreateLeadCommand(
    string Name,
    string Phone,
    LeadSource Source,
    Guid? InterestedPackageId,
    string? Notes) : IRequest<Result<Guid>>;

public class CreateLeadCommandHandler : IRequestHandler<CreateLeadCommand, Result<Guid>>
{
    private readonly IRepository<Lead> _repository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateLeadCommandHandler(IRepository<Lead> repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(CreateLeadCommand request, CancellationToken cancellationToken)
    {
        var lead = new Lead(request.Name, request.Phone, request.Source, request.InterestedPackageId, request.Notes);
        await _repository.AddAsync(lead, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result<Guid>.Success(lead.Id, "Lead created successfully");
    }
}