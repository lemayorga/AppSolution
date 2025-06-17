
using SG.Domain.Repositories.Security;
using SG.Infrastructure.Base;
using SG.Infrastructure.Data.Context;
using Action = SG.Domain.Entities.Security.Action;

namespace SG.Infrastructure.Data.Repositories.Security;

public class ActionRepository : BaseGenericRepository<Action>, IActionRepository
{
    public ActionRepository(ApplicationDbContext context) : base(context)
    {
    }
}
