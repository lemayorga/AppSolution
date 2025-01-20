using SG.Domain.Security.Entities;
using SG.Domain.Security.Repositories;
using SG.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace SG.Infrastructure.Data.Repositories.Security;

public class UserRepository : BaseGenericRepository<User>, IUserRepository
{
    public UserRepository(ApplicationDbContext context) : base(context)
    {
    }
    public async Task<IQueryable<User>> FilterByUserNameOrEmail(string search)
    {
        string likeSearch = $"%{search ?? ""}%";
        return await  Task.FromResult(FindByCondition(x => (EF.Functions.Like(x.Username, likeSearch)   ||  (EF.Functions.Like(x.Email, likeSearch))), true));
    } 

    public async Task<IQueryable<User>> FilterByOrEmail(string search)
    {
        string likeSearch = $"%{search ?? ""}%";
        return await  Task.FromResult(FindByCondition(x => EF.Functions.Like(x.Email, likeSearch), true));
    }  

    public async Task<IQueryable<User>> FilterByUserName(string search)
    {
        string likeSearch = $"%{search ?? ""}%";
        return await  Task.FromResult(FindByCondition(x => EF.Functions.Like(x.Username, likeSearch), true));
    }  

    public async Task<bool> UpdateRefreshToken(int idUser, string refreshToken, DateTime refreshTokenExpiry)
    {
       return await _entities.AsNoTracking().Where(x => x.Id == idUser)
            .ExecuteUpdateAsync(p => 
                p.SetProperty(b => b.RefreshToken, refreshToken)
                 .SetProperty(b => b.RefreshTokenExpiry, refreshTokenExpiry)
            ) > 0;
    }  

    public async Task<bool> UpdatePasswordHash(int idUser, string newPasswordHash)
    {
       return await _entities.AsNoTracking().Where(x => x.Id == idUser)
             .ExecuteUpdateAsync(p => p.SetProperty(b => b.Password, newPasswordHash)) > 0;
    }  

    public async Task<bool> UpdateStatusIsLocked(int idUser, bool newStatusLocked)
    {
       return await _entities.AsNoTracking().Where(x => x.Id == idUser)
             .ExecuteUpdateAsync(p => p.SetProperty(b => b.IsLocked, newStatusLocked)) > 0;
    } 

    public async Task<bool> UpdateStatusIsActived(int idUser, bool newStatusActive)
    {
       return await _entities.AsNoTracking().Where(x => x.Id == idUser)
             .ExecuteUpdateAsync(p => p.SetProperty(b => b.IsActive, newStatusActive)) > 0;
    }     
}
