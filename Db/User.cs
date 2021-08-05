using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace FoodDelivery.Db
{
    public class User
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Column(TypeName = "text")]
        public UserRole Role { get; set; }

#nullable disable
        [JsonIgnore]
        public ICollection<Order> Orders { get; set; }
        [JsonIgnore]
        public ICollection<Restaurant> Restaurants { get; set; }
#nullable restore

        public bool CheckAccess(long userId)
        {
            return userId == Id;
        }
    }
}