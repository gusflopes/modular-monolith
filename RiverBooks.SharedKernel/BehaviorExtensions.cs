using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace RiverBooks.SharedKernel;

public static class BehaviorExtensions
{
  public static IServiceCollection AddMediatRLoggingBehavior(this IServiceCollection services)
  {
    services.AddScoped(typeof(IPipelineBehavior<,>),
      typeof(LoggingBehavior<,>));
    
    return services;
  }
}
