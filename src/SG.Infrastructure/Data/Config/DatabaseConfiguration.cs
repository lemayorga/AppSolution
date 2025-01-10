
using SG.Shared.Enumerators;

namespace SG.Infrastructure.Data.Config;

public interface IDatabaseConfiguration 
{
    string DatabaseProviderName { get; } 
    EnumGestoresBD DatabaseProvider { get; } 
    string ConnectionString { get; }
}

internal sealed class DatabaseConfiguration : IDatabaseConfiguration
{
    public string DatabaseProviderName { get; } 
    public string ConnectionString { get; } 
    public EnumGestoresBD DatabaseProvider
    { 
        get => (EnumGestoresBD)Enum.Parse(typeof(EnumGestoresBD), DatabaseProviderName); 
    } 

    public DatabaseConfiguration(string databaseProviderName, string connectionString)
    {
        DatabaseProviderName = databaseProviderName;
        ConnectionString = connectionString;
    }
}
