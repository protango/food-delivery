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
    public class OrdersController : ControllerBase
    {
        private readonly FoodDeliveryContext _context;

        public OrdersController(FoodDeliveryContext context)
        {
            _context = context;
        }

        // GET: api/Orders
        [HttpGet]
        [Authorize(Roles = "CUSTOMER,RESTAURANT_OWNER")]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
        {
            var userId = Utilities.ExtractUserId(User);
            
            return await _context.Orders
                .Where(x => x.UserId == userId || x.Restaurant!.OwnerUserId == userId)
                .Include(x => x.User)
                .Include(x => x.Meals)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();
        }

        // GET: api/Orders/ForRestaurant/5
        [HttpGet("ForRestaurant/{id}")]
        [Authorize(Roles = "RESTAURANT_OWNER")]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrdersForRestaurant(long id)
        {
            var userId = Utilities.ExtractUserId(User);
            var restaurant = await _context.Restaurants.FindAsync(id);
            if (restaurant.OwnerUserId != userId)
                return StatusCode(403, "User does not have access to this data");

            return await _context.Orders
                .Where(x => x.RestaurantId == id)
                .Include(x => x.User)
                .Include(x => x.Meals)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();
        }

        // POST: api/Orders
        [ProducesResponseType(201)]
        [HttpPost]
        [Authorize(Roles = "CUSTOMER")]
        public async Task<ActionResult<Order>> PostOrder(CreateOrderModel createOrder)
        {
            // Check if user is blocked
            var userId = Utilities.ExtractUserId(User);
            var restaurant = (Restaurant?)await _context.Restaurants.FindAsync(createOrder.RestaurantId);
            if (restaurant == null) 
                return NotFound("Invalid restaurant id");
            if (await _context.Blocks.FindAsync(restaurant.OwnerUserId, userId) != null) {
                return StatusCode(403, "User is blocked");
            }

            // Link meals
            IEnumerable<Meal?> orderMeals =
                (from mealId in createOrder.MealIds
                 join meal in _context.Meals on mealId equals meal.Id into gj
                 from meal in gj.DefaultIfEmpty()
                 select meal).ToArray();

            // Validate meals
            if (!orderMeals.Any())
                return BadRequest($"Order must contain at least one meal");
            foreach (var meal in orderMeals)
            {
                if (meal == null)
                    return BadRequest($"Order contains one or more invalid meal ids");
                if (meal.RestaurantId != createOrder.RestaurantId)
                    return BadRequest($"Order may only contain meals from one restaurant");
            }

            // Construct order
            var order = _context.Orders.Add(new Order() { 
                RestaurantId = createOrder.RestaurantId,
                UserId = Utilities.ExtractUserId(User),
                OrderStatusChanges = new[] {
                    new OrderStatusChange() {
                        Status = OrderStatus.PLACED
                    }
                },
                Status = OrderStatus.PLACED,
                Meals = (ICollection<Meal>)orderMeals
            });
            await _context.SaveChangesAsync();
            await _context.Entry(order.Entity).Reference(x => x.User).LoadAsync();
            await _context.Entry(order.Entity).Collection(x => x.Meals).LoadAsync();
            return CreatedAtAction("PostOrder", order.Entity);
        }

        [ProducesResponseType(204)]
        [HttpPut("Status/{orderId}/{status}")]
        [Authorize(Roles = "CUSTOMER,RESTAURANT_OWNER")]
        public async Task<IActionResult> UpdateStatus(long orderId, OrderStatus status) {
            var userId = Utilities.ExtractUserId(User);
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null)
                return NotFound("Invalid order id");

            await _context.Entry(order).Reference(x => x.Restaurant).LoadAsync();
            if (order.Restaurant!.OwnerUserId != userId && order.UserId != userId)
                return StatusCode(403, "User does not have access to this data");

            // Ensure status is different
            if (order.Status == status)
                return BadRequest("Order already has this status");

            // Ensure user is correct role for requested status
            var statusRequiredRole = new Dictionary<OrderStatus, string>() {
                { OrderStatus.PLACED, "CUSTOMER" },
                { OrderStatus.CANCELLED, "CUSTOMER" },
                { OrderStatus.PROCESSING, "RESTAURANT_OWNER" },
                { OrderStatus.EN_ROUTE, "RESTAURANT_OWNER" },
                { OrderStatus.DELIVERED, "RESTAURANT_OWNER" },
                { OrderStatus.RECEIVED, "CUSTOMER" }
            };
            if (!User.IsInRole(statusRequiredRole[status])) {
                return StatusCode(403, "User does not have permission to change order to this status");
            }

            // Ensure status move in the correct direction
            var validNextStatuses = new Dictionary<OrderStatus, OrderStatus[]>() {
                { OrderStatus.PLACED, new[] { OrderStatus.CANCELLED, OrderStatus.PROCESSING } },
                { OrderStatus.CANCELLED, Array.Empty<OrderStatus>() },
                { OrderStatus.PROCESSING, new[] { OrderStatus.EN_ROUTE } },
                { OrderStatus.EN_ROUTE, new[] { OrderStatus.DELIVERED } },
                { OrderStatus.DELIVERED, new[] { OrderStatus.RECEIVED } },
                { OrderStatus.RECEIVED, Array.Empty<OrderStatus>() }
            };
            if (!validNextStatuses[order.Status].Contains(status)) {
                return BadRequest(
                    $"Order status cannot move from {Enum.GetName(typeof(OrderStatus), order.Status)} to {Enum.GetName(typeof(OrderStatus), status)}"
                );
            }

            order.Status = status;
            _context.OrderStatusChanges.Add(new OrderStatusChange() {
                OrderId = orderId,
                Status = status
            });
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
