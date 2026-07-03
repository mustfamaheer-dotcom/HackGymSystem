using Gym.Application.Leads.Import;
using Gym.Application.Members.Import;

namespace Gym.Application.Common.Interfaces;

public interface IExcelImportService
{
    Task<MemberImportResult> ImportMembersAsync(Stream fileStream, string fileName, CancellationToken cancellationToken = default);
    Task<LeadImportResult> ImportLeadsAsync(Stream fileStream, string fileName, CancellationToken cancellationToken = default);
}
