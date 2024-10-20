using System;
using FluentResults;

namespace SG.Application.Responses;

public class OperationResult<T>
{
    public bool IsSuccess  { get; set; }
    public bool IsFailed { get; set; }

    public List<IError>? Errors { get; set; }
 
    public T? Value { get; set; }
}
 

public static class OperationResultExt
{
    public static OperationResult<T> ToOperationResult<T>(this Result<T> response)
    {
         var (isSuccess, isFailed, value )  = response; 

        return new OperationResult<T>()
        {
            IsSuccess =  isSuccess,
            IsFailed = isFailed,
            Errors = response.Errors,
            Value = value
        };
    }
}


