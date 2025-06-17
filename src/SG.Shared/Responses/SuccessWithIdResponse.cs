
namespace SG.Shared.Responses;

public class SuccessWithIdResponse : BaseWithIdResponse
{
    public SuccessWithIdResponse(int id)
    {
        Id = id;
    }
}


public class SuccessResponse
{
    public bool IsSuccess { get => true; }

    public SuccessResponse(){}
}
