using AutoMapper;
using AutoMapper.QueryableExtensions;
using Gym.Application.Attendances.DTOs;
using Gym.Domain.Entities;
using Gym.Domain.Interfaces;
using Gym.Shared.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gym.Application.Attendances.Queries.GetTodayAttendances;

public record GetTodayAttendancesQuery : IRequest<Result<List<AttendanceDto>>>;

public class GetTodayAttendancesQueryHandler : IRequestHandler<GetTodayAttendancesQuery, Result<List<AttendanceDto>>>
{
    private readonly IRepository<Attendance> _attendanceRepository;
    private readonly IMapper _mapper;

    public GetTodayAttendancesQueryHandler(IRepository<Attendance> attendanceRepository, IMapper mapper)
    {
        _attendanceRepository = attendanceRepository;
        _mapper = mapper;
    }

    public async Task<Result<List<AttendanceDto>>> Handle(GetTodayAttendancesQuery request, CancellationToken cancellationToken)
    {
        var today = DateTime.UtcNow.Date;

        var attendances = await _attendanceRepository.Query()
            .Include(a => a.Member)
            .Where(a => a.Date == today)
            .OrderByDescending(a => a.Time)
            .ProjectTo<AttendanceDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        return Result<List<AttendanceDto>>.Success(attendances);
    }
}
