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
            //si le dto est null, une exception est levée pour éviter les erreurs de référence null lors de l'accès aux propriétés du dto. Cela garantit que les données nécessaires pour créer un utilisateur sont fournies avant de tenter de créer l'utilisateur dans la base de données.
            if (dto is null) //
                throw new ArgumentNullException(nameof(dto));



            //             Avant de créer un nouvel utilisateur, il est important de vérifier que l'email n'est pas déjà utilisé par un autre utilisateur pour éviter les doublons. Si l'email existe déjà, une exception est levée.
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

        public async Task<UserDto?> UpdateUserAsync(int id, UpdateUserDto dto)
        {

            //validation du dto pour s'assurer que les données nécessaires pour mettre à jour un utilisateur sont fournies. Si le dto est null, une exception est levée pour éviter les erreurs de référence null lors de l'accès aux propriétés du dto.
            if (dto is null)
                throw new ArgumentNullException(nameof(dto));

            //             Avant de mettre à jour un utilisateur, il est important de vérifier que l'email n'est pas déjà utilisé par un autre utilisateur (autre que celui que nous sommes en train de mettre à jour) pour éviter les doublons. Si l'email existe déjà, une exception est levée.
            if (await _context.Users.AnyAsync(u => u.Email == dto.Email && u.Id != id))
                throw new Exception("Email déjà utilisé.");

            var user = await _context.Users.FindAsync(id);
            if (user is null)
                return null;



            if (dto.FirstName is not null)
                user.FirstName = dto.FirstName;

            if (dto.LastName is not null)
                user.LastName = dto.LastName;

            if (dto.Email is not null)
            {
                if (await _context.Users.AnyAsync(u => u.Email == dto.Email && u.Id != id))
                    throw new Exception("Email déjà utilisé.");

                user.Email = dto.Email;
            }

            if (dto.Password is not null)
            {
                if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.Password))
                    user.Password = BCrypt.Net.BCrypt.HashPassword(dto.Password);
            }


            await _context.SaveChangesAsync();

            return new UserDto(user.Id, user.FirstName, user.LastName, user.Email);
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            //             Avant de supprimer un utilisateur, il est important de vérifier que l'utilisateur existe dans la base de données. Si l'utilisateur n'existe pas, la méthode retourne false pour indiquer que la suppression a échoué.

            var user = await _context.Users.FindAsync(id);
            if (user is null)
                return false;

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
