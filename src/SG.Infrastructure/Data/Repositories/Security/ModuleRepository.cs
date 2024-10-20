using System;
using SG.Domain.Security.Repositories;
using SG.Infrastructure.Data.Context;
using Module = SG.Domain.Security.Entities.Module;

namespace SG.Infrastructure.Data.Repositories.Security;

public class ModuleRepository : BaseGenericRepository<Module>, IModuleRepository
{
    public ModuleRepository(ApplicationDbContext context) : base(context)
    {
    }
}
