using System.Linq.Expressions;

namespace SG.Domain;

public interface IBaseGenericRepository<TEntity> where TEntity : class 
{
     bool SaveChanges();
     Task<bool> SaveChangesAsync();
     Task Add(TEntity entity);
     Task<TEntity> AddSave(TEntity entity);
     Task AddMany(IEnumerable<TEntity> entities);
     Task<IEnumerable<TEntity>> AddManySave(IEnumerable<TEntity> entities);
     Task<TEntity?> GetById(params object[] keys);
     Task<TEntity?> GetOne(Expression<Func<TEntity, bool>>? where = null);
     IQueryable<TEntity> GetAll(Expression<Func<TEntity, bool>>? where = null, Action<IQueryable<TEntity>>? orderBy = null, Action<IQueryable<TEntity>>? includes = null);
     Task<IEnumerable<TEntity>> GetPaginate(int skip, int take, Expression<Func<TEntity, bool>>? where = null, Action<IQueryable<TEntity>>? orderBy = null, Action<IQueryable<TEntity>>? includes = null);
     Task<bool> UpdateById(int id, TEntity entity);
     Task<TEntity?> UpdateByIdSave(int id, TEntity entity);
     Task<bool> UpdateOne(Expression<Func<TEntity, bool>> where, TEntity entity);
     Task<TEntity?> UpdateOneSave(Expression<Func<TEntity, bool>> where, TEntity entity);
     Task<bool> DeleteById(int id);
     Task<bool> DeleteByIdSave(int id);
     void Delete(TEntity entity);
     Task<bool> DeleteSave(TEntity entity);
     void DeleteMany(Expression<Func<TEntity, bool>> where);
     Task<bool> DeleteManySave(Expression<Func<TEntity, bool>> where);
     Task<bool> Any(Expression<Func<TEntity, bool>> predicate);
     Task<int> Count(Expression<Func<TEntity, bool>> predicate);
     Task<(int, IEnumerable<TEntity>)> Paginate(
        int pageNumber,
        int pageSize,
        string? searchTerm = null ,       
        Dictionary<string, string>? columnFilters = null, 
        Dictionary<string, string>? orderByColumns = null);

     IQueryable<TEntity> FindAll(bool trackChanges = true);

     IQueryable<TEntity> FindByCondition(Expression<Func<TEntity, bool>> expression, bool trackChanges = true);
}
