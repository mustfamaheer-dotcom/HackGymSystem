namespace Gym.Shared.Enums;

public enum BiometricType
{
    Fingerprint = 0,
    Face = 15,
    RFIDCard = 5
}

public enum SyncEventType
{
    Attendance,
    PrivilegeUpdate,
    Enrollment,
    Reconciliation,
    Alert
}

public enum SyncDirection
{
    DeviceToSystem,
    SystemToDevice,
    Bidirectional,
    SystemToAdmin
}

public enum SyncStatus
{
    Success,
    Failed
}

public enum FingerIndex
{
    RightThumb = 0,
    RightIndex = 1,
    RightMiddle = 2,
    RightRing = 3,
    RightLittle = 4,
    LeftThumb = 5,
    LeftIndex = 6,
    LeftMiddle = 7,
    LeftRing = 8,
    LeftLittle = 9
}
