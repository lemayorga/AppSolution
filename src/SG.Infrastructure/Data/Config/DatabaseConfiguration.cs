
using SG.Shared.Enumerators;

namespace SG.Infrastructure.Data.Config;

/// <summary>
/// Configuration interface of the data bank, containing the name of the provider, the connection string and the type of the provider (EnumManagersBD).
/// </summary>
public interface IDatabaseConfiguration 
{
    /// <summary>
    /// Gets the name of the database provider, which is used to determine the type of database being used (e.g., SQL Server, MySQL, PostgreSQL).
    /// </summary>
    string DatabaseProviderName { get; } 
    /// <summary>
    /// Gets the type of the database provider as an enumeration value (EnumGestoresBD), which allows for type-safe handling of different database providers in the application.
    /// </summary>
    EnumGestoresBD DatabaseProvider { get; } 
    /// <summary>
    /// Gets the connection string, which contains the necessary information to establish a connection to the database, such as server address, database name, user credentials, and other connection parameters.
    /// </summary>
    string ConnectionString { get; }
}

/// <summary>
/// Concrete implementation of the IDatabaseConfiguration interface, which provides the actual values for the database provider name, connection string, and the logic to convert the provider name to the corresponding enumeration value (EnumGestoresBD).
/// </summary>
internal sealed class DatabaseConfiguration : IDatabaseConfiguration
{
    /// <inheritdoc />
    public string DatabaseProviderName { get; } 

    /// <inheritdoc />    
    public string ConnectionString { get; } 

    /// <inheritdoc />
    public EnumGestoresBD DatabaseProvider
    { 
        get => (EnumGestoresBD)Enum.Parse(typeof(EnumGestoresBD), DatabaseProviderName); 
    } 

    /// <summary>
    /// Initializes a new instance of the DatabaseConfiguration class with the specified database provider name and connection string. 
    /// The constructor assigns the provided values to the corresponding properties, allowing the application to access the database configuration details when needed.
    /// </summary>
    /// <param name="databaseProviderName"></param>
    /// <param name="connectionString"></param>
    public DatabaseConfiguration(string databaseProviderName, string connectionString)
    {
        DatabaseProviderName = databaseProviderName;
        ConnectionString = connectionString;
    }
}
