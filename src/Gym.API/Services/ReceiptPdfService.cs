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
