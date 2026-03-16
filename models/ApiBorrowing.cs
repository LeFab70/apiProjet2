using Microsoft.EntityFrameworkCore;

namespace ApiProjetBorrowing.Models
{
    public class ApiBorrowingContext : DbContext
    {
        public ApiBorrowingContext(DbContextOptions<ApiBorrowingContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Book> Books { get; set; } = null!;
        public DbSet<Borrowing> Borrowings { get; set; } = null!;
        public DbSet<BorrowingBook> BorrowingBooks { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Relation many-to-many Borrowing <-> Book
            modelBuilder.Entity<BorrowingBook>()
                .HasKey(bb => bb.Id);

            modelBuilder.Entity<BorrowingBook>()
                .HasOne(bb => bb.Borrowing)
                .WithMany(b => b.BorrowingBooks)
                .HasForeignKey(bb => bb.BorrowingId);

            modelBuilder.Entity<BorrowingBook>()
                .HasOne(bb => bb.Book)
                .WithMany(b => b.BorrowingBooks)
                .HasForeignKey(bb => bb.BookId);
        }
    }
}
