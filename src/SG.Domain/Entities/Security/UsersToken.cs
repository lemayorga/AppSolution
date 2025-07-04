using SG.Domain.Base;

namespace SG.Domain.Entities.Security;
public class UsersToken  : BaseEntity<int>
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

    public UsersToken(int id, int idUser,string refreshToken,DateTime refreshTokenExpiry)
    {
        Id = id;
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