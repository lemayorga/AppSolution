using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Json;
using SG.Domain.Security.Entities;
using SG.Infrastructure.Auth.JwtAuthentication.Models;

namespace SG.Infrastructure.Auth.Extensions;

public static class JwtClaimsCustomNames 
{
    public const string IdClaim  = "idClaim";
    public const string AudClaim  = "aud";
    public const string UserId  = "idUser";
    public const string UserName  = "userName";
    public const string UserEmail  = "userEmail";
    public const string Roles  = "roles";
}


public static class JwtClaimExt
{
    public static List<Claim> AddWithUserToClaim(this List<Claim> claims, User user)
    {
        claims.AddRange(new List<Claim> 
        {
            new Claim(JwtClaimsCustomNames.UserId, user.Id.ToString()),
            new Claim(JwtClaimsCustomNames.UserName, user.Username),
            new Claim(JwtClaimsCustomNames.UserEmail, user.Email),
            new Claim(JwtRegisteredClaimNames.Email, user.Email.ToString(), ClaimValueTypes.Email)
        });
        return claims;
    }

    public static List<Claim> AddWithRolesToClaim(this List<Claim> claims, IEnumerable<JwtRolData> roles)
    {
        var rolesJson = JsonSerializer.Serialize(roles);
        claims.Add(new Claim(JwtClaimsCustomNames.Roles, rolesJson));
        return claims;
    }

    public static T? GetValueClaim<T>(this ClaimsPrincipal user, string typeClaim)
    {
        object valueClaim  = user?.Claims?.FirstOrDefault(claim => claim.Type == typeClaim)?.Value!;
        if(valueClaim is not null)
        {
            return  (T)Convert.ChangeType(valueClaim, typeof(T));
        }

        T? str = default(T); 
        return str;
    }

    public static T? GetUserIdFromClaims<T>(this ClaimsPrincipal user)
    {
        return user.GetValueClaim<T?>(JwtClaimsCustomNames.UserId);
    }
}
