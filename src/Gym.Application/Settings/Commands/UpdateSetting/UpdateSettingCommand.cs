using FluentValidation;
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

    public UpdateSettingCommandHandler(IRepository<Setting> repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(UpdateSettingCommand request, CancellationToken cancellationToken)
    {
        var setting = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (setting == null)
            return Result.Failure("Setting not found");

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
