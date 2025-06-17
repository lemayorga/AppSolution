using SG.Domain.Entities.Security;
using SG.Shared.Responses;

namespace SG.Application.Bussiness.Security.Users.Responses;

public class UserResponse : BaseWithIdResponse
{
    public string Username { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string Firstname { get; set; } = default!;
    public string Lastname { get; set; } = default!;
    public bool IsActive { get; set; }
    public bool IsLocked { get; set; }

    public UserResponse(){}
    public UserResponse(int id, string username, string email, string firstname, string lastname, bool isActive, bool isLocked)
    {
        Id = id;
        Username = username;
        Email = email;
        Firstname = firstname;
        Lastname = lastname;
        IsActive = isActive;
        IsLocked = isLocked;
    }

    public UserResponse(User model)
    {
        Id = model.Id;
        Username = model.Username;
        Email = model.Email;
        Firstname = model.Firstname;
        Lastname = model.Lastname;
        IsActive = model.IsActive;
        IsLocked = model.IsLocked;
    }
}
