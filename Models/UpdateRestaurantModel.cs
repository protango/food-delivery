using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FoodDelivery.Models
{
    public class UpdateRestaurantModel
    {
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
    }
}
