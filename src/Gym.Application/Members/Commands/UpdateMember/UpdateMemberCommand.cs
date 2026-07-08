using System.Text.Json.Serialization;
using FluentValidation;
using Microsoft.Extensions.Localization;
using Gym.Application;
using Gym.Domain.Entities;
using Gym.Domain.Interfaces;
using Gym.Shared.Common;
using Gym.Shared.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gym.Application.Members.Commands.UpdateMember;

public record UpdateMemberCommand(
    Guid Id,
    string FullName,
    [property: JsonPropertyName("phone")] string PhoneNumber,
    string? Email = null,
    string? Gender = null,
    DateTime? DateOfBirth = null,
    string? Notes = null,
    string Nationality = "",
    string NationalId = "",
    string? Company = null,
    string? Address = null,
    decimal? Weight = null,
    bool HasDisease = false,
    string? DiseaseType = null,
    string? ReferralSource = null,
    Guid? PackageId = null,
    Guid? FingerprintDeviceId = null,
    string? MemberSignature = null,
    string? AdminSignature = null,
    string? ImagePath = null,
    decimal? SubscriptionPrice = null,
    decimal? PaidAmount = null,
    int? DurationMonths = null,
    int? FreeMonths = null,
    int? FreezeDays = null,
    DateTime? StartDate = null,
    string? PaymentMethod = null
) : IRequest<Result>;

public class UpdateMemberCommandHandler : IRequestHandler<UpdateMemberCommand, Result>
{
    private readonly IRepository<Member> _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IStringLocalizer<ApplicationResources> _localizer;

    public UpdateMemberCommandHandler(IRepository<Member> repository, IUnitOfWork unitOfWork, IStringLocalizer<ApplicationResources> localizer)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _localizer = localizer;
    }

    public async Task<Result> Handle(UpdateMemberCommand request, CancellationToken cancellationToken)
    {
        var member = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (member is null)
            return Result.Failure(_localizer["Member not found"]);

        if (!string.IsNullOrEmpty(request.NationalId) && request.NationalId != member.NationalId)
        {
            var existingNationalId = await _repository.Query()
                .AnyAsync(m => m.NationalId == request.NationalId && m.Id != request.Id, cancellationToken);
            if (existingNationalId)
                return Result.Failure(_localizer["A member with this National ID already exists"]);
        }

        if (request.PhoneNumber != member.PhoneNumber)
        {
            var existingPhone = await _repository.Query()
                .AnyAsync(m => m.PhoneNumber == request.PhoneNumber && m.Id != request.Id, cancellationToken);
            if (existingPhone)
                return Result.Failure(_localizer["A member with this phone number already exists"]);
        }

        member.UpdateBasicInfo(
            request.FullName,
            request.Nationality,
            request.NationalId,
            request.PhoneNumber,
            request.Company,
            request.Address,
            request.Weight,
            request.HasDisease,
            request.DiseaseType,
            request.ReferralSource
        );

        member.Email = request.Email;
        member.DateOfBirth = request.DateOfBirth;
        member.Gender = string.IsNullOrEmpty(request.Gender) ? null : Enum.Parse<Gender>(request.Gender, true);
        member.Notes = request.Notes;
        member.ImagePath = request.ImagePath;
        member.PackageId = request.PackageId;
        member.FingerprintDeviceId = request.FingerprintDeviceId;
        member.MemberSignature = request.MemberSignature;
        member.AdminSignature = request.AdminSignature;

        if (request.PackageId.HasValue && request.SubscriptionPrice.HasValue && request.SubscriptionPrice > 0)
        {
            var startDate = request.StartDate ?? DateTime.UtcNow;
            var durationMonths = request.DurationMonths ?? 1;
            var expirationDate = startDate.AddMonths(durationMonths + (request.FreeMonths ?? 0));
            var paidAmount = request.PaidAmount ?? 0;
            var paymentMethod = string.IsNullOrEmpty(request.PaymentMethod)
                ? PaymentMethod.Cash
                : Enum.Parse<PaymentMethod>(request.PaymentMethod, true);

            var existingSub = await _unitOfWork.Repository<Subscription>()
                .Query()
                .FirstOrDefaultAsync(s => s.MemberId == request.Id && s.Status == SubscriptionStatus.Active, cancellationToken);

            if (existingSub != null)
            {
                existingSub.PlanId = request.PackageId.Value;
                existingSub.TotalSubscriptionValue = request.SubscriptionPrice.Value;
                existingSub.AmountPaid = paidAmount;
                existingSub.RemainingBalance = request.SubscriptionPrice.Value - paidAmount;
                existingSub.PaymentMethod = paymentMethod;
                existingSub.StartDate = startDate;
                existingSub.ExpirationDate = expirationDate;
                existingSub.AdminSignature = request.AdminSignature;
                existingSub.Notes = null;
                if (request.FreezeDays.GetValueOrDefault() > 0)
                    existingSub.TotalFreezeDays = request.FreezeDays.Value;
                existingSub.MarkUpdated();
                _unitOfWork.Repository<Subscription>().Update(existingSub);
            }
            else
            {
                var receiptNumber = member.ReceiptNumber;
                var subscription = new Subscription(
                    receiptNumber,
                    member.Id,
                    request.PackageId.Value,
                    request.SubscriptionPrice.Value,
                    paidAmount,
                    paymentMethod,
                    startDate,
                    expirationDate
                );

                if (request.FreezeDays.GetValueOrDefault() > 0)
                    subscription.TotalFreezeDays = request.FreezeDays.Value;

                await _unitOfWork.Repository<Subscription>().AddAsync(subscription, cancellationToken);
            }
        }

        _repository.Update(member);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(_localizer["Member updated successfully"]);
    }
}

