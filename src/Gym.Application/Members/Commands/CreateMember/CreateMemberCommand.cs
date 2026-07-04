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
    Guid? FingerprintDeviceId = null,
    string? MemberSignature = null,
    string? AdminSignature = null,
    string? ImagePath = null
) : IRequest<Result<Guid>>;

public class CreateMemberCommandHandler : IRequestHandler<CreateMemberCommand, Result<Guid>>
{
    private readonly IRepository<Member> _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IStringLocalizer<ApplicationResources> _localizer;

    public CreateMemberCommandHandler(IRepository<Member> repository, IUnitOfWork unitOfWork, IStringLocalizer<ApplicationResources> localizer)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _localizer = localizer;
    }

    public async Task<Result<Guid>> Handle(CreateMemberCommand request, CancellationToken cancellationToken)
    {
        if (!string.IsNullOrEmpty(request.NationalId))
        {
            var existingNationalId = await _repository.Query()
                .AnyAsync(m => m.NationalId == request.NationalId, cancellationToken);
            if (existingNationalId)
                return Result<Guid>.Failure(_localizer["A member with this National ID already exists"]);
        }

        var existingPhone = await _repository.Query()
            .AnyAsync(m => m.PhoneNumber == request.PhoneNumber, cancellationToken);
        if (existingPhone)
            return Result<Guid>.Failure(_localizer["A member with this phone number already exists"]);

        const int maxRetries = 3;

        for (int attempt = 0; attempt < maxRetries; attempt++)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync(cancellationToken);

                var receiptNumber = GenerateReceiptNumber();
                var lastCode = await _repository.Query().IgnoreQueryFilters().MaxAsync(m => (int?)m.Code, cancellationToken) ?? 0;

                var member = new Member(
                    receiptNumber,
                    request.FullName,
                    request.PhoneNumber,
                    DateTime.UtcNow
                )
                {
                    Code = lastCode + 1,
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
                    FingerprintDeviceId = request.FingerprintDeviceId,
                    MemberSignature = request.MemberSignature,
                    AdminSignature = request.AdminSignature,
                    ImagePath = request.ImagePath
                };

                await _repository.AddAsync(member, cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
                await _unitOfWork.CommitAsync(cancellationToken);

                return Result<Guid>.Success(member.Id, _localizer["Member created successfully"]);
            }
            catch (DbUpdateException) when (attempt < maxRetries - 1)
            {
                await _unitOfWork.ResetAsync(cancellationToken);
                continue;
            }
        }

        return Result<Guid>.Failure(_localizer["Failed to create member. Please try again."]);
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
