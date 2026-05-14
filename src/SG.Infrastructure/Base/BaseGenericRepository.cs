using System.Data;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using SG.Domain.Base;
using SG.Infrastructure.Data.Context;

namespace SG.Infrastructure.Base;

/// <summary>
/// A base generic repository that provides common data access methods for entities. This class can be inherited by specific repositories to implement additional functionality as needed.
/// </summary>
/// <typeparam name="TEntity"></typeparam>
public  class BaseGenericRepository<TEntity> : IBaseGenericRepository<TEntity> where TEntity : class
{
    /// <summary>
    /// The database context used for data access operations. This context is typically injected into the repository through dependency injection. It provides access to the database and allows for querying and saving data.
    /// </summary>
    protected readonly ApplicationDbContext _context;

    /// <summary>
    /// The DbSet representing the collection of entities of type TEntity in the database. This property is used to perform CRUD operations on the entities. It is initialized in the constructor using the provided database context.
    /// </summary>
    protected readonly DbSet<TEntity> _entities;

    /// <summary>
    /// The database connection used for executing raw SQL queries or stored procedures. This connection is created from the database context and can be used for more advanced data access scenarios that may not be covered by the standard Entity Framework methods.
    /// </summary>
    protected readonly IDbConnection _connection;

    /// <summary>
    /// Initializes a new instance of the BaseGenericRepository class with the specified database context. The constructor sets up the database context, initializes the DbSet for the entity type, and creates a database connection for executing raw SQL queries if needed.
    /// </summary>
    /// <param name="context"></param>
    public BaseGenericRepository(ApplicationDbContext context)
    {
        _context = context;
        _entities = context.Set<TEntity>();
        _connection = context.CreateConnection();    
    }

    /// <summary>
    /// Saves the changes made to the entities in the database context. This method calls the SaveChanges method of the database context and returns true if any changes were saved successfully (i.e., if the number of affected rows is greater than 0). It is a common practice to call this method after performing any create, update, or delete operations to persist the changes to the database.
    /// </summary>
    /// <returns></returns>
    public virtual bool SaveChanges()
    {
        return _context.SaveChanges() > 0;
    }

