using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace RiverBooks.EmailSending;

public static class EmailSendingModuleServiceExtensions
{
  public static IServiceCollection AddEmailSendingModuleServices(
      this IServiceCollection services,
      ConfigurationManager config,
      ILogger logger,
      List<System.Reflection.Assembly> mediatRAssemblies)
    {
      // Add Module services
      services.AddTransient<ISendEmail, MimeKitEmailSender>();
      
      mediatRAssemblies.Add(typeof(EmailSendingModuleServiceExtensions).Assembly);
      
      logger.Information("{Module} module services registered", "Email Sending");
      
      return services;
    }
}
