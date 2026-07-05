using Gym.Shared.Common;

namespace Gym.Application.Common.Interfaces;

public interface ICaptchaService
{
    Task<Result> ValidateTokenAsync(string token, CancellationToken cancellationToken = default);
}
