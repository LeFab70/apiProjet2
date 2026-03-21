using ApiProjetBorrowing.Dtos;
using ApiProjetBorrowing.OpenApi;
using ApiProjetBorrowing.Services;
using Microsoft.AspNetCore.Http;

namespace ApiProjetBorrowing.Endpoints.UsersEndpoints
{
    public static class UsersEndPoints
    {
        public static void MapUsersEndpoints(this WebApplication app)
        {
            // Créer un groupe de routes pour les utilisateurs avec un préfixe commun et une autorisation requise jwt en login
            var routeUser = app.MapGroup("/api/users").WithTags("Users").RequireAuthorization();
            //simplifier les routes
            routeUser.MapGet("/", GetAllUsers)
                .WithApiDoc(
                    "Lister les utilisateurs",
                    "Retourne tous les utilisateurs. Authentification JWT obligatoire.")
                .Produces<IEnumerable<UserDto>>(StatusCodes.Status200OK);

            routeUser.MapGet("/{id}", GetUserById)
                .WithApiDoc(
                    "Obtenir un utilisateur par identifiant",
                    "Retourne un utilisateur ou 404 si l’id n’existe pas. JWT requis.")
                .Produces<UserDto>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound);

            //pour permettre à tout le monde de créer un compte, on autorise l'accès à la route de création d'utilisateur sans authentification
            routeUser.MapPost("/", CreateUser).AllowAnonymous()
                .WithApiDoc(
                    "Créer un utilisateur (inscription)",
                    "Création de compte publique : prénom, nom, email, mot de passe. Aucun JWT requis pour cet appel.")
                .Produces<UserDto>(StatusCodes.Status201Created)
                .Produces(StatusCodes.Status400BadRequest);

            routeUser.MapPut("/{id}", UpdateUser)
                .WithApiDoc(
                    "Mettre à jour un utilisateur",
                    "Met à jour prénom et nom. JWT requis.")
                .Produces<UserDto>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound);

            routeUser.MapDelete("/{id}", DeleteUser)
                .WithApiDoc(
                    "Supprimer un utilisateur",
                    "Supprime l’utilisateur identifié. JWT requis.")
                .Produces(StatusCodes.Status204NoContent)
                .Produces(StatusCodes.Status404NotFound);

        }

        private static async Task<IResult> GetAllUsers(IUserService userService)
        {
            var users = await userService.GetAllUsersAsync();
            return Results.Ok(users);
        }

        private static async Task<IResult> GetUserById(int id, IUserService userService)
        {
            var user = await userService.GetUserByIdAsync(id);
            return user is not null ? Results.Ok(user) : Results.NotFound();
        }

        private static async Task<IResult> CreateUser(CreateUserDto createUserDto, IUserService userService)
        {
            var createdUser = await userService.CreateUserAsync(createUserDto);
            return Results.Created($"/api/users/{createdUser.Id}", createdUser);
        }

        private static async Task<IResult> UpdateUser(int id, UpdateUserDto updateUserDto, IUserService userService)
        {
            var updatedUser = await userService.UpdateUserAsync(id, updateUserDto);
            return updatedUser is not null ? Results.Ok(updatedUser) : Results.NotFound();
        }

        private static async Task<IResult> DeleteUser(int id, IUserService userService)
        {
            var deleted = await userService.DeleteUserAsync(id);
            return deleted ? Results.NoContent() : Results.NotFound();
        }
    }
}




//avec controller comme en java

//IActionResult : C'est l'équivalent de ResponseEntity en Spring. pour un retour de Ok(), NotFound(), ou Created().
//Dans Program.cs : utiliser builder.Services.AddControllers();.
// using ApiProjetBorrowing.Dtos;
// using ApiProjetBorrowing.Services;
// using Microsoft.AspNetCore.Mvc;

// namespace ApiProjetBorrowing.Controllers
// {
//     [ApiController] // Équivalent de @RestController
//     [Route("api/users")] // Équivalent de @RequestMapping("/api/users")
//     public class UsersController : ControllerBase
//     {
//         private readonly IUserService _userService;

//         // Injection par constructeur (recommandé en .NET, comme en Spring)
//         public UsersController(IUserService userService)
//         {
//             _userService = userService;
//         }

//         [HttpGet] // Équivalent de @GetMapping
//         public async Task<IActionResult> GetAll()
//         {
//             var users = await _userService.GetAllUsersAsync();
//             return Ok(users);
//         }

//         [HttpGet("{id}")] // Équivalent de @GetMapping("/{id}")
//         public async Task<IActionResult> GetById(int id)
//         {
//             var user = await _userService.GetUserByIdAsync(id);
//             if (user == null) return NotFound();

//             return Ok(user);
//         }

//         [HttpPost] // Équivalent de @PostMapping
//         public async Task<IActionResult> Create([FromBody] CreateUserDto createUserDto)
//         {
//             var createdUser = await _userService.CreateUserAsync(createUserDto);
//             // Retourne une 201 Created avec le lien vers la ressource
//             return CreatedAtAction(nameof(GetById), new { id = createdUser.Id }, createdUser);
//         }

//         [HttpPut("{id}")] // Équivalent de @PutMapping("/{id}")
//         public async Task<IActionResult> Update(int id, [FromBody] UpdateUserDto updateUserDto)
//         {
//             var updatedUser = await _userService.UpdateUserAsync(id, updateUserDto);
//             if (updatedUser == null) return NotFound();

//             return Ok(updatedUser);
//         }

//         [HttpDelete("{id}")] // Équivalent de @DeleteMapping("/{id}")
//         public async Task<IActionResult> Delete(int id)
//         {
//             var deleted = await _userService.DeleteUserAsync(id);
//             if (!deleted) return NotFound();

//             return NoContent();
//         }
//     }
// }