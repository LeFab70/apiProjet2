using System.ComponentModel.DataAnnotations;

namespace ApiProjetBorrowing.Models
{
    public class BorrowingBook
    {
        public int Id { get; set; }

        // FK vers Borrowing
        public int BorrowingId { get; set; }
        public Borrowing Borrowing { get; set; } = null!;

        // FK vers Book
        public int BookId { get; set; }
        public Book Book { get; set; } = null!;

        // Date de retour du livre
        public DateTime? ReturnDate { get; set; }
    }
}
