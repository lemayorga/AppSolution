using SG.Shared.Responses;

namespace SG.Shared.Settings;

public sealed class DataApplicationSeedersSettings
{
    public bool Execute { get; set; }
    public List<AppSettingRoles> Roles { get; set; } = new();
    public List<AppSettingUsers> Users { get; set; } = new();
    public List<GeneralCodesValues> PasswordPolicies { get; set; } = new();
    public List<AppSettingCatalogue> Catalogues { get; set; } = new();

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



public sealed class AppSettingCatalogue 
{
    public string Value { get; set; } = default!;
    public string Code { get; set; } = default!;
    public string? Description { get; set; }
    public int Orden { get; set; }
    public List<AppSettingCatalogueItem> Children { get; set; } = [];
}
public  class AppSettingCatalogueItem
{
    public string Value { get; set; } = default!;
    public string Code { get; set; } = default!;
    public string? Description { get; set; }
    public int Orden { get; set; }
}