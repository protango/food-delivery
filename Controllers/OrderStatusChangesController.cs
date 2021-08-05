using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FoodDelivery.Db;

namespace FoodDelivery.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderStatusChangesController : ControllerBase
    {
        private readonly FoodDeliveryContext _context;

        public OrderStatusChangesController(FoodDeliveryContext context)
        {
            _context = context;
        }

        // GET: api/OrderStatusChanges
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderStatusChange>>> GetOrderStatusChanges()
        {
            return await _context.OrderStatusChanges.ToListAsync();
        }

        // GET: api/OrderStatusChanges/5
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderStatusChange>> GetOrderStatusChange(long id)
        {
            var orderStatusChange = await _context.OrderStatusChanges.FindAsync(id);

            if (orderStatusChange == null)
            {
                return NotFound();
            }

            return orderStatusChange;
        }

        // PUT: api/OrderStatusChanges/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrderStatusChange(long id, OrderStatusChange orderStatusChange)
        {
            if (id != orderStatusChange.OrderId)
            {
                return BadRequest();
            }

            _context.Entry(orderStatusChange).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderStatusChangeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/OrderStatusChanges
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<OrderStatusChange>> PostOrderStatusChange(OrderStatusChange orderStatusChange)
        {
            _context.OrderStatusChanges.Add(orderStatusChange);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (OrderStatusChangeExists(orderStatusChange.OrderId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetOrderStatusChange", new { id = orderStatusChange.OrderId }, orderStatusChange);
        }

        // DELETE: api/OrderStatusChanges/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrderStatusChange(long id)
        {
            var orderStatusChange = await _context.OrderStatusChanges.FindAsync(id);
            if (orderStatusChange == null)
            {
                return NotFound();
            }

            _context.OrderStatusChanges.Remove(orderStatusChange);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool OrderStatusChangeExists(long id)
        {
            return _context.OrderStatusChanges.Any(e => e.OrderId == id);
        }
    }
}
