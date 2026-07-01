using AutoMapper;
using Gym.Application.Common.DTOs;
using Gym.Application.Common.Interfaces;
using Gym.Application.Members.DTOs;
using Gym.Domain.Entities;
using Gym.Domain.Interfaces;
using Gym.Shared.Common;
using Gym.Shared.Enums;
using Microsoft.EntityFrameworkCore;

namespace Gym.Infrastructure.Services;

public class MemberService : IMemberService
{
    private readonly IMemberRepository _memberRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public MemberService(IMemberRepository memberRepository, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _memberRepository = memberRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<PaginatedResult<MemberDto>>> GetAllAsync(
        int page, int pageSize, string? searchTerm, string? sortBy, bool sortDescending,
        CancellationToken cancellationToken = default)
    {
        IQueryable<Member> query = _memberRepository.Query()
            .Include(m => m.Package);

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            var search = searchTerm.ToLower();
            query = query.Where(m =>
                m.FullName.ToLower().Contains(search) ||
                m.ReceiptNumber.ToLower().Contains(search) ||
                m.PhoneNumber.Contains(search));
        }

        var totalCount = await query.CountAsync(cancellationToken);

        query = (sortBy?.ToLower()) switch
        {
            "fullname" => sortDescending
                ? query.OrderByDescending(m => m.FullName)
                : query.OrderBy(m => m.FullName),
            "registrationdate" => sortDescending
                ? query.OrderByDescending(m => m.RegistrationDate)
                : query.OrderBy(m => m.RegistrationDate),
            _ => query.OrderBy(m => m.CreatedAt)
        };

        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        var dtos = _mapper.Map<List<MemberDto>>(items);

        return Result<PaginatedResult<MemberDto>>.Success(new PaginatedResult<MemberDto>
        {
            Items = dtos,
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize
        });
    }

    public async Task<Result<MemberDto>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var member = await _memberRepository.Query()
            .Include(m => m.Package)
            .FirstOrDefaultAsync(m => m.Id == id, cancellationToken);

        if (member is null)
            return Result<MemberDto>.Failure("Member not found");

