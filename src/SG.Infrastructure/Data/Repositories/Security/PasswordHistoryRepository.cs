using SG.Domain.Entities.Security;
using SG.Domain.Repositories.Security;
using SG.Infrastructure.Base;
using SG.Infrastructure.Data.Context;

namespace SG.Infrastructure.Data.Repositories.Security;

public class PasswordHistoryRepository : BaseGenericRepository<PasswordHistory>, IPasswordHistoryRepository
{
    public PasswordHistoryRepository(ApplicationDbContext context) : base(context)
    {
    }
}
