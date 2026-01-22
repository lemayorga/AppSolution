using SG.Application;
using SG.Infrastructure.Services;
using SG.Infrastructure.Auth;

namespace SG.API.Configuration;

public static class DependencyInjection
{
    public static IServiceCollection AddDependencyInjection(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDatabaseConfiguration(configuration)
                 .AddInfrastructure()
                 .AddServicesApplication()
               //  .AddCommandHandlersAndQueryHandlers()
                 .AddValidatorsApplication()
                 .AddDynamicValidation(typeof(Program).Assembly)
                 .AddAutoMapperConfiguration()
                 .AddInfrastructureAuth(configuration)
                 .AddHealthChecks();
        return services;
    }
}