using AutoMapper;
using AutoMapper.QueryableExtensions;
using Gym.Application.Attendances.DTOs;
using Gym.Application.Common.DTOs;
using Gym.Domain.Entities;
using Gym.Domain.Interfaces;
using Gym.Shared.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gym.Application.Attendances.Queries.GetAllAttendances;

public record GetAllAttendancesQuery : PaginationRequest, IRequest<Result<PaginatedResult<AttendanceDto>>>;

public class GetAllAttendancesQueryHandler : IRequestHandler<GetAllAttendancesQuery, Result<PaginatedResult<AttendanceDto>>>
{
    private readonly IRepository<Attendance> _attendanceRepository;
    private readonly IMapper _mapper;

    public GetAllAttendancesQueryHandler(IRepository<Attendance> attendanceRepository, IMapper mapper)
    {
        _attendanceRepository = attendanceRepository;
        _mapper = mapper;
    }

    public async Task<Result<PaginatedResult<AttendanceDto>>> Handle(GetAllAttendancesQuery request, CancellationToken cancellationToken)
    {
        var query = _attendanceRepository.Query()
            .Include(a => a.Member)
            .OrderByDescending(a => a.Date);

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ProjectTo<AttendanceDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        return Result<PaginatedResult<AttendanceDto>>.Success(new PaginatedResult<AttendanceDto>
        {
            Items = items,
            TotalCount = totalCount,
            Page = request.Page,
            PageSize = request.PageSize
        });
    }
}
