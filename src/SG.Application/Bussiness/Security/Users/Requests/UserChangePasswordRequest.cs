namespace SG.Application.Bussiness.Security.Users.Requests;

public class UserChangePasswordRequest
{
    public required string UserName { get; set; }
    public required string CurrentPassword { get; set; }
    public required string NewPassword { get; set; }

    public bool? EvaluateEmail { get; set; }

    public void Deconstruct(out string userName, out string currentPassword, out string newPassword)
    {
        userName = UserName.Trim();
        currentPassword = CurrentPassword.Trim();
        newPassword = NewPassword.Trim();
    }
}