using Ecommerce.API.Data;
using Ecommerce.API.Models;
using Ecommerce.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.API.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly AppDbContext _context;

        public CartRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Cart>> GetAllAsync()
        {
            return await _context.Carts
                .Include(c => c.Product)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Cart?> GetByIdAsync(int id)
        {
            return await _context.Carts
                .Include(c => c.Product)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task AddAsync(Cart cart)
        {
            await _context.Carts.AddAsync(cart);
        }

        public async Task UpdateAsync(Cart cart)
        {
            _context.Carts.Update(cart);
            await Task.CompletedTask;
        }

        public async Task DeleteAsync(Cart cart)
        {
            _context.Carts.Remove(cart);
            await Task.CompletedTask;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
