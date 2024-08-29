namespace SG.Application;

public class ResultGeneric<T>
{
    public T Data { get; }
    public bool IsSuccess { get; }
    public string ErrorMessage { get; }

    private ResultGeneric(T data, bool isSuccess, string errorMessage = "")
    {
        Data = data;
        IsSuccess = isSuccess;
        ErrorMessage = errorMessage;
    }
    private ResultGeneric(bool isSuccess, string errorMessage = "")
    {
        Data = default(T)!;
        IsSuccess = isSuccess;
        ErrorMessage = errorMessage;
    }

    public static ResultGeneric<T> Ok(T data)
    {
        return new ResultGeneric<T>(data, true);
    }

    public static ResultGeneric<T> Failure(string errorMessage)
    {
        return new ResultGeneric<T>(false, errorMessage);
    }
}
