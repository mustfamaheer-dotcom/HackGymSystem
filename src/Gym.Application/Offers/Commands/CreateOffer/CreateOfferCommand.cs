using AutoMapper;
using FluentValidation;
using Gym.Domain.Entities;
using Gym.Domain.Interfaces;
using Gym.Shared.Common;
using Gym.Shared.Enums;
using MediatR;

namespace Gym.Application.Offers.Commands.CreateOffer;

public record CreateOfferCommand(
    string Title,
    string? Description,
    DiscountType DiscountType,
    decimal DiscountValue,
    DateTime StartDate,
    DateTime EndDate) : IRequest<Result<Guid>>;

public class CreateOfferCommandHandler : IRequestHandler<CreateOfferCommand, Result<Guid>>
{
    private readonly IRepository<Offer> _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateOfferCommandHandler(IRepository<Offer> repository, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<Guid>> Handle(CreateOfferCommand request, CancellationToken cancellationToken)
    {
        var offer = new Offer(
            request.Title,
            request.Description,
            request.DiscountType,
            request.DiscountValue,
            request.StartDate,
            request.EndDate);

        await _repository.AddAsync(offer, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<Guid>.Success(offer.Id);
    }
}

public class CreateOfferCommandValidator : AbstractValidator<CreateOfferCommand>
{
    public CreateOfferCommandValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.DiscountValue)
            .GreaterThan(0);

        RuleFor(x => x.EndDate)
            .GreaterThan(x => x.StartDate);
    }
}
