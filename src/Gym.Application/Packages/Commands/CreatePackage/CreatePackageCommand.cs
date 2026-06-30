using AutoMapper;
using FluentValidation;
using Gym.Domain.Entities;
using Gym.Domain.Interfaces;
using Gym.Shared.Common;
using MediatR;

namespace Gym.Application.Packages.Commands.CreatePackage;

public record CreatePackageCommand(
    string PackageName,
    int DurationMonths,
    decimal Price,
    int? FreePeriodMonths,
    int? MaxFreezeDays) : IRequest<Result<Guid>>;

public class CreatePackageCommandHandler : IRequestHandler<CreatePackageCommand, Result<Guid>>
{
    private readonly IRepository<Package> _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreatePackageCommandHandler(IRepository<Package> repository, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<Guid>> Handle(CreatePackageCommand request, CancellationToken cancellationToken)
    {
        var package = new Package(
            request.PackageName,
            request.DurationMonths,
            request.Price,
            request.FreePeriodMonths,
            request.MaxFreezeDays);

        await _repository.AddAsync(package, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<Guid>.Success(package.Id);
    }
}

public class CreatePackageCommandValidator : AbstractValidator<CreatePackageCommand>
{
    public CreatePackageCommandValidator()
    {
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
