using ApiProjetBorrowing.Data;
using ApiProjetBorrowing.Models;
using ApiProjetBorrowing.Dtos.BorrowingDto;
using Microsoft.EntityFrameworkCore;

namespace ApiProjetBorrowing.services.borrowingServices;

public class BorrowingService : IBorrowingService
{
    private readonly ApiBorrowingContext _context;

    public BorrowingService(ApiBorrowingContext context)
    {
        _context = context;
    }

    public async Task<BorrowingResponseDto?> CreateBorrowingAsync(CreateBorrowingDto dto)
    {
        // 1. Génération du code d'emprunt unique
        string uniqueCode = $"BOR-{DateTime.Now.Year}-{Guid.NewGuid().ToString()[..4].ToUpper()}";

        // 2. Création de l'objet principal Borrowing
        var borrowing = new Borrowing
        {
            UserId = dto.UserId,
            BorrowCode = uniqueCode,
            BorrowDate = DateTime.Now
        };

        // 3. Gestion de la relation many-to-many
        foreach (var bookId in dto.BookIds)
        {
            var borrowingBook = new BorrowingBook
            {
                BookId = bookId,
                Borrowing = borrowing,
                ReturnDate = null
            };
            _context.BorrowingBooks.Add(borrowingBook);
        }

        _context.Borrowings.Add(borrowing);
        await _context.SaveChangesAsync();

        // 4. RÉCUPÉRATION COMPLÈTE (Eager Loading) pour la réponse
        // On utilise .Include pour aller chercher les noms des livres et de l'utilisateur
        var result = await _context.Borrowings
            .Include(b => b.User)
            .Include(b => b.BorrowingBooks)
                .ThenInclude(bb => bb.Book)
            .FirstOrDefaultAsync(b => b.Id == borrowing.Id);

        if (result == null) return null;

        return new BorrowingResponseDto(
            result.Id,
            result.BorrowCode,
            result.BorrowDate,
            $"{result.User.FirstName} {result.User.LastName}",
            result.BorrowingBooks.Select(bb => bb.Book.Title).ToList()
        );
    }

    public async Task<BorrowingResponseDto?> GetBorrowingByIdAsync(int id)
    {
        var result = await _context.Borrowings
            .Include(b => b.User)
            .Include(b => b.BorrowingBooks)
                .ThenInclude(bb => bb.Book)
            .FirstOrDefaultAsync(b => b.Id == id);

        if (result == null) return null;

        return new BorrowingResponseDto(
            result.Id,
            result.BorrowCode,
            result.BorrowDate,
            $"{result.User.FirstName} {result.User.LastName}",
            result.BorrowingBooks.Select(bb => bb.Book.Title).ToList()
        );
    }

    public async Task<IEnumerable<BorrowingResponseDto>> GetUserBorrowingsAsync(int userId)
    {
        var borrowings = await _context.Borrowings
            .Include(b => b.User)
            .Include(b => b.BorrowingBooks)
                .ThenInclude(bb => bb.Book)
            .Where(b => b.UserId == userId)
            .ToListAsync();

        return borrowings.Select(b => new BorrowingResponseDto(
            b.Id,
            b.BorrowCode,
            b.BorrowDate,
            $"{b.User.FirstName} {b.User.LastName}",
            b.BorrowingBooks.Select(bb => bb.Book.Title).ToList()
        ));
    }
}