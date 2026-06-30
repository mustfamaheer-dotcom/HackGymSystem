using Gym.Domain.Entities;
using Gym.Domain.Interfaces;
using Gym.Shared.Common;
using MediatR;

namespace Gym.Application.Packages.Commands.DeletePackage;

public record DeletePackageCommand(Guid Id) : IRequest<Result>;

public class DeletePackageCommandHandler : IRequestHandler<DeletePackageCommand, Result>
{
    private readonly IRepository<Package> _repository;
    private readonly IUnitOfWork _unitOfWork;

    public DeletePackageCommandHandler(IRepository<Package> repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(DeletePackageCommand request, CancellationToken cancellationToken)
    {
        var package = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (package is null)
            return Result.Failure("Package not found");

        _repository.Delete(package);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success("Package deleted successfully");
    }
}
