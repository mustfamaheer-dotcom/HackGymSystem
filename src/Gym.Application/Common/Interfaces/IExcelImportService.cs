using Gym.Application.Members.Import;

namespace Gym.Application.Common.Interfaces;

public interface IExcelImportService
{
    Task<MemberImportResult> ImportMembersAsync(Stream fileStream, string fileName, CancellationToken cancellationToken = default);
}
