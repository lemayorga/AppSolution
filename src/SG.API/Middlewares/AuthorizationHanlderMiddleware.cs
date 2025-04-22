using System.IdentityModel.Tokens.Jwt;
using SG.Domain;
using SG.Infrastructure.Auth.Extensions;
using SG.Infrastructure.Auth.JwtAuthentication;

namespace SG.API.Middlewares;

public class AuthorizationHanlderMiddleware (RequestDelegate _next)
{
    public async Task Invoke(HttpContext context, IJwtBuilder jwtBuilder ,IUnitOfWork unitOfWork)
    {
        var bearer = context.Request.Headers["Authorization"].ToString();
        var token = bearer.Replace("Bearer ", string.Empty);

        if (!string.IsNullOrWhiteSpace(token))
        {
            var tokenDecode = new JwtSecurityToken(jwtEncodedString: token);
            if (DateTime.Compare(DateTime.UtcNow, tokenDecode.ValidTo) > 0)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            }
            else 
            {
                var idUser  =   context.User?.GetUserIdFromClaims<int>();
                var existsTokenUser = await unitOfWork.UserRepository.ExistRefreshTokenByIdUser(idUser ?? 0);
                if(existsTokenUser)
                {
                    context.Items[nameof(idUser)] = idUser;
                }
                else
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                }  
            }
        }

        await _next(context);
    }
}
// https://dev.to/isaacojeda/part-aspnet-identity-core-y-jwt-1l84  ****revisar current sUser