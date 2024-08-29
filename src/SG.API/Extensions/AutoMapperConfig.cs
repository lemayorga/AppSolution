using System.Reflection;
using AutoMapper;
using SG.Application;

namespace SG.API.Extensions;

internal static class AutoMapperConfig
{
    internal  static IServiceCollection AddAutoMapperConfiguration(this IServiceCollection services)
    {
         services.AddAutoMapper(cfg => cfg.AddProfile<MappingProfile>(), AppDomain.CurrentDomain.GetAssemblies());
        return services;
    }
}
