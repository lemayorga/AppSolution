using System;
using SG.Infrastructure.Auth.JwtAuthentication.Models;

namespace SG.Application.Bussiness.Security.Auth.Responses;

public class LoginResponse
{
    public int IdUser { get; set; }
    public string DisplayName { get; set; } = default!;
    public string UserName { get; set; } = default!;
    public string Email { get; set; } = default!;
    public TokenResponse Tokens { get; set; } = default!;

    public LoginResponse(Domain.Security.Entities.User user,string accessToken,string refreshToken)
    {
        IdUser = user.Id;
        DisplayName = user.Username;
        UserName = user.Username;
        Email = user.Email;
        Tokens = new TokenResponse 
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };
    }
}
