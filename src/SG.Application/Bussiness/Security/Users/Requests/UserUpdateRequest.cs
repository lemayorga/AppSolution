using SG.Shared.Request;

namespace SG.Application.Bussiness.Security.Users.Requests;

public class UserUpdateRequest : UserBaseRequest, IBaseWithIdRequest<int>
{
    public int Id { get; set; }
    public bool IsActive { get; set; }
    public bool IsLocked { get; set; }
}