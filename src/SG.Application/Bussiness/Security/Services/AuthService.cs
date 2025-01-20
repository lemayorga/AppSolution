using FluentResults;
using Microsoft.Extensions.Logging;
using SG.Application.Bussiness.Security.Dtos;
using SG.Application.Bussiness.Security.Interfaces;
using SG.Domain;
using SG.Infrastructure.Auth.JwtAuthentication;
using SG.Infrastructure.Auth.JwtAuthentication.Models;
using SG.Shared.Constants;
using SG.Shared.Helpers;

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

    public async Task<Result<LoginResponseDto>> Authenticate(LoginDto loginModel)
    {
        try 
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

          
            var userRoles = await  GetRolesFromUserId(user.Id);
            var accessToken  = _jwtBuilder.GenerateAccessToken(user, userRoles);
            var refreshToken = _jwtBuilder.GenerateRefreshToken();

            await _unitOfWork.UserRepository.UpdateRefreshToken(user.Id, refreshToken, DateTime.UtcNow.AddHours(12));

            return Result.Ok(new LoginResponseDto(user, accessToken, refreshToken));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return Result.Fail(ex.Message);
        } 
    }

    public async Task<Result<LoginResponseDto>> RefreshToken(RefreshTokenModel model)
    {
        try
        {
            var principalClaim =  _jwtBuilder.GetTokenPrincipal(model.AccessToken);

            if(principalClaim?.Identity?.Name  is null)
            {
                return Result.Fail<LoginResponseDto>(MESSAGE_CONSTANTES.REFRESH_TOKEN_ERROR);
            }

            int idUser = int.Parse(principalClaim.Identity.Name ?? "0");
            var user = await _unitOfWork.UserRepository.GetById(idUser);

            if(user is null ||  user?.RefreshToken != model.RefreshToken || user?.RefreshTokenExpiry < DateTime.UtcNow)
            {
                return Result.Fail<LoginResponseDto>(MESSAGE_CONSTANTES.REFRESH_TOKEN_ERROR);
            }

            var userRoles = await  GetRolesFromUserId(user!.Id);
            var accessToken  = _jwtBuilder.GenerateAccessToken(user!, userRoles);
            var refreshToken = _jwtBuilder.GenerateRefreshToken();

            await _unitOfWork.UserRepository.UpdateRefreshToken(user!.Id, refreshToken, DateTime.UtcNow.AddHours(12));

            return Result.Ok(new LoginResponseDto(user, accessToken, refreshToken));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return Result.Fail(ex.Message);
        } 
    } 

    private async Task<IEnumerable<JwtRolData>> GetRolesFromUserId(int idUser)
    {
          var userRolesResult = await _roleService.GetFilterUsersAndRoles(new FilterUsersRoles { IdUser = idUser });
          var userRoleData =  userRolesResult.Value.Select(x => new JwtRolData(x.IdRol, x.RolName));
          return userRoleData;
    }
}
