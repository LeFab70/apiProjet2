// Création des users qui loueront les livres de la bibliothèque
using System.ComponentModel.DataAnnotations;

namespace ApiProjetBorrowing.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public required string FirstName { get; set; }

        // Le nom de famille de l'utilisateur peut être vide, mais ne doit pas dépasser 50 caractères
        [StringLength(50)]
        public string LastName { get; set; } = String.Empty; // Valeur par défaut pour éviter les problèmes de nullabilité

        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [Required]
        public required string Password { get; set; }
    }
}