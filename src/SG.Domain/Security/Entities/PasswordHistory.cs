using SG.Domain;
using SG.Domain.Base;

namespace SG.Domain.Security.Entities;

public class PasswordHistory : BaseEntity<int>
{
    public string Username { get; set; }  = default!;
    public string OldPassword { get; set; }  = default!;
    public DateOnly DateChange { get; set; } 
    public string UserSave{ get; set; } = default!;  
} 
