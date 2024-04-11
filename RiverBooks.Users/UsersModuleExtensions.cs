using System.Reflection;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace RiverBooks.Users;

public static class UsersModuleExtensions
{
  public static IServiceCollection AddUsersModuleServices(
    this IServiceCollection services,
    ConfigurationManager config,
    ILogger logger)
  {
    string? connectionString = config.GetConnectionString("DefaultConnection");
    
    services.AddDbContext<UsersDbContext>(options =>
      options.UseSqlServer(connectionString));
    
    services.AddIdentityCore<ApplicationUser>()
      .AddEntityFrameworkStores<UsersDbContext>();
    
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
