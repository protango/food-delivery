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
    public class UserBlocksController : ControllerBase
    {
        private readonly FoodDeliveryContext _context;

        public UserBlocksController(FoodDeliveryContext context)
        {
            _context = context;
        }

        // GET: api/UserBlocks
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserBlock>>> GetUserBlocks()
        {
            return await _context.UserBlocks.ToListAsync();
        }

        // GET: api/UserBlocks/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserBlock>> GetUserBlock(long id)
        {
            var userBlock = await _context.UserBlocks.FindAsync(id);

            if (userBlock == null)
            {
                return NotFound();
            }

            return userBlock;
        }

        // PUT: api/UserBlocks/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserBlock(long id, UserBlock userBlock)
        {
            if (id != userBlock.RestaurantId)
            {
                return BadRequest();
            }

            _context.Entry(userBlock).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserBlockExists(id))
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

        // POST: api/UserBlocks
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<UserBlock>> PostUserBlock(UserBlock userBlock)
        {
            _context.UserBlocks.Add(userBlock);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (UserBlockExists(userBlock.RestaurantId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetUserBlock", new { id = userBlock.RestaurantId }, userBlock);
        }

        // DELETE: api/UserBlocks/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserBlock(long id)
        {
            var userBlock = await _context.UserBlocks.FindAsync(id);
            if (userBlock == null)
            {
                return NotFound();
            }

            _context.UserBlocks.Remove(userBlock);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserBlockExists(long id)
        {
            return _context.UserBlocks.Any(e => e.RestaurantId == id);
        }
    }
}
