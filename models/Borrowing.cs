using System.ComponentModel.DataAnnotations;

namespace ApiProjetBorrowing.Models
{
    public class Borrowing
    {
        public int Id { get; set; }

        [Required]
        public required string BorrowCode { get; set; }

        [Required]
        public DateTime BorrowDate { get; set; } = DateTime.UtcNow;

        // Relation vers User
        public int UserId { get; set; }
        public User User { get; set; } = null!;

        // Relation many-to-many
        public ICollection<BorrowingBook> BorrowingBooks { get; set; } = new List<BorrowingBook>();
    }
}
