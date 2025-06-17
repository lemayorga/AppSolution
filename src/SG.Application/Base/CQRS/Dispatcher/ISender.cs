namespace SG.Application.Base.CQRS.Dispatcher;

public interface ISender
{
  // Task<TResponse> SendCommand<TResponse>(IRequest<TResponse> command);
  // Task<TResponse> SendQuery<TResponse>(IRequest<TResponse> query);

  Task Send(IRequest command);
  Task<TResponse> Send<TResponse>(IRequest<TResponse> command);
}