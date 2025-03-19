using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SG.Domain.Security.Entities;
using SG.Infrastructure.Auth.JwtAuthentication.Models;

namespace SG.Infrastructure.Auth.JwtAuthentication;

public class JwtBuilder : IJwtBuilder
{
    private readonly IConfiguration _configuration;
    private readonly JwtOptions _jwtOptions;

    public JwtBuilder(IConfiguration configuration, JwtOptions jwtOptions)
    {
        _configuration = configuration;
        _jwtOptions = jwtOptions;
    }

    public string GenerateAccessToken(User user, IEnumerable<JwtRolData>? roles = null, string[]? permissions = null) 
    {
        var claims = new List<Claim> 
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Sub, user.Username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim("id", Guid.NewGuid().ToString()),
            new Claim("idUser", user.Id.ToString()),
            new Claim("username", user.Username),
            new Claim("email", user.Email),
            new Claim("aud", _jwtOptions.Audience)
        };


        roles ??= (new List<JwtRolData>()).AsEnumerable();
        if(roles is not null)
        {
            var rolesJson = JsonSerializer.Serialize(roles);
            claims.Add(new Claim("role", rolesJson));
        }
       
        permissions ??= new string[]{ };
        var roleClaims = permissions.Select(x => new Claim("role", x));
       // claims.AddRange(roleClaims);

      // var (hours , minutes ,_ , _) = ConvertUtil.SecondTo(_jwtOptions.ExpiratioMinutes);
       // var tokenExpiration = DateTime.UtcNow.AddHours(hours); //DateTime.UtcNow.AddDays(30),;
        var tokenExpiration = NewTimeTokenExpiration();

        var jwtToken = new JwtSecurityToken
        (
            claims: claims,
            notBefore: DateTime.UtcNow,
            expires:  tokenExpiration,
            issuer: _jwtOptions.Issuer,
            audience: _jwtOptions.Audience,
            signingCredentials: new SigningCredentials
            (
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SigningKey)),
                SecurityAlgorithms.HmacSha256Signature
            )
        );

        var rawToken = new JwtSecurityTokenHandler().WriteToken(jwtToken);
        return rawToken;
    }

    public  DateTime NewTimeTokenExpiration() => DateTime.UtcNow.AddMinutes(_jwtOptions.ExpiratioMinutes);

    public bool ValidateJwtToken(string token)
    {
        if (string.IsNullOrWhiteSpace(token))
            return false;

        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtOptions.SigningKey)),
                ValidateIssuer = false,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;
            var userId = jwtToken.Claims.Any(x => x.Type == "id");

            return userId;
        }
        catch
        {
            return false;
        }
    }

    public ClaimsPrincipal? GetTokenPrincipal(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        return tokenHandler.ValidateToken(token, new TokenValidationParameters
        {
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtOptions.SigningKey)),
            ValidateIssuer = false,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = _jwtOptions.Issuer,
            ValidAudience = _jwtOptions.Audience,
            // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
            ClockSkew = TimeSpan.Zero
        }, out _);
    }

    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }


    public string GenerateAccessTokenFromRefreshToken()
    {
        // Implement logic to generate a new access token from the refresh token
        // Verify the refresh token and extract necessary information (e.g., user ID)
        // Then generate a new access token

        // For demonstration purposes, return a new token with an extended expiry
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Expires = NewTimeTokenExpiration(), // Extend expiration time
            SigningCredentials = new SigningCredentials
            (
                new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtOptions.SigningKey)), 
                SecurityAlgorithms.HmacSha256Signature
            )
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        string refreshToken =  tokenHandler.WriteToken(token);
        return refreshToken;
    }
}
