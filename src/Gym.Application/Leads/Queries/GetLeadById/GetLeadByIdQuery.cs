using AutoMapper;
using Gym.Application.Leads.DTOs;
using Gym.Domain.Entities;
using Gym.Domain.Interfaces;
using Gym.Shared.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gym.Application.Leads.Queries.GetLeadById;

public record GetLeadByIdQuery(Guid Id) : IRequest<Result<LeadDto>>;

public class GetLeadByIdQueryHandler : IRequestHandler<GetLeadByIdQuery, Result<LeadDto>>
{
    private readonly IRepository<Lead> _repository;
    private readonly IMapper _mapper;

    public GetLeadByIdQueryHandler(IRepository<Lead> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<Result<LeadDto>> Handle(GetLeadByIdQuery request, CancellationToken cancellationToken)
    {
        var lead = await _repository.Query()
            .IgnoreQueryFilters()
            .Include(l => l.InterestedPackage)
            .FirstOrDefaultAsync(l => l.Id == request.Id, cancellationToken);

        if (lead == null)
            return Result<LeadDto>.Failure("Lead not found");

        return Result<LeadDto>.Success(_mapper.Map<LeadDto>(lead));
    }
}