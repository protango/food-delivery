using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace FoodDelivery.Db
{
    public class Restaurant
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public string Name { get; set; } = "";

        public string Description { get; set; } = "";

        public string OwnerUserId { get; set; } = "";

        [JsonIgnore]
        public ICollection<Meal>? Meals { get; set; }

        [JsonIgnore]
        [ForeignKey("OwnerUserId")]
        public User? Owner { get; set; }

        [JsonIgnore]
        public ICollection<Order>? Orders { get; set; }
    }
}