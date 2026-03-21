using System.ComponentModel.DataAnnotations;

namespace ApiProjetBorrowing.Dtos
{
    public record UpdateUserDto
    (
        [MinLength(5, ErrorMessage = "Le prénom doit contenir au moins 5 caractères.")]
        [MaxLength(50, ErrorMessage = "Le prénom ne doit pas dépasser 50 caractères.")]
        string? FirstName,

        [MinLength(5, ErrorMessage = "Le nom doit contenir au moins 5 caractères.")]
        [MaxLength(50, ErrorMessage = "Le nom ne doit pas dépasser 50 caractères.")]
        string? LastName,

          [EmailAddress(ErrorMessage = "Format d'email invalide.")]
        string? Email,

          [MinLength(8, ErrorMessage = "Le mot de passe doit contenir au moins 8 caractères.")]
        [MaxLength(100, ErrorMessage = "Le mot de passe ne doit pas dépasser 100 caractères.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$",
            ErrorMessage = "Le mot de passe doit contenir au moins une lettre majuscule, une lettre minuscule, un chiffre et un caractère spécial.")]
        string? Password
    );
}
