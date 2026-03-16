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
            context.Database.Migrate();

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
                context.SaveChanges();
            }
        }
    }
}
