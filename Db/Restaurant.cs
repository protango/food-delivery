using System.ComponentModel.DataAnnotations;

namespace FoodDelivery.Db
{
    public class Restaurant
    {
        [Key]
        public long Id { get; set; }
    }
}