using Gym.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gym.Infrastructure.Data.Configurations;

public class SyncAuditLogConfiguration : IEntityTypeConfiguration<SyncAuditLog>
{
    public void Configure(EntityTypeBuilder<SyncAuditLog> builder)
    {
        builder.ToTable("SyncAuditLogs");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.EventType)
            .HasConversion<string>()
            .HasMaxLength(30);

        builder.Property(x => x.Direction)
            .HasConversion<string>()
            .HasMaxLength(30);

        builder.Property(x => x.EntityId)
            .HasMaxLength(100);

        builder.Property(x => x.Status)
            .HasConversion<string>()
            .HasMaxLength(20);

        builder.Property(x => x.ErrorMessage)
            .HasMaxLength(2000);

        builder.HasIndex(x => x.CreatedAt);
        builder.HasIndex(x => x.EventType);
        builder.HasIndex(x => x.Status);
    }
}
