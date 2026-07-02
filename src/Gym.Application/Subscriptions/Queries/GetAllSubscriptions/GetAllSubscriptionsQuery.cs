using AutoMapper;
using Gym.Application.Common.DTOs;
using Gym.Application.Subscriptions.DTOs;
using Gym.Domain.Entities;
using Gym.Domain.Interfaces;
using Gym.Shared.Common;
using Gym.Shared.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gym.Application.Subscriptions.Queries.GetAllSubscriptions;

public record GetAllSubscriptionsQuery : PaginationRequest, IRequest<Result<PaginatedResult<SubscriptionDto>>>
{
    public string? SearchTerm { get; init; }
    public SubscriptionStatus? StatusFilter { get; init; }
    public DateTime? DateFrom { get; init; }
    public DateTime? DateTo { get; init; }
    public int? ExpiresWithinDays { get; init; }
    public string? SortBy { get; init; }
    public bool SortDescending { get; init; }
}

public class GetAllSubscriptionsQueryHandler : IRequestHandler<GetAllSubscriptionsQuery, Result<PaginatedResult<SubscriptionDto>>>
{
    private readonly IRepository<Domain.Entities.Subscription> _subscriptionRepo;
    private readonly IMapper _mapper;

    public GetAllSubscriptionsQueryHandler(IRepository<Domain.Entities.Subscription> subscriptionRepo, IMapper mapper)
    {
        _subscriptionRepo = subscriptionRepo;
        _mapper = mapper;
    }

    public async Task<Result<PaginatedResult<SubscriptionDto>>> Handle(GetAllSubscriptionsQuery request, CancellationToken cancellationToken)
    {
        var query = _subscriptionRepo.Query()
            .Include(s => s.Member)
            .Include(s => s.Plan)
            .Include(s => s.Offer)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            var term = request.SearchTerm.ToLower();
            query = query.Where(s =>
                s.Member.FullName.ToLower().Contains(term) ||
                s.ReceiptNumber.ToLower().Contains(term) ||
                s.Member.PhoneNumber.Contains(term));
        }

        if (request.StatusFilter.HasValue)
            query = query.Where(s => s.Status == request.StatusFilter.Value);

        if (request.DateFrom.HasValue)
            query = query.Where(s => s.StartDate >= request.DateFrom.Value);

        if (request.DateTo.HasValue)
            query = query.Where(s => s.StartDate <= request.DateTo.Value);

        if (request.ExpiresWithinDays.HasValue && request.ExpiresWithinDays.Value > 0)
        {
            var cutoff = DateTime.UtcNow.AddDays(request.ExpiresWithinDays.Value);
            query = query.Where(s => s.ExpirationDate <= cutoff && s.ExpirationDate >= DateTime.UtcNow);
        }

        query = (request.SortBy?.ToLower()) switch
        {
            "startdate" => request.SortDescending ? query.OrderByDescending(s => s.StartDate) : query.OrderBy(s => s.StartDate),
            "expirationdate" => request.SortDescending ? query.OrderByDescending(s => s.ExpirationDate) : query.OrderBy(s => s.ExpirationDate),
            "createdat" => request.SortDescending ? query.OrderByDescending(s => s.CreatedAt) : query.OrderBy(s => s.CreatedAt),
            "amountpaid" => request.SortDescending ? query.OrderByDescending(s => s.AmountPaid) : query.OrderBy(s => s.AmountPaid),
            "status" => request.SortDescending ? query.OrderByDescending(s => s.Status) : query.OrderBy(s => s.Status),
            _ => query.OrderByDescending(s => s.CreatedAt)
        };

        var total = await query.CountAsync(cancellationToken);
        var items = await query
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);

        var dtos = _mapper.Map<List<SubscriptionDto>>(items);

        return Result<PaginatedResult<SubscriptionDto>>.Success(
            new PaginatedResult<SubscriptionDto>
            {
                Items = dtos,
                TotalCount = total,
                Page = request.Page,
                PageSize = request.PageSize
            });
    }
}
