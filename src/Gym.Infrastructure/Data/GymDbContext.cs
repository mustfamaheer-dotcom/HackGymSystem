using System.Text.Json;
using Gym.Application.Common.Interfaces;
using Gym.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace Gym.Infrastructure.Data;

public class GymDbContext : DbContext
{
    public GymDbContext(DbContextOptions<GymDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<Permission> Permissions => Set<Permission>();
    public DbSet<RolePermission> RolePermissions => Set<RolePermission>();
    public DbSet<Member> Members => Set<Member>();
    public DbSet<MembershipPlan> MembershipPlans => Set<MembershipPlan>();
    public DbSet<Attendance> Attendances => Set<Attendance>();
    public DbSet<Offer> Offers => Set<Offer>();
    public DbSet<Device> Devices => Set<Device>();
    public DbSet<DeviceLog> DeviceLogs => Set<DeviceLog>();
    public DbSet<Setting> Settings => Set<Setting>();
    public DbSet<Subscription> Subscriptions => Set<Subscription>();
    public DbSet<SubscriptionPayment> SubscriptionPayments => Set<SubscriptionPayment>();
    public DbSet<SubscriptionFreezeHistory> SubscriptionFreezeHistories => Set<SubscriptionFreezeHistory>();
    public DbSet<SubscriptionTransactionLog> SubscriptionTransactionLogs => Set<SubscriptionTransactionLog>();
    public DbSet<Lead> Leads => Set<Lead>();
    public DbSet<LeadFollowUp> LeadFollowUps => Set<LeadFollowUp>();
    public DbSet<WhatsAppTemplate> WhatsAppTemplates => Set<WhatsAppTemplate>();
    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();
    public DbSet<BackupLog> BackupLogs => Set<BackupLog>();
    public DbSet<PermissionAuditLog> PermissionAuditLogs => Set<PermissionAuditLog>();
    public DbSet<DeviceMemberMapping> DeviceMemberMappings => Set<DeviceMemberMapping>();
    public DbSet<SyncAuditLog> SyncAuditLogs => Set<SyncAuditLog>();
    public DbSet<DailySession> DailySessions => Set<DailySession>();
    public DbSet<Notification> Notifications => Set<Notification>();
    public DbSet<AttendanceSummary> AttendanceSummaries => Set<AttendanceSummary>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(GymDbContext).Assembly);
        Seed.SeedData.Seed(modelBuilder);
        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var currentUserService = this.Database.GetService<ICurrentUserService>();
        var mediator = this.Database.GetService<IMediator>();
        var userId = currentUserService?.UserId;

        foreach (var entry in ChangeTracker.Entries<Shared.Common.BaseEntity>())
        {
            if (entry.State == EntityState.Added && entry.Entity.CreatedAt == default)
                entry.Entity.CreatedAt = DateTime.UtcNow;

            if (entry.State == EntityState.Modified)
                entry.Entity.MarkUpdated();
        }

        var domainEntities = ChangeTracker.Entries<Shared.Common.BaseEntity>()
            .Where(e => e.Entity.DomainEvents.Count > 0)
            .ToList();

        var events = domainEntities
            .SelectMany(e => e.Entity.DomainEvents)
            .ToList();

        domainEntities.ForEach(e => e.Entity.ClearDomainEvents());

        var auditEntries = new List<(string Action, string EntityType, string? EntityId, string? OldValues, string? NewValues)>();

        if (userId.HasValue)
        {
            foreach (var entry in ChangeTracker.Entries<Shared.Common.BaseEntity>())
            {
                if (entry.State == EntityState.Added || entry.State == EntityState.Modified || entry.State == EntityState.Deleted)
                {
                    var oldValues = entry.State == EntityState.Modified
                        ? JsonSerializer.Serialize(entry.OriginalValues.ToObject())
                        : null;
                    var newValues = entry.State != EntityState.Deleted
                        ? JsonSerializer.Serialize(entry.CurrentValues.ToObject())
                        : null;

                    auditEntries.Add((
                        entry.State.ToString(),
                        entry.Entity.GetType().Name,
                        entry.Entity.Id.ToString(),
                        oldValues,
                        newValues
                    ));
                }
            }
        }

        var result = await base.SaveChangesAsync(cancellationToken);

        if (auditEntries.Count > 0 && AuditLogs is not null)
        {
            foreach (var (action, entityType, entityId, oldValues, newValues) in auditEntries)
            {
                AuditLogs.Add(new AuditLog(
                    userId,
                    action,
                    entityType,
                    entityId,
                    oldValues,
                    newValues,
                    null
                ));
            }
            await base.SaveChangesAsync(cancellationToken);
        }

        foreach (var @event in events)
        {
            if (mediator is not null)
            {
                try
                {
                    await mediator.Publish(@event, cancellationToken);
                }
                catch
                {
                    // Log and swallow to avoid interrupting the main operation
                }
            }
        }

        return result;
    }
}
