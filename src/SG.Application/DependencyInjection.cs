using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using SG.Application.Base.CQRS;
using SG.Application.Base.CQRS.Dispatcher;
using FluentValidation;
using SG.Application.Bussiness.Security.Users.Interfaces;
using SG.Application.Bussiness.Security.Roles.Service;
using SG.Application.Bussiness.Security.Users.Services;
using SG.Application.Bussiness.Security.Auth.Service;
using SG.Application.Bussiness.Security.Auth.Interface;
using SG.Application.Bussiness.Commun.Catalogues.Interfaces;
using SG.Application.Bussiness.Commun.Catalogues.Services;
using SG.Application.Base.ServiceLogic;
using SG.Application.Bussiness.Security.Roles.Interfaces;

namespace SG.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddServicesApplication(this IServiceCollection services)
    {
        services.AddScoped(typeof(IBaseGenericService<,,,>), typeof(BaseGenericService<,,,>));
        services.AddScoped<ICatalogueService, CatalogueService>()
                .AddScoped<IRoleService, RoleService>()
                .AddScoped<IUserService, UserService>()
                .AddScoped<IAuthService, AuthService>();
        return services;
    }

    public static IServiceCollection AddCommandHandlersAndQueryHandlers(this IServiceCollection services)
    {
        services.AddSingleton<ISender, Sender>();

        // Register all command and query handlers
        var handlerTypes = Assembly.GetExecutingAssembly().GetTypes()
            .Where(t => t.GetInterfaces().Any(i =>
                i.IsGenericType &&
                (i.GetGenericTypeDefinition() == typeof(IRequestHandler<>) ||
                  i.GetGenericTypeDefinition() == typeof(IRequestHandler<,>))));

        foreach (var type in handlerTypes)
        {
            var interfaces = type.GetInterfaces()
                .Where(i => i.IsGenericType &&
                        (i.GetGenericTypeDefinition() == typeof(IRequestHandler<>) ||
                          i.GetGenericTypeDefinition() == typeof(IRequestHandler<,>)));

            foreach (var interfaceType in interfaces)
            {
                //  services.AddScoped(interfaceType, type);
                services.AddTransient(interfaceType, type);
            }
        }
        return services;
    }

    public static IServiceCollection AddValidatorsApplication(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        return services;
    }
}
