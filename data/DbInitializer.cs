using ApiProjetBorrowing.Models;
using BCrypt.Net;
using Microsoft.EntityFrameworkCore;

namespace ApiProjetBorrowing.Data
{
    // C'est cette structure de classe qui empêche l'erreur d'unité de compilation
    public static class DbInitializer
    {
        public static void Seed(ApiBorrowingContext context)
        {
            context.Database.EnsureCreated();

            // Initialisation de l'utilisateur
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
            // Initialisation des livres
            if (!context.Books.Any())
            {
                context.Books.AddRange(
                    new Book { Title = "L'Étranger", Author = "Albert Camus", ISBN = "978-2070360024" },
                    new Book { Title = "1984", Author = "George Orwell", ISBN = "978-0451524935" }
                );
                context.SaveChanges();
            }
            // Ajout d'un emprunt de test
            if (!context.Borrowings.Any())
            {
                var adminUser = context.Users.First();
                var books = context.Books.Take(2).ToList();

                var testBorrowing = new Borrowing
                {
                    UserId = adminUser.Id,
                    BorrowCode = $"BOR-{DateTime.Now.Year}-SEED",
                    BorrowDate = DateTime.Now.AddDays(-2),
                };

                context.Borrowings.Add(testBorrowing);
                context.SaveChanges();

                foreach (var book in books)
                {
                    context.BorrowingBooks.Add(new BorrowingBook
                    {
                        BorrowingId = testBorrowing.Id,
                        BookId = book.Id,
                        ReturnDate = null
                    });
                }
                context.SaveChanges();
            }
        }
    }
}