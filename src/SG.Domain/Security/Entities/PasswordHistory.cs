using SG.Domain;

namespace SG.Domain.Security.Entities;

public class PasswordHistory  : Entity
{
    public required string Username { get; set; }
    public required string OldPassword { get; set; }
    public required DateOnly DateChange { get; set; }
    public required string UserSave{ get; set; }            
}
