using book_shop_back_end.Data;
using book_shop_back_end.Models;
using book_shop_back_end.services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace book_shop_back_end.Repositories
{
    public class BookRepository : IBookRepostiry
    {
        private readonly DiscountService _discountService;
        private readonly BookStoreContext _context;

        public BookRepository(DiscountService discountService, BookStoreContext context) {
            _discountService = discountService;
            _context = context;

        }
        public void SetRegisteredUsersDiscount(decimal percent)
        {
            _discountService.SetRegisteredUsersDiscount(percent);
        }

        public async Task<Book> FindBookById(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return null;
            }
            return book;
        }

        public async Task<List<Book>> GetAllBooks()
        {
            return await _context.Books
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Book> SaveBook(Book book)
        {

            _context.Books.Update(book);
            await _context.SaveChangesAsync();
            return book;
        }
    }
}
