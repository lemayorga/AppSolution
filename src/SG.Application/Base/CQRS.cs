using FluentResults;

namespace SG.Application.Base;

// Base contracts
public interface ICommand;
public interface ICommand<TResponse>;
public interface IQuery<TRespons>;

public interface ICommandHanlder<in TCommand> 
                              where TCommand : ICommand
{
    Task Handle(TCommand command, CancellationToken cancellationToken);
}

public interface ICommandHanlder<in TCommand, TResponse> 
                              where TCommand : ICommand<TResponse>
{
    Task<Result<TResponse>> Handle(TCommand command, CancellationToken cancellationToken);
}

public interface IQueryHanlder<in TQuery, TResponse> 
                             where TQuery : IQuery<TResponse>
{
    Task<Result<TResponse>> Handle(TQuery command, CancellationToken cancellationToken);
}

