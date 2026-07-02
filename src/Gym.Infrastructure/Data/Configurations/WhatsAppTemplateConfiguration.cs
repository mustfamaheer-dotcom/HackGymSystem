using Gym.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gym.Infrastructure.Data.Configurations;

public class WhatsAppTemplateConfiguration : IEntityTypeConfiguration<WhatsAppTemplate>
{
    public void Configure(EntityTypeBuilder<WhatsAppTemplate> builder)
    {
        builder.ToTable("WhatsAppTemplates");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(x => x.MessageBody)
            .HasMaxLength(2000)
            .IsRequired();

        builder.Property(x => x.TriggerType)
            .HasConversion<int?>();

        builder.HasIndex(x => x.Name).IsUnique();
    }
}
