using System.ComponentModel.DataAnnotations;

namespace ApiProjetBorrowing.Models
{
    public class Book
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; } = null!;

        [Required]
        public string Author { get; set; } = null!;

        [Required]
        public string ISBN { get; set; } = null!;

        [Range(0, int.MaxValue)]
        public int Quantity { get; set; }

        public ICollection<BorrowingBook> BorrowingBooks { get; set; } = new List<BorrowingBook>();
    }
}
