using ApiProjetBorrowing.Data;
using ApiProjetBorrowing.Dtos;
using ApiProjetBorrowing.Endpoints;
using ApiProjetBorrowing.Endpoints.UsersEndpoints;
using ApiProjetBorrowing.Models;
using ApiProjetBorrowing.services.borrowingServices;
using ApiProjetBorrowing.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Récupérer la clé secrète pour JWT depuis la configuration
var jwtKey = builder.Configuration["Jwt:Key"];
//verifier que la clé n'est pas null ou vide, sinon une exception est levée pour éviter les erreurs de référence null lors de l'utilisation de la clé pour la validation du token JWT.
if (string.IsNullOrEmpty(jwtKey))
{
    throw new Exception("La clé JWT n'est pas configurée. Veuillez vérifier la configuration.");
}
// Convertir la clé en bytes pour l'utiliser dans la validation du token JWT
var keyBytes = System.Text.Encoding.UTF8.GetBytes(jwtKey!);


// Add services to the container.
//ajouter la validation des données d'entrée des dto
builder.Services.AddValidation();

//ajouter l'authentification JWT
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidateAudience = true,
        ValidAudience = builder.Configuration["Jwt:Audience"],
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});

// Inscrire les services de gestion des utilisateurs
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IBorrowingService, BorrowingService>();
builder.Services.AddControllers();
builder.Services.AddOpenApi();

// Base de données SQL Server
builder.Services.AddDbContext<ApiBorrowingContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

//Seed de la base de données pour les tests et le développement
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<ApiBorrowingContext>();
    DbInitializer.Seed(context);
}


if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUi(options =>
    {
        options.DocumentPath = "/openapi/v1.json";
    });
}

//redirecttoutes les requêtes HTTP vers HTTPS pour assurer la sécurité des données échangées entre le client et le serveur. Cela garantit que les communications sont chiffrées et protégées contre les interceptions potentielles.
app.UseHttpsRedirection();

// Configure the HTTP request pipeline. pour gérer les requêtes HTTP entrantes et les acheminer vers les contrôleurs appropriés. Il configure également les middlewares pour l'authentification, l'autorisation, la redirection HTTPS, et la documentation Swagger en développement.
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapGet("/", () => "Bienvenue sur l'API de gestion des emprunts de livres !");



// Mappez les endpoints pour les utilisateurs
app.MapUsersEndpoints();
// Mappez les endpoints pour l'authentification
app.MapAuthEndpoints();
// Mapper les endpoints pour les emprunts
app.MapBorrowingEndpoints();
//lance l'application
app.Run();
