using Gym.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gym.Infrastructure.Data.Configurations;

public class AttendanceSummaryConfiguration : IEntityTypeConfiguration<AttendanceSummary>
{
    public void Configure(EntityTypeBuilder<AttendanceSummary> builder)
    {
        builder.ToTable("AttendanceSummary");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Date)
            .IsRequired();

        builder.Property(x => x.Status)
            .HasConversion<string>()
            .HasMaxLength(20);

        builder.Property(x => x.WorkDurationMinutes)
            .HasPrecision(10, 2);

        builder.Property(x => x.TotalWorkHours)
            .HasPrecision(10, 2);

        builder.HasIndex(x => new { x.MemberId, x.Date })
            .IsUnique();

        builder.HasOne(s => s.Member)
            .WithMany()
            .HasForeignKey(s => s.MemberId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
