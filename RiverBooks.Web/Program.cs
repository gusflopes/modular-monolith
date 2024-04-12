using System.Reflection;
using FastEndpoints;
using FastEndpoints.Security;
using FastEndpoints.Swagger;
using RiverBooks.Books;
using RiverBooks.Users;
using Serilog;

var logger = Log.Logger = new LoggerConfiguration()
  .Enrich.FromLogContext()
  .WriteTo.Console()
  .CreateLogger();

logger.Information("Starting web host");

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((_, config) =>
  config.ReadFrom.Configuration(builder.Configuration));

builder.Services.AddFastEndpoints()
  .AddAuthenticationJwtBearer(o =>
  {
    o.SigningKey = builder.Configuration["Auth:JwtSecret"]!;
  })
  .AddAuthorization()
  .SwaggerDocument();

// Module Services
List<Assembly> mediatRAssmblies = [typeof(Program).Assembly];
// MediatR added only when needed
builder.Services.AddBookServices(builder.Configuration, logger, mediatRAssmblies);
builder.Services.AddUsersModuleServices(builder.Configuration, logger, mediatRAssmblies);

// Set up MediatR
builder.Services.AddMediatR(cfg =>
  cfg.RegisterServicesFromAssemblies(mediatRAssmblies.ToArray()));

var app = builder.Build();

// if (app.Environment.IsDevelopment())
// {
//     app.UseSwagger();
//     app.UseSwaggerUI();
// }

app.UseAuthentication()
  .UseAuthorization();

// app.UseHttpsRedirection();

// Module Endpoints
app.UseFastEndpoints()
  .UseSwaggerGen();

app.Run();

public partial class Program { } // needed for tests
