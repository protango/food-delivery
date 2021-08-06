using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace FoodDelivery.Db
{
    public class OrderStatusChange
    {
        public long OrderId { get; set; }
        public OrderStatus Status { get; set; }

        public DateTime At { get; set; }

        [JsonIgnore]
        public Order? Order { get; set; }
    }
}