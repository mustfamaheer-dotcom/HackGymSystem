using Gym.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gym.Infrastructure.Data.Configurations;

public class DailySessionConfiguration : IEntityTypeConfiguration<DailySession>
{
    public void Configure(EntityTypeBuilder<DailySession> builder)
    {
        builder.ToTable("DailySessions");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.Phone)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(x => x.VisitDate)
            .IsRequired();
    }
}
