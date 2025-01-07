
namespace SG.Infrastructure.Data.Seeders;

public partial class SeederExecute
{
    internal class AppSettingUsers
    {
        public required string UserName { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required string CodeRol { get; set; }
        public string? Firstname { get; set; }
        public string? Lastname { get; set; }
    }  
}
