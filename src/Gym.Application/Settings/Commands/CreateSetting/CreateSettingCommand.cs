using FluentValidation;
using Gym.Domain.Entities;
using Gym.Domain.Interfaces;
using Gym.Shared.Common;
using MediatR;

namespace Gym.Application.Settings.Commands.CreateSetting;

public record CreateSettingCommand(
    string Key,
    string Value,
    string? Group = null,
    string? Description = null,
    bool IsEncrypted = false) : IRequest<Result<Guid>>;

public class CreateSettingCommandHandler : IRequestHandler<CreateSettingCommand, Result<Guid>>
{
    private readonly IRepository<Setting> _repository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateSettingCommandHandler(IRepository<Setting> repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(CreateSettingCommand request, CancellationToken cancellationToken)
    {
        var setting = new Setting(
            request.Key,
            request.Value,
            request.Group,
            request.Description,
            request.IsEncrypted);

        await _repository.AddAsync(setting, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<Guid>.Success(setting.Id);
    }
}

public class CreateSettingCommandValidator : AbstractValidator<CreateSettingCommand>
{
    public CreateSettingCommandValidator()
    {
        RuleFor(v => v.Key).NotEmpty().MaximumLength(100);
        RuleFor(v => v.Value).NotEmpty();
    }
}
