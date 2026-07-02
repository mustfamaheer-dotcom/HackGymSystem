using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Gym.Application.Common.Interfaces;
using Gym.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Gym.Infrastructure.Security;

public class TokenService : ITokenService
{
    private readonly IConfiguration _configuration;

    public TokenService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<(string accessToken, string refreshToken, DateTime expiresAt)> GenerateTokensAsync(User user)
    {
        var accessToken = await GenerateAccessTokenAsync(user);
        var refreshToken = GenerateRefreshToken();
        return (accessToken, refreshToken, DateTime.UtcNow.AddMinutes(GetAccessTokenExpiryMinutes()));
    }

    public async Task<(string accessToken, string refreshToken, DateTime expiresAt)> GenerateTokensAsync(User user, List<string> permissions)
    {
        var accessToken = await GenerateAccessTokenAsync(user, permissions);
        var refreshToken = GenerateRefreshToken();
        return (accessToken, refreshToken, DateTime.UtcNow.AddMinutes(GetAccessTokenExpiryMinutes()));
    }

    public async Task<Guid?> ValidateTokenAsync(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Secret"]!);

        try
        {
            var result = await tokenHandler.ValidateTokenAsync(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidIssuer = _configuration["Jwt:Issuer"],
                ValidateAudience = true,
                ValidAudience = _configuration["Jwt:Audience"],
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            });

            if (result.SecurityToken is JwtSecurityToken jwtToken)
            {
                var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
                if (Guid.TryParse(userIdClaim, out var userId))
                    return userId;
            }

            return null;
        }
        catch
        {
            return null;
        }
    }

    public string GenerateRefreshToken()
    {
        var randomBytes = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomBytes);
        return Convert.ToBase64String(randomBytes);
    }

    private async Task<string> GenerateAccessTokenAsync(User user)
    {
        return await GenerateAccessTokenAsync(user, new List<string>());
    }

    private async Task<string> GenerateAccessTokenAsync(User user, List<string> permissions)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Secret"]!));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expiryMinutes = GetAccessTokenExpiryMinutes();

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.Username),
            new(ClaimTypes.Email, user.Email),
            new(ClaimTypes.Role, user.Role?.Name ?? "User"),
            new("fullName", user.FullName)
        };

        foreach (var perm in permissions)
        {
            claims.Add(new Claim("permission", perm));
        }

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(expiryMinutes),
            signingCredentials: credentials
        );

        return await Task.FromResult(new JwtSecurityTokenHandler().WriteToken(token));
    }

    private int GetAccessTokenExpiryMinutes()
        => int.TryParse(_configuration["Jwt:AccessTokenExpiryMinutes"], out var minutes) ? minutes : 60;
}
