using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VienoBackend.Models;
using VienoBackend.Data;
using System.Linq;
using System.Threading.Tasks;
using BCrypt.Net; // Подключаем пространство имен для использования BCrypt

namespace VienoBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AuthController(ApplicationDbContext context)
        {
            _context = context;
        }

        // POST: api/auth/register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] User user)
        {
            if (_context.Users.Any(u => u.Username == user.Username))
            {
                return BadRequest("User with this username already exists.");
            }

            // Хешируем пароль перед сохранением
            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok("Registration successful.");
        }

        // POST: api/auth/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] User login)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == login.Username);
            if (user == null || !BCrypt.Net.BCrypt.Verify(login.Password, user.Password))
            {
                return Unauthorized("Incorrect username or password.");
            }

            return Ok(new { message = "Login successful.", role = user.Role });
        }
    }
}