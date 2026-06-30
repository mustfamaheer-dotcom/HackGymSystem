using AutoMapper;
using AutoMapper.QueryableExtensions;
using Gym.Application.Memberships.DTOs;
using Gym.Domain.Entities;
using Gym.Domain.Interfaces;
using Gym.Shared.Common;
using MediatR;

namespace Gym.Application.Memberships.Queries.GetMemberMemberships;

public record GetMemberMembershipsQuery(Guid MemberId) : IRequest<Result<List<MembershipDto>>>;

public class GetMemberMembershipsQueryHandler : IRequestHandler<GetMemberMembershipsQuery, Result<List<MembershipDto>>>
{
    private readonly IRepository<Membership> _membershipRepo;
    private readonly IMapper _mapper;

    public GetMemberMembershipsQueryHandler(IRepository<Membership> membershipRepo, IMapper mapper)
    {
        _membershipRepo = membershipRepo;
        _mapper = mapper;
    }

    public Task<Result<List<MembershipDto>>> Handle(GetMemberMembershipsQuery request, CancellationToken cancellationToken)
    {
        var memberships = _membershipRepo.Query()
            .Where(m => m.MemberId == request.MemberId)
            .ProjectTo<MembershipDto>(_mapper.ConfigurationProvider)
            .ToList();

        return Task.FromResult(Result<List<MembershipDto>>.Success(memberships));
    }
}
