using SG.Domain;

namespace SG.Domain.Commun.Entities;

public class Catalogue : Entity
{
    public required string Group  { get; set; }
    public required string Value  { get; set; }
    public bool IsActive { get; set; }
    public string Description { get; set; } = string.Empty;
}
