using SG.Domain.Commun.Repositories;
using SG.Domain.Security.Repositories;

namespace SG.Domain;

public interface IUnitOfWork : IDisposable
{
    Task<bool> SaveChangesAsync();
    IBaseGenericRepository<T> Repository<T>() where T : class;

    #region Commun
    ICatalogueRepository CatalogueRepository { get; }
    #endregion

    #region Security
    IPasswordHistoryRepository PasswordHistoryRepository { get; }
    IPasswordPolicyRepository PasswordPolicyRepository { get; }
    IRoleRepository RoleRepository { get; }
    IUserRepository UserRepository { get; }
    #endregion    
}
