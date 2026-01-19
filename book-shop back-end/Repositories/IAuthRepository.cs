using book_shop_back_end.Dtos;
using book_shop_back_end.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Identity;


namespace book_shop_back_end.Repositories
{
    public interface IAuthRepository
    {

        public Task<Microsoft.AspNetCore.Identity.IdentityResult> SaveUser(User user);

        public Task<User> GetUserById(string id);
        public Task<User> GenerateToken(User user);

        public Task<bool> IsUserNameExists(User user);

        public Task<bool> IsEmailExists(User user);

        public Task<Microsoft.AspNetCore.Identity.IdentityResult> Register(User user, string password);

        Task<User?> FindByNameOrEmailAsync(string userNameOrEmail);
        Task<bool> CheckPasswordAsync(User user, string password);
    }
}
