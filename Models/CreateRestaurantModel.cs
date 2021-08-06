using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FoodDelivery.Models
{
    public class CreateRestaurantModel
    {
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public IEnumerable<CreateMealModel> Meals { get; set; } = new CreateMealModel[0];
    }
}
