
namespace SG.Application.Bussiness.Security.Users.Requests;

public abstract class UserBaseRequest
{
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Firstname { get; set; } = string.Empty;
    public string Lastname { get; set; } = string.Empty;    
}