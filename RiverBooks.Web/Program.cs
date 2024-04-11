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

// builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen();
builder.Services.AddFastEndpoints()
  .AddAuthenticationJwtBearer(o =>
  {
    o.SigningKey = builder.Configuration["Auth:JwtSecret"]!;
  })
  .AddAuthorization()
  .SwaggerDocument();

// Module Services
builder.Services.AddBookServices(builder.Configuration, logger);
builder.Services.AddUsersModuleServices(builder.Configuration, logger);

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
