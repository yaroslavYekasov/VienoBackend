using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VienoBackend.Models;
using VienoBackend.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VienoBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BasketController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public BasketController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/basket/{userId}
        [HttpGet("{userId}")]
        public async Task<ActionResult<IEnumerable<Basket>>> GetBasket(int userId)
        {
            var basketItems = await _context.Baskets
                .Where(b => b.UserId == userId)
                .Include(b => b.Product)
                .ToListAsync();

            if (basketItems == null || basketItems.Count == 0)
            {
                return NotFound("No items found in the basket.");
            }

            return Ok(basketItems);
        }

        // POST: api/basket
        [HttpPost]
        public async Task<ActionResult<Basket>> AddToBasket([FromBody] Basket basket)
        {
            if (basket == null)
            {
                return BadRequest("Invalid basket data.");
            }

            // Проверяем, есть ли такой продукт
            var product = await _context.Products.FindAsync(basket.ProductId);
            if (product == null)
            {
                return NotFound("Product not found.");
            }

            // Проверяем, есть ли уже этот продукт в корзине пользователя
            var existingItem = await _context.Baskets
                .FirstOrDefaultAsync(b => b.UserId == basket.UserId && b.ProductId == basket.ProductId);

            if (existingItem != null)
            {
                existingItem.Quantity += basket.Quantity;
            }
            else
            {
                _context.Baskets.Add(basket);
            }

            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetBasket), new { userId = basket.UserId }, basket);
        }

        // DELETE: api/basket/{userId}/{productId}
        [HttpDelete("{userId}/{productId}")]
        public async Task<IActionResult> RemoveFromBasket(int userId, int productId)
        {
            var basketItem = await _context.Baskets
                .FirstOrDefaultAsync(b => b.UserId == userId && b.ProductId == productId);

            if (basketItem == null)
            {
                return NotFound("Product not found in the basket.");
            }

            _context.Baskets.Remove(basketItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/basket/clear/{userId}
        [HttpDelete("clear/{userId}")]
        public async Task<IActionResult> ClearBasket(int userId)
        {
            var basketItems = await _context.Baskets
                .Where(b => b.UserId == userId)
                .ToListAsync();

            if (basketItems == null || basketItems.Count == 0)
            {
                return NotFound("No items found in the basket to clear.");
            }

            _context.Baskets.RemoveRange(basketItems);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
