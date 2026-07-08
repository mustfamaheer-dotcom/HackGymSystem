using FluentValidation;
using Microsoft.Extensions.Localization;
using Gym.Application;
using Gym.Domain.Entities;
using Gym.Domain.Interfaces;
using Gym.Shared.Common;
using Gym.Shared.Enums;
using MediatR;

namespace Gym.Application.DailySessions.Commands.CreateDailySession;

public record CreateDailySessionCommand(
    string Name,
    string Phone,
    DateTime VisitDate,
    Guid? PlanId,
    decimal Amount,
    decimal PaidAmount,
    PaymentMethod PaymentMethod) : IRequest<Result<Guid>>;

public class CreateDailySessionCommandHandler : IRequestHandler<CreateDailySessionCommand, Result<Guid>>
{
    private readonly IRepository<DailySession> _repository;
    private readonly IRepository<Lead> _leadRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IStringLocalizer<ApplicationResources> _localizer;

    public CreateDailySessionCommandHandler(IRepository<DailySession> repository, IRepository<Lead> leadRepository, IUnitOfWork unitOfWork, IStringLocalizer<ApplicationResources> localizer)
    {
        _repository = repository;
        _leadRepository = leadRepository;
        _unitOfWork = unitOfWork;
        _localizer = localizer;
    }

    public async Task<Result<Guid>> Handle(CreateDailySessionCommand request, CancellationToken cancellationToken)
    {
        var session = new DailySession(request.Name, request.Phone, request.VisitDate, request.PlanId, request.Amount, request.PaidAmount, request.PaymentMethod);
        await _repository.AddAsync(session, cancellationToken);

        var lead = new Lead(request.Name, request.Phone, LeadSource.WalkIn, request.PlanId, null);
        await _leadRepository.AddAsync(lead, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result<Guid>.Success(session.Id, _localizer["Daily session recorded successfully"]);
    }
}

public class CreateDailySessionCommandValidator : AbstractValidator<CreateDailySessionCommand>
{
    public CreateDailySessionCommandValidator(IStringLocalizer<ApplicationResources> localizer)
    {
        RuleFor(v => v.Name)
            .NotEmpty().WithMessage(localizer["Name is required"])
            .MaximumLength(200).WithMessage(localizer["Name must not exceed 200 characters"]);
        RuleFor(v => v.Phone)
            .NotEmpty().WithMessage(localizer["Phone is required"])
            .MaximumLength(20).WithMessage(localizer["Phone must not exceed 20 characters"]);
        RuleFor(v => v.VisitDate)
            .NotEmpty().WithMessage(localizer["Visit date is required"]);
        RuleFor(v => v.Amount)
            .GreaterThanOrEqualTo(0).WithMessage(localizer["Amount must be non-negative"]);
        RuleFor(v => v.PaidAmount)
            .GreaterThanOrEqualTo(0).WithMessage(localizer["Paid amount must be non-negative"]);
        RuleFor(v => v.PaymentMethod)
            .IsInEnum().WithMessage(localizer["Payment method is required"]);
    }
}
