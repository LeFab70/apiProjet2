using System.ComponentModel.DataAnnotations;

namespace ApiProjetBorrowing.Dtos
{
    /// <summary>Réponse JSON de POST /api/login pour la doc OpenAPI.</summary>
    public record LoginResponse(string Token);

    public record LoginRequest(
        [EmailAddress(ErrorMessage = "Format d'email invalide.")]
        [Required(ErrorMessage = "L'email est obligatoire.")]
    string Email,
        [Required(ErrorMessage = "Le mot de passe est obligatoire.")]
    string Password);
}