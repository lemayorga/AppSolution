using System;
using SG.Domain.Security.Entities;
using SG.Domain.Security.Repositories;
using SG.Infrastructure.Data.Context;

namespace SG.Infrastructure.Data.Repositories.Security;

public class PasswordPolicyRepository : BaseGenericRepository<PasswordPolicy>, IPasswordPolicyRepository
{
    public PasswordPolicyRepository(ApplicationDbContext context) : base(context)
    {
    }
}
