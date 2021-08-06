using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FoodDelivery.Models
{
    public class CreateOrderModel
    {
        public long RestaurantId { get; set; }
        public long[] MealIds { get; set; } = new long[0];
    }
}
