using System;
using System.Linq.Expressions;

namespace SG.Infrastructure.Data.Extensions;

public static class ExpressionCombiner
{
    // Combina dos expresiones con el operador AND
    public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> expr1, Expression<Func<T, bool>> expr2)
    {
        var parameter = Expression.Parameter(typeof(T));

        var combined = Expression.AndAlso(
            Expression.Invoke(expr1, parameter),
            Expression.Invoke(expr2, parameter)
        );

        return Expression.Lambda<Func<T, bool>>(combined, parameter);
    }

    public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> expr1, Expression<Func<T, bool>> expr2)
    {
        var parameter = Expression.Parameter(typeof(T));

        var combined = Expression.OrElse(
            Expression.Invoke(expr1, parameter),
            Expression.Invoke(expr2, parameter)
        );

        return Expression.Lambda<Func<T, bool>>(combined, parameter);
    }
    public static Expression<Func<T, bool>> TrimAndLowerEqual<T>(this Expression<Func<T, string>> selector, string value)
    {
        return Expression.Lambda<Func<T, bool>>(
            Expression.Equal(
                Expression.Call(
                    Expression.Call(
                        selector.Body,
                        typeof(string).GetMethod("Trim", Type.EmptyTypes)!
                    ),
                    typeof(string).GetMethod("ToLower", Type.EmptyTypes)!
                ),
                Expression.Constant(value.Trim().ToLower())
            ),
            selector.Parameters
        );
    }

    public static Expression<Func<T, string>> CreatePropertyExpression<T>(string propertyName)
    {
        // Get the property info
        var propertyInfo = typeof(T).GetProperty(propertyName);
        if (propertyInfo == null)
        {
            throw new ArgumentException($"Property '{propertyName}' not found on type '{typeof(T).Name}'");
        }

        // Parameter expression (e.g., u => u.Email)
        var parameter = Expression.Parameter(typeof(T), "u");

        // Property expression (e.g., u => u.Email)
        var property = Expression.Property(parameter, propertyInfo);

        // Convert to string if needed (e.g., if the property is not already a string)
        var toStringMethod = typeof(object).GetMethod("ToString");
        var toStringCall = Expression.Call(property, toStringMethod);

        // Create the lambda expression (u => u.Email)
        return Expression.Lambda<Func<T, string>>(toStringCall, parameter);
    }
}
