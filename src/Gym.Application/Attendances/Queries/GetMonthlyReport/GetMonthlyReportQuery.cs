using AutoMapper;
using AutoMapper.QueryableExtensions;
using Gym.Application.Attendances.DTOs;
using Gym.Domain.Entities;
using Gym.Domain.Interfaces;
using Gym.Shared.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gym.Application.Attendances.Queries.GetMonthlyReport;

public record GetMonthlyReportQuery(int Year, int Month) : IRequest<Result<List<MonthlyReportDto>>>;

public class GetMonthlyReportQueryHandler : IRequestHandler<GetMonthlyReportQuery, Result<List<MonthlyReportDto>>>
{
    private readonly IRepository<AttendanceSummary> _summaryRepository;
    private readonly IRepository<Member> _memberRepository;
    private readonly IMapper _mapper;

    public GetMonthlyReportQueryHandler(
        IRepository<AttendanceSummary> summaryRepository,
        IRepository<Member> memberRepository,
        IMapper mapper)
    {
        _summaryRepository = summaryRepository;
        _memberRepository = memberRepository;
        _mapper = mapper;
    }

    public async Task<Result<List<MonthlyReportDto>>> Handle(GetMonthlyReportQuery request, CancellationToken cancellationToken)
    {
        var startDate = new DateTime(request.Year, request.Month, 1);
        var endDate = startDate.AddMonths(1).AddDays(-1);

        var summaries = await _summaryRepository.Query()
            .Include(s => s.Member)
            .Where(s => s.Date >= startDate && s.Date <= endDate)
            .OrderBy(s => s.Member.FullName)
            .ThenBy(s => s.Date)
            .ToListAsync(cancellationToken);

        var memberSummaries = summaries
            .GroupBy(s => s.MemberId)
            .Select(g => new MonthlyReportDto
            {
                MemberId = g.Key,
                MemberName = g.First().Member.FullName,
                MemberCode = g.First().Member.Code.ToString(),
                Year = request.Year,
                Month = request.Month,
                PresentDays = g.Count(s => s.Status == AttendanceStatus.Present),
                LateDays = g.Count(s => s.Status == AttendanceStatus.Late),
                AbsentDays = g.Count(s => s.Status == AttendanceStatus.Absent),
                EarlyLeaveDays = g.Count(s => s.Status == AttendanceStatus.EarlyLeave),
                HalfDays = g.Count(s => s.Status == AttendanceStatus.HalfDay),
                OnLeaveDays = g.Count(s => s.Status == AttendanceStatus.OnLeave),
                TotalWorkHours = g.Sum(s => s.TotalWorkHours),
                WorkingDaysInMonth = GetWorkingDaysInMonth(request.Year, request.Month),
                AttendancePercentage = CalculateAttendancePercentage(g.Key, request.Year, request.Month, g.Count())
            })
            .ToList();

        return Result<List<MonthlyReportDto>>.Success(memberSummaries);
    }

    private static int GetWorkingDaysInMonth(int year, int month)
    {
        var startDate = new DateTime(year, month, 1);
        var endDate = startDate.AddMonths(1).AddDays(-1);
        var workingDays = 0;

        for (var date = startDate; date <= endDate; date = date.AddDays(1))
        {
            if (date.DayOfWeek != DayOfWeek.Friday && date.DayOfWeek != DayOfWeek.Saturday)
                workingDays++;
        }

        return workingDays;
    }

    private static double CalculateAttendancePercentage(Guid memberId, int year, int month, int presentDays)
    {
        var workingDays = GetWorkingDaysInMonth(year, month);
        return workingDays > 0 ? Math.Round((double)presentDays / workingDays * 100, 2) : 0;
    }
}
