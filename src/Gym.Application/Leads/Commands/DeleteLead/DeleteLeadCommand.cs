using Gym.Domain.Entities;
using Gym.Domain.Interfaces;
using Gym.Shared.Common;
using MediatR;

namespace Gym.Application.Leads.Commands.DeleteLead;

public record DeleteLeadCommand(Guid Id) : IRequest<Result>;

public class DeleteLeadCommandHandler : IRequestHandler<DeleteLeadCommand, Result>
{
    private readonly IRepository<Lead> _repository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteLeadCommandHandler(IRepository<Lead> repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(DeleteLeadCommand request, CancellationToken cancellationToken)
    {
        var lead = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (lead == null)
            return Result.Failure("Lead not found");

        _repository.Delete(lead);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success("Lead deleted successfully");
    }
}