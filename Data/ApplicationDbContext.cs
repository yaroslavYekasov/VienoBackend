using Microsoft.EntityFrameworkCore;
using VienoBackend.Models;

namespace VienoBackend.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; } // добавляем таблицу продуктов
        public DbSet<Basket> Baskets { get; set; }
        public DbSet<PurchaseHistory> PurchaseHistories { get; set; }

    }
}
