using System.Reflection;
using FastEndpoints;
using FastEndpoints.Security;
using FastEndpoints.Swagger;
using RiverBooks.Books;
using RiverBooks.OrderProcessing;
using RiverBooks.SharedKernel;
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
List<Assembly> mediatRAssemblies = [typeof(Program).Assembly];
// MediatR added only when needed
builder.Services.AddBookModuleServices(builder.Configuration, logger, mediatRAssemblies);
builder.Services.AddUserModuleServices(builder.Configuration, logger, mediatRAssemblies);
builder.Services.AddOrderProcessingModuleServices(builder.Configuration, logger, mediatRAssemblies);

// Set up MediatR
builder.Services.AddMediatR(cfg =>
  cfg.RegisterServicesFromAssemblies(mediatRAssemblies.ToArray()));
builder.Services.AddScoped<IDomainEventDispatcher, MediatRDomainEventDispatcher>();

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
