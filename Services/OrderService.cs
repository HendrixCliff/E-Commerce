using Ecommerce.API.Models;
using Ecommerce.API.Repositories;
using Ecommerce.API.Repositories.Interfaces;
using Ecommerce.API.Services.Interfaces;


namespace Ecommerce.API.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;

        public OrderService(OrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<IEnumerable<Order>> GetAllAsync()
        {
            return await _orderRepository.GetAllAsync();
        }

        public async Task<Order?> GetByIdAsync(int id)
        {
            return await _orderRepository.GetByIdAsync(id);
        }

        public async Task<Order> CreateAsync(Order order)
        {
            order.OrderDate = DateTime.UtcNow;
            order.Status = "Pending";

            await _orderRepository.AddAsync(order);
            await _orderRepository.SaveChangesAsync();

            return order;
        }

        public async Task<bool> UpdateAsync(int id, Order updateOrder)
        {
            var existingOrder = await _orderRepository.GetByIdAsync(id);
            if (existingOrder == null) return false;

            existingOrder.Status = updateOrder.Status;
            existingOrder.TotalAmount = updateOrder.TotalAmount;

            await _orderRepository.UpdateAsync(existingOrder);
            await _orderRepository.SaveChangesAsync();

            return true;

        }

        public async Task<bool> DeleteAsync(int id)
        {
            var order = await _orderRepository.GetByIdAsync(id);
           if (order == null) return false;

            await _orderRepository.DeleteAsync(order);
            await _orderRepository.SaveChangesAsync();

            return true;
         }


    }
}