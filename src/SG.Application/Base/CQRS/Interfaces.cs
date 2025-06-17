
namespace SG.Application.Base.CQRS;

// Command interface & Query interface

public interface IRequest;
public interface IRequest<TResponse>;



// Command handler &  Query handler
public interface IRequestHandler<in TCommand> where TCommand : IRequest
{
    Task Handle(TCommand command);
}

public interface IRequestHandler<in TCommand, TResponse> where TCommand : IRequest<TResponse>
{
    Task<TResponse> Handle(TCommand command);
}
