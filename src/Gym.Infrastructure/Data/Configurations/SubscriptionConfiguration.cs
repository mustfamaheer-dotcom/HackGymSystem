using Gym.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gym.Infrastructure.Data.Configurations;

public class SubscriptionConfiguration : IEntityTypeConfiguration<Subscription>
{
    public void Configure(EntityTypeBuilder<Subscription> builder)
    {
        builder.ToTable("Subscriptions");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.ReceiptNumber)
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(x => x.TotalSubscriptionValue)
            .HasColumnType("decimal(18,2)");

        builder.Property(x => x.AmountPaid)
            .HasColumnType("decimal(18,2)");

        builder.Property(x => x.RemainingBalance)
            .HasColumnType("decimal(18,2)");

        builder.Property(x => x.PaymentMethod)
            .HasConversion<string>()
            .HasMaxLength(20);

        builder.Property(x => x.Status)
            .HasConversion<string>()
            .HasMaxLength(20);

        builder.Property(x => x.AdminSignature)
            .HasMaxLength(500);

        builder.Property(x => x.Notes)
            .HasMaxLength(1000);

        builder.HasIndex(x => x.ReceiptNumber).IsUnique();
        builder.HasIndex(x => new { x.MemberId, x.Status });
        builder.HasIndex(x => x.Status);

        builder.HasOne(x => x.Member)
            .WithMany(m => m.Subscriptions)
            .HasForeignKey(x => x.MemberId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Plan)
            .WithMany()
            .HasForeignKey(x => x.PlanId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Offer)
            .WithMany()
            .HasForeignKey(x => x.OfferId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
