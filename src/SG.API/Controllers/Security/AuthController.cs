using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SG.Application.Bussiness.Security.Auth.Interface;
using SG.Application.Bussiness.Security.Auth.Requests;
using SG.Application.Bussiness.Security.Auth.Responses;
using SG.Application.Base.Responses;
using SG.Infrastructure.Auth.JwtAuthentication.Models;
using SG.Infrastructure.Auth.Services;
using SG.Application.Extensions;

namespace SG.API.Controllers.Security;

[Route("api/[controller]")]
[ApiController]
public class AuthController
(
    IAuthService application, 
    IHttpContextAccessor httpContextAccessor,
    Infrastructure.Auth.JwtAuthentication.IJwtBuilder jwtBuilder,
    IPrincipalCurrentUser principal
)  : ControllerBase
{
    private readonly IAuthService _application = application;
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
    private readonly IPrincipalCurrentUser _principal2 = principal;
    private readonly Infrastructure.Auth.JwtAuthentication.IJwtBuilder _jwtBuilder = jwtBuilder;

    /// <summary>
    /// Inicio de sesión
    /// </summary>
    /// <param name="request">Datos para iniciar sesión</param>   
    [AllowAnonymous]
    [HttpPost("login")]
    [ProducesResponseType(typeof(OperationResult<LoginResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        if (!ModelState.IsValid) { return BadRequest(); }
        var response = await _application.Login(request);
        return Ok(response.ToOperationResult());
    }

    /// <summary>
    /// Refrescar Token
    /// </summary>
    /// <param name="tokenResponse"></param>
    /// <returns></returns>
    [Authorize]
    [HttpPost("refreshToken")]
    [ProducesResponseType(typeof(OperationResult<LoginResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> RefreshToken(RefreshTokenModel tokenResponse)
    {
        var newAccessToken = await _application.RefreshToken(tokenResponse);
        if (newAccessToken?.Errors?.Any() ?? false)
        {
            return Unauthorized();
        }
        return Ok(newAccessToken?.ToOperationResult());
    }

    /// <summary>
    /// Logout
    /// </summary>
    /// <returns></returns>
    [Authorize]
    [HttpPost("logout")]
    [ProducesResponseType(typeof(OperationResult<LogoutResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Logout()  // https://code-maze.com/using-refresh-tokens-in-asp-net-core-authentication/ para el angular
    {
        var result = await _application.Logout(_principal2.User!.Id);
        Response.HttpContext.Items.Remove("idUser");
        return Ok(result.ToOperationResult());
    }
    
    /// <summary>
    /// MySelfUser
    /// </summary>
    /// <returns></returns>
    [Authorize]
    [HttpGet("myself")]
    public async Task<IActionResult> MySelfUser()  
    {
        var result = _httpContextAccessor.HttpContext?.User;
        var rr  = _principal2;
        return Ok(rr.User);
    }
}


// https://jasonwatmore.com/post/2022/01/16/net-6-hash-and-verify-passwords-with-bcrypt
// https://github.com/persteenolsen/dotnet-8-jwt-refresh-auth-api/blob/main/Authorization/JwtMiddleware.cs
// https://www.c-sharpcorner.com/article/implementing-jwt-refresh-tokens-in-net-8-0/
// https://www.youtube.com/watch?v=DzBwfoKnmhk
// https://github.com/abolfazlSadeqi/DotNetCleanArchitectureJwtRedisDistributedCaching/blob/master/Src/UI/PublicApi/Controllers/Admin/RolesController.cs
// https://dev.to/isaacojeda/part-aspnet-identity-core-y-jwt-1l84