using System;

namespace SG.Domain.Security.Entities;

public class UsersToken  : Entity
{
    public int IdUser { get; set; }    
    public string RefreshToken { get; set; }
    public  DateTime RefreshTokenExpiry { get; set; }
    public User User { get; set; } = default!;

    public UsersToken(int idUser,string refreshToken,DateTime refreshTokenExpiry)
    {
        IdUser = idUser;
        RefreshToken = refreshToken;
        RefreshTokenExpiry = refreshTokenExpiry;
    }
    public void Deconstruct(out string refreshToken, out DateTime refreshTokenExpiry)
    {
        refreshToken = RefreshToken;
        refreshTokenExpiry = RefreshTokenExpiry;
    }
}
