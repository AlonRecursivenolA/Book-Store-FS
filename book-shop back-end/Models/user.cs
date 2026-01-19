using Microsoft.AspNetCore.Identity;

namespace book_shop_back_end.Models
{
    public class User : IdentityUser
    {
        public int Age { get; set; } = 0;
        public string Address { get; set; } = "";
        string FavoriteGenre { get; set; } = ""!;

        public string Email { get; set; } = "";
    }
}
