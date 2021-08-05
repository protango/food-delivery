using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodDelivery.Db
{
    public class Meal
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public long RestaurantId { get; set; }

#nullable disable
        public Restaurant Restaurant { get; set; }

        public ICollection<Order> Orders { get; set; }
#nullable restore

        public Meal(string name, string description) {
            Name = name;
            Description = description;
        }
    }
}