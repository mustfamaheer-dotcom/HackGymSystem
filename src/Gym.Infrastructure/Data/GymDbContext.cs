using Gym.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Gym.Infrastructure.Data;

public class GymDbContext : DbContext
{
    public GymDbContext(DbContextOptions<GymDbContext> options) : base(options) { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.ConfigureWarnings(w => w.Ignore(RelationalEventId.PendingModelChangesWarning));
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<Permission> Permissions => Set<Permission>();
    public DbSet<RolePermission> RolePermissions => Set<RolePermission>();
    public DbSet<Member> Members => Set<Member>();
    public DbSet<MembershipPlan> MembershipPlans => Set<MembershipPlan>();
    public DbSet<Membership> Memberships => Set<Membership>();
    public DbSet<Attendance> Attendances => Set<Attendance>();
    public DbSet<Offer> Offers => Set<Offer>();
    public DbSet<Notification> Notifications => Set<Notification>();
    public DbSet<Device> Devices => Set<Device>();
    public DbSet<DeviceLog> DeviceLogs => Set<DeviceLog>();
    public DbSet<Setting> Settings => Set<Setting>();
    public DbSet<Subscription> Subscriptions => Set<Subscription>();
    public DbSet<SubscriptionPayment> SubscriptionPayments => Set<SubscriptionPayment>();
    public DbSet<SubscriptionFreezeHistory> SubscriptionFreezeHistories => Set<SubscriptionFreezeHistory>();
    public DbSet<SubscriptionTransactionLog> SubscriptionTransactionLogs => Set<SubscriptionTransactionLog>();
    public DbSet<WhatsAppTemplate> WhatsAppTemplates => Set<WhatsAppTemplate>();
    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();
    public DbSet<BackupLog> BackupLogs => Set<BackupLog>();
    public DbSet<PermissionAuditLog> PermissionAuditLogs => Set<PermissionAuditLog>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(GymDbContext).Assembly);
        Seed.SeedData.Seed(modelBuilder);
        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker.Entries<Shared.Common.BaseEntity>())
        {
            if (entry.State == EntityState.Modified)
            {
                entry.Entity.MarkUpdated();
            }
        }

        return await base.SaveChangesAsync(cancellationToken);
    }
}
