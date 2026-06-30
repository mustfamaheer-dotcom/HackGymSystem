using FluentValidation;
using Gym.Domain.Entities;
using Gym.Domain.Interfaces;
using Gym.Shared.Common;
using Gym.Shared.Enums;
using MediatR;

namespace Gym.Application.Payments.Commands.CreatePayment;

public record CreatePaymentCommand(
    Guid MemberId,
    decimal Amount,
    decimal DiscountAmount = 0,
    PaymentMethod PaymentMethod = PaymentMethod.Cash,
    string? ReferenceNumber = null,
    Guid? EmployeeId = null,
    string? Notes = null,
    string? ReceiptNumber = null) : IRequest<Result<Guid>>;

public class CreatePaymentCommandHandler : IRequestHandler<CreatePaymentCommand, Result<Guid>>
{
    private readonly IRepository<Payment> _paymentRepository;
    private readonly IRepository<Member> _memberRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreatePaymentCommandHandler(
        IRepository<Payment> paymentRepository,
        IRepository<Member> memberRepository,
        IUnitOfWork unitOfWork)
    {
        _paymentRepository = paymentRepository;
        _memberRepository = memberRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(CreatePaymentCommand request, CancellationToken cancellationToken)
    {
        var memberExists = await _memberRepository.AnyAsync(m => m.Id == request.MemberId, cancellationToken);
        if (!memberExists)
            return Result<Guid>.Failure("Member not found");

        var payment = new Payment(
            request.MemberId,
            request.Amount,
            request.DiscountAmount,
            request.PaymentMethod,
            request.ReferenceNumber,
            request.EmployeeId,
            request.Notes,
            request.ReceiptNumber);

        await _paymentRepository.AddAsync(payment, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<Guid>.Success(payment.Id);
    }
}

public class CreatePaymentCommandValidator : AbstractValidator<CreatePaymentCommand>
{
    public CreatePaymentCommandValidator()
    {
        RuleFor(v => v.MemberId).NotEmpty();
        RuleFor(v => v.Amount).GreaterThan(0);
        RuleFor(v => v.DiscountAmount).GreaterThanOrEqualTo(0);
        RuleFor(v => v.PaymentMethod).IsInEnum();
        RuleFor(v => v.ReceiptNumber).NotEmpty();
    }
}
