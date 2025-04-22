namespace SG.Shared.Settings;

public sealed class DataApplicationSeedersSettings
{
    public bool Execute { get; set; }
    public List<AppSettingRoles> Roles { get; set; } = new();
    public List<AppSettingUsers> Users { get; set; } = new();
    public Dictionary<string, bool>? FilesExecute { get; set; }

 
}
public sealed class AppSettingRoles
{
    public required string Code { get; set; }
    public required string Name { get; set; }
}

public sealed class AppSettingUsers
{
    public required string UserName { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
    public required string CodeRol { get; set; }
    public string? Firstname { get; set; }
    public string? Lastname { get; set; }
} 