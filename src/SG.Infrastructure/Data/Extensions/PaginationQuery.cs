// using System.Linq.Expressions;

// namespace SG.Infrastructure.Data.Extensions;

// public class ColumnFilter
// {
//     public string id { get; set; }
//     public string value { get; set; }
// }

// public class ColumnSorting
// {
//     public string? id { get; set; }
//     public bool desc { get; set; }
// }

// public static class CustomExpressionFilter<T> where T : class
// {
//     public class ExpressionFilter
//     {
//         public string ColumnName { get; set; }
//         public string Value { get; set; }
//     }
//     public static Expression<Func<T, bool>> CustomFilter(List<ColumnFilter> columnFilters, string className)
//     {
//         Expression<Func<T, bool>>? filters = null;
//         try
//         {
//             var expressionFilters = new List<ExpressionFilter>();
//             foreach (var item in columnFilters)
//             {                    
//                     expressionFilters.Add(new ExpressionFilter() { ColumnName = item.id, Value = item.value });
//             }
//             // Create the parameter expression for the input data
//             var parameter = Expression.Parameter(typeof(T), className);

//             // Build the filter expression dynamically
//             Expression? filterExpression = null;
//             foreach (var filter in expressionFilters)
//             {
//                 var property = Expression.Property(parameter, filter.ColumnName);

//                 Expression comparison;

//                 if (property.Type == typeof(string))
//                 {
//                     var constant = Expression.Constant(filter.Value);
//                     comparison = Expression.Call(property, "Contains", Type.EmptyTypes, constant);
//                 }
//                 else if (property.Type == typeof(double))
//                 {
//                     var constant = Expression.Constant(Convert.ToDouble(filter.Value));
//                     comparison = Expression.Equal(property, constant);
//                 }
//                 else if (property.Type == typeof(Guid))
//                 {
//                     var constant = Expression.Constant(Guid.Parse(filter.Value));
//                     comparison = Expression.Equal(property, constant);
//                 }
//                 else
//                 {
//                     var constant = Expression.Constant(Convert.ToInt32(filter.Value));
//                     comparison = Expression.Equal(property, constant);
//                 }


//                 filterExpression = filterExpression == null
//                     ? comparison
//                     : Expression.And(filterExpression, comparison);
//             }

//             // Create the lambda expression with the parameter and the filter expression
//             filters = Expression.Lambda<Func<T, bool>>(filterExpression!, parameter);
//         }
//         catch (Exception)
//         {
//             filters = null;
//         }
//         return filters!;
//     }

// }


// public static class QueryableExtensions
// {
//     public static IOrderedQueryable<T> OrderByColumn<T>(this IQueryable<T> source, string columnPath) 
//         => source.OrderByColumnUsing(columnPath, "OrderBy");

//     public static IOrderedQueryable<T> OrderByColumnDescending<T>(this IQueryable<T> source, string columnPath) 
//         => source.OrderByColumnUsing(columnPath, "OrderByDescending");

//     public static IOrderedQueryable<T> ThenByColumn<T>(this IOrderedQueryable<T> source, string columnPath) 
//         => source.OrderByColumnUsing(columnPath, "ThenBy");

//     public static IOrderedQueryable<T> ThenByColumnDescending<T>(this IOrderedQueryable<T> source, string columnPath) 
//         => source.OrderByColumnUsing(columnPath, "ThenByDescending");

//     private static IOrderedQueryable<T> OrderByColumnUsing<T>(this IQueryable<T> source, string columnPath, string method)
//     {
//         var parameter = Expression.Parameter(typeof(T), "item");
//         var member = columnPath.Split('.')
//             .Aggregate((Expression)parameter, Expression.PropertyOrField);
//         var keySelector = Expression.Lambda(member, parameter);
//         var methodCall = Expression.Call(typeof(Queryable), method, new[] 
//                 { parameter.Type, member.Type },
//             source.Expression, Expression.Quote(keySelector));

//         return (IOrderedQueryable<T>)source.Provider.CreateQuery(methodCall);
//     }


//     public static IQueryable<TEntity> OrderByColumnUsing2<TEntity>(this IQueryable<TEntity> source, string orderByStrValues)
//         where TEntity : class
//     {
//         var queryExpr = source.Expression;
//         var command = orderByStrValues.ToUpper().EndsWith("DESC") ? "OrderByDescending" : "OrderBy";
//         var propertyName = orderByStrValues.Split(' ')[0].Trim();

//         var type = typeof(TEntity);
//         var property = type.GetProperties()
//             .Where(item => item.Name.ToLower() == propertyName.ToLower())
//             .FirstOrDefault();

//         if (property == null)
//             return source;

//         // p
//         var parameter = Expression.Parameter(type, "Group");
//         // p.Price
//         var propertyAccess = Expression.MakeMemberAccess(parameter, property);
//         // p => p.Price
//         var orderByExpression = Expression.Lambda(propertyAccess, parameter);

