using AutoMapper;
using Gym.Application.Common.DTOs;
using Gym.Application.Members.DTOs;
using Gym.Domain.Entities;
using Gym.Domain.Interfaces;
using Gym.Shared.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gym.Application.Members.Queries.GetAllMembers;

public record GetAllMembersQuery(
    int Page = 1,
    int PageSize = 20,
    string? SearchTerm = null,
    string? SortBy = null,
    bool SortDescending = false
) : IRequest<Result<PaginatedResult<MemberDto>>>;

public class GetAllMembersQueryHandler : IRequestHandler<GetAllMembersQuery, Result<PaginatedResult<MemberDto>>>
{
    private readonly IRepository<Member> _repository;
    private readonly IMapper _mapper;

    public GetAllMembersQueryHandler(IRepository<Member> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<Result<PaginatedResult<MemberDto>>> Handle(GetAllMembersQuery request, CancellationToken cancellationToken)
    {
        IQueryable<Member> query = _repository.Query()
            .Include(m => m.Package);

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            var search = request.SearchTerm.ToLower();
            query = query.Where(m =>
                m.FullName.ToLower().Contains(search) ||
                m.ReceiptNumber.ToLower().Contains(search) ||
                m.PhoneNumber.Contains(search) ||
                m.NationalId.Contains(search));
        }

        var totalCount = await query.CountAsync(cancellationToken);

        query = (request.SortBy?.ToLower()) switch
        {
            "fullname" => request.SortDescending
                ? query.OrderByDescending(m => m.FullName)
                : query.OrderBy(m => m.FullName),
            "registrationdate" => request.SortDescending
                ? query.OrderByDescending(m => m.RegistrationDate)
                : query.OrderBy(m => m.RegistrationDate),
            _ => query.OrderBy(m => m.CreatedAt)
        };

        var items = await query
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);

        var dtos = _mapper.Map<List<MemberDto>>(items);

        return Result<PaginatedResult<MemberDto>>.Success(new PaginatedResult<MemberDto>
        {
            Items = dtos,
            TotalCount = totalCount,
            Page = request.Page,
            PageSize = request.PageSize
        });
    }
}
