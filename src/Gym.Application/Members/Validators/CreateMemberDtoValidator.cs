using FluentValidation;
using Gym.Application.Members.DTOs;

namespace Gym.Application.Members.Validators;

public class CreateMemberDtoValidator : AbstractValidator<CreateMemberDto>
{
    private static readonly string[] ValidReferralSources = ["Social Media", "Friend", "Walk-in", "Advertisement", "Other"];

    public CreateMemberDtoValidator()
    {
        RuleFor(x => x.FullName)
            .NotEmpty().WithMessage("Full name is required")
            .MaximumLength(200).WithMessage("Full name must not exceed 200 characters");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("Phone number is required")
            .Length(11).WithMessage("Phone number must be exactly 11 digits")
            .Matches(@"^\d{11}$").WithMessage("Phone number must be 11 digits");

        RuleFor(x => x.Nationality)
            .NotEmpty().WithMessage("Nationality is required")
            .MaximumLength(100).WithMessage("Nationality must not exceed 100 characters");

        RuleFor(x => x.NationalId)
            .NotEmpty().WithMessage("National ID is required")
            .Length(14).WithMessage("National ID must be exactly 14 digits")
            .Matches(@"^\d{14}$").WithMessage("National ID must be 14 digits");

        RuleFor(x => x.ReferralSource)
            .Must(v => string.IsNullOrEmpty(v) || ValidReferralSources.Contains(v))
            .WithMessage("Referral source must be one of: " + string.Join(", ", ValidReferralSources));

        RuleFor(x => x.Email)
            .EmailAddress().When(x => !string.IsNullOrEmpty(x.Email))
            .WithMessage("Email is not valid")
            .MaximumLength(200).WithMessage("Email must not exceed 200 characters");

        RuleFor(x => x.Gender)
            .Must(BeValidGender).When(x => !string.IsNullOrEmpty(x.Gender))
            .WithMessage("Gender must be 'Male' or 'Female'");

        RuleFor(x => x.DiseaseType)
            .NotEmpty().When(x => x.HasDisease)
            .WithMessage("Disease type is required when HasDisease is true");

        RuleFor(x => x.SubscriptionPrice)
            .GreaterThanOrEqualTo(0).WithMessage("Subscription price must be 0 or greater");

        RuleFor(x => x.PaidAmount)
            .GreaterThanOrEqualTo(0).WithMessage("Paid amount must be 0 or greater")
            .LessThanOrEqualTo(x => x.SubscriptionPrice)
            .When(x => x.SubscriptionPrice > 0 || x.PaidAmount > 0)
            .WithMessage("Paid amount cannot exceed subscription price");

        RuleFor(x => x.SubscriptionDurationMonths)
            .GreaterThan(0).When(x => x.SubscriptionDurationMonths != 0)
            .WithMessage("Subscription duration must be greater than 0 months");

        RuleFor(x => x.FreeMonths)
            .GreaterThanOrEqualTo(0).WithMessage("Free months must be 0 or greater");

        RuleFor(x => x.FreezeDays)
            .GreaterThanOrEqualTo(0).WithMessage("Freeze days must be 0 or greater");

        RuleFor(x => x.SubscriptionStartDate)
            .NotEmpty().WithMessage("Subscription start date is required");

        RuleFor(x => x.PaymentMethod)
            .Must(BeValidPaymentMethod).When(x => !string.IsNullOrEmpty(x.PaymentMethod))
            .WithMessage("Invalid payment method");
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
