using System.Security.Claims;
using FluentResults;
using Microsoft.Extensions.Logging;
using SG.Application.Bussiness.Security.Dtos;
using SG.Application.Bussiness.Security.Interfaces;
using SG.Domain;
using SG.Domain.Security.Entities;
using SG.Infrastructure.Auth.JwtAuthentication;
using SG.Infrastructure.Auth.JwtAuthentication.Models;
using SG.Shared.Constants;
using SG.Shared.Helpers;
using SG.Infrastructure.Auth.Extensions;

namespace SG.Application.Bussiness.Security.Services;

public class AuthService : IAuthService
{
    private readonly ILogger _logger;
    private readonly IJwtBuilder _jwtBuilder;
    private  readonly IUnitOfWork _unitOfWork;
    private readonly IRoleService  _roleService;

    public AuthService
    (
        ILogger<AuthService> logger,
        IJwtBuilder jwtBuilder,        
        IUnitOfWork unitOfWork,  
        IRoleService roleService 
    ) 
    {
        _logger = logger;
        _jwtBuilder = jwtBuilder;
        _unitOfWork = unitOfWork;   
        _roleService = roleService;     
    }

    public async Task<Result<LoginResponseDto>> Login(LoginDto loginModel)
    {
        var (userName, password) = loginModel;

        var userResult =  (loginModel.EvaluateEmail.HasValue && loginModel?.EvaluateEmail == true)
                        ?  (await _unitOfWork.UserRepository.FilterByUserNameOrEmail(userName))
                        :  (await _unitOfWork.UserRepository.FilterByUserName(userName));

        string messageError = !userResult.Any() ? MESSAGE_CONSTANTES.VALIDATION_USER_DOESNT_EXIST
                            : !userResult.ElementAt(0).IsActive ? MESSAGE_CONSTANTES.VALIDATION_USER_IS_BLOCKED
                            : userResult.ElementAt(0).IsLocked ? MESSAGE_CONSTANTES.VALIDATION_USER_IS_BLOCKED
                            : string.Empty;   

        if (!string.IsNullOrWhiteSpace(messageError))
        {
            return Result.Fail<LoginResponseDto>(messageError);
        }
        
        var user = userResult.ElementAt(0);

        bool resultComparePassword = EncryptionUtils.Verify(user.Password, password);
        if(!resultComparePassword)
        {
            return Result.Fail<LoginResponseDto>(MESSAGE_CONSTANTES.VALIDATION_INVALID_CREDENTIALS);
        }

        var (accessToken, refreshToken) = await GenerateTokenValues(user);

        return Result.Ok(new LoginResponseDto(user, accessToken, refreshToken));
    }


    public async Task<Result<LogoutResponse>> Logout(int idUser)
    {
        var result  = await _unitOfWork.UserRepository.RemoverRefreshTokenUser(idUser);
        return Result.Ok(new LogoutResponse{ Success = result});
    }

    public async Task<Result<LoginResponseDto>> RefreshToken(RefreshTokenModel model)
    {
        var principalClaim =  _jwtBuilder.GetPrincipal(model.AccessToken);
        // var idUser = principalClaim?.Claims?.FirstOrDefault(f => f.Type == "idUser")?.Value;
        var idUser = principalClaim?.GetUserIdFromClaims<int>();

        if(principalClaim?.Identity?.Name  is null || idUser is null)
        {
            return Result.Fail<LoginResponseDto>(MESSAGE_CONSTANTES.REFRESH_TOKEN_ERROR);
        }

        var (refreshToken, refreshTokenExpiry)  = await _unitOfWork.UserRepository.GetValuesRefreshTokenByUser(idUser ?? 0);

        if(refreshToken is null || refreshToken != model.RefreshToken || refreshTokenExpiry is null || refreshTokenExpiry < DateTime.UtcNow)
        {
            return Result.Fail<LoginResponseDto>(MESSAGE_CONSTANTES.TOKEN_INVALID_OR_EXPIRED);
        }

        var user = await _unitOfWork.UserRepository.GetById(idUser!);
        var (accessToken, newRefreshToken) = await GenerateTokenValues(user!);
        return Result.Ok(new LoginResponseDto(user!, accessToken, newRefreshToken)); 
    } 

    private async Task<IEnumerable<JwtRolData>> GetRolesFromUserId(int idUser)
    {
        var userRolesResult = await _roleService.GetFilterUsersAndRoles(new FilterUsersRoles { IdUser = idUser });
        var userRoleData =  userRolesResult.Value.Select(x => new JwtRolData(x.IdRol, x.RolName));
        return userRoleData;
    }

    private async Task<(string, string)> GenerateTokenValues(User user)
    {
        var userRoles = await  GetRolesFromUserId(user!.Id);

        var claims = new List<Claim>();
        claims.AddWithUserToClaim(user)
              .AddWithRolesToClaim(userRoles);

        var accessToken  = _jwtBuilder.GenerateAccessToken(claims);
        var refreshToken = _jwtBuilder.GenerateRefreshToken();
        
        var refreshTokent = _jwtBuilder.GenerateAccessTokenFromRefreshToken();

        var refreshTokenExpiry = _jwtBuilder.NewTimeTokenExpiration();

        await _unitOfWork.UserRepository.AddOrUpdateRefreshTokenUser(user!.Id, refreshToken, refreshTokenExpiry);
        return (accessToken, refreshToken);
    }
}


// https://code-maze.com/using-refresh-tokens-in-asp-net-core-authentication/