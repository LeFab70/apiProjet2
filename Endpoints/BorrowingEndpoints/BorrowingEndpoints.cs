using ApiProjetBorrowing.Dtos.BorrowingDto;
using ApiProjetBorrowing.services.borrowingServices;

namespace ApiProjetBorrowing.Endpoints;

public static class BorrowingEndpoints
{
    public static void MapBorrowingEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/borrowings");

        // Créer un emprunt - Protégé par JWT selon les consignes de Fabrice
        group.MapPost("/", async (CreateBorrowingDto dto, IBorrowingService service) =>
        {
            var result = await service.CreateBorrowingAsync(dto);
            return Results.Created($"/api/borrowings/{result?.Id}", result);
        }).RequireAuthorization();

        // Récupérer un emprunt par ID
        group.MapGet("/{id}", async (int id, IBorrowingService service) =>
        {
            var result = await service.GetBorrowingByIdAsync(id);
            return result is not null ? Results.Ok(result) : Results.NotFound();
        });
    }
}