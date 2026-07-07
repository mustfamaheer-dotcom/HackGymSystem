using Gym.Domain.Entities;
using Gym.Shared.Common;
using Microsoft.EntityFrameworkCore;

namespace Gym.Application.Tests;

public class TestDbContext : DbContext
{
    public DbSet<Subscription> Subscriptions => Set<Subscription>();
    public DbSet<Notification> Notifications => Set<Notification>();
    public DbSet<Member> Members => Set<Member>();

    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseInMemoryDatabase(Guid.NewGuid().ToString());

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Subscription>(e =>
        {
            e.HasKey(s => s.Id);
            e.HasOne(s => s.Member).WithMany().HasForeignKey(s => s.MemberId);
            e.Ignore(s => s.Plan);
            e.Ignore(s => s.Offer);
            e.Ignore(s => s.Payments);
            e.Ignore(s => s.FreezeHistories);
            e.Ignore(s => s.TransactionLogs);
        });
        modelBuilder.Entity<Notification>(e =>
        {
            e.HasKey(n => n.Id);
            e.HasOne(n => n.Member).WithMany().HasForeignKey(n => n.MemberId);
        });
        modelBuilder.Entity<Member>(e =>
        {
            e.HasKey(m => m.Id);
            e.Ignore(m => m.Attendances);
            e.Ignore(m => m.Subscriptions);
        });
    }
}
