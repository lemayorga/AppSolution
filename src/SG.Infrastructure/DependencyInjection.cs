using Microsoft.Extensions.DependencyInjection;
using SG.Domain;
using SG.Domain.Commun.Repositories;
using SG.Infrastructure.Data;
using SG.Infrastructure.Data.Context;
using SG.Infrastructure.Data.Repositories;
using SG.Infrastructure.Data.Repositories.Commun;

namespace SG.Infrastructure.Services;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services) =>
        services
           .AddScoped<ApplicationDbContext>()
           .AddScoped(typeof(IBaseGenericRepository<>), typeof(BaseGenericRepository<>))
           .AddScoped<ICatalogueRepository, CatalogueRepository>()
           .AddScoped<IUnitOfWork, UnitOfWork>();
}
