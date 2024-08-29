namespace SG.Application.Bussiness.Commun.Dtos;

public class CatalogueDto
{
    public required string Group  { get; set; }
    public required string Value  { get; set; }
    public bool IsActive { get; set; }
    public string? Description { get; set; }
}
