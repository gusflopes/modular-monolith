using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Reflection;
using RiverBooks.OrderProcessing.Domain;
using Serilog.Core;

namespace RiverBooks.OrderProcessing.Data;

internal class OrderProcessingDbContext : IdentityDbContext
{
  public OrderProcessingDbContext(DbContextOptions<OrderProcessingDbContext> options)
    : base(options)
  {
  }

  public DbSet<Order> Orders { get; set; }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.HasDefaultSchema("OrderProcessing");

    modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

    base.OnModelCreating(modelBuilder);
  }

  protected override void ConfigureConventions(
    ModelConfigurationBuilder configurationBuilder)
  {
    configurationBuilder.Properties<decimal>()
      .HavePrecision(18, 6);
  }
}
