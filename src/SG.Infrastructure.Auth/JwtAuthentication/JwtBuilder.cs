
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SG.Infrastructure.Auth.Extensions;

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


    public string GenerateAccessToken(List<Claim>? claimsWithValues = null) 
    {
        var idClaim = Guid.NewGuid().ToString();
        var claims = new List<Claim> 
        {
            new(ClaimTypes.NameIdentifier, idClaim),
            new(JwtRegisteredClaimNames.UniqueName, idClaim),
            new(JwtRegisteredClaimNames.Jti, idClaim),
            new(JwtClaimsCustomNames.IdClaim, idClaim),
            new(JwtClaimsCustomNames.AudClaim, _jwtOptions.Audience)
        };

        if(claimsWithValues?.Any() ?? false)
        {
            claims.AddRange(claimsWithValues);
        }

        var curentTime = DateTime.UtcNow;
        var timeTokenExpiration = NewTimeTokenExpiration();


        var jwtToken = new JwtSecurityToken
        (
            claims: claims,
            notBefore: curentTime,
            expires:  timeTokenExpiration,
            issuer: _jwtOptions.Issuer,
            audience: _jwtOptions.Audience,
            signingCredentials: new SigningCredentials
            (
                new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtOptions.SigningKey)),
                SecurityAlgorithms.HmacSha256Signature
            )
        );

        var rawToken = new JwtSecurityTokenHandler().WriteToken(jwtToken);
        return rawToken;
    }

    public  DateTime NewTimeTokenExpiration() =>  _jwtOptions.TokenLifeTimeTypeExpiration.ToLower() switch
       {
           "seconds" =>  DateTime.UtcNow.AddSeconds(_jwtOptions.TokenLifeTime),
           "minutes" =>  DateTime.UtcNow.AddMinutes(_jwtOptions.TokenLifeTime),
           "hours" =>  DateTime.UtcNow.AddHours(_jwtOptions.TokenLifeTime),
           "days" =>  DateTime.UtcNow.AddDays(_jwtOptions.TokenLifeTime),
           _ =>  DateTime.UtcNow.AddMinutes(_jwtOptions.TokenLifeTime)
       };

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
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = _jwtOptions.Issuer,
                ValidAudience = _jwtOptions.Audience,
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

    public ClaimsPrincipal? GetPrincipal(string token)
    {    
        if (string.IsNullOrWhiteSpace(token))
            return null;

        var tokenHandler = new JwtSecurityTokenHandler();
        return tokenHandler.ValidateToken(token, new TokenValidationParameters
        {
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtOptions.SigningKey)),
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = _jwtOptions.Issuer,
            ValidAudience = _jwtOptions.Audience,
            ClockSkew = TimeSpan.Zero
        }, out _);
    }

    public JwtSecurityToken ReadJwtToken(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        var jwtSecurityToken = handler.ReadJwtToken(token);
        return jwtSecurityToken;
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