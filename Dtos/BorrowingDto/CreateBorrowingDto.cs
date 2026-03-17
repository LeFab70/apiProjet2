namespace ApiProjetBorrowing.Dtos.BorrowingDto
{
    public record CreateBorrowingDto(
     int UserId,
     List<int> BookIds // Liste des IDs des livres à emprunter
 );
}
