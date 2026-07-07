using Gym.Application.Jobs;
using Gym.Domain.Entities;
using Gym.Domain.Interfaces;
using Gym.Shared.Enums;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace Gym.Application.Tests;

public class SubscriptionExpiryJobTests
{
    private static (TestDbContext db, IUnitOfWork uow) CreateContext()
    {
        var db = new TestDbContext();
        var uowMock = new Mock<IUnitOfWork>();
        uowMock.Setup(u => u.Repository<Subscription>()).Returns(new TestRepository<Subscription>(db.Subscriptions));
        uowMock.Setup(u => u.Repository<Notification>()).Returns(new TestRepository<Notification>(db.Notifications));
        uowMock.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .Returns(() => db.SaveChangesAsync());
        return (db, uowMock.Object);
    }

    [Fact]
    public async Task ExecuteAsync_ExpiredSubscriptions_MarksExpiredAndCreatesNotification()
    {
        var (db, uow) = CreateContext();
        var loggerMock = new Mock<ILogger<SubscriptionExpiryJob>>();

        var memberId = Guid.NewGuid();
        db.Members.Add(new Member("RCP-", "Test Member", "01000000000", DateTime.UtcNow.AddYears(-1)) { Id = memberId });
        var expiredSub = new Subscription("RCP-001", memberId, Guid.NewGuid(),
            1000, 500, PaymentMethod.Cash, DateTime.UtcNow.AddMonths(-2), DateTime.UtcNow.AddDays(-1));
        db.Subscriptions.Add(expiredSub);
        await db.SaveChangesAsync();

        var job = new SubscriptionExpiryJob(uow, loggerMock.Object);
        await job.ExecuteAsync(CancellationToken.None);

        expiredSub.Status.Should().Be(SubscriptionStatus.Expired);
        var notifs = db.Notifications.ToList();
        notifs.Should().ContainSingle(n =>
            n.MemberId == expiredSub.MemberId &&
            n.Title == "Subscription Expired");
    }

    [Fact]
    public async Task ExecuteAsync_NoExpiredSubscriptions_DoesNothing()
    {
        var (db, uow) = CreateContext();
        var loggerMock = new Mock<ILogger<SubscriptionExpiryJob>>();

        var activeSub = new Subscription("RCP-002", Guid.NewGuid(), Guid.NewGuid(),
            1000, 1000, PaymentMethod.Cash, DateTime.UtcNow.AddMonths(-1), DateTime.UtcNow.AddMonths(11));
        db.Subscriptions.Add(activeSub);
        await db.SaveChangesAsync();

        var job = new SubscriptionExpiryJob(uow, loggerMock.Object);
        await job.ExecuteAsync(CancellationToken.None);

        activeSub.Status.Should().Be(SubscriptionStatus.Active);
        db.Notifications.Should().BeEmpty();
    }
}
