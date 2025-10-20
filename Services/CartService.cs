using Ecommerce.API.DTOs.Cart;
using Ecommerce.API.Repositories;
using Ecommerce.API.Models;
using Ecommerce.API.Repositories.Interfaces;
using Ecommerce.API.Services.Interfaces;

namespace Ecommerce.API.Services
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;
        private readonly IProductRepository _productRepository;

        public CartService(IProductRepository productRepository, ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
            _productRepository = productRepository;
        }

        public async Task<IEnumerable<CartResponseDto>> GetAllAsync()
        {
             var carts = await _cartRepository.GetAllAsync();
            return carts.Select(c => new CartResponseDto
            {
                Id = c.Id,
                ProductId = c.ProductId,
                ProductName = c.Product?.Name,
                ProductPrice = c.Product?.Price ?? 0,
                Quantity = c.Quantity,
                UserId = c.UserId
            });
        }

        public async Task<CartResponseDto?> GetByIdAsync(int id)
        {
            var cart = await _cartRepository.GetByIdAsync(id);
            if (cart == null) return null;

            return new CartResponseDto
            {
                Id = cart.Id,
                ProductId = cart.ProductId,
                ProductName = cart.Product?.Name,
                ProductPrice = cart.Product?.Price ?? 0,
                Quantity = cart.Quantity,
                UserId = cart.UserId
            };
        }

        public async Task<CartResponseDto> CreateAsync(CreateCartDto dto)
        {
           var product = await _productRepository.GetByIdAsync(dto.ProductId);
            if (product == null)
                throw new Exception("Product not found");

            var cart = new Cart
            {
                ProductId = dto.ProductId,
                Quantity = dto.Quantity,
                UserId = dto.UserId
            };

            await _cartRepository.AddAsync(cart);
            await _cartRepository.SaveChangesAsync();

            return new CartResponseDto
            {
                Id = cart.Id,
                ProductId = cart.ProductId,
                ProductName = product.Name,
                ProductPrice = product.Price,
                Quantity = cart.Quantity,
                UserId = cart.UserId
            };
        }

        public async Task<bool> UpdateAsync(int id, UpdateCartDto dto)
        {
            var cart = await _cartRepository.GetByIdAsync(dto.Id);
            if (cart == null) return false;

            cart.Quantity = dto.Quantity;

            await _cartRepository.UpdateAsync(cart);
            await _cartRepository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
             var cart = await _cartRepository.GetByIdAsync(id);
            if (cart == null) return false;

            await _cartRepository.DeleteAsync(cart);
            await _cartRepository.SaveChangesAsync();
            return true;
    
        }
    }
}
