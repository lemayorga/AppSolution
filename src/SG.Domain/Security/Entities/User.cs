using SG.Domain.Base;

namespace SG.Domain.Security.Entities;

public class User  : BaseEntity<int>
{
    public string Username { get; set; }  = default!;
    public string Email { get; set; }  = default!;
    public string Firstname { get; set; }  = default!;
    public string Lastname { get; set; }   = default!;
    public string Password { get; set; }       = default!;       
    public bool IsActive { get; set; }
    public bool IsLocked { get; set; }
    public ICollection<UsersRoles>? UsersRoles { get; set; }
    public UsersToken? UserToken { get; set; }
}

