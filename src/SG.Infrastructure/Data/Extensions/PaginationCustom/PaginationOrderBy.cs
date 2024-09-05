using System.Linq.Expressions;

namespace SG.Infrastructure.Data.Extensions.PaginationCustom;

public static class PaginationOrderBy
{
    public static IOrderedQueryable<TSource> OrderByWithDirection<TSource,TKey>( 
        this IQueryable<TSource> source, 
        Expression<Func<TSource, TKey>> keySelector, 
        bool ascending = true 
    ) 
    { 
        return ascending ? source.OrderBy(keySelector) 
            : source.OrderByDescending(keySelector); 
    } 

    public static IOrderedQueryable<TSource> OrderByWithDirection<TSource,TKey>( 
        this IQueryable<TSource> source, 
        Expression<Func<TSource, TKey>> keySelector, 
        string order = "asc" 
    ) 
    { 
        order = order.ToLower(); 
        if (order != "asc" && order != "desc") 
            throw new ArgumentException($"{nameof(order)} can be either 'asc' or 'desc'."); 
        return source.OrderByWithDirection(keySelector, order == "asc"); 
    }

    public static Expression<Func<T, object>> PropertySelectorFrom<T>(this string propertyName) 
    { 
        ParameterExpression parameter = Expression.Parameter(typeof(T)); 
        UnaryExpression body =  Expression.Convert( 
            Expression.PropertyOrField(parameter, propertyName), typeof(object)); 
        return Expression.Lambda<Func<T, object>>(body, parameter); 
    }   

    public static IOrderedQueryable<TSource> ThenByWithDirection<TSource,TKey>( 
    this IOrderedQueryable<TSource> source, 
    Expression<Func<TSource, TKey>> keySelector, 
    bool ascending = true 
    ) 
    { 
        return ascending ? source.ThenBy(keySelector) 
            : source.ThenByDescending(keySelector); 
    }  

    public static IOrderedQueryable<TSource> ChainedOrderBy<TSource>( 
        this IQueryable<TSource> source, List<PaginationModels.ColumnSorting> columnSorting) 
    { 
        if (columnSorting == null)  
            throw new ArgumentNullException(nameof(columnSorting)); 
        if (!columnSorting.Any())  
            throw new ArgumentException($"{nameof(columnSorting)} should not be empty.");             
 
        return columnSorting
                .Skip(1)
                .Aggregate(
                       source.OrderByWithDirection(
                         columnSorting[0].ColumnName.PropertySelectorFrom<TSource>(),  
                         !columnSorting[0].Desc 
                       ),
                      (current, orderSetting) => 
                        current.ThenByWithDirection( 
                            orderSetting.ColumnName.PropertySelectorFrom<TSource>(),  
                            !orderSetting.Desc 
                        ) 
                );
    }                
}
