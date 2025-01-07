using System;
using Microsoft.Extensions.Configuration;

namespace SG.Shared.Helpers;

public class AppConfiguration
{
    public IConfigurationRoot Configuration { get; }

    public AppConfiguration(string? fileName = null)
    {
        // Construye el ConfigurationRoot desde el archivo appsettings.json
        fileName ??= "appsettings.json";
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory()) // Establece el directorio base para el archivo JSON
            .AddJsonFile(fileName, optional: false, reloadOnChange: true); // Carga el archivo JSON

        Configuration = builder.Build();
    }

    public AppConfiguration(string directorio,string? fileName = null)
    {
        // Construye el ConfigurationRoot desde el archivo appsettings.json
        fileName ??= "appsettings.json";
        var builder = new ConfigurationBuilder()
            .SetBasePath(directorio) // Establece el directorio base para el archivo JSON
            .AddJsonFile(fileName, optional: false, reloadOnChange: true); // Carga el archivo JSON

        Configuration = builder.Build();
    }

    public static void AddFileConfiguration(IConfigurationManager configuration,string directorio, string fileName)
    {
        configuration.SetBasePath(directorio)
                         .AddJsonFile(fileName, optional: false, reloadOnChange: true);
      
    }
}
