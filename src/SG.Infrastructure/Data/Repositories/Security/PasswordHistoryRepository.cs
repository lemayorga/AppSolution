using System;
using SG.Domain.Security.Entities;
using SG.Domain.Security.Repositories;
using SG.Infrastructure.Data.Context;

namespace SG.Infrastructure.Data.Repositories.Security;

public class PasswordHistoryRepository : BaseGenericRepository<PasswordHistory>, IPasswordHistoryRepository
{
    public PasswordHistoryRepository(ApplicationDbContext context) : base(context)
    {
    }
}
