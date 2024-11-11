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
}
