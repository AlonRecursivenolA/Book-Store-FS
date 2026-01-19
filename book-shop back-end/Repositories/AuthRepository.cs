using System.Security.Claims;
using book_shop_back_end.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;


namespace book_shop_back_end.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly UserManager<User> _userManager;

        public AuthRepository(UserManager<User> userManager) { 
            _userManager = userManager;
        }

        public Task<User> GenerateToken(User user)
        {
            throw new NotImplementedException();
        }

        public async Task<User> GetUserById(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return null;
            }
            return user;
        }


        public async Task<bool> IsEmailExists(User user)
        {
            var result = await _userManager.FindByEmailAsync(user.Email);
            if (result == null)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> IsUserNameExists(User user)
        {
            
            var result = await _userManager.FindByNameAsync(user.UserName);

            if (result != null)
            {
                return false;
            }

            return true;
        }

        public async Task<IdentityResult> Register(User user, string password)
        {
            return await _userManager.CreateAsync(user, password);
        }

        public async Task<IdentityResult> SaveUser(User user)
        {
            var result = await _userManager.UpdateAsync(user);
            return result;
        }

        public async Task<User?> FindByNameOrEmailAsync(string userNameOrEmail)
        {
            var user = await _userManager.FindByNameAsync(userNameOrEmail);
            if (user != null) return user;

            return await _userManager.FindByEmailAsync(userNameOrEmail);
        }

        public async Task<bool> CheckPasswordAsync(User user, string password)
        {
            return await _userManager.CheckPasswordAsync(user, password);
        }

    }
}
