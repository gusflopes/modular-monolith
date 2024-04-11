using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace RiverBooks.Books;

public static class BookServiceExtensions
{
    public static IServiceCollection AddBookServices(
      this IServiceCollection services,
      ConfigurationManager config,
      ILogger logger)
    {
      string? connectionString = config.GetConnectionString("DefaultConnection");
      services.AddDbContext<BookDbContext>(options =>
        options.UseSqlServer(connectionString));
        services.AddScoped<IBookRepository, EfBookRepository>();
        services.AddScoped<IBookService, BookService>();
        
        logger.Information("{Module} module services registered", "Books");
        
        return services;
    }
}
