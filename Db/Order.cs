using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;

namespace FoodDelivery.Db
{
    public class Order
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public long RestaurantId { get; set; }

        public DateTime CreatedAt { get; set; }

        [Column(TypeName = "text")]
        public OrderStatus Status { get; set; }

        public string UserId { get; set; } = "";

        [JsonIgnore]
        public User? User { get; set; }

        [JsonIgnore]
        public Restaurant? Restaurant { get; set; }

        [JsonIgnore]
        public ICollection<Meal>? Meals { get; set; }

        [JsonIgnore]
        public ICollection<OrderStatusChange>? OrderStatusChanges { get; set; }
    }
}