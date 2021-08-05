using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodDelivery.Db
{
    public class User
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Column(TypeName = "text")]
        public UserRole Role { get; set; }

#nullable disable
        public ICollection<Order> Orders { get; set; }
        public ICollection<Restaurant> Restaurants { get; set; }
#nullable restore
    }
}