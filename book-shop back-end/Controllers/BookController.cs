using book_shop_back_end.Data;
using book_shop_back_end.Models;
using book_shop_back_end.Repositories;
using book_shop_back_end.services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace book_shop_back_end.Controllers
{
    [Authorize]
    [Route("api/[Controller]")]
    public class BookController : Controller
    {

        private readonly DiscountService _discountService;
        private readonly BookService _bookService;

        public BookController(

            DiscountService discountService,
            BookService bookService

           )
        {
            _discountService = discountService;
            _bookService = bookService;
        }

        [Authorize]
        [HttpPut("price/{id}/{price}")]
        public async Task<IActionResult> changePrice(int id, int price)
        {
            var action = await _bookService.ChangeBookPrice(id, price);
            if (action == null)
            {
                return NotFound();
            }
            return Ok(action);
        }
        [Authorize]
        [HttpPut("category/{id}/{category}")]
        public async Task<IActionResult> ChangeCategory(int id, string category)
        {

            var action = await _bookService.changeBookCategory(id, category);

            if (action == null)
            {
                return NotFound("Book not found.");
            }

            return Ok(action);
        }

        [Authorize]
        [HttpPut("description/{id}/{description}")]
        public async Task<IActionResult> changeBookDescription(int id, string description)
        {
            var action = await _bookService.changeBookDescription(id, description);
            if (action == null)
            {
                return NotFound();
            }
            return Ok(action);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> getBooks()
        {
            bool isRegisteredUser = User.Identity.IsAuthenticated;
            var books = await _bookService.GetAllBooks(isRegisteredUser);
            return Ok(books);
        }

        [HttpGet("{id:int}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetBook(int id)
        {
            bool isRegisteredUser = User.Identity.IsAuthenticated;

            var book = await _bookService.GetBookById(id, isRegisteredUser);
            return Ok(book);
        }


        [Authorize(Roles = "Admin")]
        [HttpPut("registered-discount/{percent}")]
        public IActionResult SetRegisteredUsersDiscount(decimal percent)
        {
            _discountService.SetRegisteredUsersDiscount(percent);
            return Ok(new { message = $"Discount for registered users set to {percent}%" });
        }

    }
}

