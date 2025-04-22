using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using SG.Infrastructure.Auth.Extensions;

namespace SG.Infrastructure.Auth.Services;

public interface IPrincipalCurrentUser
{
    CurrentUser? User { get; }
    ClaimsPrincipal? ClaimPrincipal { get; }
}

public record CurrentUser(int Id, string UserName);

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

        if(userId.HasValue)
        {
            User = new CurrentUser(Convert.ToInt32(userId), userName!);
        }

       ClaimPrincipal =  _httpContextAccessor.HttpContext?.User;
    }
}