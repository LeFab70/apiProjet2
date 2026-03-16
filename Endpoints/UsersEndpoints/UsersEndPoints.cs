using ApiProjetBorrowing.Dtos;
using ApiProjetBorrowing.Services;

namespace ApiProjetBorrowing.Endpoints.UsersEndpoints
{
    public static class UsersEndPoints
    {
        public static void MapUsersEndpoints(this WebApplication app)
        {
            var routeUser = app.MapGroup("/api/users").WithTags("Users");
            //simplifier les routes
            routeUser.MapGet("/", GetAllUsers);
            routeUser.MapGet("/{id}", GetUserById);
            routeUser.MapPost("/", CreateUser);
            routeUser.MapPut("/{id}", UpdateUser);
            routeUser.MapDelete("/{id}", DeleteUser);

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