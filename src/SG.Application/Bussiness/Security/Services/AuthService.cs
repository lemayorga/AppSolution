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
            var newRefreshToken = _jwtBuilder.GenerateRefreshToken();
            var newRefreshTokenExpiry = _jwtBuilder.NewTimeTokenExpiration();

            await _unitOfWork.UserRepository.UpdateRefreshToken(user.Id, newRefreshToken, newRefreshTokenExpiry);

            return Result.Ok(new LoginResponseDto(user, accessToken, newRefreshToken));
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
            var claimIdUser = principalClaim?.Claims?.FirstOrDefault(f => f.Type == "idUser");

            if(principalClaim?.Identity?.Name  is null || claimIdUser is null)
            {
                return Result.Fail<LoginResponseDto>(MESSAGE_CONSTANTES.REFRESH_TOKEN_ERROR);
            }

            int idUser = int.Parse(claimIdUser.Value.ToString());
            var user = await _unitOfWork.UserRepository.GetById(idUser);
            var (refreshToken, refreshTokenExpiry)  = await _unitOfWork.UserRepository.GetValuesRefreshTokenByUser(idUser);

            if(user is null || (refreshToken is not null && refreshToken != model.RefreshToken) || (refreshTokenExpiry is not null && refreshTokenExpiry < DateTime.UtcNow))
            {
                return Result.Fail<LoginResponseDto>(MESSAGE_CONSTANTES.REFRESH_TOKEN_ERROR);
            }

            var userRoles = await  GetRolesFromUserId(user!.Id);
            var accessToken  = _jwtBuilder.GenerateAccessToken(user!, userRoles);
            var newRefreshToken = _jwtBuilder.GenerateRefreshToken();
            var newRefreshTokenExpiry = _jwtBuilder.NewTimeTokenExpiration();

            await _unitOfWork.UserRepository.UpdateRefreshToken(user!.Id, newRefreshToken, newRefreshTokenExpiry);

            return Result.Ok(new LoginResponseDto(user, accessToken, newRefreshToken));
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
