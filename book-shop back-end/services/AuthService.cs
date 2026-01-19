using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using book_shop_back_end.Dtos;
using book_shop_back_end.Models;
using book_shop_back_end.Repositories;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace book_shop_back_end.services
{
    public class AuthService
    {
        private readonly AuthRepository _authRepository;
        private readonly Microsoft.AspNetCore.Identity.UserManager<User> _userManager;
        private readonly IConfiguration _config;

        public AuthService(
            AuthRepository authRepository,
            Microsoft.AspNetCore.Identity.UserManager<User> userManager,
            IConfiguration config
            )
        {
            _authRepository = authRepository;
            _userManager = userManager;
            _config = config;

        }

        public async Task<ProfileDto> GetProfile(string id)
        {
            var user = await _authRepository.GetUserById(id);
            if (user == null)
            {
                return null;
            }
            return new ProfileDto
            {
                userName = user.UserName,
                age = user.Age,
                email = user.Email,
                address = user.Address
            };
        }

        public async Task<Microsoft.AspNetCore.Identity.IdentityResult> UpdateUserAddress(string id,string address)
        {

            var user = await _authRepository.GetUserById(id);
            if (user == null) {
                return null;
            }
            user.Address = address;

            var result = await _authRepository.SaveUser(user);

            return result;
        }

        public async Task<Microsoft.AspNetCore.Identity.IdentityResult> UpdateUserAge(string id, int age)
        {

            if (age < 0 || age > 120)
            {
                return null;
            }
            
            var user = await _authRepository.GetUserById(id);

            if (user == null)
            {
                return null;
            }

            user.Age = age;

            var result = await _authRepository.SaveUser(user);

            return result;
        }

        public async Task<Microsoft.AspNetCore.Identity.IdentityResult> UpdateUserEmail(string id, string email)
        {
            var user = await _authRepository.GetUserById(id);
            if (user == null)
            {
                return null;
            }

            bool isUserNameExists = await _authRepository.IsEmailExists(user);
            if (isUserNameExists)
            {
                return null;
            }

            user.Email = email;

            var result = await _authRepository.SaveUser(user);

            return result;
        }

        public async Task<Microsoft.AspNetCore.Identity.IdentityResult> UpdateUserUserName(string id, string userName)
        {
            var user = await _authRepository.GetUserById(id);
            if (user == null)
            {
                return null;
            }

            bool isUserNameExists = await _authRepository.IsUserNameExists(user);
            if (isUserNameExists)
            {
                return null;
            }

            user.UserName = userName;

            var result = await _authRepository.SaveUser(user);

            return result;
        }

        public async Task<Microsoft.AspNetCore.Identity.IdentityResult> Register(User user, string password)
        {
            var result = await _authRepository.Register(user, password);

            if(result.Succeeded && user.UserName == "admin")
            {
                await this.makeAdmin(user);
            }

            return result;
        }



        public async Task<AuthResultDto> LoginAsync(LoginDto dto)
        {
            var user = await _authRepository.FindByNameOrEmailAsync(dto.UserNameOrEmail);

            if (user == null)
            {
                return new AuthResultDto
                {
                    Succeeded = false,
                };
            }

            var valid = await _authRepository.CheckPasswordAsync(user, dto.Password);
            if (!valid)
            {
                return new AuthResultDto
                {
                    Succeeded = false,
                };
            }

            if(user.UserName == "admin")
            {
                await this.makeAdmin(user);
            }
            var token = await GenerateJwt(user);

            return new AuthResultDto
            {
                Succeeded = true,
                Token = token,
                User = new
                {
                    user.Id,
                    user.UserName
                }
            };
        }

        private async Task<Microsoft.AspNetCore.Identity.IdentityResult> makeAdmin(User user)
        {
            return await _userManager.AddToRoleAsync(user, "Admin");
        }
        public async Task<string> GenerateJwt(User user)
        {
            var claims = new List<Claim>
             {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
             };

            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddDays(7),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


    }
}
