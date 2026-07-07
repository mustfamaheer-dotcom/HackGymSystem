using Gym.Application.Jobs;
using Gym.Domain.Entities;
using Gym.Domain.Interfaces;
using Gym.Shared.Enums;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace Gym.Application.Tests;

public class SubscriptionRenewalReminderJobTests
{
    private static (TestDbContext db, IUnitOfWork uow) CreateContext()
    {
        var db = new TestDbContext();
        var uowMock = new Mock<IUnitOfWork>();
        uowMock.Setup(u => u.Repository<Subscription>()).Returns(new TestRepository<Subscription>(db.Subscriptions));
        uowMock.Setup(u => u.Repository<Notification>()).Returns(new TestRepository<Notification>(db.Notifications));
        uowMock.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .Callback(() => db.SaveChanges())
            .ReturnsAsync(1);
        return (db, uowMock.Object);
    }

    [Fact]
    public async Task ExecuteAsync_ExpiringSub_CreatesNotification()
    {
        var (db, uow) = CreateContext();
        var loggerMock = new Mock<ILogger<SubscriptionRenewalReminderJob>>();

        var memberId = Guid.NewGuid();
        db.Members.Add(new Member("RCP-", "Test Member", "01000000000", DateTime.UtcNow.AddYears(-1)) { Id = memberId });
        var sub = new Subscription("RCP-003", memberId, Guid.NewGuid(),
            1000, 1000, PaymentMethod.Cash, DateTime.UtcNow.AddMonths(-11), DateTime.UtcNow.AddDays(3));
        db.Subscriptions.Add(sub);
        await db.SaveChangesAsync();

        var job = new SubscriptionRenewalReminderJob(uow, loggerMock.Object);
        await job.ExecuteAsync(CancellationToken.None);

        var notifs = db.Notifications.ToList();
        notifs.Should().ContainSingle(n =>
            n.MemberId == memberId &&
            n.Title == "Subscription Expiring Soon");
    }

    [Fact]
    public async Task ExecuteAsync_ExistingUnreadNotification_SkipsDuplicate()
    {
        var (db, uow) = CreateContext();
        var loggerMock = new Mock<ILogger<SubscriptionRenewalReminderJob>>();

        var memberId = Guid.NewGuid();
        db.Members.Add(new Member("RCP-", "Test Member", "01000000000", DateTime.UtcNow.AddYears(-1)) { Id = memberId });
        var sub = new Subscription("RCP-004", memberId, Guid.NewGuid(),
            1000, 1000, PaymentMethod.Cash, DateTime.UtcNow.AddMonths(-11), DateTime.UtcNow.AddDays(3));
        db.Subscriptions.Add(sub);
        db.Notifications.Add(new Notification(memberId, "Subscription Expiring Soon", "Already notified"));
        await db.SaveChangesAsync();

        var job = new SubscriptionRenewalReminderJob(uow, loggerMock.Object);
        await job.ExecuteAsync(CancellationToken.None);

        db.Notifications.Should().ContainSingle(n => n.Title == "Subscription Expiring Soon");
    }
}
