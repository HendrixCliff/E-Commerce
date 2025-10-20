using Ecommerce.API.DTOs.Order;
using Ecommerce.API.Models;

namespace Ecommerce.API.Services.Interfaces
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderResponseDto>> GetAllAsync();
        Task<OrderResponseDto?> GetByIdAsync(int id);
        Task<OrderResponseDto> CreateAsync(CreateOrderDto dto);
        Task<bool> UpdateAsync(int id, UpdateOrderDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
