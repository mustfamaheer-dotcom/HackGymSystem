using Gym.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gym.Infrastructure.Data.Configurations;

public class SubscriptionFreezeHistoryConfiguration : IEntityTypeConfiguration<SubscriptionFreezeHistory>
{
    public void Configure(EntityTypeBuilder<SubscriptionFreezeHistory> builder)
    {
        builder.ToTable("SubscriptionFreezeHistories");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Reason)
            .HasMaxLength(500);

        builder.HasOne(x => x.Subscription)
            .WithMany(s => s.FreezeHistories)
            .HasForeignKey(x => x.SubscriptionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
