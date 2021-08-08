using FoodDelivery.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FoodDelivery.Models
{
    public class OrderWithHistory: Order
    {
        public new ICollection<OrderStatusChange>? OrderStatusChanges { get; set; }
    }
}
