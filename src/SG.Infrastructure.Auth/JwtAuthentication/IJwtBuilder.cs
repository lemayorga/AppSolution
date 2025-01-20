using System;
using System.Security.Claims;
using SG.Domain.Security.Entities;
using SG.Infrastructure.Auth.JwtAuthentication.Models;

namespace SG.Infrastructure.Auth.JwtAuthentication;

public interface IJwtBuilder
{
    string GenerateAccessToken(User user,IEnumerable<JwtRolData>? roles, string[]? permissions = null);
    bool ValidateJwtToken(string token);
    string GenerateRefreshToken();
    string GenerateAccessTokenFromRefreshToken();
    ClaimsPrincipal? GetTokenPrincipal(string token);
}

