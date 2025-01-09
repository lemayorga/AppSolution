using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SG.Application.Bussiness.Security.Dtos;
using SG.Application.Bussiness.Security.Interfaces;
using SG.Application.Responses;
using SG.Infrastructure.Auth.JwtAuthentication;

namespace SG.API.Controllers.Security;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _application;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuthController(IAuthService application, IHttpContextAccessor httpContextAccessor) 
    {
        _application = application;
        _httpContextAccessor = httpContextAccessor;
    }

    /// <summary>
    /// Inicio de sesión
    /// </summary>
    /// <param name="request">Datos para iniciar sesión</param>   
    [AllowAnonymous]
    [HttpPost("login")]
    [ProducesResponseType(typeof(OperationResult<UserCreateDto>), StatusCodes.Status201Created)]
    public async Task<IActionResult> Login([FromBody] LoginDto request)
    {
        if(!ModelState.IsValid) {  return BadRequest();  }
        var response = await _application.Authenticate(request);
        return Ok(response.ToOperationResult());
    }

    /// <summary>
    /// Refresh Token
    /// </summary>
    /// <param name="tokenResponse"></param>
    /// <returns></returns>
    [HttpPost("refreshToken")]
    [ProducesResponseType(typeof(OperationResult<UserCreateDto>), StatusCodes.Status201Created)]
    public async Task<IActionResult> RefreshToken(RefreshTokenModel tokenResponse)
    {
        var newAccessToken = await _application.RefreshToken(tokenResponse);
        if(newAccessToken?.Errors?.Any() ?? false){
            return Unauthorized();
        }
        return Ok(newAccessToken?.ToOperationResult());
    }   
}


// https://jasonwatmore.com/post/2022/01/16/net-6-hash-and-verify-passwords-with-bcrypt
// https://github.com/persteenolsen/dotnet-8-jwt-refresh-auth-api/blob/main/Authorization/JwtMiddleware.cs
// https://www.c-sharpcorner.com/article/implementing-jwt-refresh-tokens-in-net-8-0/
// https://www.youtube.com/watch?v=DzBwfoKnmhk