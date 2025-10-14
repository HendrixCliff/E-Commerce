using Ecommerce.API.Models;
using Microsoft.AspNetCore;
using Ecommerce.API.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using Ecommerce.API.Services.Interfaces;

namespace Ecommerce.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
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
        public async Task<IActionResult> GetById(int id)
        {
            var order =  await _orderService.GetByIdAsync(id);
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
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Order updated)
        {
            var success = await _orderService.UpdateAsync(id, updated);
            if (!success) {
                return NotFound();
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id) {
           var success = await _orderService.DeleteAsync(id);
           if (!success) {
                return NotFound();
            }
            return NoContent();
        }
    }
    

}
