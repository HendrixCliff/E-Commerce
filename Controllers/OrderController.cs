using Ecommerce.API.Models;
using Microsoft.AspNetCore;
using Ecommerce.API.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections;

namespace Ecommerce.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly OrderService _orderService;

        public OrderController(OrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var orders = await _orderService.GetAllAsync();
            return Ok(orders);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var order = _orderService.GetByIdAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            return Ok(order);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Order order)
        {
            var created = await _orderService.CreateAsync(order);
            return CreatedAtAction(nameof(Create), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Order updated)
        {
            var success = await _orderService.UpdateAsync(id, updated);
            if (success == null) {
                return NotFound();
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id) {
           var success = await _orderService.DeleteAsync(id);
           if (success == null) {
                return NotFound();
            }
            return NoContent();
        }
    }
    

}
