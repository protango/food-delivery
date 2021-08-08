using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;

namespace FoodDelivery.Db
{
    public class OrderMeal
    {
        public long OrderId { get; set; }
        public long MealId { get; set; }
        public int Qty { get; set; }

        [JsonIgnore]
        public Order? Order { get; set; }
        public Meal? Meal { get; set; }
    }
}