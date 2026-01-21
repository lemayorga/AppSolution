using System.Data;
using System.Linq.Expressions;
using Dapper;
using Microsoft.EntityFrameworkCore;
using SG.Domain.Base;
using SG.Infrastructure.Data.Context;

namespace SG.Infrastructure.Base;

public  class BaseGenericRepository<TEntity> : IBaseGenericRepository<TEntity> where TEntity : class
{
    protected readonly ApplicationDbContext _context;
    protected readonly DbSet<TEntity> _entities;
    protected readonly IDbConnection _connection;
    public BaseGenericRepository(ApplicationDbContext context)
    {
        _context = context;
        _entities = context.Set<TEntity>();
        _connection = context.CreateConnection();    
    }

    public virtual bool SaveChanges()
    {
        return _context.SaveChanges() > 0;
    }
    public virtual async Task<bool> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync() > 0;
    }

    public virtual async Task Add(TEntity entity)
    {
        await _entities.AddAsync(entity);
    }

    public virtual async Task AddMany(IEnumerable<TEntity> entities)
    {
        await _entities.AddRangeAsync(entities);
    }


    public virtual async Task<TEntity?> GetById(params object[] keys)
    {
        return await _entities.FindAsync(keys);
    }

    public virtual async Task<TEntity?> GetOne(Expression<Func<TEntity, bool>>? where = null)
    {
        if (where != null)
        {
            return await _entities.AsNoTracking().FirstOrDefaultAsync(where);
        }
        return await _entities.AsNoTracking().FirstOrDefaultAsync();
    }
    public async Task<TResult?> GetOneWithSelector<TResult>(Expression<Func<TEntity, TResult>> selector, Expression<Func<TEntity, bool>>? where = null, Action<IQueryable<TEntity>>? includes = null) 
    {
        IQueryable<TEntity> query = _entities.AsNoTracking();
        if (includes != null)
        {
            includes.Invoke(query);
        }
        if (where != null)
        {
            query = query.Where(where);
        }

        return await query.Select(selector).FirstOrDefaultAsync();
    }


    public virtual IQueryable<TEntity> GetAll(Expression<Func<TEntity, bool>>? where = null, Action<IQueryable<TEntity>>? orderBy = null, Action<IQueryable<TEntity>>? includes = null)
    {
        IQueryable<TEntity> query = _entities.AsNoTracking();
        if (includes != null)
        {
            includes.Invoke(query);
        }
        if (where != null)
        {
            query = query.Where(where);
        }
        if (orderBy != null)
        {
            orderBy.Invoke(query);
        }
        return query.AsNoTracking();
    }

    public IQueryable<TResult> GetAllWithSelector<TResult>(Expression<Func<TEntity, TResult>> selector, Expression<Func<TEntity, bool>>? where = null, Action<IQueryable<TEntity>>? includes = null) 
    {
        IQueryable<TEntity> query = _entities.AsNoTracking();
        if (includes != null)
        {
            includes.Invoke(query);
        }
        if (where != null)
        {
            query = query.Where(where);
        }

        return query.AsNoTracking().Select(selector);
    }

    public virtual async Task<IEnumerable<TEntity>> GetPaginate(int skip, int take, Expression<Func<TEntity, bool>>? where = null, Action<IQueryable<TEntity>>? orderBy = null, Action<IQueryable<TEntity>>? includes = null)
    {
        IQueryable<TEntity> query = _entities.AsNoTracking();
        if (includes != null)
        {
            includes.Invoke(query);
        }
        if (where != null)
        {
            query = query.Where(where);
        }
        if (orderBy != null)
        {
            orderBy.Invoke(query);
        }
        return await query.Skip(skip).Take(take).ToListAsync();
    }

    public virtual void Update(TEntity entity)
    {
        _context!.Entry(entity).CurrentValues.SetValues(entity);
    }

    public virtual async Task<bool> UpdateById(int id, TEntity entity)
    {
        TEntity? _entity = await GetById(id);
        if (_entity != null)
        {
             _context.Entry(_entity).CurrentValues.SetValues(entity);
            return true;
        }
        return false;
    }

    public virtual async Task<bool> UpdateOne(Expression<Func<TEntity, bool>> where, TEntity entity)
    {
        TEntity? _entity = await GetOne(where: where);
        if (_entity != null)
        {
            _entities.Update(entity);
            return true;
        }
        return false;
    }

    public virtual async Task<TEntity?> UpdateOneSave(Expression<Func<TEntity, bool>> where, TEntity entity)
    {
        TEntity? _entity = await GetOne(where: where);
        if (_entity != null)
        {
           _context.Entry(_entity).CurrentValues.SetValues(entity);
           await SaveChangesAsync();
        }
        
        return _entity;
    }    

   public virtual async Task<bool> DeleteById(int id)
    {
        TEntity? entity = await GetById(id);
        if (entity != null)
        {
            _entities.Remove(entity);
            return true;
        }
        return false;
    }

    public virtual void Delete(TEntity entity)
    {
        _entities.Remove(entity);
    }

    public virtual void DeleteMany(Expression<Func<TEntity, bool>> where)
    {
        var entities = GetAll(where);
        _entities.RemoveRange(entities);
    }

    public virtual async Task<bool> Any(Expression<Func<TEntity, bool>> predicate)
    {
        return await _entities.AnyAsync(predicate);
    }
    public virtual async Task<int> Count(Expression<Func<TEntity, bool>> predicate)
    {
        return await _entities.CountAsync(predicate);
    }
    public IQueryable<TEntity> FindAll(bool trackChanges = true)
    {
        return trackChanges ? _entities.AsNoTracking() : _entities;
    }

    public IQueryable<TEntity> FindByCondition(Expression<Func<TEntity, bool>> expression, bool trackChanges = true)
    {    
        return trackChanges ?_entities.Where(expression) .AsNoTracking() : _entities .Where(expression);    
    }

    public virtual async Task<(int, IEnumerable<TEntity>)> Paginate(
        int pageNumber,
        int pageSize,
        string? searchTerm = null ,       
        Dictionary<string, string>? columnFilters = null, 
        Dictionary<string, string>? orderByColumns = null)
    {                
        await Task.FromResult(() =>  "");
        return (0, new List<TEntity>());
    }    
}