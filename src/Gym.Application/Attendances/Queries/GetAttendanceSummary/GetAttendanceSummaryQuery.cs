using AutoMapper;
using Gym.Application.Attendances.DTOs;
using Gym.Domain.Entities;
using Gym.Domain.Interfaces;
using Gym.Shared.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gym.Application.Attendances.Queries.GetAttendanceSummary;

public record GetAttendanceSummaryQuery(Guid MemberId, DateTime Date) : IRequest<Result<AttendanceSummaryDto?>>;

public class GetAttendanceSummaryQueryHandler : IRequestHandler<GetAttendanceSummaryQuery, Result<AttendanceSummaryDto?>>
{
    private readonly IRepository<AttendanceSummary> _summaryRepository;
    private readonly IMapper _mapper;

    public GetAttendanceSummaryQueryHandler(IRepository<AttendanceSummary> summaryRepository, IMapper mapper)
    {
        _summaryRepository = summaryRepository;
        _mapper = mapper;
    }

    public async Task<Result<AttendanceSummaryDto?>> Handle(GetAttendanceSummaryQuery request, CancellationToken cancellationToken)
    {
        var summary = await _summaryRepository.Query()
            .Include(s => s.Member)
            .FirstOrDefaultAsync(s => s.MemberId == request.MemberId && s.Date.Date == request.Date.Date, cancellationToken);

        if (summary is null)
            return Result<AttendanceSummaryDto?>.Success(null);

        var dto = _mapper.Map<AttendanceSummaryDto>(summary);
        return Result<AttendanceSummaryDto?>.Success(dto);
    }
}

public record GetDailySummariesQuery(DateTime Date) : IRequest<Result<List<AttendanceSummaryDto>>>;

public class GetDailySummariesQueryHandler : IRequestHandler<GetDailySummariesQuery, Result<List<AttendanceSummaryDto>>>
{
    private readonly IRepository<AttendanceSummary> _summaryRepository;
    private readonly IMapper _mapper;

    public GetDailySummariesQueryHandler(IRepository<AttendanceSummary> summaryRepository, IMapper mapper)
    {
        _summaryRepository = summaryRepository;
        _mapper = mapper;
    }

    public async Task<Result<List<AttendanceSummaryDto>>> Handle(GetDailySummariesQuery request, CancellationToken cancellationToken)
    {
        var summaries = await _summaryRepository.Query()
            .Include(s => s.Member)
            .Where(s => s.Date.Date == request.Date.Date)
            .OrderBy(s => s.Member.FullName)
            .ToListAsync(cancellationToken);

        var dtos = _mapper.Map<List<AttendanceSummaryDto>>(summaries);
        return Result<List<AttendanceSummaryDto>>.Success(dtos);
    }
}
