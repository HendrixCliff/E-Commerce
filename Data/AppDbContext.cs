using Microsoft.EntityFrameworkCore;
using Ecommerce.API.Models;

namespace Ecommerce.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; }

        public DbSet<Cart> Carts { get; set; }

        public DbSet<Order> Orders { get; set; }
         public DbSet<OrderItem> OrderItems { get; set; }
    }
}
