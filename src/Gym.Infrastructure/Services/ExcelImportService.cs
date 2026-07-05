using System.Threading;
using ClosedXML.Excel;
using Gym.Application.Common.Interfaces;
using Gym.Application.Leads.Import;
using Gym.Application.Members.Import;
using Gym.Domain.Entities;
using Gym.Domain.Interfaces;
using Gym.Shared.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Gym.Application;

namespace Gym.Infrastructure.Services;

public class ExcelImportService : IExcelImportService
{
    private static readonly Dictionary<string, int> MemberColumns = new(StringComparer.OrdinalIgnoreCase)
    {
        ["FullName"] = 1,
        ["PhoneNumber"] = 2,
        ["NationalId"] = 3,
        ["Nationality"] = 4,
        ["Company"] = 5,
        ["Address"] = 6,
        ["Weight"] = 7,
        ["HasDisease"] = 8,
        ["DiseaseType"] = 9,
        ["ReferralSource"] = 10,
        ["PlanName"] = 11,
        ["SubscriptionPrice"] = 12,
        ["PaidAmount"] = 13,
        ["Duration"] = 14,
        ["FreeMonths"] = 15,
        ["FreezeDays"] = 16,
        ["StartDate"] = 17,
        ["PaymentMethod"] = 18,
    };

    private static readonly Dictionary<string, int> LeadColumns = new(StringComparer.OrdinalIgnoreCase)
    {
        ["Name"] = 1,
        ["Phone"] = 2,
        ["Email"] = 3,
        ["Gender"] = 4,
        ["Source"] = 5,
        ["PlanName"] = 6,
        ["Notes"] = 7,
    };

    private readonly IMemberRepository _memberRepository;
    private readonly IRepository<MembershipPlan> _planRepository;
    private readonly IRepository<Subscription> _subscriptionRepository;
    private readonly IRepository<Lead> _leadRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IStringLocalizer<ApplicationResources> _localizer;

    public ExcelImportService(
        IMemberRepository memberRepository,
        IRepository<MembershipPlan> planRepository,
        IRepository<Subscription> subscriptionRepository,
        IRepository<Lead> leadRepository,
        IUnitOfWork unitOfWork,
        IStringLocalizer<ApplicationResources> localizer)
    {
        _memberRepository = memberRepository;
        _planRepository = planRepository;
        _subscriptionRepository = subscriptionRepository;
        _leadRepository = leadRepository;
        _unitOfWork = unitOfWork;
        _localizer = localizer;
    }

