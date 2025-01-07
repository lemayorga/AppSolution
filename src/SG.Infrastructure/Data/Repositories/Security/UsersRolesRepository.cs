using SG.Domain.Security.Entities;
using SG.Domain.Security.Repositories;
using SG.Infrastructure.Data.Context;

namespace SG.Infrastructure.Data.Repositories.Security;


public class UsersRolesRepository : BaseGenericRepository<UsersRoles>, IUsersRolesRepository
{
    public UsersRolesRepository(ApplicationDbContext context) : base(context)
    {
    }
}
