using AutoMapper;
using Gym.Application.Members.DTOs;
using Gym.Domain.Interfaces;
using Gym.Shared.Common;
using MediatR;

namespace Gym.Application.Members.Queries.GetMembersWithOutstandingBalance;

public record GetMembersWithOutstandingBalanceQuery : IRequest<Result<List<MemberDto>>>;

public class GetMembersWithOutstandingBalanceQueryHandler : IRequestHandler<GetMembersWithOutstandingBalanceQuery, Result<List<MemberDto>>>
{
    private readonly IMemberRepository _memberRepository;
    private readonly IMapper _mapper;

    public GetMembersWithOutstandingBalanceQueryHandler(IMemberRepository memberRepository, IMapper mapper)
    {
        _memberRepository = memberRepository;
        _mapper = mapper;
    }

    public async Task<Result<List<MemberDto>>> Handle(GetMembersWithOutstandingBalanceQuery request, CancellationToken cancellationToken)
    {
        var members = await _memberRepository.GetMembersWithOutstandingBalanceAsync(cancellationToken);
        var dtos = _mapper.Map<List<MemberDto>>(members);
        return Result<List<MemberDto>>.Success(dtos);
    }
}
