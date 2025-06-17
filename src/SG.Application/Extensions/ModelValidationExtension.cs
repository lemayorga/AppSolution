using System;
using FluentResults;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace SG.Application.Extensions;

//https://ravindradevrani.medium.com/fluent-validation-in-net-core-8-0-c748da274204
public static class ModelValidationExtension
{
    public static void AddToModelState(this ValidationResult result, ModelStateDictionary modelState)
    {
        foreach (var error in result.Errors)
        {
            modelState.AddModelError(error.PropertyName, error.ErrorMessage);
        }
    }
    
    public static Result<T> GetResultErrors<T>(this ValidationResult validator) where T: class 
    {
        var errors = new List<string>();
        foreach (var failure in validator.Errors)
        {
            errors.Add($"Validation failed for {failure.PropertyName} " +
                       $"with the error: {failure.ErrorMessage}");
        }

        return Result.Fail<T>(errors); 
    }
}
