using Gym.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gym.Infrastructure.Data.Configurations;

public class MemberConfiguration : IEntityTypeConfiguration<Member>
{
    public void Configure(EntityTypeBuilder<Member> builder)
    {
        builder.ToTable("Members");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Code)
            .ValueGeneratedOnAdd()
            .HasDefaultValue(0);

        builder.HasIndex(x => x.Code)
            .IsUnique();

        builder.Property(x => x.ReceiptNumber)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(x => x.ReceiptNumber)
            .IsUnique();

        builder.Property(x => x.FullName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.Nationality)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.NationalId)
            .IsRequired()
            .HasMaxLength(14);

        builder.HasIndex(x => x.NationalId)
            .IsUnique();

        builder.Property(x => x.PhoneNumber)
            .IsRequired()
            .HasMaxLength(11);

        builder.Property(x => x.Email)
            .HasMaxLength(200);

        builder.Property(x => x.DateOfBirth);

        builder.Property(x => x.Gender)
            .HasConversion<string?>()
            .HasMaxLength(10);

        builder.Property(x => x.Notes)
            .HasMaxLength(2000);

        builder.Property(x => x.Company)
            .HasMaxLength(200);

        builder.Property(x => x.Address)
            .HasMaxLength(500);

        builder.Property(x => x.Weight)
            .HasPrecision(5, 1);

        builder.Property(x => x.DiseaseType)
            .HasMaxLength(500);

        builder.Property(x => x.ImagePath)
            .HasMaxLength(500);

        builder.Property(x => x.ReferralSource)
            .HasMaxLength(200);

        builder.Property(x => x.MemberSignature)
            .HasMaxLength(5000);

        builder.Property(x => x.AdminSignature)
            .HasMaxLength(5000);

        builder.HasOne(x => x.Package)
            .WithMany()
            .HasForeignKey(x => x.PackageId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasMany(m => m.Attendances)
            .WithOne(a => a.Member)
            .HasForeignKey(a => a.MemberId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(m => m.Subscriptions)
            .WithOne(s => s.Member)
            .HasForeignKey(s => s.MemberId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasQueryFilter(x => !x.IsDeleted);
    }
}
