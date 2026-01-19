using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using book_shop_back_end.Dtos;
using book_shop_back_end.Models;
using book_shop_back_end.services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace book_shop_back_end.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(
            DiscountService discountService,
            AuthService authService
            )
        {
            _authService = authService;
        }


        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] Dtos.RegisterDto dto)
        {
            var user = new User { UserName = dto.UserName, Email = dto.Email };

            var result = await _authService.Register(user, dto.Password);
            if (!result.Succeeded)
            {
                return BadRequest(new { errors = result.Errors.Select(e => e.Description) });
            }

            var token = _authService.GenerateJwt(user);

            return Ok(new { message = "Registered", token, user = new { user.Id, user.UserName, user.Email } });
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var result = await _authService.LoginAsync(dto);

            if (!result.Succeeded)
                return Unauthorized();

            return Ok(new
            {
                message = "Logged in",
                token = result.Token,
                user = result.User
            });
        }

        [HttpPut("age/{age}")]
        [Authorize]
        public async Task<IActionResult> updateAge(int age)
        {
            var id = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = await _authService.UpdateUserAge(id, age);

            if (!result.Succeeded)
            {
                return BadRequest();
            }

            return Ok("Age Has Been Changed");
        }

        [HttpPut("email/{email}")]
        [Authorize]
        public async Task<IActionResult> updateEmail(string email)
        {
            var id = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = await _authService.UpdateUserEmail(id, email);

            if (!result.Succeeded)
            {
                return BadRequest();
            }

            return Ok("Email Has Been Changed");

        }

        [HttpPut("username/{userName}")]
        [Authorize]
        public async Task<IActionResult> updateUserName(string userName)
        {
            var id = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = await _authService.UpdateUserUserName(id, userName);

            if (!result.Succeeded){

                return BadRequest();

            }

            return Ok("User Name Has Been Changed");

            }

        [HttpPut("{address}")]
        [Authorize]
        public async Task<IActionResult> updateAddress(string address)
        {

            var id = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = await _authService.UpdateUserAddress(id, address);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors.ToString());
            }
        
            return Ok("Address Has Been Updated");

        }

        [HttpGet("profile")]
        [Authorize]
        public async Task<IActionResult> GetProfile()
        {

            var id = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var user = await _authService.GetProfile(id);

            if (user == null)
            {
                return NotFound("User Not Found");
            }

            return Ok(user);
        }

    }


}
