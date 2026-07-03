using System.Threading;
using ClosedXML.Excel;
using Gym.Application.Common.Interfaces;
using Gym.Application.Members.Import;
using Gym.Domain.Entities;
using Gym.Domain.Interfaces;
using Gym.Shared.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Gym.Application.Resources;

namespace Gym.Infrastructure.Services;

public class ExcelImportService : IExcelImportService
{
    private readonly IMemberRepository _memberRepository;
    private readonly IRepository<MembershipPlan> _planRepository;
    private readonly IRepository<Subscription> _subscriptionRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IStringLocalizer<ApplicationResources> _localizer;

    public ExcelImportService(
        IMemberRepository memberRepository,
        IRepository<MembershipPlan> planRepository,
        IRepository<Subscription> subscriptionRepository,
        IUnitOfWork unitOfWork,
        IStringLocalizer<ApplicationResources> localizer)
    {
        _memberRepository = memberRepository;
        _planRepository = planRepository;
        _subscriptionRepository = subscriptionRepository;
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

        var existingNationalIds = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        var existingPhones = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        var allMembers = await _memberRepository.GetAllAsync(cancellationToken);
        foreach (var m in allMembers)
        {
            if (!string.IsNullOrEmpty(m.NationalId))
                existingNationalIds.Add(m.NationalId);
            if (!string.IsNullOrEmpty(m.PhoneNumber))
                existingPhones.Add(m.PhoneNumber);
        }

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
                var fullName = GetCellString(row, 1);
                var phoneNumber = GetCellString(row, 2);
                var nationalId = GetCellString(row, 3);
                var nationality = GetCellString(row, 4);
                var company = GetCellString(row, 5);
                var address = GetCellString(row, 6);
                var weightStr = GetCellString(row, 7);
                var hasDiseaseStr = GetCellString(row, 8);
                var diseaseType = GetCellString(row, 9);
                var referralSource = GetCellString(row, 10);
                var planName = GetCellString(row, 11);
                var subscriptionPriceStr = GetCellString(row, 12);
                var paidAmountStr = GetCellString(row, 13);
                var durationStr = GetCellString(row, 14);
                var freeMonthsStr = GetCellString(row, 15);
                var freezeDaysStr = GetCellString(row, 16);
                var startDateStr = GetCellString(row, 17);
                var paymentMethodStr = GetCellString(row, 18);

                var errors = new List<string>();

                importRow.FullName = fullName;
                importRow.PhoneNumber = phoneNumber;
                importRow.NationalId = nationalId;

                if (string.IsNullOrWhiteSpace(fullName))
                    errors.Add(_localizer["Full name is required"]);

                if (string.IsNullOrWhiteSpace(phoneNumber))
                    errors.Add(_localizer["Phone number is required"]);
                else if (phoneNumber.Length != 11 || !phoneNumber.All(char.IsDigit))
                    errors.Add(_localizer["Phone number must be exactly 11 digits"]);

                if (!string.IsNullOrEmpty(nationalId) && (nationalId.Length != 14 || !nationalId.All(char.IsDigit)))
                    errors.Add(_localizer["National ID must be exactly 14 digits"]);

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

    private static string GetCellString(IXLRangeRow row, int column)
    {
        var cell = row.Cell(column);
        return cell.IsEmpty() ? string.Empty : cell.GetString().Trim();
    }

    private static int _receiptCounter = 0;
    private static string GenerateReceiptNumber()
    {
        var seq = Interlocked.Increment(ref _receiptCounter);
        return DateTime.UtcNow.ToString("yyyyMMddHHmmssfff") + seq.ToString("D4");
    }
}
