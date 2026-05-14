using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using SG.Domain.Entities.Commun;
using SG.Domain.Repositories.Commun;
using SG.Infrastructure.Base;
using SG.Infrastructure.Data.Context;
using SG.Infrastructure.Data.Extensions.PaginationCustom;
using SG.Infrastructure.Data.Extensions.PaginationCustom.PaginationModels;

namespace SG.Infrastructure.Data.Repositories.Commun;

/// <summary>
/// CatalogueRepository is a repository class that provides data access methods for the Catalogue entity. It inherits from BaseGenericRepository and implements the ICatalogueRepository interface. The Paginate method allows for paginated retrieval of Catalogue records with optional search, filtering, and sorting capabilities.
/// </summary>
public class CatalogueRepository : BaseGenericRepository<Catalogue>, ICatalogueRepository
{
    /// <summary>
    /// Initializes a new instance of the CatalogueRepository class with the specified ApplicationDbContext. This constructor calls the base class constructor to set up the database context for data access operations.
    /// </summary>
    /// <param name="context"></param>
    public CatalogueRepository(ApplicationDbContext context) : base(context) {  }
  

     public override async Task<(int, IEnumerable<Catalogue>)> Paginate(
        int pageNumber,
        int pageSize,
        string? searchTerm = null ,       
        Dictionary<string, string>? columnFilters = null, 
        Dictionary<string, string>? orderByColumns = null)
     {
        var _columnFilters =  columnFilters == null ? [] : columnFilters.Select(p => new ColumnFilter { ColumnName =  p.Key, Value =  p.Value }).ToList();
        var _columnSorting =  orderByColumns == null ? [] : orderByColumns.Select(p => new ColumnSorting { ColumnName =  p.Key, Desc =  bool.Parse(p.Value ?? "false") }).ToList();

        Expression<Func<Catalogue, bool>>? filters = null;
        //First, we are checking our SearchTerm. If it contains information we are creating a filter.
        if (!string.IsNullOrEmpty(searchTerm) && !string.IsNullOrWhiteSpace(searchTerm))
        {
            searchTerm = searchTerm.Trim().ToLower();
            filters = x => x.Value.ToLower().Contains(searchTerm);
            
            // filters = x => x.Value.ToLower().Contains(searchTerm)
            //             || x.Group.ToLower().Contains(searchTerm);
        }
        // Then we are overwriting a filter if columnFilters has data.
        if (_columnFilters.Count > 0)
        {
            var customFilter = CustomExpressionFilter<Catalogue>.CustomFilter(_columnFilters, "value");
            filters = customFilter;
        }

        var query = _entities.AsQueryable().CustomQuery(filters);

        if(_columnSorting.Any())
        {
            query =  query.ChainedOrderBy(_columnSorting);
        }

        var count= await query.CountAsync();
        var filteredData = await query.CustomPagination(pageNumber,pageSize).ToListAsync();    
        return (count, filteredData);
     }
}
