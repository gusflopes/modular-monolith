using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Reflection;

namespace RiverBooks.Users.Data;

public class UsersDbContext : IdentityDbContext
{
  private readonly IDomainEventDispatcher? _dispatcher;

  public UsersDbContext(
    DbContextOptions<UsersDbContext> options,
    IDomainEventDispatcher? dispatcher)
    : base(options)
  {
    _dispatcher = dispatcher;
  }

  public DbSet<ApplicationUser> ApplicationUsers { get; set; }
  public DbSet<UserStreetAddress> UserStreetAddresses { get; set; }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.HasDefaultSchema("Users");

    modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

    base.OnModelCreating(modelBuilder);
  }

  protected override void ConfigureConventions(
    ModelConfigurationBuilder configurationBuilder)
  {
    configurationBuilder.Properties<decimal>()
      .HavePrecision(18, 6);
  }

  /// <summary>
  /// Dispatches domain events to MediatR after SaveChanges is called.
  /// </summary>
  /// <param name="cancellationToken"></param>
  /// <returns></returns>
  public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
  {
    int result = await base.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    
    // ignore events if no dispatcher provided
    if (_dispatcher == null) return result;
    
    // dispatch events only if save was successful
    var entitiesWithEvents = ChangeTracker.Entries<IHaveDomainEvents>()
      .Select(e => e.Entity)
      .Where(e => e.DomainEvents.Any())
      .ToArray();

    await _dispatcher.DispatchAndClearEvents(entitiesWithEvents);

    return result;
  }
}
