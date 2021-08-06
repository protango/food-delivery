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
    public class MealsController : ControllerBase
    {
        private readonly FoodDeliveryContext _context;

        public MealsController(FoodDeliveryContext context)
        {
            _context = context;
        }

        // GET: api/Meals/ForRestaurant/5
        [HttpGet("ForRestaurant/{id}")]
        [Authorize(Roles = "CUSTOMER,RESTAURANT_OWNER")]
        public async Task<ActionResult<IEnumerable<Meal>>> GetMealsForRestaurant(long id)
        {
            var restaurant = await _context.Restaurants.FindAsync(id);

            if (restaurant == null)
                return NotFound("Invalid id");

            if (!User.IsInRole("CUSTOMER") && restaurant.OwnerUserId != Utilities.ExtractUserId(User))
                return StatusCode(403, "User does not have access to this data");

            return await _context.Meals.Where(x => x.RestaurantId == id).ToListAsync();
        }

        // PUT: api/Meals/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [ProducesResponseType(204)]
        [HttpPut("{id}")]
        [Authorize(Roles = "RESTAURANT_OWNER")]
        public async Task<IActionResult> PutMeal(long id, CreateMealModel meal)
        {
            var dbMeal = (Meal?)await _context.Meals.FirstOrDefaultAsync(x => x.Id == id);
            if (dbMeal == null) 
                return NotFound("Invalid restaurant id");

            await _context.Entry(dbMeal).Reference(x => x.Restaurant).LoadAsync();
            if (dbMeal.Restaurant!.OwnerUserId != Utilities.ExtractUserId(User))
                return StatusCode(403, "User does not have access to this data");

            dbMeal.Name = meal.Name;
            dbMeal.Description = meal.Description;
            dbMeal.Price = meal.Price;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // POST: api/Meals
        [ProducesResponseType(201)]
        [HttpPost("{restaurantId}")]
        [Authorize(Roles = "RESTAURANT_OWNER")]
        public async Task<ActionResult<Meal>> PostMeal(long restaurantId, CreateMealModel meal)
        {
            Restaurant? restaurant = await _context.Restaurants.FindAsync(restaurantId);
            if (restaurant == null)
                return NotFound("Invalid restaurant id");

            if (restaurant.OwnerUserId != Utilities.ExtractUserId(User))
                return StatusCode(403, "User does not have access to this data");

            var dbMeal = new Meal()
            {
                RestaurantId = restaurantId,
                Description = meal.Description,
                Name = meal.Name,
                Price = meal.Price
            };

            _context.Meals.Add(dbMeal);
            await _context.SaveChangesAsync();

            return CreatedAtAction("PostMeal", dbMeal);
        }

        // DELETE: api/Meals/5
        [ProducesResponseType(204)]
        [HttpDelete("{id}")]
        [Authorize(Roles = "RESTAURANT_OWNER")]
        public async Task<IActionResult> DeleteMeal(long id)
        {
            var meal = await _context.Meals.FindAsync(id);
            if (meal == null)
                return NotFound("Invalid id");

            await _context.Entry(meal).Reference(x => x.Restaurant).LoadAsync();
            if (meal.Restaurant!.OwnerUserId != Utilities.ExtractUserId(User))
                return StatusCode(403, "User does not have access to this data");

            _context.Meals.Remove(meal);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
