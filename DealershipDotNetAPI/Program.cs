using DealershipDotNetAPI.Domain.DTOs;
using DealershipDotNetAPI.Domain.Entities;
using DealershipDotNetAPI.Domain.Interfaces;
using DealershipDotNetAPI.Domain.ModelViews;
using DealershipDotNetAPI.Domain.Services;
using DealershipDotNetAPI.Infrastructure.Db;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Model;

#region builder

var builder = WebApplication.CreateBuilder(args);

var jwtKey = builder.Configuration.GetSection("Jwt")["Key"].ToString();

if (string.IsNullOrEmpty(jwtKey))
{
    jwtKey = "123456";
}

//Adding Authentication by JWT
builder.Services.AddAuthentication(option =>
{
    option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

}).AddJwtBearer(option =>
{
    option.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateLifetime = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
    };
});

builder.Services.AddAuthorization();

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
string GenerateJwtToken(Administrator administrator)
{
    if (string.IsNullOrEmpty(jwtKey) || jwtKey.Length < 32)
    {
        throw new InvalidOperationException("JWT key must be at least 32 characters long.");
    }

    var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
    var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

    var claims = new List<Claim>
    {
        new Claim("Email", administrator.Email),
        new Claim("Profile", administrator.Profile)
    };

    var token = new JwtSecurityToken(
        claims: claims,
        expires: DateTime.UtcNow.AddDays(1),
        signingCredentials: credentials
    );

    return new JwtSecurityTokenHandler().WriteToken(token);
}

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
    var admin = administratorService.Login(loginDTO);


    if (admin!= null)
    {
        string jwtToken = GenerateJwtToken(admin);
        return Results.Ok(new LoggedAdmin
        {
            Email = admin.Email,
            Profile = admin.Profile,
            JwtToken = jwtToken
        });

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
     
    
   
}).RequireAuthorization().WithTags("Administrator");


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



}).RequireAuthorization().WithTags("Administrator");


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
        
    



}).RequireAuthorization().WithTags("Administrator");


app.MapGet("/administrator", (IAdministratorService administratorService) =>
{
    List<Administrator> administrators = administratorService.GetAllAdministrators();

    if (administrators != null)
    {
        return Results.Ok(administrators);
    }
    else
    {
        return Results.NotFound();
    }





}).RequireAuthorization().WithTags("Administrator");

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





}).RequireAuthorization().WithTags("Administrator");


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
}).RequireAuthorization().WithTags("Vehicle");

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
}).RequireAuthorization().WithTags("Vehicle");

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

}).RequireAuthorization().WithTags("Vehicle");


app.MapDelete("/vehicles/{id}", ([FromRoute] int id, IVehicleService vehicleService) =>
{
    var vehicle = vehicleService.GetById(id);

    if (vehicle == null)
    {
        return Results.NotFound();
    }

    vehicleService.Delete(vehicle);

    return Results.NoContent();

}).RequireAuthorization().WithTags("Vehicle");
#endregion


#region app
// Start the application and listen for incoming requests
app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();
app.UseAuthorization();

app.Run();

#endregion

