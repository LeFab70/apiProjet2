using ApiProjetBorrowing.Dtos;
using ApiProjetBorrowing.Services;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ApiProjetBorrowing.Endpoints
{
    public static class AuthEndpoints
    {
        public static void MapAuthEndpoints(this WebApplication app)
        {
            app.MapPost("/api/login", async (LoginRequest login, IUserService userService, IConfiguration config) =>
            {
                // 1. Chercher l'utilisateur 
                var user = await userService.GetUserByEmailAsync(login.Email);

                // 2. Vérifier si l'utilisateur existe et si le mot de passe correspond (BCrypt)
                if (user == null || !BCrypt.Net.BCrypt.Verify(login.Password, user.Password))
                {
                    return Results.Unauthorized();
                }

                // 3. Préparer le Token
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]!));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                var claims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim(ClaimTypes.Name, user.FirstName)
                };

                var token = new JwtSecurityToken(
                    issuer: config["Jwt:Issuer"],
                    audience: config["Jwt:Audience"],
                    claims: claims,
                    expires: DateTime.Now.AddHours(3),
                    signingCredentials: credentials);

                var jwt = new JwtSecurityTokenHandler().WriteToken(token);

                return Results.Ok(new { Token = jwt });
            })
            .AllowAnonymous()//permet à tout le monde d'accéder à cette route sans authentification, ce qui est nécessaire pour permettre aux utilisateurs de se connecter et d'obtenir un token JWT.
            .WithTags("Authentification");

        }
    }


}