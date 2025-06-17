
namespace SG.Application.Bussiness.Security.Auth.Requests;

public class LoginRequest
{
    public required string UserName { get; set; }
    public required string Password { get; set; }

    public bool? EvaluateEmail { get; set; }

    public void Deconstruct(out string userName, out string password)
    {
        userName = UserName.Trim();
        password = Password.Trim();
    }
}
