using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace SG.Infrastructure.Auth.JwtAuthentication;

public interface IJwtBuilder
{
    DateTime NewTimeTokenExpiration();
    string GenerateAccessToken(List<Claim>? claimsWithValues = null);
    bool ValidateJwtToken(string token);
    string GenerateRefreshToken();
    string GenerateAccessTokenFromRefreshToken();
    ClaimsPrincipal? GetPrincipal(string token);
    JwtSecurityToken ReadJwtToken(string token);
}