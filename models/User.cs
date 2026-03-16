//creation des users qui loueront les livres de la bibliothèque
using System.ComponentModel.DataAnnotations;

namespace ApiProjetBorrowing.Models
{
    public class User
    {
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }
        [StringLength(50)]
        public string LastName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}