using DealershipDotNetAPI.Domain.DTOs;
using DealershipDotNetAPI.Domain.Interfaces;
using DealershipDotNetAPI.Domain.Services;
using DealershipDotNetAPI.Infrastructure.Db;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IAdministratorService, AdministratorService>();

// Add services to the container.
// This adds controller support for handling HTTP requests and responses.
builder.Services.AddControllers();

// Add support for API documentation using Swagger/OpenAPI.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure the database context with MySQL.
// The connection string is retrieved from the application configuration.
builder.Services.AddDbContext<ContextDb>(options =>
{
    options.UseMySql(
            builder.Configuration.GetConnectionString("mysql"),
            ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("mysql"))
            );
});

var app = builder.Build();

// Configure the HTTP request pipeline for development environment.
if (app.Environment.IsDevelopment())
{
    // Enable Swagger UI for API testing and documentation in development.
    app.UseSwagger();
    app.UseSwaggerUI();
}


// Redirect HTTP requests to HTTPS.
app.UseHttpsRedirection();

// Configure the app to use authorization middleware.
app.UseAuthorization();

// Map controllers to handle requests based on routes defined in the controllers.
app.MapControllers();

// Define a simple GET endpoint at the root ('/').
// Returns "Hello Word!" when accessed.
app.MapGet("/", () => "Hello Word!");

// Define a POST endpoint for login functionality.
// Accepts a LoginDTO object and verifies the email and password for authentication.
app.MapPost("/login", ([FromBody] LoginDTO loginDTO, IAdministratorService administratorService) =>
{
    if (administratorService.Login(loginDTO) != null)
    {
        return Results.Ok("Authorized user");

    }
    else { return Results.Unauthorized(); }
});


// Start the application and listen for incoming requests.
app.Run();



