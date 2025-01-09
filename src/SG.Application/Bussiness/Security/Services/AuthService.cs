using FluentResults;
using Microsoft.Extensions.Logging;
using SG.Application.Bussiness.Security.Dtos;
using SG.Application.Bussiness.Security.Interfaces;
using SG.Domain.Security.Repositories;
using SG.Infrastructure.Auth.JwtAuthentication;
using SG.Shared.Constants;
using SG.Shared.Helpers;

namespace SG.Application.Bussiness.Security.Services;

public class AuthService : IAuthService
{
    private readonly ILogger _logger;
    private readonly IJwtBuilder _jwtBuilder;
    private  readonly IUserRepository _userRepository;
    public AuthService(IUserRepository userRepository, IJwtBuilder jwtBuilder,  ILogger<AuthService> logger) 
    {
        _logger = logger;
        _jwtBuilder = jwtBuilder;
        _userRepository = userRepository;        
    }

    public async Task<Result<LoginResponseDto>> Authenticate(LoginDto loginModel)
    {
        try 
        {
            var (userName, password) = loginModel;

            var userResult =  (loginModel.EvaluateEmail.HasValue && loginModel?.EvaluateEmail == true)
                            ?  (await _userRepository.FilterByUserNameOrEmail(userName))
                            :  (await _userRepository.FilterByUserName(userName));

            string messageError = !userResult.Any() ? MESSAGE_CONSTANTES.USER_DOESNT_EXIST
                                : !userResult.ElementAt(0).IsActive ? MESSAGE_CONSTANTES.USER_IS_DISABLED
                                : userResult.ElementAt(0).IsLocked ? MESSAGE_CONSTANTES.USER_IS_BLOCKED
                                : string.Empty;   

            if (!string.IsNullOrWhiteSpace(messageError))
            {
                return Result.Fail<LoginResponseDto>(messageError);
            }
            
            var user = userResult.ElementAt(0);

            bool resultComparePassword = EncryptionUtils.Verify(user.Password, password);
            if(!resultComparePassword)
            {
                return Result.Fail<LoginResponseDto>(MESSAGE_CONSTANTES.INVALID_CREDENTIALS);
            }

            var accessToken  = _jwtBuilder.GenerateAccessToken(user);
            var refreshToken = _jwtBuilder.GenerateRefreshToken();

            await _userRepository.UpdateRefreshToken(user.Id, refreshToken, DateTime.UtcNow.AddHours(12));

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

            if(principalClaim?.Identity?.Name  is null){
                return Result.Fail<LoginResponseDto>(MESSAGE_CONSTANTES.REFRESH_TOKEN_ERROR);
            }

            int idUser = int.Parse(principalClaim.Identity.Name ?? "0");
            var user = await _userRepository.GetById(idUser);

            if(user is null ||  user?.RefreshToken != model.RefreshToken || user?.RefreshTokenExpiry < DateTime.UtcNow)
                return Result.Fail<LoginResponseDto>(MESSAGE_CONSTANTES.REFRESH_TOKEN_ERROR);

            var accessToken  = _jwtBuilder.GenerateAccessToken(user!);
            var refreshToken = _jwtBuilder.GenerateRefreshToken();

            await _userRepository.UpdateRefreshToken(user!.Id, refreshToken, DateTime.UtcNow.AddHours(12));

            return Result.Ok(new LoginResponseDto(user, accessToken, refreshToken));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return Result.Fail(ex.Message);
        } 
    } 
}
