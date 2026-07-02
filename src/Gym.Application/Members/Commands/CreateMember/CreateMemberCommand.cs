using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using FluentValidation;
using Microsoft.Extensions.Localization;
using Gym.Application.Resources;
using Gym.Domain.Entities;
using Gym.Domain.Interfaces;
using Gym.Shared.Common;
using Gym.Shared.Enums;
using MediatR;

namespace Gym.Application.Members.Commands.CreateMember;

public record CreateMemberCommand(
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
) : IRequest<Result<Guid>>;

public class CreateMemberCommandHandler : IRequestHandler<CreateMemberCommand, Result<Guid>>
{
    private readonly IRepository<Member> _repository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateMemberCommandHandler(IRepository<Member> repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(CreateMemberCommand request, CancellationToken cancellationToken)
    {
        var paymentMethod = Enum.Parse<PaymentMethod>(request.PaymentMethod, true);
        var startDate = request.SubscriptionStartDate ?? DateTime.UtcNow;
        var receiptNumber = GenerateReceiptNumber();

        var member = new Member(
            receiptNumber,
            request.FullName,
            request.PhoneNumber,
            request.SubscriptionPrice,
            request.PaidAmount,
            request.SubscriptionDurationMonths,
            startDate,
            paymentMethod,
            DateTime.UtcNow
        )
        {
            Email = request.Email,
            DateOfBirth = request.DateOfBirth,
            Gender = string.IsNullOrEmpty(request.Gender) ? null : Enum.Parse<Gender>(request.Gender, true),
            Notes = request.Notes,
            Nationality = request.Nationality,
            NationalId = request.NationalId,
            Company = request.Company,
            Address = request.Address,
            Weight = request.Weight,
            HasDisease = request.HasDisease,
            DiseaseType = request.HasDisease ? request.DiseaseType : null,
            ReferralSource = request.ReferralSource,
            PackageId = request.PackageId,
            FreeMonths = request.FreeMonths,
            FreezeDays = request.FreezeDays,
            FingerprintDeviceId = request.FingerprintDeviceId,
            MemberSignature = request.MemberSignature,
            AdminSignature = request.AdminSignature
        };

        await _repository.AddAsync(member, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<Guid>.Success(member.Id, "Member created successfully");
    }

    private static string GenerateReceiptNumber()
    {
        return DateTime.UtcNow.ToString("yyyyMMddHHmmssfff");
    }
}

public class CreateMemberCommandValidator : AbstractValidator<CreateMemberCommand>
{
    private readonly IStringLocalizer<ApplicationResources> _localizer;

    public CreateMemberCommandValidator(IStringLocalizer<ApplicationResources> localizer)
    {
        _localizer = localizer;
        RuleFor(v => v.FullName)
            .NotEmpty().WithMessage(_localizer["Full name is required"])
            .MaximumLength(200).WithMessage(_localizer["Full name must not exceed 200 characters"]);

        RuleFor(v => v.PhoneNumber)
            .NotEmpty().WithMessage(_localizer["Phone number is required"])
            .Length(11).WithMessage(_localizer["Phone number must be exactly 11 digits"])
            .Matches(@"^\d{11}$").WithMessage(_localizer["Phone number must be 11 digits"]);
    }
}
