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
        public async Task<ActionResult<IEnumerable<Block>>> GetBlocks()
        {
            var userId = Utilities.ExtractUserId(User);
            return await _context.Blocks
                .Where(x => x.BlockingUserId == userId)
                .Include(x => x.BlockedUser)
                .ToListAsync();
        }

        // POST: api/UserBlocks
        [ProducesResponseType(204)]
        [HttpPost("{username}")]
        [Authorize(Roles = "RESTAURANT_OWNER")]
        public async Task<ActionResult<Block>> BlockUser(string username)
        {
            var blockingUserId = Utilities.ExtractUserId(User);

            var blockedUser = await _context.Users.FirstOrDefaultAsync(x => x.UserName == username);
            if (blockedUser == null)
                return NotFound("Invalid user id");

            if (blockedUser.Id == blockingUserId)
                return BadRequest("You cannot block yourself");

            if (await _context.Blocks.FindAsync(blockingUserId, blockedUser.Id) != null)
                return Conflict("This user is already blocked");

            _context.Blocks.Add(new Block()
            {
                BlockingUserId = blockingUserId,
                BlockedUserId = blockedUser.Id
            });

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/UserBlocks/5
        [ProducesResponseType(204)]
        [HttpDelete("{userId}")]
        [Authorize(Roles = "RESTAURANT_OWNER")]
        public async Task<IActionResult> DeleteUserBlock(string userId)
        {
            var myUserId = Utilities.ExtractUserId(User);
            var block = await _context.Blocks.FindAsync(myUserId, userId);
            if (block == null)
                return NotFound("Invalid user id");

            _context.Blocks.Remove(block);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
