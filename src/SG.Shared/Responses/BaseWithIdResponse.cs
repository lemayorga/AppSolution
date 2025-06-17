
namespace SG.Shared.Responses;

public interface IBaseWithIdResponse<Tkey>
{
    Tkey Id { get; set; }
}

public abstract class BaseWithIdResponse : IBaseWithIdResponse<int>
{
    public int Id { get; set; }
};
