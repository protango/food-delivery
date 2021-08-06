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
                return NotFound();

            if (!User.IsInRole("CUSTOMER") && restaurant.OwnerUserId != Utilities.ExtractUserId(User))
                return Unauthorized("Cannot access a restaurant you are not the owner of");

            return restaurant.Meals.ToList();
        }

        // PUT: api/Meals/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize(Roles = "RESTAURANT_OWNER")]
        public async Task<IActionResult> PutMeal(long id, Meal meal)
        {
            if (meal.Restaurant.OwnerUserId != Utilities.ExtractUserId(User))
                return Unauthorized("Cannot access a restaurant you are not the owner of");

            if (id != meal.Id)
            {
                return BadRequest();
            }

            _context.Entry(meal).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MealExists(id))
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

        // POST: api/Meals
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Roles = "RESTAURANT_OWNER")]
        public async Task<ActionResult<Meal>> PostMeal(Meal meal)
        {
            if (meal.Restaurant.OwnerUserId != Utilities.ExtractUserId(User))
                return Unauthorized("Cannot access a restaurant you are not the owner of");

            _context.Meals.Add(meal);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMeal", new { id = meal.Id }, meal);
        }

        // DELETE: api/Meals/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "RESTAURANT_OWNER")]
        public async Task<IActionResult> DeleteMeal(long id)
        {
            var meal = await _context.Meals.FindAsync(id);
            if (meal == null)
                return NotFound();

            if (meal.Restaurant.OwnerUserId != Utilities.ExtractUserId(User))
                return Unauthorized("Cannot access a restaurant you are not the owner of");

            _context.Meals.Remove(meal);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MealExists(long id)
        {
            return _context.Meals.Any(e => e.Id == id);
        }
    }
}
