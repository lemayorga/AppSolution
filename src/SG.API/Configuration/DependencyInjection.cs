using SG.Application;
using SG.Infrastructure.Services;
using SG.Infrastructure.Auth;
using FluentValidation;
using System.Reflection;

namespace SG.API.Configuration;

public static class DependencyInjection
{
    public static IServiceCollection AddDependencyInjection(this IServiceCollection services, IConfiguration configuration)
    {

        services.AddDatabaseConfiguration(configuration)
                 .AddInfrastructure()
                 .AddValidatorsApplication()
                 .AddServicesApplication()
                 .AddCommandHandlersAndQueryHandlers()
                .AddAutoMapperConfiguration()
                .AddInfrastructureAuth(configuration)
                .AddHealthChecks();

        return services;
    }
}
