using SG.Shared.Settings;

namespace SG.API.Configuration;

internal static class AppSettingsConfig
{
    internal static IServiceCollection AddOptionsSettings(this IServiceCollection services, ConfigurationManager configuration)
    {
        services.Configure<AppSettings>(configuration.GetSection(NamesApplicationSettings.ApplicationSettings));
        return services;
    }
}
