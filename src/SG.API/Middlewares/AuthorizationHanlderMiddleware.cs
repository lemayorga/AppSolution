using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using SG.Infrastructure.Auth.Extensions;
using SG.Infrastructure.Auth.JwtAuthentication;

namespace SG.API.Middlewares;

public class AuthorizationHanlderMiddleware (RequestDelegate _next)
{
    public async Task Invoke(HttpContext context, JwtOptions jwtOptions)
    {
        var bearer = context.Request.Headers["Authorization"].ToString();
        var token = bearer.Replace("Bearer ", string.Empty);
        
        if (!string.IsNullOrWhiteSpace(token))
        {
            var (isValid,validationMessage) = await IsValidateAndAttachAccountToContext(context, jwtOptions, token); 
            if(!isValid)
            {
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = StatusCodes.Status401Unauthorized; 
                await context.Response.WriteAsync(validationMessage);
                return;
            }
        }

        await _next(context);
    }


    private async Task<(bool,string)> IsValidateAndAttachAccountToContext(HttpContext context,JwtOptions jwtOptions, string token)
    {
        try
        {        
            var tokenHandler = new JwtSecurityTokenHandler();
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtOptions.SigningKey)),
                ValidAudience  = jwtOptions.Audience,
                ValidIssuer = jwtOptions.Issuer,
                ValidateIssuerSigningKey = true,
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);
        
            var idUser =  context.User?.GetUserIdFromClaims<int>();
            if (idUser == null)
            {
                return (false, "Token not provided");
            }

            context.Items[nameof(idUser)] = idUser;
        }
        catch (SecurityTokenException ex)
        {
            _ = ex;        
            return (false, "Invalid token");
        }

        return (true, string.Empty);
    }
}
// https://dev.to/isaacojeda/part-aspnet-identity-core-y-jwt-1l84  ****revisar current sUser