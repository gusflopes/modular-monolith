﻿using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace RiverBooks.Books;

public class BookDbContext : DbContext
{
  internal DbSet<Book> Books { get; set; }

  public BookDbContext(DbContextOptions<BookDbContext> options) : base(options)
  {
  }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.HasDefaultSchema("Books");
    modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
  }
  
  protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
  {
    configurationBuilder.Properties<decimal>()
      .HavePrecision(18,6);
  }
}
