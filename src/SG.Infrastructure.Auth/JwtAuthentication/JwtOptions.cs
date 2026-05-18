
namespace SG.Infrastructure.Auth.JwtAuthentication;

public sealed class JwtOptions
{
    public string Issuer { get; init; }  = default!; 
    public string Audience { get; set; }  = default!; 
    public string SigningKey { get; set; }  = default!; 
    public double TokenLifeTime { get; set; }
    public string TokenLifeTimeTypeExpiration { get; set; }  = default!; 
}