using AutoMapper;
using AutoMapper.QueryableExtensions;
using Gym.Application.Attendances.DTOs;
using Gym.Domain.Entities;
using Gym.Domain.Interfaces;
using Gym.Shared.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gym.Application.Attendances.Queries.GetAttendanceById;

public record GetAttendanceByIdQuery(Guid Id) : IRequest<Result<AttendanceDto>>;

public class GetAttendanceByIdQueryHandler : IRequestHandler<GetAttendanceByIdQuery, Result<AttendanceDto>>
{
    private readonly IRepository<Attendance> _attendanceRepository;
    private readonly IMapper _mapper;

    public GetAttendanceByIdQueryHandler(IRepository<Attendance> attendanceRepository, IMapper mapper)
    {
        _attendanceRepository = attendanceRepository;
        _mapper = mapper;
    }

    public async Task<Result<AttendanceDto>> Handle(GetAttendanceByIdQuery request, CancellationToken cancellationToken)
    {
        var attendance = await _attendanceRepository.Query()
            .Include(a => a.Member)
            .Include(a => a.Device)
            .ProjectTo<AttendanceDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(a => a.Id == request.Id, cancellationToken);

        if (attendance == null)
            return Result<AttendanceDto>.Failure("Attendance not found");

        return Result<AttendanceDto>.Success(attendance);
    }
}
