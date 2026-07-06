namespace HackGym.ZKTeco.Bridge.Models;

public enum VerifyMethod
{
    Fingerprint = 0,
    Face = 15,
    RFIDCard = 5
}

public class ZKAttendanceEvent
{
    public string EnrollmentId { get; set; } = string.Empty;
    public VerifyMethod Method { get; set; }
    public DateTime Timestamp { get; set; }
    public int Direction { get; set; }
    public int MachineNumber { get; set; }
}
