using System.Runtime.CompilerServices;
using book_shop_back_end.Dtos;
using book_shop_back_end.Models;
using book_shop_back_end.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace book_shop_back_end.services
{
    public class BookService
    {
        private readonly BookRepository _bookRepository;
        private readonly DiscountService _discountService;

        public BookService(BookRepository bookRepository, DiscountService discountService) { 
        
            _bookRepository = bookRepository;
            _discountService = discountService;
        }
        public async Task<List<BookDto>> GetAllBooks(bool isRegisterdUser)
        {
            decimal discountFactor = _discountService.RegisteredUsersDiscount;

            var books = await _bookRepository.GetAllBooks();

            var result = books.Select(b => new BookDto
            {
                Id = b.Id,
                Title = b.Title,
                Author = b.Author,
                Price = b.Price,
                ImageUrl = b.ImageUrl,
                Category = b.Category,
                DiscountedPrice =
                    (isRegisterdUser && discountFactor > 0m)
                        ? b.Price * (1 - discountFactor)
                        : b.Price
            }).ToList();

            return result;
        }


        public async Task<object> GetBookById(int id, bool isRegisterdUser)
        {
            var book = await _bookRepository.FindBookById(id);
            decimal discountFactor = _discountService.RegisteredUsersDiscount;

            return new
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author,
                Price = book.Price,
                ImageUrl = book.ImageUrl,
                Category = book.Category,
                DiscountedPrice =
                    (isRegisterdUser && discountFactor > 0m)
                        ? book.Price * (1 - discountFactor)
                        : book.Price
            };
        }

        public async Task<Book> changeBookDescription(int id, string description)
        {
            
            var book = await _bookRepository.FindBookById(id);
            if(book == null)
            {
                return null;
            }
            book.Description = description;

             await _bookRepository.SaveBook(book);

            return book;
        }

        public async Task<Book> ChangeBookPrice(int id, int price)
        {
            var book = await _bookRepository.FindBookById(id);
            if(book == null)
            {
                return null;
            }
            book.Price = price;

            await _bookRepository.SaveBook(book);

            return book;

        }

        public async Task<Book> changeBookCategory(int id, string category)
        {
            var book = await _bookRepository.FindBookById(id);
            if (book == null)
            {
                return null;
            }

            book.Category = category;

            await _bookRepository.SaveBook(book);

            return book;
        }
    }
    }

