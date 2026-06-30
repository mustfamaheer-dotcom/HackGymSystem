using AutoMapper;
using AutoMapper.QueryableExtensions;
using Gym.Application.Notifications.DTOs;
using Gym.Domain.Entities;
using Gym.Domain.Interfaces;
using Gym.Shared.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gym.Application.Notifications.Queries.GetMemberNotifications;

public record GetMemberNotificationsQuery(Guid MemberId) : IRequest<Result<List<NotificationDto>>>;

public class GetMemberNotificationsQueryHandler : IRequestHandler<GetMemberNotificationsQuery, Result<List<NotificationDto>>>
{
    private readonly IRepository<Notification> _repository;
    private readonly IMapper _mapper;

    public GetMemberNotificationsQueryHandler(IRepository<Notification> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<Result<List<NotificationDto>>> Handle(GetMemberNotificationsQuery request, CancellationToken cancellationToken)
    {
        var notifications = await _repository.Query()
            .Where(n => n.MemberId == request.MemberId)
            .Include(n => n.Member)
            .ProjectTo<NotificationDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        return Result<List<NotificationDto>>.Success(notifications);
    }
}
