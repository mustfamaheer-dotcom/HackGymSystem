using Gym.Application.Common.DTOs;
using Gym.Application.Members.DTOs;
using Gym.Shared.Common;

namespace Gym.Application.Common.Interfaces;

public interface IMemberService
{
    Task<Result<PaginatedResult<MemberDto>>> GetAllAsync(int page, int pageSize, string? searchTerm, string? sortBy, bool sortDescending, CancellationToken cancellationToken = default);
    Task<Result<PaginatedResult<MemberDto>>> AdvancedSearchAsync(string? name, string? nationalId, string? phoneNumber, int? code, string? receiptNumber, Guid? packageId, string? subscriptionStatus, string? paymentStatus, bool expiringSoon, int expiringWithinDays, bool outstandingBalance, int page, int pageSize, CancellationToken cancellationToken = default);
    Task<Result<MemberDto>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Result<List<MemberDto>>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default);
    Task<Result<List<MemberDto>>> GetExpiringMembersAsync(int withinDays, CancellationToken cancellationToken = default);
    Task<Result<List<MemberDto>>> GetMembersWithOutstandingBalanceAsync(CancellationToken cancellationToken = default);
    Task<Result<Guid>> CreateAsync(CreateMemberDto dto, CancellationToken cancellationToken = default);
    Task<Result> UpdateAsync(UpdateMemberDto dto, CancellationToken cancellationToken = default);
    Task<Result> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Result> RestoreAsync(Guid id, CancellationToken cancellationToken = default);
}