        var dto = _mapper.Map<MemberDto>(member);
        return Result<MemberDto>.Success(dto);
    }

    public async Task<Result<List<MemberDto>>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        IQueryable<Member> query = _memberRepository.Query()
            .Include(m => m.Package);

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            var search = searchTerm.ToLower();
            query = query.Where(m =>
                m.FullName.ToLower().Contains(search) ||
                m.ReceiptNumber.ToLower().Contains(search) ||
                m.PhoneNumber.Contains(search) ||
                m.NationalId.Contains(search));
        }

        var members = await query.ToListAsync(cancellationToken);
        var dtos = _mapper.Map<List<MemberDto>>(members);
        return Result<List<MemberDto>>.Success(dtos);
    }

    public async Task<Result<PaginatedResult<MemberDto>>> AdvancedSearchAsync(
        string? name, string? nationalId, string? phoneNumber, int? code, string? receiptNumber,
        Guid? packageId, string? subscriptionStatus, string? paymentStatus,
        bool expiringSoon, int expiringWithinDays, bool outstandingBalance,
        int page, int pageSize, CancellationToken cancellationToken = default)
    {
        IQueryable<Member> query = _memberRepository.Query()
            .Include(m => m.Package);

        if (!string.IsNullOrWhiteSpace(name))
        {
            var n = name.ToLower();
            query = query.Where(m => m.FullName.ToLower().Contains(n));
        }

        if (!string.IsNullOrWhiteSpace(nationalId))
            query = query.Where(m => m.NationalId.Contains(nationalId));

        if (!string.IsNullOrWhiteSpace(phoneNumber))
            query = query.Where(m => m.PhoneNumber.Contains(phoneNumber));

        if (code.HasValue)
            query = query.Where(m => m.Code == code.Value);

        if (!string.IsNullOrWhiteSpace(receiptNumber))
        {
            var r = receiptNumber.ToLower();
            query = query.Where(m => m.ReceiptNumber.ToLower().Contains(r));
        }

        if (packageId.HasValue)
            query = query.Where(m => m.PackageId == packageId.Value);

        if (!string.IsNullOrWhiteSpace(subscriptionStatus))
        {
            var now = DateTime.UtcNow;
            query = subscriptionStatus.ToLower() switch
            {
                "active" => query.Where(m => !m.IsDeleted && m.SubscriptionEndDate > now),
                "expired" => query.Where(m => !m.IsDeleted && m.SubscriptionEndDate <= now),
                "deleted" => query.Where(m => m.IsDeleted),
                _ => query
            };
        }

        if (!string.IsNullOrWhiteSpace(paymentStatus))
        {
            query = paymentStatus.ToLower() switch
            {
                "paid" => query.Where(m => m.RemainingAmount == 0),
                "outstanding" => query.Where(m => m.RemainingAmount > 0),
                _ => query
            };
        }

        if (expiringSoon)
        {
            var cutoff = DateTime.UtcNow.AddDays(expiringWithinDays);
            query = query.Where(m => !m.IsDeleted && m.SubscriptionEndDate <= cutoff && m.SubscriptionEndDate >= DateTime.UtcNow);
        }

        if (outstandingBalance)
            query = query.Where(m => m.RemainingAmount > 0);

        var totalCount = await query.CountAsync(cancellationToken);

        query = query.OrderBy(m => m.FullName);

        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        var dtos = _mapper.Map<List<MemberDto>>(items);

        return Result<PaginatedResult<MemberDto>>.Success(new PaginatedResult<MemberDto>
        {
            Items = dtos,
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize
        });
    }

    public async Task<Result<List<MemberDto>>> GetExpiringMembersAsync(int withinDays, CancellationToken cancellationToken = default)
    {
        var members = await _memberRepository.GetExpiringMembersAsync(withinDays, cancellationToken);
        var dtos = _mapper.Map<List<MemberDto>>(members);
        return Result<List<MemberDto>>.Success(dtos);
    }

    public async Task<Result<List<MemberDto>>> GetMembersWithOutstandingBalanceAsync(CancellationToken cancellationToken = default)
    {
        var members = await _memberRepository.GetMembersWithOutstandingBalanceAsync(cancellationToken);
        var dtos = _mapper.Map<List<MemberDto>>(members);
        return Result<List<MemberDto>>.Success(dtos);
    }

    public async Task<Result<Guid>> CreateAsync(CreateMemberDto dto, CancellationToken cancellationToken = default)
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(dto.FullName))
            errors.Add("Full name is required");

        if (string.IsNullOrWhiteSpace(dto.PhoneNumber))
            errors.Add("Phone number is required");
        else if (dto.PhoneNumber.Length != 11 || !dto.PhoneNumber.All(char.IsDigit))
            errors.Add("Phone number must be exactly 11 digits");

        if (string.IsNullOrWhiteSpace(dto.Nationality))
            errors.Add("Nationality is required");

        if (string.IsNullOrWhiteSpace(dto.NationalId))
            errors.Add("National ID is required");
        else if (dto.NationalId.Length != 14 || !dto.NationalId.All(char.IsDigit))
            errors.Add("National ID must be exactly 14 digits");

        if (dto.HasDisease && string.IsNullOrWhiteSpace(dto.DiseaseType))
            errors.Add("Disease type is required when HasDisease is true");

        if (errors.Count > 0)
            return Result<Guid>.Failure(errors.ToArray());

        var nationalIdExists = await _memberRepository.AnyAsync(m => m.NationalId == dto.NationalId, cancellationToken);
        if (nationalIdExists)
            return Result<Guid>.Failure("National ID is already registered to another member");

        var phoneExists = await _memberRepository.AnyAsync(m => m.PhoneNumber == dto.PhoneNumber, cancellationToken);
        if (phoneExists)
            return Result<Guid>.Failure("Phone number is already registered to another member");

        var paidAmount = dto.PaidAmount;
        var subscriptionPrice = dto.SubscriptionPrice;
        if (paidAmount > subscriptionPrice)
            return Result<Guid>.Failure("Paid amount cannot exceed subscription price");

        var paymentMethod = string.IsNullOrWhiteSpace(dto.PaymentMethod)
            ? PaymentMethod.Cash
            : Enum.Parse<PaymentMethod>(dto.PaymentMethod, true);
        var startDate = dto.SubscriptionStartDate == default ? DateTime.UtcNow : dto.SubscriptionStartDate;
        var duration = dto.SubscriptionDurationMonths > 0 ? dto.SubscriptionDurationMonths : 1;
        var lastCode = await _memberRepository.Query().MaxAsync(m => (int?)m.Code, cancellationToken) ?? 0;
        var receiptNumber = GenerateReceiptNumber();

        var member = new Member(
            receiptNumber,
            dto.FullName,
            dto.PhoneNumber,
            subscriptionPrice,
            paidAmount,
            duration,
            startDate,
            paymentMethod,
            DateTime.UtcNow
        )
        {
            Code = lastCode + 1,
            Nationality = dto.Nationality,
            NationalId = dto.NationalId,
            Email = dto.Email,
            DateOfBirth = dto.DateOfBirth,
            Gender = string.IsNullOrEmpty(dto.Gender) ? null : Enum.Parse<Gender>(dto.Gender, true),
            Notes = dto.Notes,
            Company = dto.Company,
            Address = dto.Address,
            Weight = dto.Weight,
            HasDisease = dto.HasDisease,
            DiseaseType = dto.HasDisease ? dto.DiseaseType : null,
            ReferralSource = dto.ReferralSource,
            PackageId = dto.PackageId,
            FreeMonths = dto.FreeMonths,
            FreezeDays = dto.FreezeDays,
            FingerprintDeviceId = dto.FingerprintDeviceId,
            MemberSignature = dto.MemberSignature,
            AdminSignature = dto.AdminSignature
        };

        await _memberRepository.AddAsync(member, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<Guid>.Success(member.Id, "Member created successfully");
    }

    public async Task<Result> UpdateAsync(UpdateMemberDto dto, CancellationToken cancellationToken = default)
    {
        var member = await _memberRepository.GetByIdAsync(dto.Id, cancellationToken);
        if (member is null)
            return Result.Failure("Member not found");

        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(dto.FullName))
            errors.Add("Full name is required");

        if (string.IsNullOrWhiteSpace(dto.PhoneNumber))
            errors.Add("Phone number is required");
        else if (dto.PhoneNumber.Length != 11 || !dto.PhoneNumber.All(char.IsDigit))
            errors.Add("Phone number must be exactly 11 digits");

        if (string.IsNullOrWhiteSpace(dto.Nationality))
            errors.Add("Nationality is required");

        if (string.IsNullOrWhiteSpace(dto.NationalId))
            errors.Add("National ID is required");
        else if (dto.NationalId.Length != 14 || !dto.NationalId.All(char.IsDigit))
            errors.Add("National ID must be exactly 14 digits");

        if (dto.HasDisease && string.IsNullOrWhiteSpace(dto.DiseaseType))
            errors.Add("Disease type is required when HasDisease is true");

        if (errors.Count > 0)
            return Result.Failure(string.Join("; ", errors));

        if (dto.NationalId != member.NationalId)
        {
            var nationalIdExists = await _memberRepository.AnyAsync(m => m.NationalId == dto.NationalId && m.Id != dto.Id, cancellationToken);
            if (nationalIdExists)
                return Result.Failure("National ID is already registered to another member");
        }

        if (dto.PhoneNumber != member.PhoneNumber)
        {
            var phoneExists = await _memberRepository.AnyAsync(m => m.PhoneNumber == dto.PhoneNumber && m.Id != dto.Id, cancellationToken);
            if (phoneExists)
                return Result.Failure("Phone number is already registered to another member");
        }

        member.UpdateBasicInfo(
            dto.FullName,
            dto.Nationality,
            dto.NationalId,
            dto.PhoneNumber,
            dto.Company,
            dto.Address,
            dto.Weight,
            dto.HasDisease,
            dto.DiseaseType,
            dto.ReferralSource
        );

        member.Email = dto.Email;
        member.DateOfBirth = dto.DateOfBirth;
        member.Gender = string.IsNullOrEmpty(dto.Gender) ? null : Enum.Parse<Gender>(dto.Gender, true);
        member.Notes = dto.Notes;

        if (dto.SubscriptionStartDate != default)
        {
            if (dto.PaidAmount > dto.SubscriptionPrice)
                return Result.Failure("Paid amount cannot exceed subscription price");

            member.UpdateSubscription(
                dto.PackageId,
                dto.SubscriptionPrice,
                dto.PaidAmount,
                dto.SubscriptionDurationMonths,
                dto.FreeMonths,
                dto.FreezeDays,
                dto.SubscriptionStartDate
            );
        }

        _memberRepository.Update(member);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success("Member updated successfully");
    }

    public async Task<Result> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var member = await _memberRepository.GetByIdAsync(id, cancellationToken);
        if (member is null)
            return Result.Failure("Member not found");

        member.SoftDelete();
        _memberRepository.Update(member);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success("Member deleted successfully");
    }

    public async Task<Result> RestoreAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var member = await _memberRepository.GetByIdAsync(id, true, cancellationToken);
        if (member is null)
            return Result.Failure("Member not found");

        member.Restore();
        _memberRepository.Update(member);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success("Member restored successfully");
    }

    private static string GenerateReceiptNumber()
    {
        return DateTime.UtcNow.ToString("yyyyMMddHHmmssfff");
    }
}
