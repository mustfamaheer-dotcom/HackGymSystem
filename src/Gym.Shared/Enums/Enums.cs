namespace Gym.Shared.Enums;

public enum Gender
{
    Male,
    Female
}

public enum MemberStatus
{
    Active,
    Inactive,
    Suspended
}

public enum MembershipStatus
{
    Active,
    Expired,
    Frozen,
    Cancelled
}

public enum PaymentMethod
{
    Cash,
    Visa,
    Instapay,
    Wallet,
    VodafoneCash
}

public enum SubscriptionStatus
{
    Active,
    Frozen,
    Expired,
    Renewed
}

public enum NotificationChannel
{
    WhatsApp,
    SMS,
    Email
}

public enum NotificationType
{
    MembershipExpiring,
    Promotional,
    PaymentReminder,
    General
}

public enum NotificationStatus
{
    Pending,
    Sent,
    Failed
}

public enum DeviceStatus
{
    Online,
    Offline,
    Error
}

public enum UserRole
{
    Owner,
    Receptionist,
    Trainer
}

public enum DiscountType
{
    Percentage,
    Fixed
}

public enum AttendanceSyncStatus
{
    Pending,
    Synced,
    Failed
}

public enum OfferType
{
    BonusDuration,
    FixedPrice,
    ExtraFreeze,
    FreeRegistration,
    Custom
}