    /// <summary>
    /// Asynchronously saves the changes made to the entities in the database context. This method calls the SaveChangesAsync method of the database context and returns true if any changes were saved successfully (i.e., if the number of affected rows is greater than 0). It is a common practice to call this method after performing any create, update, or delete operations to persist the changes to the database, especially in scenarios where asynchronous programming is preferred for better performance and responsiveness.
    /// </summary>
    /// <returns></returns>
    public virtual async Task<bool> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync() > 0;
    }

    /// <summary>
    /// Adds a new entity to the database context. This method takes an instance of the entity as a parameter and adds it to the DbSet representing the collection of entities in the database. The actual insertion into the database will occur when the SaveChanges or SaveChangesAsync method is called. This method is typically used to create new records in the database.
    /// </summary>
    /// <param name="entity">The entity to add.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public virtual async Task Add(TEntity entity)
    {
        await _entities.AddAsync(entity);
    }

    /// <summary>
    /// Adds multiple entities to the database context. This method takes an enumerable collection of entities as a parameter and adds them to the DbSet representing the collection of entities in the database. Similar to the Add method, the actual insertion into the database will occur when the SaveChanges or SaveChangesAsync method is called. This method is useful for bulk inserting multiple records into the database efficiently.
    /// </summary>
    /// <param name="entities">The entities to add.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public virtual async Task AddMany(IEnumerable<TEntity> entities)
    {
        await _entities.AddRangeAsync(entities);
    }

     /// <summary>
     /// Retrieves an entity from the database by its primary key(s). This method takes an array of objects representing the primary key values as parameters and uses the FindAsync method of the DbSet to locate the entity in the database. If an entity with the specified primary key(s) is found, it is returned; otherwise, null is returned. This method is commonly used to retrieve a specific record from the database based on its unique identifier(s).    
     /// </summary>
     /// <param name="keys">The primary key values.</param>
     /// <returns>The entity if found; otherwise, null.</returns>
    public virtual async Task<TEntity?> GetById(params object[] keys)
    {
        return await _entities.FindAsync(keys);
    }

     /// <summary>
     /// Retrieves a single entity from the database that matches the specified condition. This method takes an optional expression as a parameter to filter the entities based on a specific condition. If the expression is provided, it uses the FirstOrDefaultAsync method to find the first entity that matches the condition; otherwise, it retrieves the first entity in the DbSet. The AsNoTracking method is used to improve performance by not tracking the retrieved entity in the context, which is beneficial when the entity is only needed for read-only operations. If an entity matching the condition is found, it is returned; otherwise, null is returned.
     /// </summary>
     /// <param name="where">An optional expression to filter the entities.</param>
     /// <returns>The entity if found; otherwise, null.</returns>
    public virtual async Task<TEntity?> GetOne(Expression<Func<TEntity, bool>>? where = null)
    {
        if (where != null)
        {
            return await _entities.AsNoTracking().FirstOrDefaultAsync(where);
        }
        return await _entities.AsNoTracking().FirstOrDefaultAsync();
    }

    /// <summary>
    ///  Retrieves a single entity from the database that matches the specified condition and projects it to a different type using the provided selector expression. This method takes a selector expression to specify how to project the entity to a different type (TResult), an optional expression to filter the entities based on a specific condition, and an optional action to include related entities in the query. The method constructs a query based on the provided parameters and returns the first result that matches the condition, projected to the specified type. If no matching entity is found, it returns null.
    /// </summary>
    /// <typeparam name="TResult">The type to project the entity to.</typeparam>
    /// <param name="selector">The expression to specify the projection.</param>
    /// <param name="where">An optional expression to filter the entities.</param>
    /// <param name="includes">An optional action to include related entities in the query.</param>
    /// <returns>The projected entity if found; otherwise, null.</returns>
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

    /// <summary>
    ///  Retrieves a collection of entities from the database that match the specified condition, with optional ordering and inclusion of related entities. This method takes an optional expression to filter the entities based on a specific condition, an optional action to specify the ordering of the results, and an optional action to include related entities in the query. The method constructs a query based on the provided parameters and returns an IQueryable of entities that match the condition, ordered and including related entities as specified. If no condition is provided, it returns all entities in the DbSet, optionally ordered and including related entities.
    /// </summary>
    /// <param name="where">An optional expression to filter the entities.</param>
    /// <param name="orderBy">An optional action to specify the ordering of the results.</param>
    /// <param name="includes">An optional action to include related entities in the query.</param>
    /// <returns>An IQueryable of entities that match the condition.</returns>
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

    /// <summary>
    /// Retrieves a collection of entities from the database that match the specified condition and projects them to a different type using the provided selector expression. This method takes a selector expression to specify how to project the entities to a different type (TResult), an optional expression to filter the entities based on a specific condition, and an optional action to include related entities in the query. The method constructs a query based on the provided parameters and returns an IQueryable of the projected entities that match the condition. If no matching entities are found, it returns an empty IQueryable.
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="selector"></param>
    /// <param name="where"></param>
    /// <param name="includes"></param>
    /// <returns></returns>
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

    /// <summary>
    /// Updates an existing entity in the database context. This method takes an instance of the entity as a parameter and updates its values in the DbSet representing the collection of entities in the database. The actual update in the database will occur when the SaveChanges or SaveChangesAsync method is called. This method is typically used to modify existing records in the database. It uses the Entry method of the database context to access the current values of the entity and sets them to the new values provided in the parameter.
    /// </summary>
    /// <param name="entity">The entity to update.</param>
    public virtual void Update(TEntity entity)
    {
        _context!.Entry(entity).CurrentValues.SetValues(entity);
    }

    /// <summary>
    /// Updates an existing entity in the database context by its ID. This method takes the ID of the entity to update and the new entity values as parameters. It retrieves the existing entity from the database, updates its values with the new values provided, and returns a boolean indicating whether the update was successful.
    /// </summary>
    /// <param name="id">The ID of the entity to update.</param>
    /// <param name="entity">The new values for the entity.</param>
    /// <returns>A boolean indicating whether the update was successful.</returns>
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

    /// <summary>
    /// Deletes an entity from the database context by its ID. This method takes the ID of the entity to delete as a parameter, retrieves the entity from the database, and removes it from the DbSet representing the collection of entities in the database. The actual deletion in the database will occur when the SaveChanges or SaveChangesAsync method is called. This method returns a boolean indicating whether the deletion was successful (i.e., if an entity with the specified ID was found and removed).
    /// </summary>
    /// <param name="id">The ID of the entity to delete.</param>
    /// <returns></returns>
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

    /// <summary>
    /// Deletes an entity from the database context. This method takes an instance of the entity to delete as a parameter and removes it from the DbSet representing the collection of entities in the database. The actual deletion in the database will occur when the SaveChanges or SaveChangesAsync method is called. This method is typically used to remove existing records from the database.
    /// </summary>
    /// <param name="entity">The entity to delete.</param>
    public virtual void Delete(TEntity entity)
    {
        _entities.Remove(entity);
    }

    /// <summary>
    /// Deletes multiple entities from the database context that match a specified condition. This method takes an expression as a parameter to filter the entities based on a specific condition. It retrieves all entities that match the condition and removes them from the DbSet representing the collection of entities in the database. The actual deletion in the database will occur when the SaveChanges or SaveChangesAsync method is called. This method is useful for bulk deleting multiple records from the database based on a specific condition.
    /// </summary>
    /// <param name="where">The condition to filter entities.</param>
    public virtual void DeleteMany(Expression<Func<TEntity, bool>> where)
    {
        var entities = GetAll(where);
        _entities.RemoveRange(entities);
    }

    /// <summary>
    /// Checks if any entities in the database context match a specified condition. This method takes an expression as a parameter to filter the entities based on a specific condition and returns a boolean indicating whether any entities match the condition. It uses the AnyAsync method of the DbSet to perform this check asynchronously. This method is commonly used to verify the existence of records in the database that meet certain criteria without retrieving the actual entities.
    /// </summary>
    /// <param name="where">The condition to filter entities.</param>
    /// <returns></returns>
    public virtual async Task<bool> Any(Expression<Func<TEntity, bool>> where)
    {
        return await _entities.AnyAsync(where);
    }

    /// <summary>
    /// Counts the number of entities in the database context that match a specified condition. This method takes an expression as a parameter to filter the entities based on a specific condition and returns the count of entities that match the condition. It uses the CountAsync method of the DbSet to perform this count asynchronously. This method is useful for determining how many records in the database meet certain criteria without retrieving the actual entities.
    /// </summary>
    /// <param name="where">The condition to filter entities.</param>
    /// <returns>The number of entities that match the condition.</returns>
    public virtual async Task<int> Count(Expression<Func<TEntity, bool>> where)
    {
        return await _entities.CountAsync(where);
    }

    /// <summary>
    /// Retrieves a collection of entities from the database that match the specified condition and projects them to a different type using the provided selector expression. This method takes a selector expression to specify how to project the entities to a different type (TResult), an optional expression to filter the entities based on a specific condition, and an optional action to include related entities in the query. The method constructs a query based on the provided parameters and returns an IQueryable of the projected entities that match the condition. If no matching entities are found, it returns an empty IQueryable.       
    /// </summary>
    /// <param name="trackChanges">Indicates whether to track changes for the retrieved entities.</param>
    /// <returns>An IQueryable of the retrieved entities.</returns>
    public IQueryable<TEntity> FindAll(bool trackChanges = true)
    {
        return trackChanges ? _entities.AsNoTracking() : _entities;
    }

     /// <summary>
     ///    
     /// </summary>
     /// <param name="expression"></param>
     /// <param name="trackChanges"></param>
     /// <returns></returns>
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