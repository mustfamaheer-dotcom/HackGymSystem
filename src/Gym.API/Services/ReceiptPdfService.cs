using Gym.Application.Members.DTOs;
using Microsoft.AspNetCore.Hosting;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Gym.API.Services;

public class ReceiptPdfService
{
    private readonly IWebHostEnvironment _env;

    private static readonly Color PrimaryColor = Color.FromHex("#1a365d");
    private static readonly Color AccentColor = Color.FromHex("#2b6cb0");
    private static readonly Color LightBg = Color.FromHex("#f7fafc");
    private static readonly Color BorderColor = Color.FromHex("#e2e8f0");
    private static readonly Color SuccessColor = Color.FromHex("#10B981");
    private static readonly Color WarningColor = Color.FromHex("#F59E0B");

    public ReceiptPdfService(IWebHostEnvironment env)
    {
        _env = env;
    }

    public byte[] GenerateReceipt(MemberDto member)
    {
        return Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A5);
                page.Margin(15);
                page.DefaultTextStyle(x => x.FontSize(8).FontColor(Colors.Black));

                page.Header().Element(c => ComposeHeader(c, member));
                page.Content().Element(c => ComposeContent(c, member));
                page.Footer().Element(ComposeFooter);
            });
        }).GeneratePdf();
    }

    public byte[] GeneratePaymentHistory(string memberName, int memberCode, string memberPhone, List<MemberPaymentDto> payments)
    {
        return Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(20);
                page.DefaultTextStyle(x => x.FontSize(9).FontColor(Colors.Black));

                page.Header().Element(c => ComposePaymentHeader(c, memberName, memberCode));
                page.Content().Element(c => ComposePaymentContent(c, memberName, memberPhone, payments));
                page.Footer().Element(ComposeFooter);
            });
        }).GeneratePdf();
    }

    private void ComposePaymentHeader(IContainer container, string memberName, int memberCode)
    {
        container.Column(col =>
        {
            col.Item().Row(row =>
            {
                var logo = LoadLogo();
                if (logo != null)
                    row.ConstantItem(80).AlignLeft().Image(logo);

                row.RelativeItem().PaddingLeft(8).Column(headerCol =>
                {
                    headerCol.Item().Text("HACK GYM").Bold().FontSize(20).FontColor(PrimaryColor);
                    headerCol.Item().Text("PAYMENT HISTORY").FontSize(10).FontColor(AccentColor);
                });

                row.ConstantItem(120).AlignRight().Column(infoCol =>
                {
                    infoCol.Item().AlignRight().Text($"Member #{memberCode}").SemiBold().FontSize(10).FontColor(PrimaryColor);
                    infoCol.Item().AlignRight().Text(memberName).FontSize(9).FontColor(Colors.Grey.Darken1);
                });
            });

            col.Item().PaddingVertical(3);
            col.Item().LineHorizontal(1.5f).LineColor(PrimaryColor);
            col.Item().PaddingVertical(3);
        });
    }

    private void ComposePaymentContent(IContainer container, string memberName, string memberPhone, List<MemberPaymentDto> payments)
    {
        container.Column(col =>
        {
            var totalPaid = payments.Sum(p => p.Amount);

            col.Item().Background(LightBg).Padding(6).Row(summary =>
            {
                summary.RelativeItem().Text($"Total Payments: {payments.Count}").SemiBold().FontSize(10).FontColor(PrimaryColor);
                summary.RelativeItem().AlignRight().Text($"Total Amount: {totalPaid:N2} EGP").SemiBold().FontSize(10).FontColor(SuccessColor);
            });

            col.Item().PaddingVertical(4);

            if (payments.Count == 0)
            {
                col.Item().Padding(10).AlignCenter().Text("No payment records found.").FontSize(10).FontColor(Colors.Grey.Darken2);
            }
            else
            {
                col.Item().Table(table =>
                {
                    table.ColumnsDefinition(c =>
                    {
                        c.ConstantColumn(25);
                        c.ConstantColumn(65);
                        c.RelativeColumn();
                        c.ConstantColumn(50);
                        c.ConstantColumn(50);
                        c.ConstantColumn(50);
                        c.ConstantColumn(65);
                    });

                    table.Header(header =>
                    {
                        Func<IContainer, IContainer> headerStyle = x => x.Background(PrimaryColor);
                        header.Cell().Element(headerStyle).PaddingVertical(4).PaddingHorizontal(2).Text("#").SemiBold().FontSize(8).FontColor(Colors.White).AlignCenter();
                        header.Cell().Element(headerStyle).PaddingVertical(4).PaddingHorizontal(2).Text("Date").SemiBold().FontSize(8).FontColor(Colors.White);
                        header.Cell().Element(headerStyle).PaddingVertical(4).PaddingHorizontal(2).Text("Plan / Receipt").SemiBold().FontSize(8).FontColor(Colors.White);
                        header.Cell().Element(headerStyle).PaddingVertical(4).PaddingHorizontal(2).Text("Method").SemiBold().FontSize(8).FontColor(Colors.White).AlignCenter();
                        header.Cell().Element(headerStyle).PaddingVertical(4).PaddingHorizontal(2).Text("Amount").SemiBold().FontSize(8).FontColor(Colors.White).AlignRight();
                        header.Cell().Element(headerStyle).PaddingVertical(4).PaddingHorizontal(2).Text("Balance").SemiBold().FontSize(8).FontColor(Colors.White).AlignRight();
                        header.Cell().Element(headerStyle).PaddingVertical(4).PaddingHorizontal(2).Text("Recorded By").SemiBold().FontSize(8).FontColor(Colors.White);
                    });

                    for (int i = 0; i < payments.Count; i++)
                    {
                        var p = payments[i];
                        var rowBg = i % 2 == 0 ? Colors.White : LightBg;
                        Func<IContainer, IContainer> cellStyle = x => x.Background(rowBg);

                        table.Cell().Element(cellStyle).PaddingVertical(2).PaddingHorizontal(2).Text((i + 1).ToString()).FontSize(8).AlignCenter();
                        table.Cell().Element(cellStyle).PaddingVertical(2).PaddingHorizontal(2).Text(p.PaymentDate.ToString("dd/MM/yyyy")).FontSize(8);
                        table.Cell().Element(cellStyle).PaddingVertical(2).PaddingHorizontal(2).Column(d =>
                        {
                            d.Item().Text(p.PlanName).FontSize(8).SemiBold();
                            d.Item().Text($"Receipt: {p.SubscriptionReceipt}").FontSize(7).FontColor(Colors.Grey.Darken2);
                        });
                        table.Cell().Element(cellStyle).PaddingVertical(2).PaddingHorizontal(2).Text(p.PaymentMethod).FontSize(8).AlignCenter();
                        table.Cell().Element(cellStyle).PaddingVertical(2).PaddingHorizontal(2).Text($"{p.Amount:N2}").FontSize(8).Bold().AlignRight().FontColor(SuccessColor);
                        table.Cell().Element(cellStyle).PaddingVertical(2).PaddingHorizontal(2).Text($"{p.RunningBalance:N2}").FontSize(8).AlignRight().FontColor(p.RunningBalance > 0 ? WarningColor : Colors.Grey.Darken1);
                        table.Cell().Element(cellStyle).PaddingVertical(2).PaddingHorizontal(2).Text(p.RecordedBy ?? "-").FontSize(8);
                    }
                });
            }

            col.Item().PaddingVertical(6);
            col.Item().LineHorizontal(0.5f).LineColor(BorderColor);
            col.Item().PaddingVertical(3);

            col.Item().Background(LightBg).Padding(6).Row(sig =>
            {
                sig.RelativeItem().Text("Member: " + memberName).FontSize(8).FontColor(Colors.Grey.Darken1);
                sig.RelativeItem().AlignRight().Text("Phone: " + memberPhone).FontSize(8).FontColor(Colors.Grey.Darken1);
            });
        });
    }

    private void ComposeHeader(IContainer container, MemberDto member)
    {
        container.Column(col =>
        {
            col.Item().Row(row =>
            {
                var logo = LoadLogo();
                if (logo != null)
                    row.ConstantItem(80).AlignLeft().Image(logo);

                row.RelativeItem().PaddingLeft(8).Column(headerCol =>
                {
                    headerCol.Item().Text("HACK GYM").Bold().FontSize(18).FontColor(PrimaryColor);
                    headerCol.Item().Text("RECEIPT VOUCHER").FontSize(9).FontColor(AccentColor);
                });

                row.ConstantItem(55).AlignCenter().Column(photoCol =>
                {
                    var photo = LoadMemberPhoto(member);
                    if (photo != null)
                        photoCol.Item().Width(50).Height(50).Image(photo);
                    else
                        photoCol.Item().Width(50).Height(50).Background(LightBg);
                });
            });

            col.Item().PaddingVertical(2);
            col.Item().LineHorizontal(1.5f).LineColor(PrimaryColor);
            col.Item().PaddingVertical(2);

            col.Item().Row(row =>
            {
                row.RelativeItem().Column(nameCol =>
                {
                    nameCol.Item().Text(member.FullName).Bold().FontSize(11).FontColor(PrimaryColor);
                });

                row.RelativeItem().AlignRight().Column(infoCol =>
                {
                    infoCol.Item().AlignRight().Text($"Receipt # {member.Code}").SemiBold().FontSize(9).FontColor(AccentColor);
                    infoCol.Item().AlignRight().Text(member.RegistrationDate.ToString("dd/MM/yyyy")).FontSize(8).FontColor(Colors.Grey.Darken1);
                });
            });

            col.Item().PaddingBottom(3);
        });
    }

    private void ComposeContent(IContainer container, MemberDto member)
    {
        container.Column(col =>
        {
            col.Item().Background(LightBg).Padding(6).Table(table =>
            {
                table.ColumnsDefinition(c =>
                {
                    c.ConstantColumn(90);
                    c.RelativeColumn();
                });

                table.Header(header =>
                {
                    header.Cell().PaddingBottom(2).Text("Member Information").SemiBold().FontSize(10).FontColor(PrimaryColor);
                    header.Cell().PaddingBottom(2).Text("");
                });

                void AddRow(string label, string? value)
                {
                    table.Cell().PaddingVertical(1).Text(label).FontSize(8).FontColor(Colors.Grey.Darken1);
                    table.Cell().PaddingVertical(1).Text(value ?? "-").FontSize(8).FontColor(Colors.Black);
                }

                AddRow("Receipt Number", member.ReceiptNumber);
                AddRow("Phone", member.PhoneNumber);
                AddRow("Nationality", member.Nationality);
                AddRow("National ID", member.NationalId);
                AddRow("Email", member.Email);
                AddRow("Gender", member.Gender?.ToString());
                AddRow("Date of Birth", member.DateOfBirth?.ToString("dd/MM/yyyy"));
                AddRow("Membership Plan", member.PackageName);
                AddRow("Company", member.Company);
                AddRow("Address", member.Address);
                AddRow("Referral Source", member.ReferralSource);
                AddRow("Weight", member.Weight != null ? member.Weight.Value.ToString("N1") + " kg" : "-");
                AddRow("Has Disease", member.HasDisease ? "Yes" : "No");
                if (member.HasDisease)
                    AddRow("Disease Type", member.DiseaseType);
                AddRow("Notes", member.Notes);
            });

            col.Item().PaddingVertical(3);
            col.Item().LineHorizontal(0.5f).LineColor(BorderColor);
            col.Item().PaddingVertical(3);

            col.Item().Background(LightBg).Padding(6).Column(sigCol =>
            {
                sigCol.Item().PaddingBottom(3).Text("Signatures").SemiBold().FontSize(10).FontColor(PrimaryColor);

                sigCol.Item().Row(row =>
                {
                    row.RelativeItem().PaddingRight(3).Border(1).BorderColor(BorderColor).Padding(6).Column(left =>
                    {
                        left.Item().Text("Member Signature").FontSize(8).FontColor(Colors.Grey.Darken1);
                        left.Item().Height(20);
                        left.Item().Text(member.MemberSignature ?? "________________________").FontSize(7).FontColor(Colors.Grey.Darken2);
                    });

                    row.RelativeItem().PaddingLeft(3).Border(1).BorderColor(BorderColor).Padding(6).Column(right =>
                    {
                        right.Item().Text("Admin Signature").FontSize(8).FontColor(Colors.Grey.Darken1);
                        right.Item().Height(20);
                        right.Item().Text(member.AdminSignature ?? "________________________").FontSize(7).FontColor(Colors.Grey.Darken2);
                    });
                });
            });
        });
    }

    private void ComposeFooter(IContainer container)
    {
        container.AlignCenter().Column(col =>
        {
            col.Item().LineHorizontal(1).LineColor(BorderColor);
            col.Item().PaddingVertical(2).Row(row =>
            {
                row.RelativeItem().Text("Generated by Hack Gym Management System").FontSize(7).FontColor(Colors.Grey.Darken2);
                row.RelativeItem().AlignRight().Text(t =>
                {
                    t.Span("Page ").FontSize(7).FontColor(Colors.Grey.Darken2);
                    t.CurrentPageNumber().FontSize(7).FontColor(Colors.Grey.Darken2);
                });
            });
        });
    }

    private byte[]? LoadLogo()
    {
        var logoPath = Path.Combine(_env.WebRootPath, "logo.png");
        return File.Exists(logoPath) ? File.ReadAllBytes(logoPath) : null;
    }

    private byte[]? LoadMemberPhoto(MemberDto member)
    {
        if (string.IsNullOrEmpty(member.ImagePath))
            return null;

        var photoPath = Path.Combine(_env.WebRootPath, member.ImagePath);
        return File.Exists(photoPath) ? File.ReadAllBytes(photoPath) : null;
    }
}
