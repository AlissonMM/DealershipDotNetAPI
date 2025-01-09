using DealershipDotNetAPI.Domain.DTOs;
using DealershipDotNetAPI.Domain.Entities;
using DealershipDotNetAPI.Domain.Interfaces;
using DealershipDotNetAPI.Domain.ModelViews;
using DealershipDotNetAPI.Domain.Services;
using DealershipDotNetAPI.Infrastructure.Db;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Model;

#region builder

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IAdministratorService, AdministratorService>();
builder.Services.AddScoped<IVehicleService, VehicleService>();

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
#endregion

#region app
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

#endregion

// Define a simple GET endpoint at the root ('/').
// Returns "Hello Word!" when accessed.

#region home
app.MapGet("/", () => Results.Json(new Home())).WithTags("Home");
#endregion

// Define a POST endpoint for login functionality.
// Accepts a LoginDTO object and verifies the email and password for authentication.
#region administrators
ValidationErrors verifyAdministratorDTO(AdministratorDTO administratorDTO)
{

    var validation = new ValidationErrors
    {
        Messages = new List<string>()
    };


    if (string.IsNullOrEmpty(administratorDTO.Email))
    {
        validation.Messages.Add("The Email must not be empty!");
    }

    if (string.IsNullOrEmpty(administratorDTO.Password))
    {
        validation.Messages.Add("The Password must not be empty!");
    }

    if (string.IsNullOrEmpty(administratorDTO.Profile))
    {
        validation.Messages.Add("The Profile must not be empty!");
    }

    return validation;
}

app.MapPost("/login", ([FromBody] LoginDTO loginDTO, IAdministratorService administratorService) =>
{

    if (administratorService.Login(loginDTO) != null)
    {
        return Results.Ok("Authorized user");

    }
    else { return Results.Unauthorized(); }
}).WithTags("Administrator");


app.MapPost("/administrator", ([FromBody] AdministratorDTO administratorDTO, IAdministratorService administratorService) =>
{
    var validation = verifyAdministratorDTO(administratorDTO);

    if (validation.Messages.Count > 0)
    {
        return Results.BadRequest(validation);
    }

    var administrator = new Administrator
    {
        Email = administratorDTO.Email,
        Password = administratorDTO.Password,
        Profile = administratorDTO.Profile,
    };

    Administrator administratorCreated = administratorService.AddAdministrator(administrator);

    if (administratorCreated != null)
    {
        return Results.Created($"/administrator/{administrator.Id}", administrator);
    }

    else
    {
        return Results.BadRequest();
    }
     
    
   
}).WithTags("Administrator");


app.MapPut("/administrator/{id}", ([FromRoute] int id, AdministratorDTO administratorDTO, IAdministratorService administratorService) =>
{

    Administrator administratorFound = administratorService.GetAdministratorById(id);

    if (administratorFound != null)
    {
        var validation = verifyAdministratorDTO(administratorDTO);

        if (validation.Messages.Count > 0)
        {
            return Results.BadRequest(validation);
        }


        administratorFound.Email = administratorDTO.Email;
        administratorFound.Password = administratorDTO.Password;
        administratorFound.Profile = administratorDTO.Profile;


        administratorService.UpdateAdministrator(administratorFound);
        return Results.Created($"/administrator/{administratorFound.Id}", administratorFound);
    }

    else
    {
        return Results.BadRequest();
    }



}).WithTags("Administrator");


app.MapGet("/administrator/{id}", ([FromQuery] int id, IAdministratorService administratorService) =>
{
    Administrator administrator = administratorService.GetAdministratorById(id);

    if (administrator != null)
    {
        return Results.Ok(administrator);
    }
    else
    {
        return Results.NotFound();
    }
        
    



}).WithTags("Administrator");

app.MapDelete("/administrator/{id}", ([FromQuery] int id, IAdministratorService administratorService) =>
{
    Administrator administrator = administratorService.GetAdministratorById(id);

    if (administrator != null)
    {
        administratorService.DeleteAdministrator(administrator);
        return Results.Ok("Administrator deleted");
    }
    else
    {
        return Results.NotFound();
    }





}).WithTags("Administrator");


#endregion

#region vehicles

ValidationErrors verifyDTO(VehicleDTO vehicleDTO)
{

    var validation = new ValidationErrors
    {
        Messages = new List<string>()
    };


    if (string.IsNullOrEmpty(vehicleDTO.Name))
    {
        validation.Messages.Add("The name must not be empty!");
    }

    if (string.IsNullOrEmpty(vehicleDTO.Brand))
    {
        validation.Messages.Add("The brand must not be empty!");
    }

    if (vehicleDTO.Year < 1900)
    {
        validation.Messages.Add("Vehicle must not be older than the 1900s!");
    }

    return validation;
}

app.MapPost("/vehicles", ([FromBody] VehicleDTO vehicleDTO, IVehicleService vehicleService) =>
{
    var validation = verifyDTO(vehicleDTO);


    if (validation.Messages.Count > 0)
    {
        return Results.BadRequest(validation);
    }

    var vehicle = new Vehicle
    {
        Name = vehicleDTO.Name,
        Brand = vehicleDTO.Brand,
        Year = vehicleDTO.Year,
    };
    vehicleService.Save(vehicle);
    return Results.Created($"/vehicle/{vehicle.Id}", vehicle);
}).WithTags("Vehicle");

app.MapGet("/vehicles", ( [FromQuery] int? page, IVehicleService vehicleService) =>
{
    var vehicles = vehicleService.AllVehicles(page);
    
    
    return Results.Ok(vehicles);
}).WithTags("Vehicle");

app.MapGet("/vehicles/{id}", ([FromRoute] int id, IVehicleService vehicleService) =>
{
    var vehicle = vehicleService.GetById(id);

    if (vehicle == null)
    {
        return Results.NotFound();
    }


    return Results.Ok(vehicle);
}).WithTags("Vehicle");

app.MapPut("/vehicles/{id}", ([FromRoute] int id, VehicleDTO vehicleDTO, IVehicleService vehicleService) =>
{


    var vehicle = vehicleService.GetById(id);

    if (vehicle == null)
    {
        return Results.NotFound();
    }

    var validation = verifyDTO(vehicleDTO);


    if (validation.Messages.Count > 0)
    {
        return Results.BadRequest(validation);
    }

    vehicle.Name = vehicleDTO.Name;
    vehicle.Brand = vehicleDTO.Brand;
    vehicle.Year = vehicleDTO.Year;

    vehicleService.Update(vehicle);

    return Results.Ok(vehicle);

}).WithTags("Vehicle");


app.MapDelete("/vehicles/{id}", ([FromRoute] int id, IVehicleService vehicleService) =>
{
    var vehicle = vehicleService.GetById(id);

    if (vehicle == null)
    {
        return Results.NotFound();
    }

    vehicleService.Delete(vehicle);

    return Results.NoContent();

}).WithTags("Vehicle");
#endregion


// Start the application and listen for incoming requests.
app.Run();



