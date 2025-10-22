using Ecommerce.API.DTOs.Order;
using Ecommerce.API.Models;
using Ecommerce.API.Repositories;
using Ecommerce.API.Repositories.Interfaces;
using Ecommerce.API.Services.Interfaces;


namespace Ecommerce.API.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
         private readonly IProductRepository _productRepository;


        public OrderService(IProductRepository productRepository, IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
        }

        public async Task<IEnumerable<OrderResponseDto>> GetAllAsync()
        {
           var orders = await _orderRepository.GetAllAsync();

            return orders.Select(o => new OrderResponseDto
            {
                Id = o.Id,
                UserId = o.UserId,
                CreatedAt = o.CreatedAt,
                Status = o.Status,
                TotalAmount = o.TotalAmount,
                Items = o.Items.Select(i => new OrderItemResponseDto
                {
                    ProductId = i.ProductId,
                    ProductName = i.Product?.Name ?? string.Empty,
                    Price = i.Product?.Price ?? 0,
                    Quantity = i.Quantity
                }).ToList()
            });
        }

        public async Task<OrderResponseDto?> GetByIdAsync(int id)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            if (order == null) return null;

            return new OrderResponseDto
            {
                Id = order.Id,
                UserId = order.UserId,
                CreatedAt = order.CreatedAt,
                Status = order.Status,
                TotalAmount = order.TotalAmount,
                Items = order.Items.Select(i => new OrderItemResponseDto
                {
                    ProductId = i.ProductId,
                    ProductName = i.Product?.Name ?? string.Empty,
                    Price = i.Product?.Price ?? 0,
                    Quantity = i.Quantity
                }).ToList()
            };
        }

        public async Task<OrderResponseDto> CreateAsync(CreateOrderDto dto)
        {
            var order = new Order
            {
                UserId = dto.UserId,
                CreatedAt = DateTime.UtcNow,
                Status = "Pending",
                Items = new List<OrderItem>()
            };

            decimal total = 0;

            foreach (var item in dto.Items)
            {
                var product = await _productRepository.GetByIdAsync(item.ProductId);
                if (product == null) throw new Exception($"Product with ID {item.ProductId} not found.");

                var orderItem = new OrderItem
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity
                };

                total += product.Price * item.Quantity;
                order.Items.Add(orderItem);
            }

            order.TotalAmount = total;

            await _orderRepository.AddAsync(order);
            await _orderRepository.SaveChangesAsync();

            return await GetByIdAsync(order.Id) ?? throw new Exception("Order not found after creation");
        }

        public async Task<bool> UpdateAsync(int id, UpdateOrderDto dto)
        {
           var order = await _orderRepository.GetByIdAsync(dto.Id);
            if (order == null) return false;

            order.Status = dto.Status;
            await _orderRepository.UpdateAsync(order);
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