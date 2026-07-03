using FluentValidation;
using Microsoft.Extensions.Localization;
using Gym.Application.Resources;
using Gym.Domain.Entities;
using Gym.Domain.Interfaces;
using Gym.Shared.Common;
using MediatR;

namespace Gym.Application.Settings.Commands.DeleteSetting;

public record DeleteSettingCommand(Guid Id) : IRequest<Result>;

public class DeleteSettingCommandHandler : IRequestHandler<DeleteSettingCommand, Result>
{
    private readonly IRepository<Setting> _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IStringLocalizer<ApplicationResources> _localizer;

    public DeleteSettingCommandHandler(IRepository<Setting> repository, IUnitOfWork unitOfWork, IStringLocalizer<ApplicationResources> localizer)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _localizer = localizer;
    }

    public async Task<Result> Handle(DeleteSettingCommand request, CancellationToken cancellationToken)
    {
        var setting = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (setting == null)
            return Result.Failure(_localizer["Setting not found"]);

        _repository.Delete(setting);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}

public class DeleteSettingCommandValidator : AbstractValidator<DeleteSettingCommand>
{
    public DeleteSettingCommandValidator()
    {
        RuleFor(v => v.Id).NotEmpty();
    }
}
