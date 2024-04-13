using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace RiverBooks.Books;

public static class BookServiceExtensions
{
    public static IServiceCollection AddBookModuleServices(this IServiceCollection services,
      ConfigurationManager config,
      ILogger logger, List<Assembly> mediatRAssmblies)
    {
      string? connectionString = config.GetConnectionString("DefaultConnection");
      services.AddDbContext<BookDbContext>(options =>
        options.UseSqlServer(connectionString));
        services.AddScoped<IBookRepository, EfBookRepository>();
        services.AddScoped<IBookService, BookService>();
        
        // if using MediatR in this module, add any assemblies that contain handlers to the list
        mediatRAssmblies.Add(typeof(BookServiceExtensions).Assembly);
        
        logger.Information("{Module} module services registered", "Books");
        
        return services;
    }
}
