
namespace SG.Infrastructure.Data.Seeders;

public partial class SeederExecute
{
    internal class DataApplication
    {
        public bool Execute { get; set; }
        public List<AppSettingRoles> Roles { get; set; } = new();
        public List<AppSettingUsers> Users { get; set; } = new();
        public Dictionary<string, bool>? FilesExecute { get; set; }
    }
}
