using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using SG.Infrastructure.Auth.Extensions;

namespace SG.Infrastructure.Auth.Services;

public interface IPrincipalCurrentUser
{
    CurrentUser? User { get; }
    ClaimsPrincipal? ClaimPrincipal { get; }
}

public record CurrentUser(int Id, string UserName, string userEmail);

public sealed class PrincipalCurrentUserService : IPrincipalCurrentUser
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    public CurrentUser? User { get; }
    public ClaimsPrincipal? ClaimPrincipal { get; }

    public PrincipalCurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;

        var userId = _httpContextAccessor.HttpContext?.User?.GetUserIdFromClaims<int>();
        var userName = _httpContextAccessor.HttpContext?.User?.GetValueClaim<string>(JwtClaimsCustomNames.UserName);
        var userEmail = _httpContextAccessor.HttpContext?.User?.GetValueClaim<string>(JwtClaimsCustomNames.UserEmail);

        if(userId.HasValue)
        {
            User = new CurrentUser(Convert.ToInt32(userId), userName!, userEmail!);
        }

       ClaimPrincipal =  _httpContextAccessor.HttpContext?.User;
    }
}