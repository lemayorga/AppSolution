namespace SG.Application.Bussiness.Commun.Dtos;

public class CatalogueDto : BaseDto
{
    public required string Group  { get; set; }
    public required string Value  { get; set; }
    public bool IsActive { get; set; }
    public string? Description { get; set; }
}

public class CatalogueCreateDto 
{
    public required string Group  { get; set; }
    public required string Value  { get; set; }
    public string? Description { get; set; }
}

public class CatalogueUpdateDto  : CatalogueCreateDto
{
   public bool IsActive { get; set; }
}
