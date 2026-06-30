using AutoMapper;
using AutoMapper.QueryableExtensions;
using Gym.Application.Attendances.DTOs;
using Gym.Domain.Entities;
using Gym.Domain.Interfaces;
using Gym.Shared.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gym.Application.Attendances.Queries.GetMemberAttendances;

public record GetMemberAttendancesQuery(Guid MemberId) : IRequest<Result<List<AttendanceDto>>>;

public class GetMemberAttendancesQueryHandler : IRequestHandler<GetMemberAttendancesQuery, Result<List<AttendanceDto>>>
{
    private readonly IRepository<Attendance> _attendanceRepository;
    private readonly IMapper _mapper;

    public GetMemberAttendancesQueryHandler(IRepository<Attendance> attendanceRepository, IMapper mapper)
    {
        _attendanceRepository = attendanceRepository;
        _mapper = mapper;
    }

    public async Task<Result<List<AttendanceDto>>> Handle(GetMemberAttendancesQuery request, CancellationToken cancellationToken)
    {
        var attendances = await _attendanceRepository.Query()
            .Include(a => a.Member)
            .Where(a => a.MemberId == request.MemberId)
            .OrderByDescending(a => a.Date)
            .ProjectTo<AttendanceDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        return Result<List<AttendanceDto>>.Success(attendances);
    }
}
