using System.Reflection;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using RiverBooks.OrderProcessing.Infrastructure;
using RiverBooks.OrderProcessing.Infrastructure.Data;
using RiverBooks.OrderProcessing.Integrations;
using Serilog;

namespace RiverBooks.OrderProcessing;

public static class OrderProcessingModuleExtensions
{
  public static IServiceCollection AddOrderProcessingModuleServices(
    this IServiceCollection services,
    ConfigurationManager config,
    ILogger logger,
    List<Assembly> mediatRAssemblies)
  {
    string? connectionString = config.GetConnectionString("DefaultConnection");
    
    services.AddDbContext<OrderProcessingDbContext>(options =>
      options.UseSqlServer(connectionString));
    
    services.AddScoped<IOrderRepository, EfOrderRepository>();
    
    // Added the concrete implementation of the cache, and then the interface on the decorator
    services.AddScoped<RedisOrderAddressCache>();
    services.AddScoped<IOrderAddressCache, ReadThroughOrderAddressCache>();
    
    // if using MediatR in this module, add any assemblies that contain handlers to the list
    mediatRAssemblies.Add(typeof(OrderProcessingModuleExtensions).Assembly);
    
    logger.Information("{Module} module services registered", "OrderProcessing");

    return services;
  }
}
