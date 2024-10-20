using System;
using SG.Domain.Security.Repositories;
using SG.Infrastructure.Data.Context;
using Action = SG.Domain.Security.Entities.Action;

namespace SG.Infrastructure.Data.Repositories.Security;

public class ActionRepository : BaseGenericRepository<Action>, IActionRepository
{
    public ActionRepository(ApplicationDbContext context) : base(context)
    {
    }
}
