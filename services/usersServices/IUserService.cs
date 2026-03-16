//Interfaces des services de gestion des utilisateurs
// Elle définit les méthodes pour créer, récupérer, mettre à jour et supprimer des utilisateurs.
using ApiProjetBorrowing.Dtos;
using ApiProjetBorrowing.Models;
namespace ApiProjetBorrowing.Services
{
    public interface IUserService
    {
        Task<IEnumerable<UserDto>> GetAllUsersAsync();
        Task<UserDto?> GetUserByIdAsync(int id);
        Task<UserDto> CreateUserAsync(CreateUserDto dto);
        Task<UserDto?> UpdateUserAsync(int id, CreateUserDto dto);
        Task<bool> DeleteUserAsync(int id);
    }
}

