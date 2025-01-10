
namespace SG.Infrastructure.Auth.JwtAuthentication;

public sealed class JwtOptions
{
    public string Issuer { get; init; }  = default!; 
    public string Audience { get; set; }  = default!; 
    public string SigningKey { get; set; }  = default!; 
    public int ExpirationSeconds { get; set; }
}

public class TokenResponse 
{
    public string AccessToken { get; set; } = default!;
    public string RefreshToken { get; set; } = default!;
}

public class RefreshTokenModel 
{
    public string AccessToken { get; set; } = default!;
    public string RefreshToken { get; set; } = default!;
}