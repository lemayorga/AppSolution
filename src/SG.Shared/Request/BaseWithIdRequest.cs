namespace SG.Shared.Request;

public interface IBaseWithIdRequest<Tkey>
{
    Tkey Id { get; set; }
}

public abstract class BaseWithIdRequest : IBaseWithIdRequest<int>
{
    public int Id { get; set; }
    public BaseWithIdRequest() { }

    public BaseWithIdRequest(int id)
    {
        Id = id;
    }
}
