using FluentValidation;
using Microsoft.Extensions.Localization;
using Gym.Application.Resources;
using Gym.Application.Members.DTOs;

namespace Gym.Application.Members.Validators;

public class CreateMemberDtoValidator : AbstractValidator<CreateMemberDto>
{
    private readonly IStringLocalizer<ApplicationResources> _localizer;
    private static readonly string[] ValidReferralSources = ["Social Media", "Friend", "Walk-in", "Advertisement", "Other"];

    public CreateMemberDtoValidator(IStringLocalizer<ApplicationResources> localizer)
    {
        _localizer = localizer;
        RuleFor(x => x.FullName)
            .NotEmpty().WithMessage(_localizer["Full name is required"])
            .MaximumLength(200).WithMessage(_localizer["Full name must not exceed 200 characters"]);

        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage(_localizer["Phone number is required"])
            .Length(11).WithMessage(_localizer["Phone number must be exactly 11 digits"])
            .Matches(@"^\d{11}$").WithMessage(_localizer["Phone number must be 11 digits"]);

        RuleFor(x => x.Nationality)
            .NotEmpty().WithMessage(_localizer["Nationality is required"])
            .MaximumLength(100).WithMessage(_localizer["Nationality must not exceed 100 characters"]);

        RuleFor(x => x.NationalId)
            .NotEmpty().WithMessage(_localizer["National ID is required"])
            .Length(14).WithMessage(_localizer["National ID must be exactly 14 digits"])
            .Matches(@"^\d{14}$").WithMessage(_localizer["National ID must be 14 digits"]);

        RuleFor(x => x.ReferralSource)
            .Must(v => string.IsNullOrEmpty(v) || ValidReferralSources.Contains(v))
            .WithMessage(_localizer["Referral source must be one of: Social Media, Friend, Walk-in, Advertisement, Other"]);

        RuleFor(x => x.Email)
            .EmailAddress().When(x => !string.IsNullOrEmpty(x.Email))
            .WithMessage(_localizer["Email is not valid"])
            .MaximumLength(200).WithMessage(_localizer["Email must not exceed 200 characters"]);

        RuleFor(x => x.Gender)
            .Must(BeValidGender).When(x => !string.IsNullOrEmpty(x.Gender))
            .WithMessage(_localizer["Gender must be 'Male' or 'Female'"]);

        RuleFor(x => x.DiseaseType)
            .NotEmpty().When(x => x.HasDisease)
            .WithMessage(_localizer["Disease type is required when HasDisease is true"]);

        RuleFor(x => x.SubscriptionPrice)
            .GreaterThanOrEqualTo(0).WithMessage(_localizer["Subscription price must be 0 or greater"]);

        RuleFor(x => x.PaidAmount)
            .GreaterThanOrEqualTo(0).WithMessage(_localizer["Paid amount must be 0 or greater"])
            .LessThanOrEqualTo(x => x.SubscriptionPrice)
            .When(x => x.SubscriptionPrice > 0 || x.PaidAmount > 0)
            .WithMessage(_localizer["Paid amount cannot exceed subscription price"]);

        RuleFor(x => x.SubscriptionDurationMonths)
            .GreaterThan(0).When(x => x.SubscriptionDurationMonths != 0)
            .WithMessage(_localizer["Subscription duration must be greater than 0 months"]);

        RuleFor(x => x.FreeMonths)
            .GreaterThanOrEqualTo(0).WithMessage(_localizer["Free months must be 0 or greater"]);

        RuleFor(x => x.FreezeDays)
            .GreaterThanOrEqualTo(0).WithMessage(_localizer["Freeze days must be 0 or greater"]);

        RuleFor(x => x.SubscriptionStartDate)
            .NotEmpty().WithMessage(_localizer["Subscription start date is required"]);

        RuleFor(x => x.PaymentMethod)
            .Must(BeValidPaymentMethod).When(x => !string.IsNullOrEmpty(x.PaymentMethod))
            .WithMessage(_localizer["Invalid payment method"]);
    }

    private static bool BeValidPaymentMethod(string value)
    {
        return Enum.TryParse<Shared.Enums.PaymentMethod>(value, true, out _);
    }

    private static bool BeValidGender(string value)
    {
        return Enum.TryParse<Shared.Enums.Gender>(value, true, out _);
    }
}
