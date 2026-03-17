using ApiProjetBorrowing.Dtos.BorrowingDto;

namespace ApiProjetBorrowing.services.borrowingServices
{
    public interface IBorrowingService
    {
        // Créer un nouvel emprunt avec plusieurs livres
        Task<BorrowingResponseDto?> CreateBorrowingAsync(CreateBorrowingDto dto);

        // Récupérer un emprunt spécifique par son ID
        Task<BorrowingResponseDto?> GetBorrowingByIdAsync(int id);

        // Optionnel : Récupérer tous les emprunts d'un utilisateur
        Task<IEnumerable<BorrowingResponseDto>> GetUserBorrowingsAsync(int userId);
    }
}