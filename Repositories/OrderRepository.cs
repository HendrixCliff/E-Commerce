using Ecommerce.API.Data;
using Ecommerce.API.Models;
using Ecommerce.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace Ecommerce.API.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly AppDbContext _context;

        public OrderRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Order>> GetAllAsync()
        {
            return await _context.Orders
            .Include(o => o.Items).ThenInclude(i => i.Product)
            .AsNoTracking().ToListAsync();
        }

        public async Task<Order?> GetByIdAsync(int id)
        {
            return await _context.Orders
            .Include(o => o.Items).ThenInclude(i => i.Product)
            .FirstOrDefaultAsync(o => o.Id == id);

        }

        public async Task AddAsync(Order order)
        {
            await _context.Orders.AddAsync(order);

        }

        public async Task UpdateAsync(Order order)
        {
            _context.Orders.Update(order);
            await Task.CompletedTask;
        }

        public async Task DeleteAsync(Order order)
        {
            _context.Orders.RemoveRange(order);
            await Task.CompletedTask;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}