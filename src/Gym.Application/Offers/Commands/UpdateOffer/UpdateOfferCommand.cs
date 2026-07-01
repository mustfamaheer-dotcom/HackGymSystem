using FluentValidation;
using Gym.Domain.Entities;
using Gym.Domain.Interfaces;
using Gym.Shared.Common;
using Gym.Shared.Enums;
using MediatR;

namespace Gym.Application.Offers.Commands.UpdateOffer;

public record UpdateOfferCommand(
    Guid Id,
    string OfferTitle,
    OfferType OfferType,
    DateTime StartDate,
    DateTime EndDate,
    Guid? LinkedPackageId = null,
    int? BonusMonths = null,
    int? BonusDays = null,
    decimal? OfferPrice = null,
    int? ExtraFreezeDays = null,
    string? Description = null) : IRequest<Result>;

public class UpdateOfferCommandHandler : IRequestHandler<UpdateOfferCommand, Result>
{
    private readonly IRepository<Offer> _repository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateOfferCommandHandler(IRepository<Offer> repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(UpdateOfferCommand request, CancellationToken cancellationToken)
    {
        var offer = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (offer is null)
            return Result.Failure("Offer not found");

        offer.Update(
            request.OfferTitle, request.OfferType, request.StartDate, request.EndDate,
            request.LinkedPackageId, request.BonusMonths, request.BonusDays,
            request.OfferPrice, request.ExtraFreezeDays, request.Description);

        _repository.Update(offer);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success("Offer updated successfully");
    }
}

public class UpdateOfferCommandValidator : AbstractValidator<UpdateOfferCommand>
{
    public UpdateOfferCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.OfferTitle).NotEmpty().MaximumLength(200);
        RuleFor(x => x.OfferType).IsInEnum();
        RuleFor(x => x.EndDate).GreaterThan(x => x.StartDate).WithMessage("End date must be after start date");

        When(x => x.OfferType == OfferType.BonusDuration, () =>
        {
            RuleFor(x => x.BonusMonths)
                .GreaterThan(0).When(x => x.BonusDays == null || x.BonusDays == 0);
            RuleFor(x => x.BonusDays)
                .GreaterThan(0).When(x => x.BonusMonths == null || x.BonusMonths == 0);
            RuleFor(x => x.OfferPrice).Null().WithMessage("Offer price must be null for bonus duration offers");
        });

        When(x => x.OfferType == OfferType.FixedPrice, () =>
        {
            RuleFor(x => x.OfferPrice).NotNull().GreaterThan(0);
            RuleFor(x => x.BonusMonths).Null();
            RuleFor(x => x.BonusDays).Null();
        });

        When(x => x.OfferType == OfferType.ExtraFreeze, () =>
        {
            RuleFor(x => x.ExtraFreezeDays).NotNull().GreaterThan(0);
        });
    }
}