public class UpdateMemberCommandValidator : AbstractValidator<UpdateMemberCommand>
{
    private readonly IStringLocalizer<ApplicationResources> _localizer;
    private static readonly string[] ValidReferralSources = ["Social Media", "Friend", "Walk-in", "Advertisement", "Other"];

    public UpdateMemberCommandValidator(IStringLocalizer<ApplicationResources> localizer)
    {
        _localizer = localizer;
        RuleFor(v => v.Id)
            .NotEmpty().WithMessage(_localizer["Member ID is required"]);

        RuleFor(v => v.FullName)
            .NotEmpty().WithMessage(_localizer["Full name is required"])
            .MaximumLength(200).WithMessage(_localizer["Full name must not exceed 200 characters"]);

        RuleFor(v => v.PhoneNumber)
            .NotEmpty().WithMessage(_localizer["Phone number is required"])
            .Length(11).WithMessage(_localizer["Phone number must be exactly 11 digits"])
            .Matches(@"^\d{11}$").WithMessage(_localizer["Phone number must be 11 digits"]);

        RuleFor(v => v.Nationality)
            .NotEmpty().WithMessage(_localizer["Nationality is required"])
            .MaximumLength(100).WithMessage(_localizer["Nationality must not exceed 100 characters"]);

        RuleFor(v => v.NationalId)
            .NotEmpty().WithMessage(_localizer["National ID is required"])
            .Length(14).WithMessage(_localizer["National ID must be exactly 14 digits"])
            .Matches(@"^\d{14}$").WithMessage(_localizer["National ID must be 14 digits"]);

        RuleFor(v => v.ReferralSource)
            .Must(v => string.IsNullOrEmpty(v) || ValidReferralSources.Contains(v))
            .WithMessage(_localizer["Referral source must be one of: Social Media, Friend, Walk-in, Advertisement, Other"]);

        RuleFor(v => v.DiseaseType)
            .NotEmpty().When(v => v.HasDisease)
            .WithMessage(_localizer["Disease type is required when HasDisease is true"]);
    }
}
