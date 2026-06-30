using AutoMapper;
using Gym.Application.Members.DTOs;
using Gym.Domain.Interfaces;
using Gym.Shared.Common;
using MediatR;

namespace Gym.Application.Members.Queries.GetExpiringMembers;

public record GetExpiringMembersQuery(int WithinDays = 7) : IRequest<Result<List<MemberDto>>>;

public class GetExpiringMembersQueryHandler : IRequestHandler<GetExpiringMembersQuery, Result<List<MemberDto>>>
{
    private readonly IMemberRepository _memberRepository;
    private readonly IMapper _mapper;

    public GetExpiringMembersQueryHandler(IMemberRepository memberRepository, IMapper mapper)
    {
        _memberRepository = memberRepository;
        _mapper = mapper;
    }

    public async Task<Result<List<MemberDto>>> Handle(GetExpiringMembersQuery request, CancellationToken cancellationToken)
    {
        var members = await _memberRepository.GetExpiringMembersAsync(request.WithinDays, cancellationToken);
        var dtos = _mapper.Map<List<MemberDto>>(members);
        return Result<List<MemberDto>>.Success(dtos);
    }
}
