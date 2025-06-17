namespace SG.Application.Bussiness.Security.Users.Requests;

public class UserResetPasswordRequest
{
    public required string UserName { get; set; }

    public bool? EvaluateEmail { get; set; }
}
