using SG.Domain.Base;

namespace SG.Domain.Entities.Commun;

public class Catalogue : BaseEntity<int>
{
    public string Group { get; set; } = default!;
    public string Value { get; set; } = default!;
    public bool IsActive { get; set; }
    public string? Description { get; set; }

    public Catalogue() { }

    public Catalogue(string group, string value, bool isActive, string? description): this
    (
        id: 0,
        group: group,
        value: value,
        isActive: isActive,
        description: description
    ) { }

    public Catalogue(int id, string group, string value, bool isActive, string? description)
    {
        Id = id;
        Group = group;
        Value = value;
        IsActive = isActive;
        Description = description;
    }
}
