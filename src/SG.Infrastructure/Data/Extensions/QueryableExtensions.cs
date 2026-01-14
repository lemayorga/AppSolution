using System.Linq.Expressions;
using SG.Infrastructure.Base.Pagination;

namespace SG.Infrastructure.Data.Extensions;

public static partial class QueryableExtensions
{
    public static (int totalCount, IQueryable<T> data) ApplyPagination<T>(this IQueryable<T> query, PaginationParams pagination)
    {
        if (pagination.SortField is not null)
        {
            query = query.OrderByDynamic(pagination.SortField, nameof(pagination.SortDirection));
        }
        var totalCount = query.Count();

        return (totalCount, query.Skip((pagination.PageNumber - 1) * pagination.PageSize)
                   .Take(pagination.PageSize));
    }

    public static IQueryable<T> OrderByDynamic<T>(this IQueryable<T> query, string field, string direction)
    {
        var parameter = Expression.Parameter(typeof(T), "x");
        var property = Expression.Property(parameter, field);
        var lambda = Expression.Lambda(property, parameter);

        direction = direction.ToLower();
        var methodName = direction.Equals("desc", StringComparison.OrdinalIgnoreCase)
            ? "OrderByDescending"
            : "OrderBy";

        var methodCall = Expression.Call(
            typeof(Queryable),
            methodName,
            new[] { typeof(T), property.Type },
            query.Expression,
            Expression.Quote(lambda));

        return query.Provider.CreateQuery<T>(methodCall);
    }


    public static IQueryable<T> ApplyFilters<T>(this IQueryable<T> query, FilterParam[]? filters)
    {
        if (filters == null || !filters.Any())
            return query;

        var parameter = Expression.Parameter(typeof(T), "x");
        Expression? combinedExpression = null;

        foreach (var filter in filters)
        {
            var property = Expression.Property(parameter, filter.Field);
            var constant = GetConstantExpression(property.Type, filter.Value);

            string operatorFilter = nameof(filter.Operator).ToLower();
            Expression expression = operatorFilter switch
            {
                "eq" => Expression.Equal(property, constant),
                "neq" => Expression.NotEqual(property, constant),
                "contains" => Expression.Call(property, typeof(string).GetMethod("Contains", new[] { typeof(string) })!, constant),
                "startswith" => Expression.Call(property, typeof(string).GetMethod("StartsWith", new[] { typeof(string) })!, constant),
                "endswith" => Expression.Call(property, typeof(string).GetMethod("EndsWith", new[] { typeof(string) })!, constant),
                "gt" => Expression.GreaterThan(property, constant),
                "gte" => Expression.GreaterThanOrEqual(property, constant),
                "lt" => Expression.LessThan(property, constant),
                "lte" => Expression.LessThanOrEqual(property, constant),
                _ => throw new ArgumentException($"Operador no soportado: {filter.Operator}")
            };

            combinedExpression = combinedExpression == null
                ? expression
                : Expression.AndAlso(combinedExpression, expression);
        }

        if (combinedExpression != null)
        {
            var lambda = Expression.Lambda<Func<T, bool>>(combinedExpression, parameter);
            query = query.Where(lambda);
        }

        return query;
    }

    private static Expression GetConstantExpression(Type type, string value)
    {
        if (type == typeof(string))
            return Expression.Constant(value);

        if (type == typeof(int) && int.TryParse(value, out var intValue))
            return Expression.Constant(intValue);

        if (type == typeof(decimal) && decimal.TryParse(value, out var decimalValue))
            return Expression.Constant(decimalValue);

        if (type == typeof(DateTime) && DateTime.TryParse(value, out var dateValue))
            return Expression.Constant(dateValue);

        if (type == typeof(bool) && bool.TryParse(value, out var boolValue))
            return Expression.Constant(boolValue);

        throw new ArgumentException($"Tipo no soportado para conversión: {type.Name}");
    }
    
    public static IQueryable<T> ApplySearch<T>(this IQueryable<T> query,  SearchParameters parameters)
    {
        if (string.IsNullOrWhiteSpace(parameters.SearchTerm))
            return query;

        // Create a parameter expression (x => ...)
        var parameter = Expression.Parameter(typeof(T), "x");
        
        // Build OR conditions for each search field
        Expression? combinedExpression = null;
        foreach (var field in parameters.SearchFields)
        {
            var property = Expression.Property(parameter, field);
            var toStringCall = Expression.Call(property, "ToString", null, null);
            var containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });
            var containsCall = Expression.Call(toStringCall, containsMethod!, 
                Expression.Constant(parameters.SearchTerm));
            
            combinedExpression = combinedExpression == null 
                ? containsCall 
                : Expression.OrElse(combinedExpression, containsCall);
        }

        if (combinedExpression != null)
        {
            var lambda = Expression.Lambda<Func<T, bool>>(combinedExpression, parameter);
            query = query.Where(lambda);
        }

        return query;
    }

}
