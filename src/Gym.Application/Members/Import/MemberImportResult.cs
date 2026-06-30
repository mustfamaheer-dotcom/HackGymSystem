namespace Gym.Application.Members.Import;

public class MemberImportResult
{
    public List<MemberImportRow> Imported { get; set; } = new();
    public List<MemberImportRow> Failed { get; set; } = new();
    public int TotalRows => Imported.Count + Failed.Count;
    public int SuccessCount => Imported.Count;
    public int FailedCount => Failed.Count;
}

public class MemberImportRow
{
    public int RowNumber { get; set; }
    public string? FullName { get; set; }
    public string? PhoneNumber { get; set; }
    public string? NationalId { get; set; }
    public string? ReceiptNumber { get; set; }
    public string? FailureReason { get; set; }
    public bool IsSuccess => string.IsNullOrEmpty(FailureReason);
}
