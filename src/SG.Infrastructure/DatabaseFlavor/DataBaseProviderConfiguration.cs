using Microsoft.EntityFrameworkCore;
namespace SG.Infrastructure.DatabaseFlavor;

public class DataBaseProviderConfiguration
{
    private readonly string _connectionString;
    public DataBaseProviderConfiguration Witch() => this;
    
    public static DataBaseProviderConfiguration Build(string connnString)
    {
        return new DataBaseProviderConfiguration(connnString);
    }

    public DataBaseProviderConfiguration(string connnString)
    {
        _connectionString = connnString;
    }

    public Action<DbContextOptionsBuilder> SqlServer =>
        options => options.UseSqlServer(_connectionString, 
        sqlServerOptionsAction: sqlOptions => 
        {
            sqlOptions.EnableRetryOnFailure(
                maxRetryCount: 5,
                maxRetryDelay: TimeSpan.FromSeconds(5),
                errorNumbersToAdd: null
            );
        });

    public Action<DbContextOptionsBuilder> PostgreSql =>
        options => {
            options.UseNpgsql(_connectionString,
                npgsqlOptionsAction: npgsqlOption => {
                    npgsqlOption.EnableRetryOnFailure(
                        maxRetryCount: 5,
                        maxRetryDelay: TimeSpan.FromSeconds(5),
                        errorCodesToAdd: null
                );
            }); 
        };
}