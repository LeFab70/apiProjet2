using ApiProjetBorrowing.Dtos.bookDto;

namespace ApiProjetBorrowing.services.bookServices
{
    public interface IBookService
    {
        // Ajouter un livre
        Task<LivreDto> AddBookAsync(AjouterLivreDto dto);

        // Modifier un livre existant
        Task<LivreDto?> UpdateBookAsync(int id, UpdateLivreDto dto);

        // Supprimer un livre
        Task<bool> DeleteBookAsync(int id);

        // Récupérer un livre par ID
        Task<LivreDto?> GetBookByIdAsync(int id);

        // Récupérer tous les livres
        Task<IEnumerable<LivreDto>> GetAllBooksAsync();
    }
}