using Gym.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gym.Infrastructure.Data.Configurations;

public class LeadFollowUpConfiguration : IEntityTypeConfiguration<LeadFollowUp>
{
    public void Configure(EntityTypeBuilder<LeadFollowUp> builder)
    {
        builder.ToTable("LeadFollowUps");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Notes)
            .IsRequired()
            .HasMaxLength(2000);
    }
}