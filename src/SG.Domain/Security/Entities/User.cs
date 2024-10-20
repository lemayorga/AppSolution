using SG.Domain;

namespace SG.Domain.Security.Entities;

public class User  : Entity
{
    public required string Username { get; set; }
    public required string Email { get; set; }
    public required string Firstname { get; set; }
    public required string Lastname { get; set; } 
    public required string Password { get; set; }            
    public bool IsActive { get; set; }
    public bool IsLocked { get; set; }
}

