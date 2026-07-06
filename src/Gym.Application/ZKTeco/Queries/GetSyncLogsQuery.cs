using Gym.Application.Common.DTOs;
using Gym.Domain.Entities;
using Gym.Domain.Interfaces;
using Gym.Shared.Common;
using Gym.Shared.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gym.Application.ZKTeco.Queries;

public record GetSyncLogsQuery : IRequest<Result<PaginatedResult<SyncLogDto>>>
{
    public int Page { get; init; } = 1;
    public int PageSize { get; init; } = 20;
    public SyncEventType? EventType { get; init; }
    public SyncStatus? Status { get; init; }
    public DateTime? From { get; init; }
    public DateTime? To { get; init; }
}

public class SyncLogDto
{
    public Guid Id { get; set; }
    public string EventType { get; set; } = string.Empty;
    public string Direction { get; set; } = string.Empty;
    public string? EntityId { get; set; }
    public string? Payload { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? ErrorMessage { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class GetSyncLogsQueryHandler : IRequestHandler<GetSyncLogsQuery, Result<PaginatedResult<SyncLogDto>>>
{
    private readonly IRepository<SyncAuditLog> _repo;

    public GetSyncLogsQueryHandler(IRepository<SyncAuditLog> repo)
    {
        _repo = repo;
    }

    public async Task<Result<PaginatedResult<SyncLogDto>>> Handle(GetSyncLogsQuery request, CancellationToken cancellationToken)
    {
        var query = _repo.Query();

        if (request.EventType.HasValue)
            query = query.Where(l => l.EventType == request.EventType.Value);
        if (request.Status.HasValue)
            query = query.Where(l => l.Status == request.Status.Value);
        if (request.From.HasValue)
            query = query.Where(l => l.CreatedAt >= request.From.Value);
        if (request.To.HasValue)
            query = query.Where(l => l.CreatedAt <= request.To.Value);

        var total = await query.CountAsync(cancellationToken);
        var items = await query
            .OrderByDescending(l => l.CreatedAt)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(l => new SyncLogDto
            {
                Id = l.Id,
                EventType = l.EventType.ToString(),
                Direction = l.Direction.ToString(),
                EntityId = l.EntityId,
                Payload = l.Payload,
                Status = l.Status.ToString(),
                ErrorMessage = l.ErrorMessage,
                CreatedAt = l.CreatedAt
            })
            .ToListAsync(cancellationToken);

        return Result<PaginatedResult<SyncLogDto>>.Success(new PaginatedResult<SyncLogDto>
        {
            Items = items,
            TotalCount = total,
            Page = request.Page,
            PageSize = request.PageSize
        });
    }
}
