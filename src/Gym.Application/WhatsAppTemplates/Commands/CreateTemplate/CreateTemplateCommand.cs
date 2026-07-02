using FluentValidation;
using Gym.Domain.Entities;
using Gym.Domain.Interfaces;
using Gym.Shared.Common;
using Gym.Shared.Enums;
using MediatR;

namespace Gym.Application.WhatsAppTemplates.Commands.CreateTemplate;

public record CreateTemplateCommand(string Name, string MessageBody, NotificationType? TriggerType = null) : IRequest<Result<Guid>>;

public class CreateTemplateCommandHandler : IRequestHandler<CreateTemplateCommand, Result<Guid>>
{
    private readonly IRepository<WhatsAppTemplate> _repo;

    public CreateTemplateCommandHandler(IRepository<WhatsAppTemplate> repo)
    {
        _repo = repo;
    }

    public async Task<Result<Guid>> Handle(CreateTemplateCommand request, CancellationToken cancellationToken)
    {
        var exists = await _repo.AnyAsync(t => t.Name == request.Name, cancellationToken);
        if (exists)
            return Result<Guid>.Failure("A template with this name already exists");

        var template = new WhatsAppTemplate(request.Name, request.MessageBody, request.TriggerType);
        await _repo.AddAsync(template, cancellationToken);

        return Result<Guid>.Success(template.Id, "Template created successfully");
    }
}

public class CreateTemplateCommandValidator : AbstractValidator<CreateTemplateCommand>
{
    public CreateTemplateCommandValidator()
    {
        RuleFor(v => v.Name)
            .NotEmpty().WithMessage("Name is required")
            .MaximumLength(200).WithMessage("Name must not exceed 200 characters");
        RuleFor(v => v.MessageBody)
            .NotEmpty().WithMessage("Message body is required")
            .MaximumLength(2000).WithMessage("Message body must not exceed 2000 characters");
    }
}
