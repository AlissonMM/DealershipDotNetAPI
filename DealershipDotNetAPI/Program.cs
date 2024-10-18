var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

//Requisi��o GET no Endpoint '/'
app.MapGet("/", () => "Ol� Mundo!");

app.MapPost("/login", (LoginDTO loginDTO) =>
{
    if (loginDTO.Email == "adm@test.com" && loginDTO.Senha == "1234")
    {
        return Results.Ok("Authorized login");

    }
    else { return Results.Unauthorized(); }
});



app.Run();

//O sufixo DTO � uma
//conven��o que indica que esta classe
//� um objeto para transfer�ncia de dados.
public class LoginDTO
{
    public string Email { get; set; } = default!;
    public string Senha { get; set; } = default!;

};
