using SG.Application;

namespace SG.API.Configuration;

internal static class AutoMapperConfig
{
    internal static IServiceCollection AddAutoMapperConfiguration(this IServiceCollection services)
    {
        services.AddAutoMapper(cfg => cfg.AddProfile<MappingProfile>(), AppDomain.CurrentDomain.GetAssemblies());
        return services;
    }
}
