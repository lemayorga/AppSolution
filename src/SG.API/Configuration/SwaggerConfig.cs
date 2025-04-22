using System.Reflection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace SG.API.Configuration;

internal static class SwaggerConfig
{
    internal static IServiceCollection AddSwaggerConfigurationOpenApi(this IServiceCollection services)
    {
      // services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>()

        services.AddSwaggerGen(swaggerOptions =>
        {
           

           swaggerOptions.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description =
                    "JWT Authorization Header - utilizado com Bearer Authentication.\r\n\r\n" +
                    "Escriba 'Bearer' [espacio] y, a continuación, su ficha en el campo siguiente .\r\n\r\n" +
                    "Exemplo (informar sem as aspas): 'Bearer 12345abcdef'",
                Name = "JWT Authentication",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = JwtBearerDefaults.AuthenticationScheme,
                BearerFormat = "JWT"
            });

         // swaggerOptions.OperationFilter<FromQueryDictionaryFilter>();

            swaggerOptions.ExampleFilters();
           
            swaggerOptions.SwaggerDoc("v1", new OpenApiInfo()
            {
                Title = "SG API Documentation",
                Version = "v1",
                Description = "Description of your API",
                Contact = new OpenApiContact()
                {
                    Name = "Your name",
                    Email = "your@email.com",
                }

            });         

            swaggerOptions.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });

            swaggerOptions.OperationFilter<Filters.SwaggerDefaultValuesFilter>();
            
            swaggerOptions.ResolveConflictingActions(apiDescription => apiDescription.FirstOrDefault());

            // Set the comments path for the Swagger JSON and UI.
            swaggerOptions.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"), true);
        });

        services.AddSwaggerExamplesFromAssemblies(Assembly.GetEntryAssembly());
        return services;
    }

    internal static void AddSwaggerConfigurationUI(this IApplicationBuilder app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(swaggerOptions =>
        {

            // swaggerOptions.SwaggerEndpoint($"swagger/v1/swagger.json", "v1")
             swaggerOptions.DefaultModelExpandDepth(2);
             swaggerOptions.DocExpansion(DocExpansion.None);
             swaggerOptions.DisplayRequestDuration();             
        });
    }
}