using AutoMapper;
using Gym.Application.Members.DTOs;
using Gym.Domain.Entities;
using Gym.Domain.Interfaces;
using Gym.Shared.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gym.Application.Members.Queries.GetMemberById;

public record GetMemberByIdQuery(Guid Id) : IRequest<Result<MemberDto>>;

public class GetMemberByIdQueryHandler : IRequestHandler<GetMemberByIdQuery, Result<MemberDto>>
{
    private readonly IRepository<Member> _repository;
    private readonly IMapper _mapper;

    public GetMemberByIdQueryHandler(IRepository<Member> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<Result<MemberDto>> Handle(GetMemberByIdQuery request, CancellationToken cancellationToken)
    {
        var member = await _repository.Query()
            .Include(m => m.Package)
            .FirstOrDefaultAsync(m => m.Id == request.Id, cancellationToken);

        if (member is null)
            return Result<MemberDto>.Failure("Member not found");

        var dto = _mapper.Map<MemberDto>(member);
        return Result<MemberDto>.Success(dto);
    }
}
