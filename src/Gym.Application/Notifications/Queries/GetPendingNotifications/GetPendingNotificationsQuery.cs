using AutoMapper;
using AutoMapper.QueryableExtensions;
using Gym.Application.Notifications.DTOs;
using Gym.Domain.Entities;
using Gym.Domain.Interfaces;
using Gym.Shared.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gym.Application.Notifications.Queries.GetPendingNotifications;

public record GetPendingNotificationsQuery : IRequest<Result<List<NotificationDto>>>;

public class GetPendingNotificationsQueryHandler : IRequestHandler<GetPendingNotificationsQuery, Result<List<NotificationDto>>>
{
    private readonly IRepository<Notification> _repository;
    private readonly IMapper _mapper;

    public GetPendingNotificationsQueryHandler(IRepository<Notification> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<Result<List<NotificationDto>>> Handle(GetPendingNotificationsQuery request, CancellationToken cancellationToken)
    {
        var notifications = await _repository.Query()
            .Where(n => n.IsPending)
            .Include(n => n.Member)
            .ProjectTo<NotificationDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        return Result<List<NotificationDto>>.Success(notifications);
    }
}
