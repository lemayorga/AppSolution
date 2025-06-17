using FluentResults;
using SG.Application.Base.ServiceLogic;
using SG.Application.Bussiness.Security.Auth.Requests;
using SG.Infrastructure.Auth.JwtAuthentication.Models;

namespace SG.Application.Bussiness.Security.Auth.Interface;

public interface IAuthService: IServices
{
    Task<Result<Responses.LoginResponse>> Login(LoginRequest loginModel);
    Task<Result<Responses.LogoutResponse>> Logout(int idUser);
    Task<Result<Responses.LoginResponse>> RefreshToken(RefreshTokenModel model);
}
