using Gym.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gym.Infrastructure.Data.Configurations;

public class AttendanceConfiguration : IEntityTypeConfiguration<Attendance>
{
    public void Configure(EntityTypeBuilder<Attendance> builder)
    {
        builder.ToTable("Attendance");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Time)
            .IsRequired();

        builder.Property(x => x.SyncStatus)
            .HasConversion<string>()
            .HasMaxLength(20);

        builder.HasIndex(x => new { x.MemberId, x.Date });
        builder.HasIndex(x => new { x.DeviceId, x.Date });

        builder.HasOne(a => a.Member)
            .WithMany(m => m.Attendances)
            .HasForeignKey(a => a.MemberId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(a => a.Device)
            .WithMany(d => d.Attendances)
            .HasForeignKey(a => a.DeviceId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
