using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
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
    // configure MongoDB
    services.Configure<MongoDbSettings>(config.GetSection("MongoDB"));
    services.AddMongoDb(config);

    // Add Module services
    services.AddTransient<ISendEmail, MimeKitEmailSender>();
    services.AddTransient<IOutboxService, MongoDbOutboxService>();

    mediatRAssemblies.Add(typeof(EmailSendingModuleServiceExtensions).Assembly);

    logger.Information("{Module} module services registered", "Email Sending");

    return services;
  }

  public static IServiceCollection AddMongoDb(this IServiceCollection services, IConfiguration configuration)
  {
    // Register the MongoDB client as a singleton
    services.AddSingleton<IMongoClient>(serviceProvider =>
    {
      var settings = configuration.GetSection("MongoDB").Get<MongoDbSettings>();
      return new MongoClient(settings!.ConnectionString);
    });

    // Register the MongoDB database as a singleton
    services.AddSingleton(serviceProvider =>
    {
      var settings = configuration.GetSection("MongoDB").Get<MongoDbSettings>();
      var client = serviceProvider.GetService<IMongoClient>();
      return client!.GetDatabase(settings!.DatabaseName);
    });

    //// Optionally, registerr specific collections here as scoped or singleton services
    /// Example for a 'EmailOutboxEntity' collection
    services.AddTransient(serviceProvider =>
    {
      var database = serviceProvider.GetService<IMongoDatabase>();
      return database!.GetCollection<EmailOutboxEntity>("EmailOutboxEntityCollection");
    });

    return services;
  }
}
