using AutoMapper;
using Gym.Application.Subscriptions.DTOs;
using Gym.Domain.Entities;
using Gym.Domain.Interfaces;
using Gym.Shared.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gym.Application.Subscriptions.Queries.GetSubscriptionById;

public record GetSubscriptionByIdQuery(Guid Id) : IRequest<Result<SubscriptionDetailDto>>;

public class GetSubscriptionByIdQueryHandler : IRequestHandler<GetSubscriptionByIdQuery, Result<SubscriptionDetailDto>>
{
    private readonly IRepository<Domain.Entities.Subscription> _subscriptionRepo;
    private readonly IMapper _mapper;

    public GetSubscriptionByIdQueryHandler(IRepository<Domain.Entities.Subscription> subscriptionRepo, IMapper mapper)
    {
        _subscriptionRepo = subscriptionRepo;
        _mapper = mapper;
    }

    public async Task<Result<SubscriptionDetailDto>> Handle(GetSubscriptionByIdQuery request, CancellationToken cancellationToken)
    {
        var subscription = await _subscriptionRepo.Query()
            .Include(s => s.Member)
            .Include(s => s.Plan)
            .Include(s => s.Offer)
            .Include(s => s.Payments).ThenInclude(p => p.Employee)
            .Include(s => s.FreezeHistories)
            .Include(s => s.TransactionLogs).ThenInclude(t => t.Performer)
            .FirstOrDefaultAsync(s => s.Id == request.Id, cancellationToken);

        if (subscription == null)
            return Result<SubscriptionDetailDto>.Failure("Subscription not found");

        var dto = _mapper.Map<SubscriptionDetailDto>(subscription);
        dto.Payments = _mapper.Map<List<SubscriptionPaymentDto>>(subscription.Payments.OrderByDescending(p => p.CreatedAt));
        dto.FreezeHistories = _mapper.Map<List<SubscriptionFreezeHistoryDto>>(subscription.FreezeHistories.OrderByDescending(h => h.CreatedAt));
        dto.TransactionLogs = _mapper.Map<List<SubscriptionTransactionLogDto>>(subscription.TransactionLogs.OrderByDescending(t => t.CreatedAt));

        return Result<SubscriptionDetailDto>.Success(dto);
    }
}
