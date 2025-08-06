
namespace SG.Application.Bussiness.Security.Users.Requests;

public class UserUpdateRequest : UserBaseRequest
{
    public bool IsActive { get; set; }
    public bool IsLocked { get; set; }
}