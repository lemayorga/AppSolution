
namespace SG.Application.Base.Responses;

public class OperationResult<T>
{
    public bool IsSuccess  { get; set; }
    public bool IsFailed { get; set; }

    public string[]? Errors { get; set; }
 
    public T? Value { get; set; }
}
 
