using SG.Domain.Security.Entities;

namespace SG.Domain.Security.Repositories;

public interface IUserRepository : IBaseGenericRepository<User>
{
    Task<IQueryable<User>> FilterByUserNameOrEmail(string search);
    Task<IQueryable<User>> FilterByOrEmail(string search);
    Task<IQueryable<User>> FilterByUserName(string search);
}
