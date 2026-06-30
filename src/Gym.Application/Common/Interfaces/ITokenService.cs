using Gym.Shared.Common;

namespace Gym.Application.Common.Interfaces;

public interface ITokenService
{
    Task<(string accessToken, string refreshToken, DateTime expiresAt)> GenerateTokensAsync(Domain.Entities.User user);
    Task<Guid?> ValidateTokenAsync(string token);
    string GenerateRefreshToken();
}
