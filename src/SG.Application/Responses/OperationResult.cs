using System;
using FluentResults;

namespace SG.Application.Responses;

public class OperationResult<T>
{
    public bool IsSuccess  { get; set; }
    public bool IsFailed { get; set; }

    public string[]? Errors { get; set; }
 
    public T? Value { get; set; }
}
 

public static class OperationResultExt
{
    public static OperationResult<T> ToOperationResult<T>(this Result<T> response)
    {
        var (isSuccess, isFailed, value, errors)  = response; 
        return new OperationResult<T>()
        {
            IsSuccess =  isSuccess,
            IsFailed = isFailed,
            Errors = errors?.Select(x => x.Message)?.ToArray(),
            Value = value
        };
    }
}


