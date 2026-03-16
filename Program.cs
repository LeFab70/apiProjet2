using ApiProjetBorrowing.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddOpenApi();

//inscrire le context de la base de données, ici on utilise une base de données en mémoire pour le développement et les tests
builder.Services.AddDbContext<ApiBorrowingContext>(options =>
    options.UseInMemoryDatabase("BorrowingDb"));


//pour utiliser une base de données SQL Server, décommentez la ligne suivante et commentez la ligne précédente
builder.Services.AddDbContext<ApiBorrowingContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUi(options =>
    {
        options.DocumentPath = "/openapi/v1.json";
    });
}


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
