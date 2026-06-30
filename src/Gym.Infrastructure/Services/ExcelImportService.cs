using ClosedXML.Excel;
using Gym.Application.Common.Interfaces;
using Gym.Application.Members.Import;
using Gym.Domain.Entities;
using Gym.Domain.Interfaces;
using Gym.Shared.Enums;
using Microsoft.EntityFrameworkCore;

namespace Gym.Infrastructure.Services;

public class ExcelImportService : IExcelImportService
{
    private readonly IMemberRepository _memberRepository;
    private readonly IRepository<MembershipPlan> _planRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ExcelImportService(
        IMemberRepository memberRepository,
        IRepository<MembershipPlan> planRepository,
        IUnitOfWork unitOfWork)
    {
        _memberRepository = memberRepository;
        _planRepository = planRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<MemberImportResult> ImportMembersAsync(Stream fileStream, string fileName, CancellationToken cancellationToken = default)
    {
        var result = new MemberImportResult();

        if (!fileName.EndsWith(".xlsx", StringComparison.OrdinalIgnoreCase))
        {
            result.Failed.Add(new MemberImportRow
            {
                RowNumber = 0,
                FailureReason = "File must be an .xlsx file"
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
                    errors.Add("Full name is required");

                if (string.IsNullOrWhiteSpace(phoneNumber))
                    errors.Add("Phone number is required");
                else if (phoneNumber.Length != 11 || !phoneNumber.All(char.IsDigit))
                    errors.Add("Phone number must be exactly 11 digits");

                if (!string.IsNullOrEmpty(nationalId) && (nationalId.Length != 14 || !nationalId.All(char.IsDigit)))
                    errors.Add("National ID must be exactly 14 digits");

                var hasDisease = !string.IsNullOrEmpty(hasDiseaseStr) &&
                    (hasDiseaseStr.Equals("yes", StringComparison.OrdinalIgnoreCase) ||
                     hasDiseaseStr.Equals("true", StringComparison.OrdinalIgnoreCase) ||
                     hasDiseaseStr == "1");

                if (hasDisease && string.IsNullOrWhiteSpace(diseaseType))
                    errors.Add("Disease type is required when HasDisease is true");

                decimal.TryParse(subscriptionPriceStr, out var subscriptionPrice);
                decimal.TryParse(paidAmountStr, out var paidAmount);

                if (paidAmount > subscriptionPrice)
                    errors.Add("Paid amount cannot exceed subscription price");

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
                        errors.Add($"Invalid payment method '{paymentMethodStr}'. Use Cash, Visa, Instapay, or Wallet");
                }

                if (!string.IsNullOrEmpty(nationalId))
                {
                    if (existingNationalIds.Contains(nationalId) || seenNationalIds.Contains(nationalId))
                        errors.Add($"Duplicate National ID '{nationalId}' - already registered");
                }

                if (existingPhones.Contains(phoneNumber) || seenPhones.Contains(phoneNumber))
                    errors.Add($"Duplicate phone number '{phoneNumber}' - already registered");

                Guid? packageId = null;
                if (!string.IsNullOrWhiteSpace(planName))
                {
                    var plan = plans.FirstOrDefault(p =>
                        p.Name.Equals(planName, StringComparison.OrdinalIgnoreCase));
                    if (plan is null)
                        errors.Add($"Plan '{planName}' not found");
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
                    subscriptionPrice,
                    paidAmount,
                    durationMonths,
                    startDate,
                    paymentMethod,
                    DateTime.UtcNow
                )
                {
                    Nationality = nationality,
                    NationalId = nationalId,
                    Company = company,
                    Address = address,
                    Weight = decimal.TryParse(weightStr, out var w) ? w : null,
                    HasDisease = hasDisease,
                    DiseaseType = hasDisease ? diseaseType : null,
                    ReferralSource = referralSource,
                    PackageId = packageId,
                    FreeMonths = freeMonths,
                    FreezeDays = freezeDays
                };

                await _memberRepository.AddAsync(member, cancellationToken);

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
                importRow.FailureReason = $"Unexpected error: {ex.Message}";
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
        return "RCP-" + DateTime.UtcNow.ToString("yyyyMMdd-HHmmss-fff");
    }
}
