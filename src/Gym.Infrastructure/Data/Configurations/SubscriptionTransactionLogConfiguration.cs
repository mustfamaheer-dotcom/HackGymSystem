using Gym.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gym.Infrastructure.Data.Configurations;

public class SubscriptionTransactionLogConfiguration : IEntityTypeConfiguration<SubscriptionTransactionLog>
{
    public void Configure(EntityTypeBuilder<SubscriptionTransactionLog> builder)
    {
        builder.ToTable("SubscriptionTransactionLogs");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Action)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.Description)
            .HasMaxLength(1000);

        builder.HasOne(x => x.Subscription)
            .WithMany(s => s.TransactionLogs)
            .HasForeignKey(x => x.SubscriptionId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Performer)
            .WithMany()
            .HasForeignKey(x => x.PerformedBy)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
