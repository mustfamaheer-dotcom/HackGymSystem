using Gym.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gym.Infrastructure.Data.Configurations;

public class OfferConfiguration : IEntityTypeConfiguration<Offer>
{
    public void Configure(EntityTypeBuilder<Offer> builder)
    {
        builder.ToTable("Offers");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.OfferTitle)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.OfferType)
            .HasConversion<string>()
            .HasMaxLength(30);

        builder.Property(x => x.BonusMonths);
        builder.Property(x => x.BonusDays);
        builder.Property(x => x.ExtraFreezeDays);

        builder.Property(x => x.OfferPrice)
            .HasPrecision(18, 2);

        builder.Property(x => x.Description)
            .HasMaxLength(2000);

        builder.HasOne(x => x.LinkedPackage)
            .WithMany()
            .HasForeignKey(x => x.LinkedPackageId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasIndex(x => x.IsActive);
    }
}
