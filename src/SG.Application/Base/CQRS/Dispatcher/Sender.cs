using Microsoft.Extensions.DependencyInjection;
using SG.Application.Base.Validations;

namespace SG.Application.Base.CQRS.Dispatcher;

public class Sender(IServiceProvider serviceProvider) : ISender
{
  private readonly IServiceProvider _serviceProvider = serviceProvider;

  IServiceScope CreateScope(IServiceProvider provider)
    => provider.GetRequiredService<IServiceScopeFactory>().CreateScope();


  public async Task Send(IRequest command)
  {
    var handlerType = typeof(IRequestHandler<>).MakeGenericType(command.GetType());
    var serviceProvider = CreateScope(_serviceProvider);
    dynamic handler = serviceProvider.ServiceProvider.GetRequiredService(handlerType);
    await handler.Handle((dynamic)command);
  }

  public async Task<TResponse> Send<TResponse>(IRequest<TResponse> command)
  {
    var handlerType = typeof(IRequestHandler<,>).MakeGenericType(command.GetType(), typeof(TResponse));
    var serviceProvider = CreateScope(_serviceProvider);
    dynamic handler = serviceProvider.ServiceProvider.GetRequiredService(handlerType);
    return await handler.Handle((dynamic)command);
  }

  // public async Task Send(IRequest command)
  // {
  //   var handlerType = typeof(IRequestHandler<>).MakeGenericType(command.GetType());
  //   var serviceProvider = CreateScope(_serviceProvider);
  //   dynamic handler = serviceProvider.ServiceProvider.GetRequiredService(handlerType);
  //   // await handler.Handle((dynamic)command);

  //   var behaviors = serviceProvider.ServiceProvider.GetServices<IPipelineBehavior<IRequest>>().Reverse();
  //   Func<Task> handlerDelegate = () => handler.Handle((dynamic)command);
  //   foreach (var behavior in behaviors)
  //   {
  //     var next = handlerDelegate;
  //     handlerDelegate = () => behavior.HandleAsync(command, next);
  //   }

  //   await handlerDelegate();
  // }

  // public async Task<TResponse> Send<TResponse>(IRequest<TResponse> command)
  // {
  //   var handlerType = typeof(IRequestHandler<,>).MakeGenericType(command.GetType(), typeof(TResponse));
  //   var serviceProvider = CreateScope(_serviceProvider);
  //   dynamic handler = serviceProvider.ServiceProvider.GetRequiredService(handlerType);

  //   var behaviors = serviceProvider.ServiceProvider.GetServices<IPipelineBehavior<IRequest<TResponse>, TResponse>>().Reverse();

  //   Func<Task<TResponse>> handlerDelegate = () => handler.Handle((dynamic)command);
  //   foreach (var behavior in behaviors)
  //   {
  //     var next = handlerDelegate;
  //     handlerDelegate = () => behavior.HandleAsync(command, next);
  //   }

  //   return await handlerDelegate();
  // }

}

// https://medium.com/@paveluzunov/the-easiest-way-to-replace-mediatr-cb6a0fa07ded 
// 
// Artiuclo sin mediator para validationes

// https://www.netmentor.es/entrada/introduccion-fluent-validations