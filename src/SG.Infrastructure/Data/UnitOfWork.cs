using SG.Domain;
using SG.Domain.Commun.Repositories;
using SG.Domain.Security.Repositories;
using SG.Infrastructure.Data.Context;
using SG.Infrastructure.Data.Repositories;

namespace SG.Infrastructure.Data;

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
    #endregion

    public UnitOfWork
    (
        ApplicationDbContext context,
        ICatalogueRepository _catalogueRepository,
        IPasswordHistoryRepository _passwordHistoryRepository,
        IPasswordPolicyRepository _passwordPolicyRepository,
        IRoleRepository _roleRepository,
        IUserRepository _userRepository
    ) {
        _context = context;
        CatalogueRepository = _catalogueRepository;
        PasswordHistoryRepository =_passwordHistoryRepository;
        PasswordPolicyRepository = _passwordPolicyRepository;
        RoleRepository = _roleRepository;
        UserRepository = _userRepository;
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