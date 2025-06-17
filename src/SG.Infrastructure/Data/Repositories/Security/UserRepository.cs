using SG.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;
using SG.Infrastructure.Base;
using SG.Domain.Entities.Security;
using SG.Domain.Repositories.Security;

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
    public async Task<bool> ExistRefreshTokenByIdUser(int idUser)
    {
        return await _context.UsersToken.FirstOrDefaultAsync(x => x.IdUser == idUser) is not null;
    } 

    public async Task<bool> AddOrUpdateRefreshTokenUser(int idUser, string refreshToken, DateTime refreshTokenExpiry)
    {
        var existsUserToken =  await ExistRefreshTokenByIdUser(idUser);
       if(existsUserToken)
       {
            return await _context.UsersToken.Where(x => x.IdUser == idUser)
                .ExecuteUpdateAsync(p => 
                    p.SetProperty(b => b.RefreshToken, refreshToken)
                        .SetProperty(b => b.RefreshTokenExpiry, refreshTokenExpiry)
                ) > 0;
       }

       _context.UsersToken.Add(new UsersToken(idUser,refreshToken, refreshTokenExpiry));
       return await _context.SaveChangesAsync() > 0;
    }   

    public async Task<bool> RemoverRefreshTokenUser(int idUser)
    {
        return await _context.UsersToken.Where(x => x.IdUser == idUser)
            .ExecuteDeleteAsync() > 0;
    }     
    
    public async Task<(string?, DateTime?)> GetValuesRefreshTokenByUser(int idUser)
    {
      var userToken =  await _context.UsersToken.FirstOrDefaultAsync(x => x.Id == idUser);
      if(userToken is  not null) 
      {
        var (refreshToken, refreshTokenExpiry) = userToken!;
        return  (refreshToken, refreshTokenExpiry);
      }

      return  (null, null);
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
