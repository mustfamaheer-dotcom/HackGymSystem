using AutoMapper;
using Gym.Application.WhatsAppTemplates.DTOs;
using Gym.Domain.Interfaces;
using Gym.Shared.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gym.Application.WhatsAppTemplates.Queries.GetAllTemplates;

public record GetAllTemplatesQuery : IRequest<Result<List<WhatsAppTemplateDto>>>;

public class GetAllTemplatesQueryHandler : IRequestHandler<GetAllTemplatesQuery, Result<List<WhatsAppTemplateDto>>>
{
    private readonly IRepository<Domain.Entities.WhatsAppTemplate> _repo;
    private readonly IMapper _mapper;

    public GetAllTemplatesQueryHandler(IRepository<Domain.Entities.WhatsAppTemplate> repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<Result<List<WhatsAppTemplateDto>>> Handle(GetAllTemplatesQuery request, CancellationToken cancellationToken)
    {
        var items = await _repo.Query()
            .OrderByDescending(t => t.CreatedAt)
            .ToListAsync(cancellationToken);

        var dtos = _mapper.Map<List<WhatsAppTemplateDto>>(items);
        return Result<List<WhatsAppTemplateDto>>.Success(dtos);
    }
}
