using System;

namespace SG.Infrastructure.Auth.JwtAuthentication.Models;

public class TokenResponse 
{
    public string AccessToken { get; set; } = default!;
    public string RefreshToken { get; set; } = default!;
}