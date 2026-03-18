using ApiProjetBorrowing.Dtos.bookDto;
using ApiProjetBorrowing.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiProjetBorrowing.services.bookServices
{
    public class BookService : IBookService
    {
        private readonly ApiBorrowingContext _context;

        public BookService(ApiBorrowingContext context)
        {
            _context = context;
        }

        public async Task<LivreDto> AddBookAsync(AjouterLivreDto dto)
        {
            var book = new Book
            {
                Title = dto.Title,
                Author = dto.Author,
                ISBN = dto.ISBN,
                Quantity = dto.Quantity
            };

            _context.Books.Add(book);
            await _context.SaveChangesAsync();

            return new LivreDto(book.Id, book.Title, book.Author, book.ISBN, book.Quantity);
        }

        public async Task<LivreDto?> UpdateBookAsync(int id, UpdateLivreDto dto)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null) return null;

            if (!string.IsNullOrEmpty(dto.Title)) book.Title = dto.Title;
            if (!string.IsNullOrEmpty(dto.Author)) book.Author = dto.Author;
            if (!string.IsNullOrEmpty(dto.ISBN)) book.ISBN = dto.ISBN;
            if (dto.Quantity.HasValue) book.Quantity = dto.Quantity.Value;

            await _context.SaveChangesAsync();
            return new LivreDto(book.Id, book.Title, book.Author, book.ISBN, book.Quantity);
        }

        public async Task<bool> DeleteBookAsync(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null) return false;

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<LivreDto?> GetBookByIdAsync(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null) return null;

            return new LivreDto(book.Id, book.Title, book.Author, book.ISBN, book.Quantity);
        }
    }
}