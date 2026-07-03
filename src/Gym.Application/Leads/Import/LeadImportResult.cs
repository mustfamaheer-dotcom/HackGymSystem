namespace Gym.Application.Leads.Import;

public class LeadImportResult
{
    public List<LeadImportRow> Imported { get; set; } = new();
    public List<LeadImportRow> Failed { get; set; } = new();
    public int TotalRows => Imported.Count + Failed.Count;
    public int SuccessCount => Imported.Count;
    public int FailedCount => Failed.Count;
}

public class LeadImportRow
{
    public int RowNumber { get; set; }
    public string? Name { get; set; }
    public string? Phone { get; set; }
    public string? FailureReason { get; set; }
    public bool IsSuccess => string.IsNullOrEmpty(FailureReason);
}
