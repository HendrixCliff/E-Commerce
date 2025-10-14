using Ecommerce.API.Models;

namespace Ecommerce.API.Services.Interfaces
{
    public interface IOrderService
    {
        Task<IEnumerable<Order>> GetAllAsync();
        Task<Order?> GetByIdAsync(int id);
        Task<Order> CreateAsync(Order order);
        Task<bool> UpdateAsync(int id, Order updatedOrder);
        Task<bool> DeleteAsync(int id);
    }
}
