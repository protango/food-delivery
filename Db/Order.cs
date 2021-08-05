using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace FoodDelivery.Db
{
    public class Order
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public long RestaurantId { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime CreatedAt { get; set; }

        [Column(TypeName = "text")]
        public OrderStatus Status { get; set; }

        public long UserId { get; set; }

#nullable disable
        public User User { get; set; }
        public Restaurant Restaurant { get; set; }
        public ICollection<Meal> Meals { get; set; }
        public ICollection<OrderStatusChange> OrderStatusChanges { get; set; }
#nullable restore

        [NotMapped]
        public decimal TotalAmount { get => Meals.Sum(x => x.Price); }


    }
}