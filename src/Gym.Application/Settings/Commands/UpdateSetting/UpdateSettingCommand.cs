using FluentValidation;
using Microsoft.Extensions.Localization;
using Gym.Application;
using Gym.Domain.Entities;
using Gym.Domain.Interfaces;
using Gym.Shared.Common;
using MediatR;

namespace Gym.Application.Settings.Commands.UpdateSetting;

public record UpdateSettingCommand(Guid Id, string Value) : IRequest<Result>;

public class UpdateSettingCommandHandler : IRequestHandler<UpdateSettingCommand, Result>
{
    private readonly IRepository<Setting> _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IStringLocalizer<ApplicationResources> _localizer;

    public UpdateSettingCommandHandler(IRepository<Setting> repository, IUnitOfWork unitOfWork, IStringLocalizer<ApplicationResources> localizer)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _localizer = localizer;
    }

    public async Task<Result> Handle(UpdateSettingCommand request, CancellationToken cancellationToken)
    {
        var setting = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (setting == null)
            return Result.Failure(_localizer["Setting not found"]);

        setting.UpdateValue(request.Value);
        _repository.Update(setting);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}

public class UpdateSettingCommandValidator : AbstractValidator<UpdateSettingCommand>
{
    public UpdateSettingCommandValidator()
    {
        RuleFor(v => v.Id).NotEmpty();
        RuleFor(v => v.Value).NotEmpty();
    }
}