//         // Ejem. final: .OrderByDescending(p => p.Price)
//         queryExpr = Expression.Call(
//             type: typeof(Queryable),
//             methodName: command,
//             typeArguments: new Type[] { type, property.PropertyType },
//             queryExpr,
//             Expression.Quote(orderByExpression));

//         return source.Provider.CreateQuery<TEntity>(queryExpr); ;
//     }    
// }


// public static class PaginationQuery
// {
//     public static IQueryable<T> CustomQuery<T>(this IQueryable<T> query, Expression<Func<T, bool>>? filter = null) where T : class
//     {
//         if (filter != null)
//         {
//             query = query.Where(filter);
//         }          

//         return query;
//     }

//     public static IQueryable<T> CustomPagination<T>(this IQueryable<T> query, int? page = 0, int? pageSize = null)
//     {
//         if (page != null)
//         {
//             query = query.Skip(((int)page - 1) * (int)pageSize);
//         }

//         if (pageSize != null)
//         {
//             query = query.Take((int)pageSize);
//         }
//         return query;
//     }
// }



// public class PagedList<T> : List<T>
// {
//     public PagedList(IEnumerable<T> items, int count, int pageNumber, int pageSize)
//     {
//         MetaData = new MetaData
//         {
//             TotalCount = count,
//             PageSize = pageSize,
//             CurrentPage = pageNumber,
//             TotalPages = (int)Math.Ceiling(count / (double)pageSize),
//         };
//         AddRange(items);
//     }
//     public MetaData MetaData { get; set; }
// }

// public class MetaData
// {
//     public int CurrentPage { get; set; }
//     public int TotalPages { get; set; }
//     public int PageSize { get; set; }
//     public int TotalCount { get; set; }
// }





// public static class QueryableExtensions2
// { 
//     //flag with boolean 
//     public static IOrderedQueryable<TSource> OrderByWithDirection<TSource,TKey>( 
//         this IQueryable<TSource> source, 
//         Expression<Func<TSource, TKey>> keySelector, 
//         bool ascending = true 
//     ) 
//     { 
//         return ascending ? source.OrderBy(keySelector) 
//             : source.OrderByDescending(keySelector); 
//     } 
//     //OR 
//     //string as flag 
//     // public static IOrderedQueryable<TSource> OrderByWithDirection<TSource,TKey>( 
//     //     this IQueryable<TSource> source, 
//     //     Expression<Func<TSource, TKey>> keySelector, 
//     //     string order = "asc" 
//     // ) 
//     // { 
//     //     order = order.ToLower(); `
//     //     if (order != "asc" && order != "desc") 
//     //         throw new ArgumentException($"{nameof(order)} can be either 'asc' or 'desc'."); 
//     //     return source.OrderByWithDirection(keySelector, order == "asc"); 
//     // } 

//     public static IOrderedQueryable<TSource> ThenByWithDirection<TSource,TKey>( 
//     this IOrderedQueryable<TSource> source, 
//     Expression<Func<TSource, TKey>> keySelector, 
//     bool ascending = true 
//     ) 
//     { 
//         return ascending ? source.ThenBy(keySelector) 
//             : source.ThenByDescending(keySelector); 
//     }  

//     public static Expression<Func<T, object>> PropertySelectorFrom<T>(this string propertyName) 
//     { 
//         ParameterExpression parameter = Expression.Parameter(typeof(T)); 
//         UnaryExpression body =  Expression.Convert( 
//             Expression.PropertyOrField(parameter, propertyName), typeof(object)); 
//         return Expression.Lambda<Func<T, object>>(body, parameter); 
//     }    


//     public static IOrderedQueryable<TSource> ChainedOrderBy<TSource>( 
//         this IQueryable<TSource> source, List<ColumnSorting> columnSorting) 
//     { 
//         // if (orderBy == null)  
//         //     throw new ArgumentNullException(nameof(orderBy)); 
//         // if (orderDirection == null)  
//         //     throw new ArgumentNullException(nameof(orderDirection)); 
//         // if (!orderBy.Any())  
//         //     throw new ArgumentException($"{nameof(orderBy)} should not be empty."); 
//         // if (!orderDirection.Any())  
//         //     throw new ArgumentException($"{nameof(orderDirection)} should not be empty."); 
//         // if (orderBy.Length != orderDirection.Length)  
//         //     throw new ArgumentException( 
//         //         $"size of {nameof(orderBy)} and {nameof(orderDirection)} must be equal."); 


 
//         return columnSorting
//                 .Skip(1)
//                 .Aggregate(
//                        source.OrderByWithDirection(
//                          columnSorting[0].id!.PropertySelectorFrom<TSource>(),  
//                          !columnSorting[0].desc 
//                        ),
//                       (current, orderSetting) => 
//                         current.ThenByWithDirection( 
//                             orderSetting.id!.PropertySelectorFrom<TSource>(),  
//                             !orderSetting.desc 
//                         ) 
//                 );
//     }      
// }  