using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodDelivery.Db
{
    public class UserBlock
    {
        public long RestaurantId { get; set; }

        public string UserId { get; set; }

#nullable disable
        public Restaurant Restaurant { get; set; }
        public User User { get; set; }
#nullable restore
    }
}