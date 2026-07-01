using FluentValidation;
using Gym.Domain.Entities;
using Gym.Domain.Interfaces;
using Gym.Shared.Common;
using Gym.Shared.Enums;
using MediatR;

namespace Gym.Application.Offers.Commands.CreateOffer;

public record CreateOfferCommand(
    string OfferTitle,
    OfferType OfferType,
    DateTime StartDate,
    DateTime EndDate,
    Guid? LinkedPackageId = null,
    int? BonusMonths = null,
    int? BonusDays = null,
    decimal? OfferPrice = null,
    int? ExtraFreezeDays = null,
    string? Description = null) : IRequest<Result<Guid>>;

public class CreateOfferCommandHandler : IRequestHandler<CreateOfferCommand, Result<Guid>>
{
    private readonly IRepository<Offer> _repository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateOfferCommandHandler(IRepository<Offer> repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(CreateOfferCommand request, CancellationToken cancellationToken)
    {
        var offer = new Offer(
            request.OfferTitle, request.OfferType, request.StartDate, request.EndDate,
            request.LinkedPackageId, request.BonusMonths, request.BonusDays,
            request.OfferPrice, request.ExtraFreezeDays, request.Description);

        await _repository.AddAsync(offer, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<Guid>.Success(offer.Id, "Offer created successfully");
    }
}

public class CreateOfferCommandValidator : AbstractValidator<CreateOfferCommand>
{
    public CreateOfferCommandValidator()
    {
        RuleFor(x => x.OfferTitle)
            .NotEmpty().WithMessage("Offer title is required")
            .MaximumLength(200);

        RuleFor(x => x.OfferType).IsInEnum();

        RuleFor(x => x.EndDate)
            .GreaterThan(x => x.StartDate).WithMessage("End date must be after start date");

        When(x => x.OfferType == OfferType.BonusDuration, () =>
        {
            RuleFor(x => x.BonusMonths)
                .GreaterThan(0).When(x => x.BonusDays == null || x.BonusDays == 0)
                .WithMessage("Bonus months or bonus days must be provided");
            RuleFor(x => x.BonusDays)
                .GreaterThan(0).When(x => x.BonusMonths == null || x.BonusMonths == 0)
                .WithMessage("Bonus months or bonus days must be provided");
            RuleFor(x => x.OfferPrice).Null().WithMessage("Offer price must be null for bonus duration offers");
        });

        When(x => x.OfferType == OfferType.FixedPrice, () =>
        {
            RuleFor(x => x.OfferPrice)
                .NotNull().WithMessage("Offer price is required")
                .GreaterThan(0).WithMessage("Offer price must be positive");
            RuleFor(x => x.BonusMonths).Null().WithMessage("Bonus months must be null for fixed price offers");
            RuleFor(x => x.BonusDays).Null().WithMessage("Bonus days must be null for fixed price offers");
        });

        When(x => x.OfferType == OfferType.ExtraFreeze, () =>
        {
            RuleFor(x => x.ExtraFreezeDays)
                .NotNull().WithMessage("Extra freeze days is required")
                .GreaterThan(0).WithMessage("Extra freeze days must be positive");
        });

        When(x => x.OfferType == OfferType.FreeRegistration, () =>
        {
            RuleFor(x => x.OfferPrice).Null().WithMessage("Offer price must be null for free registration");
        });
    }
}
