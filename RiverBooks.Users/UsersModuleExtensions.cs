using System.Reflection;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using RiverBooks.Users.Data;
using RiverBooks.Users.Integrations;
using Serilog;

namespace RiverBooks.Users;

public static class UsersModuleExtensions
{
  public static IServiceCollection AddUserModuleServices(
    this IServiceCollection services,
    ConfigurationManager config,
    ILogger logger,
    List<Assembly> mediatRAssemblies)
  {
    string? connectionString = config.GetConnectionString("DefaultConnection");
    
    services.AddDbContext<UsersDbContext>(options =>
      options.UseSqlServer(connectionString));
    
    services.AddIdentityCore<ApplicationUser>()
      .AddEntityFrameworkStores<UsersDbContext>();

    services.AddScoped<IApplicationUserRepository, EfApplicationUserRepository>();
    services.AddScoped<IReadOnlyUserStreetAddressRepository, EfUserStreetAddressRepository>();
    
    // Add MediatR Domain Event Dispatcher
    services.AddScoped<IDomainEventDispatcher, MediatRDomainEventDispatcher>();
    
    // if using MediatR in this module, add any assemblies that contain handlers to the list
    mediatRAssemblies.Add(typeof(UsersModuleExtensions).Assembly);
    
    logger.Information("{Module} module services registered", "Users");

    return services;
  }
}
