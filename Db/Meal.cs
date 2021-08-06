using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace FoodDelivery.Db
{
    public class Meal
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public string Name { get; set; } = "";

        public string Description { get; set; } = "";

        public decimal Price { get; set; }

        public long RestaurantId { get; set; }

        [JsonIgnore]
        public Restaurant? Restaurant { get; set; }

        [JsonIgnore]
        public ICollection<Order>? Orders { get; set; }
    }
}