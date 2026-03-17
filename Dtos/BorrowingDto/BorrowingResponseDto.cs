namespace ApiProjetBorrowing.Dtos.BorrowingDto
{
    public record BorrowingResponseDto(
     int Id,
     string BorrowCode,
     DateTime BorrowDate,
     string UserName,
     List<string> BookTitles // On renvoie les titres des livres pour que ce soit plus clair
 );
}
