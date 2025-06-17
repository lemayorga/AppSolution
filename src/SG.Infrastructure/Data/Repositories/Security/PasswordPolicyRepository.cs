using SG.Domain.Entities.Security;
using SG.Domain.Repositories.Security;
using SG.Infrastructure.Base;
using SG.Infrastructure.Data.Context;

namespace SG.Infrastructure.Data.Repositories.Security;

public class PasswordPolicyRepository : BaseGenericRepository<PasswordPolicy>, IPasswordPolicyRepository
{
    public PasswordPolicyRepository(ApplicationDbContext context) : base(context)
    {
    }
}
