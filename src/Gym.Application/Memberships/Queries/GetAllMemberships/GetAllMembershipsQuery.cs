using AutoMapper;
using AutoMapper.QueryableExtensions;
using Gym.Application.Common.DTOs;
using Gym.Application.Memberships.DTOs;
using Gym.Domain.Entities;
using Gym.Domain.Interfaces;
using Gym.Shared.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gym.Application.Memberships.Queries.GetAllMemberships;

public record GetAllMembershipsQuery : PaginationRequest, IRequest<Result<PaginatedResult<MembershipDto>>>;

public class GetAllMembershipsQueryHandler : IRequestHandler<GetAllMembershipsQuery, Result<PaginatedResult<MembershipDto>>>
{
    private readonly IRepository<Membership> _membershipRepo;
    private readonly IMapper _mapper;

    public GetAllMembershipsQueryHandler(IRepository<Membership> membershipRepo, IMapper mapper)
    {
        _membershipRepo = membershipRepo;
        _mapper = mapper;
    }

    public async Task<Result<PaginatedResult<MembershipDto>>> Handle(GetAllMembershipsQuery request, CancellationToken cancellationToken)
    {
        var query = _membershipRepo.Query()
            .ProjectTo<MembershipDto>(_mapper.ConfigurationProvider);

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);

        return Result<PaginatedResult<MembershipDto>>.Success(new PaginatedResult<MembershipDto>
        {
            Items = items,
            TotalCount = totalCount,
            Page = request.Page,
            PageSize = request.PageSize
        });
    }
}
