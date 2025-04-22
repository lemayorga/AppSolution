using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace SG.API.Filters;

public class SwaggerDefaultValuesFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var apiDescription = context.ApiDescription;

        if (operation.Parameters == null) return;

        foreach (var parameter in operation.Parameters)
        {
            var description = apiDescription.ParameterDescriptions.FirstOrDefault(p => p.Name == parameter.Name);
            if (description == null) continue;

            parameter.Description ??= description.ModelMetadata.Description;

            if (parameter.Schema.Default == null && description.DefaultValue != null)
                parameter.Schema.Default = new OpenApiString(description.DefaultValue.ToString());

            parameter.Required |= description.IsRequired;
        }
    }
}