using ApiProjetBorrowing.Dtos.bookDto;
using ApiProjetBorrowing.OpenApi;
using ApiProjetBorrowing.services.bookServices;
using Microsoft.AspNetCore.Http;

namespace ApiProjetBorrowing.Endpoints.BookEndpoints
{
    public static class BookEndpoints
    {
        public static void MapBookEndpoints(this WebApplication app)
        {
            app.MapPost("/api/books", async (AjouterLivreDto dto, IBookService bookService) =>
            {
                var result = await bookService.AddBookAsync(dto);
                return Results.Created($"/api/books/{result.Id}", result);
            })
            .WithTags("Livres")
            .WithApiDoc(
                "Ajouter un livre",
                "Crée un livre (titre, auteur, ISBN, quantité). Accès public (aucun JWT requis sur ce groupe de routes).")
            .Produces<LivreDto>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest);

            app.MapPut("/api/books/{id}", async (int id, UpdateLivreDto dto, IBookService bookService) =>
            {
                var result = await bookService.UpdateBookAsync(id, dto);
                return result is not null ? Results.Ok(result) : Results.NotFound();
            })
            .WithTags("Livres")
            .WithApiDoc(
                "Mettre à jour un livre",
                "Met à jour les champs fournis. Retourne 404 si l’id n’existe pas.")
            .Produces<LivreDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

            app.MapDelete("/api/books/{id}", async (int id, IBookService bookService) =>
            {
                var success = await bookService.DeleteBookAsync(id);
                return success ? Results.NoContent() : Results.NotFound();
            })
            .WithTags("Livres")
            .WithApiDoc(
                "Supprimer un livre",
                "Supprime le livre identifié. Réponse 204 sans corps si succès, 404 sinon.")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound);

            app.MapGet("/api/books/{id}", async (int id, IBookService bookService) =>
            {
                var result = await bookService.GetBookByIdAsync(id);
                return result is not null ? Results.Ok(result) : Results.NotFound();
            })
            .WithTags("Livres")
            .WithApiDoc(
                "Obtenir un livre par identifiant",
                "Retourne un livre ou 404.")
            .Produces<LivreDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

            app.MapGet("/api/books", async (IBookService bookService) =>
            {
                var result = await bookService.GetAllBooksAsync();
                return Results.Ok(result);
            })
            .WithTags("Livres")
            .WithApiDoc(
                "Lister les livres",
                "Retourne la liste des livres disponibles.")
            .Produces<IEnumerable<LivreDto>>(StatusCodes.Status200OK);
        }
    }
}