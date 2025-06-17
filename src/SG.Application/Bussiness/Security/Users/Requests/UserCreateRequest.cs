namespace SG.Application.Bussiness.Security.Users.Requests;

public class UserCreateRequest : UserBaseRequest
{
    public required string Password { get; set; }   
    public bool IsActive { get => true; }
    public bool IsLocked { get => false; }  
}
