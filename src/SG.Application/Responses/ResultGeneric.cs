namespace SG.Application.Responses;

public class ResultGeneric<T>
{
    public T Data  { get; set; }
    public bool IsSuccess  { get; set; }
    public string Message  { get; set; }
    public object? Error  { get; set; }
    public ResultGeneric()
    {
        Data = default(T)!;
        IsSuccess = true;
        Message = string.Empty;
    }

    public ResultGeneric(T data)
    {
        IsSuccess = true;
        Message = string.Empty;
        Data = data;
    }
    private ResultGeneric(T data, bool isSuccess, string message = "")
    {
        Data = data;
        IsSuccess = isSuccess;
        Message = message;
    }
    private ResultGeneric(bool isSuccess, string message = "")
    {
        Data = default(T)!;
        IsSuccess = isSuccess;
        Message = message;
    }

    private ResultGeneric(bool isSuccess, object error)
    {
        Data = default(T)!;
        IsSuccess = isSuccess;
        Error = error;
        Message = string.Empty;
    }    

    public static ResultGeneric<T> Ok(T data)
    {
        return new ResultGeneric<T>(data, true);
    }

    public static ResultGeneric<T> Failure(string errorMessage)
    {
        return new ResultGeneric<T>(false, errorMessage);
    }
    public static ResultGeneric<T> Failure(object error)
    {
        return new ResultGeneric<T>(false, error);
    }    
}