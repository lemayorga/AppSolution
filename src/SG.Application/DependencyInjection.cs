using Microsoft.Extensions.DependencyInjection;
using SG.Application.Bussiness;
using SG.Application.Bussiness.Commun.Intefaces;
using SG.Application.Bussiness.Commun.Services;

namespace SG.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddServicesApplication(this IServiceCollection services) =>
        services
           .AddScoped(typeof(IBaseGenericService<,,,>), typeof(BaseGenericService<,,,>))
           .AddScoped<ICatalogueService, CatalogueService>();                 
}
