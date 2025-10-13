using Microsoft.AspNetCore.Mvc;
using Ecommerce.API.Services;
using Ecommerce.API.Models;

namespace Ecommerce.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartController : ControllerBase
    {
        private readonly CartService _service;

        public CartController(CartService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var carts = await _service.GetAllAsync();
            return Ok(carts);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var cart = await _service.GetByIdAsync(id);
            if (cart == null)
                return NotFound();

            return Ok(cart);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Cart cart)
        {
            var created = await _service.CreateAsync(cart);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Cart updatedCart)
        {
            var success = await _service.UpdateAsync(id, updatedCart);
            if (!success)
                return NotFound();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _service.DeleteAsync(id);
            if (!success)
                return NotFound();

            return NoContent();
        }
    }
}
