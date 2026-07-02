using FluentValidation;
using Gym.Domain.Entities;
using Gym.Domain.Interfaces;
using Gym.Shared.Common;
using Gym.Shared.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gym.Application.Subscriptions.Commands.RecordSubscriptionPayment;

public record RecordSubscriptionPaymentCommand(
    Guid SubscriptionId,
    decimal Amount,
    PaymentMethod PaymentMethod,
    string? ReferenceNumber,
    string? Notes) : IRequest<Result>;

public class RecordSubscriptionPaymentCommandHandler : IRequestHandler<RecordSubscriptionPaymentCommand, Result>
{
    private readonly IRepository<Domain.Entities.Subscription> _subscriptionRepo;
    private readonly IUnitOfWork _unitOfWork;

    public RecordSubscriptionPaymentCommandHandler(
        IRepository<Domain.Entities.Subscription> subscriptionRepo,
        IUnitOfWork unitOfWork)
    {
        _subscriptionRepo = subscriptionRepo;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(RecordSubscriptionPaymentCommand request, CancellationToken cancellationToken)
    {
        var subscription = await _subscriptionRepo.Query()
            .Include(s => s.Member)
            .FirstOrDefaultAsync(s => s.Id == request.SubscriptionId, cancellationToken);

        if (subscription == null)
            return Result.Failure("Subscription not found");

        if (request.Amount <= 0)
            return Result.Failure("Payment amount must be greater than zero");

        if (request.Amount > subscription.RemainingBalance)
            return Result.Failure("Payment amount exceeds remaining balance");

        subscription.RecordPayment(request.Amount);

        var payment = new SubscriptionPayment(
            subscription.Id, request.Amount, request.PaymentMethod,
            subscription.RemainingBalance, request.ReferenceNumber, null, request.Notes);
        await _unitOfWork.Repository<SubscriptionPayment>().AddAsync(payment, cancellationToken);

        var log = new SubscriptionTransactionLog(
            subscription.Id, "Payment",
            $"Payment of {request.Amount:N2} received. Remaining balance: {subscription.RemainingBalance:N2}");
        await _unitOfWork.Repository<SubscriptionTransactionLog>().AddAsync(log, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success("Payment recorded successfully");
    }
}

public class RecordSubscriptionPaymentCommandValidator : AbstractValidator<RecordSubscriptionPaymentCommand>
{
    public RecordSubscriptionPaymentCommandValidator()
    {
        RuleFor(v => v.SubscriptionId)
            .NotEmpty().WithMessage("Subscription ID is required");
        RuleFor(v => v.Amount)
            .GreaterThan(0).WithMessage("Amount must be greater than 0");
        RuleFor(v => v.PaymentMethod)
            .IsInEnum().WithMessage("Invalid payment method");
    }
}
