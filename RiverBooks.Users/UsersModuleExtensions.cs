using System.Reflection;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace RiverBooks.Users;

public static class UsersModuleExtensions
{
  public static IServiceCollection AddUsersModuleServices(this IServiceCollection services,
    ConfigurationManager config,
    ILogger logger,
    List<Assembly> mediatRAssmblies)
  {
    string? connectionString = config.GetConnectionString("DefaultConnection");
    
    services.AddDbContext<UsersDbContext>(options =>
      options.UseSqlServer(connectionString));
    
    services.AddIdentityCore<ApplicationUser>()
      .AddEntityFrameworkStores<UsersDbContext>();
    
    // if using MediatR in this module, add any assemblies that contain handlers to the list
    mediatRAssmblies.Add(typeof(UsersModuleExtensions).Assembly);
    
    logger.Information("{Module} module services registered", "Users");

    return services;
  }
}

public class UsersDbContext : IdentityDbContext
{
  public UsersDbContext(DbContextOptions<UsersDbContext> options): base(options)
  {
  }
  
  public DbSet<ApplicationUser> ApplicationUsers { get; set; } = null!;

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.HasDefaultSchema("Users");

    modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    
    base.OnModelCreating(modelBuilder);
  }

  protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
  {
    configurationBuilder.Properties<decimal>()
      .HavePrecision(18, 6);
  }
}
