

using System.Reflection;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace SG.API.Filters;

using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

public class ArrayObjectQueryParameterFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if (operation.Parameters == null) return;

        foreach (var parameter in context.ApiDescription.ParameterDescriptions)
        {
            if (parameter.Source.Id != "Query") continue;

            var parameterType = parameter.Type;
            
            if (IsArrayOfObjects(parameterType))
            {
                // Remove the original parameter
              //  operation.Parameters.RemoveAll(p => p.Name == parameter.Name);
                
                // Get the element type
                var elementType = parameterType.GetElementType() ?? 
                                  parameterType.GetGenericArguments()[0];
                
                // Add parameters for each property
                foreach (var prop in elementType.GetProperties())
                {
                    operation.Parameters.Add(new OpenApiParameter
                    {
                        Name = $"{parameter.Name}[].{ToCamelCase(prop.Name)}",
                        In = ParameterLocation.Query,
                        Schema = new OpenApiSchema
                        {
                            Type = GetSwaggerType(prop.PropertyType),
                            Format = GetSwaggerFormat(prop.PropertyType)
                        },
                        Required = prop.GetCustomAttribute<RequiredAttribute>() != null,
                       // Description = GetPropertyDescription(prop)
                    });
                }
            }
        }
    }

    private bool IsArrayOfObjects(Type type)
    {
        if (type.IsArray && type.GetElementType()?.IsClass == true)
            return true;
        
        if (type.IsGenericType && 
            (type.GetGenericTypeDefinition() == typeof(List<>) ||
             type.GetGenericTypeDefinition() == typeof(IEnumerable<>)) &&
            type.GetGenericArguments()[0].IsClass)
            return true;
            
        return false;
    }

    private string GetSwaggerType(Type type) => Type.GetTypeCode(type) switch
    {
        TypeCode.Int32 or TypeCode.Int64 => "integer",
        TypeCode.Decimal or TypeCode.Double or TypeCode.Single => "number",
        TypeCode.Boolean => "boolean",
        TypeCode.DateTime => "string",
        _ => "string"
    };

    private string? GetSwaggerFormat(Type type) => Type.GetTypeCode(type) switch
    {
        TypeCode.Int32 => "int32",
        TypeCode.Int64 => "int64",
        TypeCode.Single => "float",
        TypeCode.Double => "double",
        TypeCode.DateTime => "date-time",
        _ => null
    };

   /* private string GetPropertyDescription(PropertyInfo prop)
    {
        return prop.GetCustomAttribute<SwaggerSchemaAttribute>()?.Description 
               ?? prop.GetCustomAttribute<DescriptionAttribute>()?.Description
               ?? string.Empty;
    }
*/
    private string ToCamelCase(string str) => 
        string.IsNullOrEmpty(str) || char.IsLower(str[0]) 
            ? str 
            : char.ToLowerInvariant(str[0]) + str[1..];
}