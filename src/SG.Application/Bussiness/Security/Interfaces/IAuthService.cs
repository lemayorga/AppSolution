using FluentResults;
using SG.Application.Bussiness.Security.Dtos;
using SG.Infrastructure.Auth.JwtAuthentication.Models;

namespace SG.Application.Bussiness.Security.Interfaces;

public interface IAuthService
{
    Task<Result<LoginResponseDto>> Login(LoginDto loginModel);
    Task<Result<LogoutResponse>> Logout(int idUser);
    Task<Result<LoginResponseDto>> RefreshToken(RefreshTokenModel model);
}
