using Gym.Domain.Entities;
using Gym.Shared.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gym.Infrastructure.Data.Configurations;

public class LeadConfiguration : IEntityTypeConfiguration<Lead>
{
    public void Configure(EntityTypeBuilder<Lead> builder)
    {
        builder.ToTable("Leads");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.Phone)
            .IsRequired()
            .HasMaxLength(11);

        builder.Property(x => x.Source)
            .HasConversion<string>()
            .HasMaxLength(20);

        builder.Property(x => x.Status)
            .HasConversion<string>()
            .HasMaxLength(20);

        builder.Property(x => x.Notes)
            .HasMaxLength(2000);

        builder.Property(x => x.NextFollowUpDate);

        builder.HasOne(x => x.InterestedPackage)
            .WithMany()
            .HasForeignKey(x => x.InterestedPackageId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasMany(x => x.FollowUps)
            .WithOne(f => f.Lead)
            .HasForeignKey(f => f.LeadId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasQueryFilter(x => x.Status != LeadStatus.Converted);
    }
}