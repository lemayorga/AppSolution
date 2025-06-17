using System;
using SG.Application.Base.CQRS;

namespace SG.Application.Base.Validations;

public interface IPipelineBehavior<in TInput, TOutput>
{
    Task<TOutput> HandleAsync(TInput input, Func<Task<TOutput>> next);
}

public interface IPipelineBehavior<in TInput>
{
        Task HandleAsync(TInput input, Func<Task> next);
}