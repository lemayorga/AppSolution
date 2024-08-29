using SG.Domain;
using SG.Domain.Commun.Repositories;
using SG.Infrastructure.Data.Context;
using SG.Infrastructure.Data.Repositories;

namespace SG.Infrastructure.Data;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    public ICatalogueRepository CatalogueRepository { get; }

    public UnitOfWork
    (
        ApplicationDbContext context,
        ICatalogueRepository _catalogueRepository
    ) {
        _context = context;
        CatalogueRepository = _catalogueRepository;
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