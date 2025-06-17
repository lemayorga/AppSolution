using SG.Domain.Base;
using SG.Domain.Entities.Security;

namespace SG.Domain.Repositories.Security;

public interface IUserRepository : IBaseGenericRepository<User>
{
    Task<IQueryable<User>> FilterByUserNameOrEmail(string search);
    Task<IQueryable<User>> FilterByOrEmail(string search);
    Task<IQueryable<User>> FilterByUserName(string search);
    Task<bool> ExistRefreshTokenByIdUser(int idUser);
    Task<bool> AddOrUpdateRefreshTokenUser(int idUser, string refreshToken, DateTime refreshTokenExpiry);
    Task<bool> RemoverRefreshTokenUser(int idUser);
    Task<(string?, DateTime?)> GetValuesRefreshTokenByUser(int idUser);
    Task<bool> UpdatePasswordHash(int idUser, string newPasswordHash);
    Task<bool> UpdateStatusIsLocked(int idUser, bool newStatusLocked);
    Task<bool> UpdateStatusIsActived(int idUser, bool newStatusActive);
}