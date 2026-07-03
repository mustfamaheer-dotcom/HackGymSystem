using FluentValidation;
using Microsoft.Extensions.Localization;
using Gym.Application.Resources;
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
    string? Notes,
    string? Email = null,
    Gender? Gender = null) : IRequest<Result<Guid>>;

public class CreateLeadCommandHandler : IRequestHandler<CreateLeadCommand, Result<Guid>>
{
    private readonly IRepository<Lead> _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IStringLocalizer<ApplicationResources> _localizer;

    public CreateLeadCommandHandler(IRepository<Lead> repository, IUnitOfWork unitOfWork, IStringLocalizer<ApplicationResources> localizer)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _localizer = localizer;
    }

    public async Task<Result<Guid>> Handle(CreateLeadCommand request, CancellationToken cancellationToken)
    {
        var lead = new Lead(request.Name, request.Phone, request.Source, request.InterestedPackageId, request.Notes, request.Email, request.Gender);
        await _repository.AddAsync(lead, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result<Guid>.Success(lead.Id, _localizer["Lead created successfully"]);
    }
}