using Microsoft.EntityFrameworkCore;
using SG.Shared.Enumerators;

namespace SG.Infrastructure.DatabaseFlavor;

public class ProviderSelector
{
    public static Action<DbContextOptionsBuilder> WithProviderAutoSelection(EnumGestoresBD database,string connString)
    {
        return database switch
        {
            EnumGestoresBD.SqlServer => DataBaseProviderConfiguration.Build(connString).Witch().SqlServer,
            EnumGestoresBD.PostgreSql => DataBaseProviderConfiguration.Build(connString).Witch().PostgreSql,
            _ => throw new ArgumentOutOfRangeException(nameof(database), database, null)
        };
    }
}
