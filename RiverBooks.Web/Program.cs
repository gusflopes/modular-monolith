using FastEndpoints;
using RiverBooks.Books;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddFastEndpoints();

// Module Services
builder.Services.AddBookServices(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Module Endpoints
app.UseFastEndpoints();

app.Run();

public partial class Program { } // needed for tests
