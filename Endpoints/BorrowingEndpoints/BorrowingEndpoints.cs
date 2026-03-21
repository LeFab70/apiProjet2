using ApiProjetBorrowing.Dtos.BorrowingDto;
using ApiProjetBorrowing.OpenApi;
using ApiProjetBorrowing.services.borrowingServices;
using Microsoft.AspNetCore.Http;

namespace ApiProjetBorrowing.Endpoints;

public static class BorrowingEndpoints
{
    public static void MapBorrowingEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/borrowings").WithTags("Emprunts");

        // Créer un emprunt - Protégé par JWT selon les consignes de Fabrice
        group.MapPost("/", async (CreateBorrowingDto dto, IBorrowingService service) =>
        {
            var result = await service.CreateBorrowingAsync(dto);
            return Results.Created($"/api/borrowings/{result?.Id}", result);
        })
        .WithApiDoc(
            "Créer un emprunt",
            "Crée un emprunt à partir du corps JSON (utilisateur, livres, etc.). Retourne l’emprunt créé avec code et dates.")
        .Produces<BorrowingResponseDto>(StatusCodes.Status201Created)
        .Produces(StatusCodes.Status400BadRequest);

        // Liste des emprunts par utilisateur (déclaré avant GET /{id} pour que le segment "user" ne soit pas confondu avec un id)
        group.MapGet("/user/{userId:int}", async (int userId, IBorrowingService service) =>
        {
            var result = await service.GetUserBorrowingsAsync(userId);
            return Results.Ok(result);
        })
        .WithApiDoc(
            "Lister les emprunts d’un utilisateur",
            "Retourne tous les emprunts pour l’identifiant utilisateur donné.")
        .Produces<IEnumerable<BorrowingResponseDto>>(StatusCodes.Status200OK);

        group.MapGet("/{id:int}", async (int id, IBorrowingService service) =>
        {
            var result = await service.GetBorrowingByIdAsync(id);
            return result is not null ? Results.Ok(result) : Results.NotFound();
        })
        .WithApiDoc(
            "Obtenir un emprunt par identifiant",
            "Retourne le détail d’un emprunt (code, date, utilisateur, titres des livres) ou 404.")
        .Produces<BorrowingResponseDto>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);
    }
}