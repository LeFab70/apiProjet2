using ApiProjetBorrowing.Dtos.bookDto;
using ApiProjetBorrowing.services.bookServices;

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
            });

            app.MapPut("/api/books/{id}", async (int id, UpdateLivreDto dto, IBookService bookService) =>
            {
                var result = await bookService.UpdateBookAsync(id, dto);
                return result is not null ? Results.Ok(result) : Results.NotFound();
            });

            app.MapDelete("/api/books/{id}", async (int id, IBookService bookService) =>
            {
                var success = await bookService.DeleteBookAsync(id);
                return success ? Results.NoContent() : Results.NotFound();
            });

            app.MapGet("/api/books/{id}", async (int id, IBookService bookService) =>
            {
                var result = await bookService.GetBookByIdAsync(id);
                return result is not null ? Results.Ok(result) : Results.NotFound();
            });
        }
    }
}