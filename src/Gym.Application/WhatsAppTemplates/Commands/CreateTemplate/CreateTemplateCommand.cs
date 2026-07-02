using FluentValidation;
using Microsoft.Extensions.Localization;
using Gym.Application.Resources;
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
    private readonly IUnitOfWork _unitOfWork;

    public CreateTemplateCommandHandler(IRepository<WhatsAppTemplate> repo, IUnitOfWork unitOfWork)
    {
        _repo = repo;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(CreateTemplateCommand request, CancellationToken cancellationToken)
    {
        var exists = await _repo.AnyAsync(t => t.Name == request.Name, cancellationToken);
        if (exists)
            return Result<Guid>.Failure("A template with this name already exists");

        var template = new WhatsAppTemplate(request.Name, request.MessageBody, request.TriggerType);
        await _repo.AddAsync(template, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<Guid>.Success(template.Id, "Template created successfully");
    }
}

public class CreateTemplateCommandValidator : AbstractValidator<CreateTemplateCommand>
{
    private readonly IStringLocalizer<ApplicationResources> _localizer;

    public CreateTemplateCommandValidator(IStringLocalizer<ApplicationResources> localizer)
    {
        _localizer = localizer;
        RuleFor(v => v.Name)
            .NotEmpty().WithMessage(_localizer["Name is required"])
            .MaximumLength(200).WithMessage(_localizer["Name must not exceed 200 characters"]);
        RuleFor(v => v.MessageBody)
            .NotEmpty().WithMessage(_localizer["Message body is required"])
            .MaximumLength(2000).WithMessage(_localizer["Message body must not exceed 2000 characters"]);
    }
}
