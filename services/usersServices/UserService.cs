using ApiProjetBorrowing.Dtos;
using ApiProjetBorrowing.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiProjetBorrowing.Services
{
    public class UserService : IUserService
    {
        //ajout de la dépendance du contexte de données pour accéder à la base de données
        private readonly ApiBorrowingContext _context;

        //         Le constructeur de UserService prend une instance de ApiBorrowingContext en paramètre, qui est injectée par le système de dépendance d'ASP.NET Core. Cette instance est utilisée pour interagir avec la base de données.
        public UserService(ApiBorrowingContext context)
        {
            _context = context;
        }

        //         Les méthodes de UserService implémentent les opérations CRUD pour les utilisateurs. Par exemple, GetAllUsersAsync récupère tous les utilisateurs de la base de données et les projette en UserDto, tandis que CreateUserAsync crée un nouvel utilisateur à partir d'un CreateUserDto et le sauvegarde dans la base de données.
        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            return await _context.Users
                .Select(u => new UserDto(u.Id, u.FirstName, u.LastName, u.Email))
                .ToListAsync();
        }

        public async Task<UserDto?> GetUserByIdAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);

            return user is null
                ? null
                : new UserDto(user.Id, user.FirstName, user.LastName, user.Email);
        }

        public async Task<UserDto> CreateUserAsync(CreateUserDto dto)
        {
            if (await _context.Users.AnyAsync(u => u.Email == dto.Email))
                throw new Exception("Email déjà utilisé.");
            //hasher le mot de passe avant de le stocker en base de données pour des raisons de sécurité
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            var user = new User
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                Password = hashedPassword
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return new UserDto(user.Id, user.FirstName, user.LastName, user.Email);
        }

        public async Task<UserDto?> UpdateUserAsync(int id, CreateUserDto dto)
        {
            var user = await _context.Users.FindAsync(id);
            if (user is null)
                return null;

            //             Si le mot de passe a été modifié, il doit être re-hashé avant d'être stocké en base de données
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            user.FirstName = dto.FirstName;
            user.LastName = dto.LastName;
            user.Email = dto.Email;
            user.Password = hashedPassword;

            await _context.SaveChangesAsync();

            return new UserDto(user.Id, user.FirstName, user.LastName, user.Email);
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user is null)
                return false;

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
