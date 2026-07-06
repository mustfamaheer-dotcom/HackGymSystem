using Gym.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gym.Infrastructure.Data.Configurations;

public class DeviceMemberMappingConfiguration : IEntityTypeConfiguration<DeviceMemberMapping>
{
    public void Configure(EntityTypeBuilder<DeviceMemberMapping> builder)
    {
        builder.ToTable("DeviceMemberMappings");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.DeviceEnrollmentId)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.BiometricType)
            .HasConversion<string>()
            .HasMaxLength(20);

        builder.HasIndex(x => new { x.DeviceEnrollmentId, x.BiometricType })
            .IsUnique()
            .HasFilter("[IsDeleted] = 0");

        builder.HasIndex(x => new { x.MemberId, x.BiometricType });

        builder.HasOne(x => x.Member)
            .WithMany()
            .HasForeignKey(x => x.MemberId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasQueryFilter(x => !x.IsDeleted);
    }
}
