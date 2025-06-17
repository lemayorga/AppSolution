
using SG.Domain.Repositories.Security;
using SG.Infrastructure.Base;
using SG.Infrastructure.Data.Context;
using Module = SG.Domain.Entities.Security.Module;

namespace SG.Infrastructure.Data.Repositories.Security;

public class ModuleRepository : BaseGenericRepository<Module>, IModuleRepository
{
    public ModuleRepository(ApplicationDbContext context) : base(context)
    {
    }
}
