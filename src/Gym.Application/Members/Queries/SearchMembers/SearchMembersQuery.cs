using AutoMapper;
using Gym.Application.Members.DTOs;
using Gym.Domain.Entities;
using Gym.Domain.Interfaces;
using Gym.Shared.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gym.Application.Members.Queries.SearchMembers;

public record SearchMembersQuery(string SearchTerm) : IRequest<Result<List<MemberDto>>>;

public class SearchMembersQueryHandler : IRequestHandler<SearchMembersQuery, Result<List<MemberDto>>>
{
    private readonly IRepository<Member> _repository;
    private readonly IMapper _mapper;

    public SearchMembersQueryHandler(IRepository<Member> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<Result<List<MemberDto>>> Handle(SearchMembersQuery request, CancellationToken cancellationToken)
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

        var members = await query.ToListAsync(cancellationToken);
        var dtos = _mapper.Map<List<MemberDto>>(members);

        return Result<List<MemberDto>>.Success(dtos);
    }
}
