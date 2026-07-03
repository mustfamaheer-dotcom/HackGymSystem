using FluentValidation;
using Microsoft.Extensions.Localization;
using Gym.Application.Resources;
using Gym.Domain.Entities;
using Gym.Domain.Interfaces;
using Gym.Shared.Common;
using MediatR;

namespace Gym.Application.Offers.Commands.DeleteOffer;

public record DeleteOfferCommand(Guid Id) : IRequest<Result>;

public class DeleteOfferCommandHandler : IRequestHandler<DeleteOfferCommand, Result>
{
    private readonly IRepository<Offer> _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IStringLocalizer<ApplicationResources> _localizer;

    public DeleteOfferCommandHandler(
        IRepository<Offer> repository,
        IUnitOfWork unitOfWork,
        IStringLocalizer<ApplicationResources> localizer)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _localizer = localizer;
    }

    public async Task<Result> Handle(DeleteOfferCommand request, CancellationToken cancellationToken)
    {
        var offer = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (offer is null)
            return Result.Failure(_localizer["Offer not found"]);

        _repository.Delete(offer);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(_localizer["Offer deleted successfully"]);
    }
}

public class DeleteOfferCommandValidator : AbstractValidator<DeleteOfferCommand>
{
    public DeleteOfferCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
