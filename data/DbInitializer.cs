//Seed pour mettre le first user dans la base de données, pour les tests et le développement
using ApiProjetBorrowing.Models;
using BCrypt.Net;
using Microsoft.EntityFrameworkCore;

namespace ApiProjetBorrowing.Data
{
    public static class DbInitializer
    {
        public static void Seed(ApiBorrowingContext context)
        { 
            context.Database.EnsureCreated();

            if (!context.Users.Any())
            {
                var user = new User
                {
                    FirstName = "Admin",
                    LastName = "User",
                    Email = "admin@test.com",
                    Password = BCrypt.Net.BCrypt.HashPassword("Password123!")
                };

                context.Users.Add(user);
            }

            // Ajoutons quelques livres pour pouvoir tester ses emprunts !
            if (!context.Books.Any())
            {
                context.Books.AddRange(
                    new Book
                    {
                        Title = "L'Étranger",
                        Author = "Albert Camus",
                        ISBN = "978-2070360024" // Ajout de l'ISBN obligatoire
                    },
                    new Book
                    {
                        Title = "1984",
                        Author = "George Orwell",
                        ISBN = "978-0451524935" // Ajout de l'ISBN obligatoire
                    }
                );
            }

            context.SaveChanges();
        }
    }
}
