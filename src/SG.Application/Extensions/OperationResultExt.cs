using FluentResults;
using SG.Application.Responses;

namespace SG.Application.Extensions;

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


