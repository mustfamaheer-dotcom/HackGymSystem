using Gym.Domain.Entities;
using Gym.Shared.Enums;
using FluentAssertions;

namespace Gym.Domain.Tests;

public class SubscriptionTests
{
    private static Subscription CreateActiveSubscription()
    {
        return new Subscription("RCP-001", Guid.NewGuid(), Guid.NewGuid(),
            1000, 500, PaymentMethod.Cash, DateTime.UtcNow.AddMonths(-1), DateTime.UtcNow.AddMonths(11));
    }

    [Fact]
    public void Constructor_ValidArgs_CreatesActiveSubscription()
    {
        var sub = CreateActiveSubscription();

        sub.Status.Should().Be(SubscriptionStatus.Active);
        sub.AmountPaid.Should().Be(500);
        sub.RemainingBalance.Should().Be(500);
    }

    [Fact]
    public void MarkExpired_ActiveSubscription_SetsExpired()
    {
        var sub = CreateActiveSubscription();

        sub.MarkExpired();

        sub.Status.Should().Be(SubscriptionStatus.Expired);
    }

    [Fact]
    public void Freeze_ActiveSubscription_SetsFrozenAndExtendsExpiry()
    {
        var sub = CreateActiveSubscription();
        var originalExpiry = sub.ExpirationDate;

        sub.Freeze(DateTime.UtcNow, DateTime.UtcNow.AddDays(7), 7);

        sub.Status.Should().Be(SubscriptionStatus.Frozen);
        sub.ExpirationDate.Should().Be(originalExpiry.AddDays(7));
    }

    [Fact]
    public void Freeze_NonActiveSubscription_Throws()
    {
        var sub = CreateActiveSubscription();
        sub.MarkExpired();

        Action act = () => sub.Freeze(DateTime.UtcNow, DateTime.UtcNow.AddDays(7), 7);

        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void Unfreeze_FrozenSubscription_RestoresActive()
    {
        var sub = CreateActiveSubscription();
        sub.Freeze(DateTime.UtcNow, DateTime.UtcNow.AddDays(7), 7);

        sub.Unfreeze();

        sub.Status.Should().Be(SubscriptionStatus.Active);
    }

    [Fact]
    public void RecordPayment_ValidAmount_UpdatesBalances()
    {
        var sub = CreateActiveSubscription();

        sub.RecordPayment(250);

        sub.AmountPaid.Should().Be(750);
        sub.RemainingBalance.Should().Be(250);
    }

    [Fact]
    public void RecordPayment_ExcessAmount_ClampsToZero()
    {
        var sub = CreateActiveSubscription();

        sub.RecordPayment(600);

        sub.RemainingBalance.Should().Be(0);
    }
}
