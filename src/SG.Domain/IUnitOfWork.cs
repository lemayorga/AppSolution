
using System.Reflection.Metadata;
using SG.Domain.Commun.Repositories;

namespace SG.Domain;

public interface IUnitOfWork : IDisposable
{
    Task<bool> SaveChangesAsync();
    IBaseGenericRepository<T> Repository<T>() where T : class;
    ICatalogueRepository CatalogueRepository { get; }
}
