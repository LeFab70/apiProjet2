//Context de la base de données pour les emprunts de livres
using Microsoft.EntityFrameworkCore;
namespace ApiProjetBorrowing.Models
{
    public class ApiBorrowingContext : DbContext
    {
        public ApiBorrowingContext(DbContextOptions<ApiBorrowingContext> options) : base(options)
        {
        }

        public DbSet<User> Borrowings { get; set; } = null!;
    }
}