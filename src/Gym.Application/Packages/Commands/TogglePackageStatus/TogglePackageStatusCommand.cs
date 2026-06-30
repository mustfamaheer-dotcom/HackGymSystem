using Gym.Domain.Entities;
using Gym.Domain.Interfaces;
using Gym.Shared.Common;
using MediatR;

namespace Gym.Application.Packages.Commands.TogglePackageStatus;

public record TogglePackageStatusCommand(Guid Id, bool IsActive) : IRequest<Result>;

public class TogglePackageStatusCommandHandler : IRequestHandler<TogglePackageStatusCommand, Result>
{
    private readonly IRepository<Package> _repository;
    private readonly IUnitOfWork _unitOfWork;

    public TogglePackageStatusCommandHandler(IRepository<Package> repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(TogglePackageStatusCommand request, CancellationToken cancellationToken)
    {
        var package = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (package is null)
            return Result.Failure("Package not found");

        package.ToggleActive(request.IsActive);
        _repository.Update(package);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success("Package status updated");
    }
}
