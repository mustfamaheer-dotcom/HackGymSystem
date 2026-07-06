using FluentValidation;
using Gym.Application.Common.Interfaces;
using Gym.Domain.Entities;
using Gym.Domain.Interfaces;
using Gym.Shared.Common;
using Gym.Shared.Enums;
using MediatR;

namespace Gym.Application.ZKTeco.Commands;

public record EnrollBiometricCommand(Guid MemberId, BiometricType Type, int? FingerIndex = null) : IRequest<Result<string>>;

public class EnrollBiometricCommandHandler : IRequestHandler<EnrollBiometricCommand, Result<string>>
{
    private readonly IZKTecoBridgeClient _bridge;
    private readonly IDeviceMemberMappingRepository _mappingRepo;
    private readonly IRepository<Member> _memberRepo;
    private readonly ISyncAuditService _audit;

    public EnrollBiometricCommandHandler(
        IZKTecoBridgeClient bridge,
        IDeviceMemberMappingRepository mappingRepo,
        IRepository<Member> memberRepo,
        ISyncAuditService audit)
    {
        _bridge = bridge;
        _mappingRepo = mappingRepo;
        _memberRepo = memberRepo;
        _audit = audit;
    }

    public async Task<Result<string>> Handle(EnrollBiometricCommand request, CancellationToken cancellationToken)
    {
        var member = await _memberRepo.GetByIdAsync(request.MemberId, cancellationToken);
        if (member is null)
            return Result<string>.Failure("Member not found");

        // Check if already enrolled
        var existing = await _mappingRepo.GetActiveMappingAsync(request.MemberId, request.Type, cancellationToken);
        if (existing is not null)
            return Result<string>.Failure($"Member already has a {request.Type} enrollment (ID: {existing.DeviceEnrollmentId})");

        var enrollmentId = await _mappingRepo.GetNextEnrollmentIdAsync(cancellationToken);

        EnrollmentResult result;
        if (request.Type == BiometricType.Face)
            result = await _bridge.EnrollFaceAsync(member.Id.ToString(), enrollmentId, 60, cancellationToken);
        else
            result = await _bridge.EnrollFingerprintAsync(member.Id.ToString(), enrollmentId, request.FingerIndex ?? 0, 60, cancellationToken);

        await _audit.LogAsync(new SyncAuditEntry
        {
            EventType = SyncEventType.Enrollment,
            Direction = SyncDirection.SystemToDevice,
            EntityId = member.Id.ToString(),
            Payload = System.Text.Json.JsonSerializer.Serialize(new { memberId = member.Id, type = request.Type.ToString(), enrollmentId }),
            Status = result.Success ? SyncStatus.Success : SyncStatus.Failed,
            ErrorMessage = result.ErrorMessage
        }, cancellationToken);

        if (!result.Success)
            return Result<string>.Failure(result.ErrorMessage ?? "Enrollment failed");

        // Persist mapping
        var mapping = new DeviceMemberMapping(member.Id, enrollmentId, request.Type, request.FingerIndex);
        await _mappingRepo.SaveMappingAsync(mapping, cancellationToken);

        // Set active privilege
        await _bridge.SetUserPrivilegeAsync(enrollmentId, 1, cancellationToken: cancellationToken);

        return Result<string>.Success(enrollmentId);
    }
}

public class EnrollBiometricCommandValidator : AbstractValidator<EnrollBiometricCommand>
{
    public EnrollBiometricCommandValidator()
    {
        RuleFor(v => v.MemberId).NotEmpty();
        RuleFor(v => v.Type).IsInEnum();
        RuleFor(v => v.FingerIndex)
            .InclusiveBetween(0, 9)
            .When(v => v.Type == BiometricType.Fingerprint && v.FingerIndex.HasValue);
    }
}
