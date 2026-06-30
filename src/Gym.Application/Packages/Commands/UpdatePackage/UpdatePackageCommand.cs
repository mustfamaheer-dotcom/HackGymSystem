using FluentValidation;
using Gym.Domain.Entities;
using Gym.Domain.Interfaces;
using Gym.Shared.Common;
using MediatR;

namespace Gym.Application.Packages.Commands.UpdatePackage;

public record UpdatePackageCommand(
    Guid Id,
    string PackageName,
    int DurationMonths,
    decimal Price,
    int? FreePeriodMonths,
    int? MaxFreezeDays) : IRequest<Result>;

public class UpdatePackageCommandHandler : IRequestHandler<UpdatePackageCommand, Result>
{
    private readonly IRepository<Package> _repository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdatePackageCommandHandler(IRepository<Package> repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(UpdatePackageCommand request, CancellationToken cancellationToken)
    {
        var package = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (package is null)
            return Result.Failure("Package not found");

        package.Update(
            request.PackageName,
            request.DurationMonths,
            request.Price,
            request.FreePeriodMonths,
            request.MaxFreezeDays);

        _repository.Update(package);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success("Package updated successfully");
    }
}

public class UpdatePackageCommandValidator : AbstractValidator<UpdatePackageCommand>
{
    public UpdatePackageCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();

        RuleFor(x => x.PackageName)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.DurationMonths)
            .GreaterThan(0);

        RuleFor(x => x.Price)
            .GreaterThan(0);

        RuleFor(x => x.FreePeriodMonths)
            .GreaterThanOrEqualTo(0)
            .When(x => x.FreePeriodMonths.HasValue);

        RuleFor(x => x.MaxFreezeDays)
            .GreaterThanOrEqualTo(0)
            .When(x => x.MaxFreezeDays.HasValue);
    }
}
