using AutoMapper;
using FluentValidation;
using Gym.Domain.Entities;
using Gym.Domain.Interfaces;
using Gym.Shared.Common;
using MediatR;

namespace Gym.Application.Offers.Commands.ToggleOfferStatus;

public record ToggleOfferStatusCommand(Guid Id, bool IsActive) : IRequest<Result>;

public class ToggleOfferStatusCommandHandler : IRequestHandler<ToggleOfferStatusCommand, Result>
{
    private readonly IRepository<Offer> _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ToggleOfferStatusCommandHandler(IRepository<Offer> repository, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result> Handle(ToggleOfferStatusCommand request, CancellationToken cancellationToken)
    {
        var offer = await _repository.GetByIdAsync(request.Id, cancellationToken);

        if (offer is null)
            return Result.Failure("Offer not found");

        offer.ToggleActive(request.IsActive);
        _repository.Update(offer);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}

public class ToggleOfferStatusCommandValidator : AbstractValidator<ToggleOfferStatusCommand>
{
    public ToggleOfferStatusCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
