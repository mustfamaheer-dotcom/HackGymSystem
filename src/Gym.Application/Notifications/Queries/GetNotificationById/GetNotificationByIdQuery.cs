using AutoMapper;
using AutoMapper.QueryableExtensions;
using Gym.Application.Notifications.DTOs;
using Gym.Domain.Entities;
using Gym.Domain.Interfaces;
using Gym.Shared.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gym.Application.Notifications.Queries.GetNotificationById;

public record GetNotificationByIdQuery(Guid Id) : IRequest<Result<NotificationDto>>;

public class GetNotificationByIdQueryHandler : IRequestHandler<GetNotificationByIdQuery, Result<NotificationDto>>
{
    private readonly IRepository<Notification> _repository;
    private readonly IMapper _mapper;

    public GetNotificationByIdQueryHandler(IRepository<Notification> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<Result<NotificationDto>> Handle(GetNotificationByIdQuery request, CancellationToken cancellationToken)
    {
        var notification = await _repository.Query()
            .Include(n => n.Member)
            .ProjectTo<NotificationDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(n => n.Id == request.Id, cancellationToken);

        if (notification == null)
            return Result<NotificationDto>.Failure("Notification not found");

        return Result<NotificationDto>.Success(notification);
    }
}
