using FluentValidation;
using Microsoft.Extensions.Localization;
using Gym.Application.Resources;
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
    private readonly IStringLocalizer<ApplicationResources> _localizer;

    public UpdateOfferCommandHandler(
        IRepository<Offer> repository,
        IUnitOfWork unitOfWork,
        IStringLocalizer<ApplicationResources> localizer)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _localizer = localizer;
    }

    public async Task<Result> Handle(UpdateOfferCommand request, CancellationToken cancellationToken)
    {
        var offer = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (offer is null)
            return Result.Failure(_localizer["Offer not found"]);

        offer.Update(
            request.OfferTitle, request.OfferType, request.StartDate, request.EndDate,
            request.LinkedPackageId, request.BonusMonths, request.BonusDays,
            request.OfferPrice, request.ExtraFreezeDays, request.Description);

        _repository.Update(offer);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(_localizer["Offer updated successfully"]);
    }
}

    public class UpdateOfferCommandValidator : AbstractValidator<UpdateOfferCommand>
    {
        private readonly IStringLocalizer<ApplicationResources> _localizer;

        public UpdateOfferCommandValidator(IStringLocalizer<ApplicationResources> localizer)
        {
            _localizer = localizer;
            RuleFor(x => x.Id).NotEmpty().WithMessage(_localizer["Offer ID is required"]);
            RuleFor(x => x.OfferTitle).NotEmpty().WithMessage(_localizer["Offer title is required"]).MaximumLength(200);
            RuleFor(x => x.OfferType).IsInEnum().WithMessage(_localizer["Invalid offer type"]);
            RuleFor(x => x.EndDate).GreaterThan(x => x.StartDate).WithMessage(_localizer["End date must be after start date"]);

            When(x => x.OfferType == OfferType.BonusDuration, () =>
            {
                RuleFor(x => x.BonusMonths)
                    .GreaterThan(0).When(x => x.BonusDays == null || x.BonusDays == 0)
                    .WithMessage(_localizer["Bonus months or bonus days must be provided"]);
                RuleFor(x => x.BonusDays)
                    .GreaterThan(0).When(x => x.BonusMonths == null || x.BonusMonths == 0)
                    .WithMessage(_localizer["Bonus months or bonus days must be provided"]);
                RuleFor(x => x.OfferPrice).Null().WithMessage(_localizer["Offer price must be null for bonus duration offers"]);
            });

            When(x => x.OfferType == OfferType.FixedPrice, () =>
            {
                RuleFor(x => x.OfferPrice).NotNull().WithMessage(_localizer["Offer price is required"]).GreaterThan(0).WithMessage(_localizer["Offer price must be positive"]);
                RuleFor(x => x.BonusMonths).Null().WithMessage(_localizer["Bonus months must be null for fixed price offers"]);
                RuleFor(x => x.BonusDays).Null().WithMessage(_localizer["Bonus days must be null for fixed price offers"]);
            });

            When(x => x.OfferType == OfferType.ExtraFreeze, () =>
            {
                RuleFor(x => x.ExtraFreezeDays).NotNull().WithMessage(_localizer["Extra freeze days is required"]).GreaterThan(0).WithMessage(_localizer["Extra freeze days must be positive"]);
            });
        }
    }
