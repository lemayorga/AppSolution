using FluentResults;
using SG.Application.Bussiness.Security.Dtos;
using SG.Infrastructure.Auth.JwtAuthentication.Models;

namespace SG.Application.Bussiness.Security.Interfaces;

public interface IAuthService
{
    Task<Result<LoginResponseDto>> Authenticate(LoginDto loginModel);
    Task<Result<LoginResponseDto>> RefreshToken(RefreshTokenModel model);
}
