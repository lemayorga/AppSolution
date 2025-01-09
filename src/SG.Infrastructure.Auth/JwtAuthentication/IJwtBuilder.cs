using System;
using System.Security.Claims;
using SG.Domain.Security.Entities;

namespace SG.Infrastructure.Auth.JwtAuthentication;

public interface IJwtBuilder
{
    string GenerateAccessToken(User user, string[]? permissions = null);
    bool ValidateJwtToken(string token);
    string GenerateRefreshToken();
    string GenerateAccessTokenFromRefreshToken();
    ClaimsPrincipal? GetTokenPrincipal(string token);
}

