using System.ComponentModel.DataAnnotations;

namespace ApiProjetBorrowing.Models
{
    public class Book
    {
        public int Id { get; set; }

        [Required]
        public required string Title { get; set; }

        [Required]
        public required string Author { get; set; }

        [Required]
        public required string ISBN { get; set; }

        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }

        public ICollection<BorrowingBook> BorrowingBooks { get; set; } = new List<BorrowingBook>();
    }
}