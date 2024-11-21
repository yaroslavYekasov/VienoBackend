using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VienoBackend.Data;
using VienoBackend.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VienoBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UsersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/users (доступно только для админа)
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            var users = await _context.Users.ToListAsync();
            return Ok(users);
        }

        // GET: api/users/{id} (доступно только для админа)
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound("User not found.");
            }
            return Ok(user);
        }

        // PUT: api/users/{id} (редактирование пользователя - только админ)
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] User updatedUser)
        {
            if (id != updatedUser.Id)
            {
                return BadRequest("User ID mismatch.");
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            user.Username = updatedUser.Username;
            user.Role = updatedUser.Role; // разрешаем администратору менять роль
            user.Password = BCrypt.Net.BCrypt.HashPassword(updatedUser.Password); // хешируем новый пароль

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/users/{id} (удаление пользователя - только админ)
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
