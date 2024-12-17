using DealershipDotNetAPI.Domain.DTOs;
using DealershipDotNetAPI.Infrastructure.Db;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ContextDb>(options =>
{
    options.UseMySql(
            builder.Configuration.GetConnectionString("mysql"),
            ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("mysql"))
            );
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

//Requisição GET no Endpoint '/'
app.MapGet("/", () => "Hello Word!");

app.MapPost("/login", (LoginDTO loginDTO) =>
{
    if (loginDTO.Email == "adm@test.com" && loginDTO.Password == "1234")
    {
        return Results.Ok("Authorized login");

    }
    else { return Results.Unauthorized(); }
});



app.Run();

//O sufixo DTO é uma
//convenção que indica que esta classe
//é um objeto para transferência de dados.

