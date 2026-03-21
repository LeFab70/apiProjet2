namespace ApiProjetBorrowing.Dtos.bookDto
{
    public record LivreDto
    (
        int Id,
        string Title,
        string Author,
        string ISBN,
        int Quantity
    );

    public record AjouterLivreDto
    (
        string Title,
        string Author,
        string ISBN,
        int Quantity
    );

    public record UpdateLivreDto
    (
        string? Title,
        string? Author,
        string? ISBN,
        int? Quantity
    );
}