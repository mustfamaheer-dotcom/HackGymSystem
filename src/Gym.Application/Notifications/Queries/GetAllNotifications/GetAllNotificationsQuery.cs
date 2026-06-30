using AutoMapper;
using AutoMapper.QueryableExtensions;
using Gym.Application.Common.DTOs;
using Gym.Application.Notifications.DTOs;
using Gym.Domain.Entities;
using Gym.Domain.Interfaces;
using Gym.Shared.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gym.Application.Notifications.Queries.GetAllNotifications;

public record GetAllNotificationsQuery : PaginationRequest, IRequest<Result<PaginatedResult<NotificationDto>>>;

public class GetAllNotificationsQueryHandler : IRequestHandler<GetAllNotificationsQuery, Result<PaginatedResult<NotificationDto>>>
{
    private readonly IRepository<Notification> _repository;
    private readonly IMapper _mapper;

    public GetAllNotificationsQueryHandler(IRepository<Notification> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<Result<PaginatedResult<NotificationDto>>> Handle(GetAllNotificationsQuery request, CancellationToken cancellationToken)
    {
        IQueryable<Notification> query = _repository.Query()
            .Include(n => n.Member)
            .OrderByDescending(n => n.CreatedAt);

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ProjectTo<NotificationDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        return Result<PaginatedResult<NotificationDto>>.Success(new PaginatedResult<NotificationDto>
        {
            Items = items,
            TotalCount = totalCount,
            Page = request.Page,
            PageSize = request.PageSize
        });
    }
}
