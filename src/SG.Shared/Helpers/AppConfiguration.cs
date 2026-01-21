using Microsoft.Extensions.Configuration;
using SG.Shared.Settings;
namespace SG.Shared.Helpers;

public class AppConfiguration
{
    private IConfiguration _configurationRoot;
    public IConfiguration ConfigurationRoot  {   get { return _configurationRoot; }  }

    public AppConfiguration() =>
        _configurationRoot = ReadFileSetting(string.Empty);

    public AppConfiguration(string fileName, string? path = null)  =>
        _configurationRoot = ReadFileSetting(fileName, path);

    public AppConfiguration(IConfigurationRoot configuration) =>
        _configurationRoot = configuration;

    public AppConfiguration(IConfiguration configuration) =>
        _configurationRoot = configuration;

    /// <summary>
    /// Add file configuration en execute time
    /// </summary>
    /// <param name="configuration">Configuration from inyection dependes</param>
    /// <param name="path">Path from  file</param>
    /// <param name="fileName">File name</param>
   /// <param name="_reloadOnChange">Option of reload on change</param>
    public static void AddFileConfiguration(IConfigurationManager configuration,string path, string fileName, bool _reloadOnChange = true)
    {
        configuration.SetBasePath(path)
                    .AddJsonFile(fileName, optional: false, reloadOnChange: _reloadOnChange);
    }
    
    /// <summary>
    /// Get AppSettings
    /// </summary>
    /// <returns></returns>
    public AppSettings GetAppSettings()
    {
        return this.GetSectionAsObject<AppSettings>(NamesApplicationSettings.ApplicationSettings);
    }


    /// <summary>
    /// Read section from configuration
    /// </summary>
    /// <typeparam name="T">Convert values</typeparam>
    /// <param name="sectionName">Section name</param>
    /// <returns></returns>
    public T GetSectionAsObject<T>(string? sectionName = null) where T : new() 
    {
        T resultModel = new();
        if(string.IsNullOrWhiteSpace(sectionName))
        {
            ConfigurationRoot.Bind(resultModel);
        }
        else 
        {
            ConfigurationRoot.GetSection(sectionName).Bind(resultModel);
        }
        
        return resultModel;
    }
    
    /// <summary>
    /// Read settings from file name configuration
    /// </summary>
    /// <param name="fileName"></param>
    /// <param name="path"></param>
    /// <returns></returns>
    private IConfigurationRoot ReadFileSetting(string fileName, string? path = null)
    {
        path ??= Directory.GetCurrentDirectory();
        var builder = new ConfigurationBuilder()
            .SetBasePath(path); // Establece el directorio base para el archivo JSON
        
        if(!string.IsNullOrWhiteSpace(fileName))
        {
            builder.AddJsonFile(fileName, optional: false, reloadOnChange: true); // Carga el archivo JSON
        }

        return builder.Build();
    }
}
