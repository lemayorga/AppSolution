
namespace SG.Application.Bussiness.Security.Auth.Requests;

public class LoginRequest
{
    public string UserName { get; set; } = default!;
    public  string Password { get; set; }  = default!;

    public bool? EvaluateEmail { get; set; }

    public LoginRequest(string userName, string password, bool? evaluateEmail = null)
    {
        UserName = userName.Trim();
        Password = password.Trim();
        EvaluateEmail = evaluateEmail;
    }

    public void Deconstruct(out string userName, out string password)
    {
        userName = UserName.Trim();
        password = Password.Trim();
    }
}
