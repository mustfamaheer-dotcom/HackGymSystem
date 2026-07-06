using Gym.Application.Common.Interfaces;
using Gym.Domain.Entities;
using Gym.Domain.Interfaces;
using Gym.Shared.Common;
using Gym.Shared.Enums;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Gym.Application.ZKTeco.EventHandlers;

public class ZKTecoSyncHandler :
    INotificationHandler<SubscriptionActivatedEvent>,
    INotificationHandler<SubscriptionExpiredEvent>,
    INotificationHandler<SubscriptionUpgradedEvent>,
    INotificationHandler<SubscriptionSuspendedEvent>,
    INotificationHandler<SubscriptionRenewedEvent>
{
    private readonly IDeviceMemberMappingRepository _mappingRepo;
    private readonly IZKTecoBridgeClient _bridgeClient;
    private readonly ISyncAuditService _audit;
    private readonly ILogger<ZKTecoSyncHandler> _logger;

    public ZKTecoSyncHandler(
        IDeviceMemberMappingRepository mappingRepo,
        IZKTecoBridgeClient bridgeClient,
        ISyncAuditService audit,
        ILogger<ZKTecoSyncHandler> logger)
    {
        _mappingRepo = mappingRepo;
        _bridgeClient = bridgeClient;
        _audit = audit;
        _logger = logger;
    }

    public async Task Handle(SubscriptionActivatedEvent notification, CancellationToken cancellationToken)
    {
        await SyncPrivilege(notification.MemberId, notification.SubscriptionId, 1, notification.ExpiryDate, "Activated", cancellationToken);
    }

    public async Task Handle(SubscriptionExpiredEvent notification, CancellationToken cancellationToken)
    {
        await SyncPrivilege(notification.MemberId, notification.SubscriptionId, 0, null, "Expired", cancellationToken);
    }

    public async Task Handle(SubscriptionUpgradedEvent notification, CancellationToken cancellationToken)
    {
        await SyncPrivilege(notification.MemberId, notification.SubscriptionId, 1, notification.ExpiryDate, "Upgraded", cancellationToken);
    }

    public async Task Handle(SubscriptionSuspendedEvent notification, CancellationToken cancellationToken)
    {
        await SyncPrivilege(notification.MemberId, notification.SubscriptionId, 0, null, "Suspended", cancellationToken);
    }

    public async Task Handle(SubscriptionRenewedEvent notification, CancellationToken cancellationToken)
    {
        await SyncPrivilege(notification.MemberId, notification.SubscriptionId, 1, notification.ExpiryDate, "Renewed", cancellationToken);
    }

    private async Task SyncPrivilege(Guid memberId, Guid subscriptionId, int privilege, DateTime? expiryDate, string action, CancellationToken ct)
    {
        try
        {
            var mappings = await _mappingRepo.GetByMemberIdAsync(memberId, ct);
            if (mappings.Count == 0)
            {
                _logger.LogInformation("No device mappings found for member {MemberId}, skipping privilege sync ({Action})", memberId, action);
                return;
            }

            foreach (var mapping in mappings)
            {
                var success = await _bridgeClient.SetUserPrivilegeAsync(mapping.DeviceEnrollmentId, privilege, expiryDate, ct);
                await _audit.LogAsync(new SyncAuditEntry
                {
                    EventType = SyncEventType.PrivilegeUpdate,
                    Direction = SyncDirection.SystemToDevice,
                    EntityId = mapping.DeviceEnrollmentId,
                    Payload = System.Text.Json.JsonSerializer.Serialize(new { memberId, privilege, expiryDate, action }),
                    Status = success ? SyncStatus.Success : SyncStatus.Failed,
                    ErrorMessage = success ? null : "Bridge returned failure"
                }, ct);

                if (success)
                    _logger.LogInformation("Privilege {Privilege} synced for enrollment {EnrollId} ({Action})", privilege, mapping.DeviceEnrollmentId, action);
                else
                    _logger.LogWarning("Failed to sync privilege {Privilege} for enrollment {EnrollId} ({Action})", privilege, mapping.DeviceEnrollmentId, action);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error syncing privilege for member {MemberId} ({Action})", memberId, action);
            await _audit.LogAsync(new SyncAuditEntry
            {
                EventType = SyncEventType.PrivilegeUpdate,
                Direction = SyncDirection.SystemToDevice,
                EntityId = memberId.ToString(),
                Payload = System.Text.Json.JsonSerializer.Serialize(new { memberId, privilege, expiryDate, action }),
                Status = SyncStatus.Failed,
                ErrorMessage = ex.Message
            }, ct);
        }
    }
}
