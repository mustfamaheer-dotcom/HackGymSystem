using AutoMapper;
using AutoMapper.QueryableExtensions;
using Gym.Application.Memberships.DTOs;
using Gym.Domain.Entities;
using Gym.Domain.Interfaces;
using Gym.Shared.Common;
using MediatR;

namespace Gym.Application.Memberships.Queries.GetMembershipById;

public record GetMembershipByIdQuery(Guid Id) : IRequest<Result<MembershipDetailDto>>;

public class GetMembershipByIdQueryHandler : IRequestHandler<GetMembershipByIdQuery, Result<MembershipDetailDto>>
{
    private readonly IRepository<Membership> _membershipRepo;
    private readonly IMapper _mapper;

    public GetMembershipByIdQueryHandler(IRepository<Membership> membershipRepo, IMapper mapper)
    {
        _membershipRepo = membershipRepo;
        _mapper = mapper;
    }

    public Task<Result<MembershipDetailDto>> Handle(GetMembershipByIdQuery request, CancellationToken cancellationToken)
    {
        var membership = _membershipRepo.Query()
            .ProjectTo<MembershipDetailDto>(_mapper.ConfigurationProvider)
            .FirstOrDefault(m => m.Id == request.Id);

        if (membership is null)
            return Task.FromResult(Result<MembershipDetailDto>.Failure("Membership not found"));

        return Task.FromResult(Result<MembershipDetailDto>.Success(membership));
    }
}
