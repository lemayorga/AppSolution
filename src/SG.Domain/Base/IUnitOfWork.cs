
using SG.Domain.Repositories.Commun;
using SG.Domain.Repositories.Security;

namespace SG.Domain.Base;
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
    IUsersRolesRepository UsersRolesRepository { get; }
    #endregion    
}
