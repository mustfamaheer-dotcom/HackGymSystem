using Gym.Shared.Common;

namespace Gym.Domain.Entities;

public class BackupLog : BaseEntity
{
    public string FileName { get; set; }
    public string FilePath { get; set; }
    public long? Size { get; set; }
    public string Status { get; set; }
    public string? ErrorMessage { get; set; }
    public DateTime BackupDate { get; set; }

    private BackupLog() { }

    public BackupLog(string fileName, string filePath, long? size, string status, string? errorMessage = null)
    {
        FileName = fileName;
        FilePath = filePath;
        Size = size;
        Status = status;
        ErrorMessage = errorMessage;
        BackupDate = DateTime.UtcNow;
    }
}
