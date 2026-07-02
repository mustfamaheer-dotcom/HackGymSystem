using FluentValidation;
using Gym.Domain.Entities;
using Gym.Domain.Interfaces;
using Gym.Shared.Common;
using Gym.Shared.Enums;
using MediatR;

namespace Gym.Application.WhatsAppTemplates.Commands.UpdateTemplate;

public record UpdateTemplateCommand(Guid Id, string Name, string MessageBody, bool IsActive, NotificationType? TriggerType = null) : IRequest<Result<Unit>>;

public class UpdateTemplateCommandHandler : IRequestHandler<UpdateTemplateCommand, Result<Unit>>
{
    private readonly IRepository<WhatsAppTemplate> _repo;

    public UpdateTemplateCommandHandler(IRepository<WhatsAppTemplate> repo)
    {
        _repo = repo;
    }

    public async Task<Result<Unit>> Handle(UpdateTemplateCommand request, CancellationToken cancellationToken)
    {
        var template = await _repo.GetByIdAsync(request.Id, cancellationToken);
        if (template == null)
            return Result<Unit>.Failure("Template not found");

        var duplicate = await _repo.AnyAsync(t => t.Name == request.Name && t.Id != request.Id, cancellationToken);
        if (duplicate)
            return Result<Unit>.Failure("Another template with this name already exists");

        template.Update(request.Name, request.MessageBody, request.IsActive, request.TriggerType);
        _repo.Update(template);

        return Result<Unit>.Success(Unit.Value, "Template updated successfully");
    }
}

public class UpdateTemplateCommandValidator : AbstractValidator<UpdateTemplateCommand>
{
    public UpdateTemplateCommandValidator()
    {
        RuleFor(v => v.Id)
            .NotEmpty().WithMessage("Id is required");
        RuleFor(v => v.Name)
            .NotEmpty().WithMessage("Name is required")
            .MaximumLength(200).WithMessage("Name must not exceed 200 characters");
        RuleFor(v => v.MessageBody)
            .NotEmpty().WithMessage("Message body is required")
            .MaximumLength(2000).WithMessage("Message body must not exceed 2000 characters");
    }
}
