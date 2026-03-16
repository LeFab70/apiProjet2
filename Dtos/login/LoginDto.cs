using System.ComponentModel.DataAnnotations;

namespace ApiProjetBorrowing.Dtos
{
    public record LoginRequest(
        [EmailAddress(ErrorMessage = "Format d'email invalide.")]
        [Required(ErrorMessage = "L'email est obligatoire.")]
    string Email,
        [Required(ErrorMessage = "Le mot de passe est obligatoire.")]
    string Password);
}