
namespace SG.Shared.Settings;

public sealed class AppSettings
{
    public bool EnableLoggingSerilog { get; set; }
    public bool EnableLoggingEntityFrameworkCore { get; set; }
    public bool EnableSensitiveDataLoggingEntityFrameworkCore { get; set; }
    public bool EnableLoggingGlobalExceptionHandler { get; set; }
}