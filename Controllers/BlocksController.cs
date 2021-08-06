using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FoodDelivery.Db;
using Microsoft.AspNetCore.Authorization;

namespace FoodDelivery.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "RESTAURANT_OWNER")]
    public class BlocksController : ControllerBase
    {
        private readonly FoodDeliveryContext _context;

        public BlocksController(FoodDeliveryContext context)
        {
            _context = context;
        }

        // GET: api/UserBlocks
        [HttpGet]
        [Authorize(Roles = "RESTAURANT_OWNER")]
        public async Task<ActionResult<IEnumerable<string>>> GetBlocks()
        {
            var userId = Utilities.ExtractUserId(User);
            return await _context.Blocks
                .Where(x => x.BlockingUserId == userId)
                .Select(x => x.BlockedUser!.Id)
                .ToListAsync();
        }

        // POST: api/UserBlocks
        [ProducesResponseType(204)]
        [HttpPost("{userId}")]
        [Authorize(Roles = "RESTAURANT_OWNER")]
        public async Task<ActionResult<Block>> BlockUser(string userId)
        {
            var blockingUserId = Utilities.ExtractUserId(User);

            if (await _context.Users.FindAsync(userId) == null)
                return NotFound("Invalid user id");

            if (await _context.Blocks.FindAsync(blockingUserId, userId) != null)
                return Conflict("This user is already blocked");

            _context.Blocks.Add(new Block()
            {
                BlockingUserId = blockingUserId,
                BlockedUserId = userId
            });

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/UserBlocks/5
        [ProducesResponseType(204)]
        [HttpDelete("{id}")]
        [Authorize(Roles = "RESTAURANT_OWNER")]
        public async Task<IActionResult> DeleteUserBlock(long id)
        {
            var block = await _context.Blocks.FindAsync(id);
            if (block == null)
                return NotFound("Invalid id");
            if (block.BlockingUserId != Utilities.ExtractUserId(User))
                return Forbid();

            _context.Blocks.Remove(block);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
