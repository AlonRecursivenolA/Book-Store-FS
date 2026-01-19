using book_shop_back_end.Models;
using Microsoft.AspNetCore.Mvc;

namespace book_shop_back_end.Repositories
{
    public interface IBookRepostiry
    {
        public void SetRegisteredUsersDiscount(decimal percent);
        public Task<Book> FindBookById(int id);

        public Task<List<Book>> GetAllBooks();

        public Task<Book> SaveBook(Book book);

    }
}
