using System.Text.Json.Serialization;
using FluentValidation;
using Microsoft.Extensions.Localization;
using Gym.Application.Resources;
using Gym.Domain.Entities;
using Gym.Domain.Interfaces;
using Gym.Shared.Common;
using Gym.Shared.Enums;
using MediatR;

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
    decimal SubscriptionPrice = 0,
    decimal PaidAmount = 0,
    int SubscriptionDurationMonths = 1,
    int FreeMonths = 0,
    int FreezeDays = 0,
    DateTime? SubscriptionStartDate = null,
    string PaymentMethod = "Cash",
    int? FingerprintDeviceId = null,
    string? MemberSignature = null,
    string? AdminSignature = null
) : IRequest<Result>;

public class UpdateMemberCommandHandler : IRequestHandler<UpdateMemberCommand, Result>
{
    private readonly IRepository<Member> _repository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateMemberCommandHandler(IRepository<Member> repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(UpdateMemberCommand request, CancellationToken cancellationToken)
    {
        var member = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (member is null)
            return Result.Failure("Member not found");

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

        if (request.SubscriptionStartDate.HasValue)
        {
            member.UpdateSubscription(
                request.PackageId,
                request.SubscriptionPrice,
                request.PaidAmount,
                request.SubscriptionDurationMonths,
                request.FreeMonths,
                request.FreezeDays,
                request.SubscriptionStartDate.Value
            );
        }

        _repository.Update(member);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success("Member updated successfully");
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
