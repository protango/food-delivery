using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FoodDelivery.Db;
using Microsoft.AspNetCore.Authorization;
using FoodDelivery.Models;

namespace FoodDelivery.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RestaurantsController : ControllerBase
    {
        private readonly FoodDeliveryContext _context;

        public RestaurantsController(FoodDeliveryContext context)
        {
            _context = context;
        }

        // GET: api/Restaurants
        [HttpGet]
        [Authorize(Roles = "CUSTOMER")]
        public async Task<ActionResult<IEnumerable<Restaurant>>> GetRestaurants()
        {
            var userId = Utilities.ExtractUserId(User);
            var blockedBy = _context.Blocks.Where(x => x.BlockedUserId == userId).Select(x => x.BlockingUserId);

            return await _context.Restaurants.Where(x => !blockedBy.Contains(x.OwnerUserId)).ToListAsync();
        }

        // GET: api/Restaurants
        [HttpGet("{id}")]
        [Authorize(Roles = "CUSTOMER,RESTAURANT_OWNER")]
        public async Task<ActionResult<Restaurant>> GetRestaurant(long id)
        {
            var userId = Utilities.ExtractUserId(User);
            var restaurant = (Restaurant?)await _context.Restaurants.FindAsync(id);
            if (restaurant == null)
                return NotFound("Invalid restaurant id");
            if (!User.IsInRole("CUSTOMER") && restaurant.OwnerUserId != Utilities.ExtractUserId(User))
                return StatusCode(403, "User does not have access to this data");
            if (await _context.Blocks.FindAsync(restaurant.OwnerUserId, userId) != null)
                return StatusCode(403, "User is blocked");

            return restaurant;
        }

        [HttpPost]
        [Authorize(Roles = "RESTAURANT_OWNER")]
        [ProducesResponseType(201)]
        public async Task<ActionResult<Restaurant>> PostRestaurant(CreateRestaurantModel restaurant)
        {
            var dbRestaurant = _context.Restaurants.Add(new Restaurant() {
                Name = restaurant.Name,
                Description = restaurant.Description,
                OwnerUserId = Utilities.ExtractUserId(User),
                Meals = restaurant.Meals.Select(x => new Meal() { 
                    Name = x.Name,
                    Description = x.Description,
                    Price = x.Price
                }).ToArray()
            });

            await _context.SaveChangesAsync();

            return CreatedAtAction("PostRestaurant", dbRestaurant);
        }

        [ProducesResponseType(204)]
        [HttpPut("{id}")]
        [Authorize(Roles = "RESTAURANT_OWNER")]
        public async Task<ActionResult<Restaurant>> PutRestaurant(long id, UpdateRestaurantModel restaurant)
        {
            var dbRestaurant = (Restaurant?)await _context.Restaurants.FindAsync(id);
            if (dbRestaurant == null)
                return NotFound("Invalid id");
            if (dbRestaurant.OwnerUserId != Utilities.ExtractUserId(User))
                return StatusCode(403, "User does not have access to this data");

            dbRestaurant.Name = restaurant.Name;
            dbRestaurant.Description = restaurant.Description;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [ProducesResponseType(204)]
        [HttpDelete("{id}")]
        [Authorize(Roles = "RESTAURANT_OWNER")]
        public async Task<ActionResult<Restaurant>> DeleteRestaurant(long id)
        {
            var restaurant = await _context.Restaurants.FindAsync(id);
            if (restaurant == null)
                return NotFound("Invalid id");

            if (restaurant.OwnerUserId != Utilities.ExtractUserId(User))
                return StatusCode(403, "User does not have access to this data");

            _context.Restaurants.Remove(restaurant);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
