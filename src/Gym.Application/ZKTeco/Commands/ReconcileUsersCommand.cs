using Gym.Application.Common.Interfaces;
using Gym.Domain.Entities;
using Gym.Domain.Interfaces;
using Gym.Shared.Common;
using Gym.Shared.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Gym.Application.ZKTeco.Commands;

public record ReconcileUsersCommand : IRequest<Result<ReconcileResult>>;

public class ReconcileResult
{
    public int UsersChecked { get; set; }
    public int DiscrepanciesFixed { get; set; }
    public List<string> Details { get; set; } = [];
}

public class ReconcileUsersCommandHandler : IRequestHandler<ReconcileUsersCommand, Result<ReconcileResult>>
{
    private readonly IZKTecoBridgeClient _bridge;
    private readonly IDeviceMemberMappingRepository _mappingRepo;
    private readonly IRepository<Subscription> _subscriptionRepo;
    private readonly ISyncAuditService _audit;
    private readonly ILogger<ReconcileUsersCommandHandler> _logger;

    public ReconcileUsersCommandHandler(
        IZKTecoBridgeClient bridge,
        IDeviceMemberMappingRepository mappingRepo,
        IRepository<Subscription> subscriptionRepo,
        ISyncAuditService audit,
        ILogger<ReconcileUsersCommandHandler> logger)
    {
        _bridge = bridge;
        _mappingRepo = mappingRepo;
        _subscriptionRepo = subscriptionRepo;
        _audit = audit;
        _logger = logger;
    }

    public async Task<Result<ReconcileResult>> Handle(ReconcileUsersCommand request, CancellationToken cancellationToken)
    {
        var result = new ReconcileResult();

        try
        {
            var mappings = await _mappingRepo.GetAllActiveMappingsAsync(cancellationToken);
            result.UsersChecked = mappings.Count;

            foreach (var mapping in mappings)
            {
                var activeSub = await _subscriptionRepo.FirstOrDefaultAsync(
                    s => s.MemberId == mapping.MemberId && s.Status == SubscriptionStatus.Active
                          && s.StartDate <= DateTime.UtcNow && s.ExpirationDate >= DateTime.UtcNow,
                    cancellationToken);

                var shouldHaveAccess = activeSub is not null;
                // We assume privilege 1 = active, 0 = inactive
                // For simplicity we just sync the privilege
                var success = await _bridge.SetUserPrivilegeAsync(
                    mapping.DeviceEnrollmentId,
                    shouldHaveAccess ? 1 : 0,
                    activeSub?.ExpirationDate,
                    cancellationToken);

                if (!success)
                {
                    result.DiscrepanciesFixed++;
                    result.Details.Add($"Fixed privilege for {mapping.DeviceEnrollmentId} (active: {shouldHaveAccess})");
                }
            }

            await _audit.LogAsync(new SyncAuditEntry
            {
                EventType = SyncEventType.Reconciliation,
                Direction = SyncDirection.Bidirectional,
                Payload = System.Text.Json.JsonSerializer.Serialize(result),
                Status = SyncStatus.Success
            }, cancellationToken);

            _logger.LogInformation("Reconciliation complete: {Checked} users, {Fixed} fixes", result.UsersChecked, result.DiscrepanciesFixed);
            return Result<ReconcileResult>.Success(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Reconciliation failed");
            await _audit.LogAsync(new SyncAuditEntry
            {
                EventType = SyncEventType.Reconciliation,
                Direction = SyncDirection.Bidirectional,
                Status = SyncStatus.Failed,
                ErrorMessage = ex.Message
            }, cancellationToken);
            return Result<ReconcileResult>.Failure($"Reconciliation failed: {ex.Message}");
        }
    }
}