    public async Task<MemberImportResult> ImportMembersAsync(Stream fileStream, string fileName, CancellationToken cancellationToken = default)
    {
        var result = new MemberImportResult();

        if (!fileName.EndsWith(".xlsx", StringComparison.OrdinalIgnoreCase))
        {
            result.Failed.Add(new MemberImportRow
            {
                RowNumber = 0,
                FailureReason = _localizer["File must be an .xlsx file"]
            });
            return result;
        }

        var plans = await _planRepository.Query()
            .Where(p => p.IsActive)
            .ToListAsync(cancellationToken);

        var existingPhones = new HashSet<string>(await _memberRepository.Query()
            .Where(m => !string.IsNullOrEmpty(m.PhoneNumber))
            .Select(m => m.PhoneNumber)
            .ToListAsync(cancellationToken), StringComparer.OrdinalIgnoreCase);

        var existingNationalIds = new HashSet<string>(await _memberRepository.Query()
            .Where(m => !string.IsNullOrEmpty(m.NationalId))
            .Select(m => m.NationalId)
            .ToListAsync(cancellationToken), StringComparer.OrdinalIgnoreCase);

        var seenNationalIds = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        var seenPhones = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        var nextCode = (await _memberRepository.Query().IgnoreQueryFilters().MaxAsync(m => (int?)m.Code, cancellationToken) ?? 0) + 1;

        using var workbook = new XLWorkbook(fileStream);
        var sheet = workbook.Worksheet(1);
        var rows = sheet.RangeUsed().RowsUsed().Skip(1);

        foreach (var row in rows)
        {
            var rowNumber = row.RowNumber();
            var importRow = new MemberImportRow { RowNumber = rowNumber };

            try
            {
                var fullName = GetCellString(row, MemberColumns["FullName"]);
                var phoneNumber = GetCellString(row, MemberColumns["PhoneNumber"]);
                var nationalId = GetCellString(row, MemberColumns["NationalId"]);
                var nationality = GetCellString(row, MemberColumns["Nationality"]);
                var company = GetCellString(row, MemberColumns["Company"]);
                var address = GetCellString(row, MemberColumns["Address"]);
                var weightStr = GetCellString(row, MemberColumns["Weight"]);
                var hasDiseaseStr = GetCellString(row, MemberColumns["HasDisease"]);
                var diseaseType = GetCellString(row, MemberColumns["DiseaseType"]);
                var referralSource = GetCellString(row, MemberColumns["ReferralSource"]);
                var planName = GetCellString(row, MemberColumns["PlanName"]);
                var subscriptionPriceStr = GetCellString(row, MemberColumns["SubscriptionPrice"]);
                var paidAmountStr = GetCellString(row, MemberColumns["PaidAmount"]);
                var durationStr = GetCellString(row, MemberColumns["Duration"]);
                var freeMonthsStr = GetCellString(row, MemberColumns["FreeMonths"]);
                var freezeDaysStr = GetCellString(row, MemberColumns["FreezeDays"]);
                var startDateStr = GetCellString(row, MemberColumns["StartDate"]);
                var paymentMethodStr = GetCellString(row, MemberColumns["PaymentMethod"]);

                var errors = new List<string>();

                importRow.FullName = fullName;
                importRow.PhoneNumber = phoneNumber;
                importRow.NationalId = nationalId;

                if (string.IsNullOrWhiteSpace(fullName))
                    errors.Add(_localizer["Full name is required"]);

                if (string.IsNullOrWhiteSpace(phoneNumber))
                    errors.Add(_localizer["Phone number is required"]);
                else if (phoneNumber.Length < 7 || !phoneNumber.All(char.IsDigit))
                    errors.Add(_localizer["Phone number must be at least 7 digits and contain only numbers"]);

                if (!string.IsNullOrEmpty(nationalId) && nationalId.Length < 5)
                    errors.Add(_localizer["National ID must be at least 5 characters"]);

                var hasDisease = !string.IsNullOrEmpty(hasDiseaseStr) &&
                    (hasDiseaseStr.Equals("yes", StringComparison.OrdinalIgnoreCase) ||
                     hasDiseaseStr.Equals("true", StringComparison.OrdinalIgnoreCase) ||
                     hasDiseaseStr == "1");

                if (hasDisease && string.IsNullOrWhiteSpace(diseaseType))
                    errors.Add(_localizer["Disease type is required when HasDisease is true"]);

                decimal.TryParse(subscriptionPriceStr, out var subscriptionPrice);
                decimal.TryParse(paidAmountStr, out var paidAmount);

                if (paidAmount > subscriptionPrice)
                    errors.Add(_localizer["Paid amount cannot exceed subscription price"]);

                if (!int.TryParse(durationStr, out var durationMonths) || durationMonths <= 0)
                    durationMonths = 1;

                int.TryParse(freeMonthsStr, out var freeMonths);
                int.TryParse(freezeDaysStr, out var freezeDays);

                var startDate = DateTime.UtcNow;
                if (!string.IsNullOrWhiteSpace(startDateStr) && !DateTime.TryParse(startDateStr, out startDate))
                    startDate = DateTime.UtcNow;

                var paymentMethod = PaymentMethod.Cash;
                if (!string.IsNullOrWhiteSpace(paymentMethodStr))
                {
                    if (!Enum.TryParse<PaymentMethod>(paymentMethodStr, true, out paymentMethod))
                        errors.Add(string.Format(_localizer["Invalid payment method '{0}'. Use Cash, Visa, Instapay, or Wallet"], paymentMethodStr));
                }

                if (!string.IsNullOrEmpty(nationalId))
                {
                    if (existingNationalIds.Contains(nationalId) || seenNationalIds.Contains(nationalId))
                        errors.Add(string.Format(_localizer["Duplicate National ID '{0}' - already registered"], nationalId));
                }

                if (existingPhones.Contains(phoneNumber) || seenPhones.Contains(phoneNumber))
                    errors.Add(string.Format(_localizer["Duplicate phone number '{0}' - already registered"], phoneNumber));

                Guid? packageId = null;
                if (!string.IsNullOrWhiteSpace(planName))
                {
                    var plan = plans.FirstOrDefault(p =>
                        p.Name.Equals(planName, StringComparison.OrdinalIgnoreCase));
                    if (plan is null)
                        errors.Add(string.Format(_localizer["Plan '{0}' not found"], planName));
                    else
                        packageId = plan.Id;
                }

                if (errors.Count > 0)
                {
                    importRow.FailureReason = string.Join("; ", errors);
                    result.Failed.Add(importRow);
                    continue;
                }

                var receiptNumber = GenerateReceiptNumber();

                var member = new Member(
                    receiptNumber,
                    fullName!,
                    phoneNumber!,
                    DateTime.UtcNow
                )
                {
                    Code = nextCode++,
                    Nationality = nationality,
                    NationalId = nationalId,
                    Company = company,
                    Address = address,
                    Weight = decimal.TryParse(weightStr, out var w) ? w : null,
                    HasDisease = hasDisease,
                    DiseaseType = hasDisease ? diseaseType : null,
                    ReferralSource = referralSource,
                    PackageId = packageId
                };

                await _memberRepository.AddAsync(member, cancellationToken);

                if (packageId.HasValue && durationMonths > 0 && subscriptionPrice > 0)
                {
                    var expirationDate = startDate.AddMonths(durationMonths + freeMonths);

                    var subscription = new Subscription(
                        receiptNumber,
                        member.Id,
                        packageId.Value,
                        subscriptionPrice,
                        paidAmount,
                        paymentMethod,
                        startDate,
                        expirationDate
                    );

                    if (freezeDays > 0)
                    {
                        subscription.TotalFreezeDays = freezeDays;
                    }

                    await _subscriptionRepository.AddAsync(subscription, cancellationToken);
                }

                if (!string.IsNullOrEmpty(nationalId))
                {
                    existingNationalIds.Add(nationalId);
                    seenNationalIds.Add(nationalId);
                }
                existingPhones.Add(phoneNumber!);
                seenPhones.Add(phoneNumber!);

                importRow.ReceiptNumber = receiptNumber;
                result.Imported.Add(importRow);
            }
            catch (Exception ex)
            {
                importRow.FailureReason = string.Format(_localizer["Unexpected error: {0}"], ex.Message);
                result.Failed.Add(importRow);
            }
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return result;
    }

    public async Task<LeadImportResult> ImportLeadsAsync(Stream fileStream, string fileName, CancellationToken cancellationToken = default)
    {
        var result = new LeadImportResult();

        if (!fileName.EndsWith(".xlsx", StringComparison.OrdinalIgnoreCase))
        {
            result.Failed.Add(new LeadImportRow
            {
                RowNumber = 0,
                FailureReason = _localizer["File must be an .xlsx file"]
            });
            return result;
        }

        var plans = await _planRepository.Query()
            .Where(p => p.IsActive)
            .ToListAsync(cancellationToken);

        var existingPhones = new HashSet<string>(await _leadRepository.Query()
            .IgnoreQueryFilters()
            .Where(l => !string.IsNullOrEmpty(l.Phone))
            .Select(l => l.Phone)
            .ToListAsync(cancellationToken), StringComparer.OrdinalIgnoreCase);

        var seenPhones = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        using var workbook = new XLWorkbook(fileStream);
        var sheet = workbook.Worksheet(1);
        var rows = sheet.RangeUsed().RowsUsed().Skip(1);

        foreach (var row in rows)
        {
            var rowNumber = row.RowNumber();
            var importRow = new LeadImportRow { RowNumber = rowNumber };

            try
            {
                var name = GetCellString(row, LeadColumns["Name"]);
                var phone = GetCellString(row, LeadColumns["Phone"]);
                var email = GetCellString(row, LeadColumns["Email"]);
                var genderStr = GetCellString(row, LeadColumns["Gender"]);
                var sourceStr = GetCellString(row, LeadColumns["Source"]);
                var planName = GetCellString(row, LeadColumns["PlanName"]);
                var notes = GetCellString(row, LeadColumns["Notes"]);

                var errors = new List<string>();

                importRow.Name = name;
                importRow.Phone = phone;

                if (string.IsNullOrWhiteSpace(name))
                    errors.Add(_localizer["Full name is required"]);

                if (string.IsNullOrWhiteSpace(phone))
                    errors.Add(_localizer["Phone number is required"]);
                else if (phone.Length < 7 || !phone.All(char.IsDigit))
                    errors.Add(_localizer["Phone number must be at least 7 digits and contain only numbers"]);

                if (!string.IsNullOrEmpty(email) && !email.Contains('@'))
                    errors.Add(_localizer["Email is not valid"]);

                Gender? gender = null;
                if (!string.IsNullOrWhiteSpace(genderStr))
                {
                    if (genderStr.Equals("Male", StringComparison.OrdinalIgnoreCase))
                        gender = Gender.Male;
                    else if (genderStr.Equals("Female", StringComparison.OrdinalIgnoreCase))
                        gender = Gender.Female;
                    else
                        errors.Add(string.Format(_localizer["Gender must be 'Male' or 'Female'"]));
                }

                if (!Enum.TryParse<LeadSource>(sourceStr, true, out var source))
                    errors.Add(string.Format(_localizer["Invalid lead source '{0}'. Use SocialMedia, Referral, WalkIn, or Other"], sourceStr));

                Guid? packageId = null;
                if (!string.IsNullOrWhiteSpace(planName))
                {
                    var plan = plans.FirstOrDefault(p =>
                        p.Name.Equals(planName, StringComparison.OrdinalIgnoreCase));
                    if (plan is null)
                        errors.Add(string.Format(_localizer["Plan '{0}' not found"], planName));
                    else
                        packageId = plan.Id;
                }

                if (existingPhones.Contains(phone) || seenPhones.Contains(phone))
                    errors.Add(string.Format(_localizer["Duplicate phone number '{0}' - already registered"], phone));

                if (errors.Count > 0)
                {
                    importRow.FailureReason = string.Join("; ", errors);
                    result.Failed.Add(importRow);
                    continue;
                }

                var lead = new Lead(name!, phone!, source, packageId, notes, email, gender);
                await _leadRepository.AddAsync(lead, cancellationToken);

                existingPhones.Add(phone!);
                seenPhones.Add(phone!);

                result.Imported.Add(importRow);
            }
            catch (Exception ex)
            {
                importRow.FailureReason = string.Format(_localizer["Unexpected error: {0}"], ex.Message);
                result.Failed.Add(importRow);
            }
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return result;
    }

    private static string GetCellString(IXLRangeRow row, int column)
    {
        var cell = row.Cell(column);
        return cell.IsEmpty() ? string.Empty : cell.GetString().Trim();
    }

    private static string GenerateReceiptNumber()
    {
        return $"IMP-{DateTime.UtcNow:yyyyMMddHHmmss}-{Random.Shared.Next(1000, 9999)}";
    }
}
