using SG.Domain.Base;

namespace SG.Domain.Commun.Entities;

public class Catalogue : BaseEntity<int>
{
    public string Group { get; set; } = default!;
    public string Value { get; set; } = default!;
    public bool IsActive { get; set; }
    public string? Description { get; set; }

    public Catalogue() { }

    public Catalogue(string group, string value, bool isActive, string? description)
    {
        Group = group;
        Value = value;
        IsActive = isActive;
        Description = description;
    }
}
