using SG.Domain;
using SG.Domain.Base;
using SG.Domain.Repositories.Commun;
using SG.Domain.Repositories.Security;
using SG.Infrastructure.Data.Context;

namespace SG.Infrastructure.Base;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    
    #region Commun
    public ICatalogueRepository CatalogueRepository { get; }
    
    #endregion

    #region Security
    public IPasswordHistoryRepository PasswordHistoryRepository { get; }
    public IPasswordPolicyRepository PasswordPolicyRepository { get; }
    public IRoleRepository RoleRepository { get; }
    public IUserRepository UserRepository { get; }
    public IUsersRolesRepository UsersRolesRepository { get; }
    
    #endregion

    public UnitOfWork
    (
        ApplicationDbContext context,
        ICatalogueRepository _catalogueRepository,
        IPasswordHistoryRepository _passwordHistoryRepository,
        IPasswordPolicyRepository _passwordPolicyRepository,
        IRoleRepository _roleRepository,
        IUserRepository _userRepository,
        IUsersRolesRepository _usersRolesRepository
    )
    {
        _context = context;
        CatalogueRepository = _catalogueRepository;
        PasswordHistoryRepository = _passwordHistoryRepository;
        PasswordPolicyRepository = _passwordPolicyRepository;
        RoleRepository = _roleRepository;
        UserRepository = _userRepository;
        UsersRolesRepository = _usersRolesRepository;
    }

    public async Task<bool> SaveChangesAsync()
     => await _context.SaveChangesAsync() > 0;

    public IBaseGenericRepository<T> Repository<T>() where T : class
    {
        return new BaseGenericRepository<T>(_context);
    }
    
    public void Dispose()
    {
        _context?.Dispose();
    }
}