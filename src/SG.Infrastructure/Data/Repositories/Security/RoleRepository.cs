using System;
using SG.Domain.Security.Entities;
using SG.Domain.Security.Repositories;
using SG.Infrastructure.Data.Context;

namespace SG.Infrastructure.Data.Repositories.Security;

public class RoleRepository : BaseGenericRepository<Role>, IRoleRepository
{
    public RoleRepository(ApplicationDbContext context) : base(context)
    {
    }
}
