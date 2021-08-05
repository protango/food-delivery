using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodDelivery.Db
{
    public class OrderStatusChange
    {
        public long OrderId { get; set; }
        public OrderStatus Status { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime At { get; set; }

#nullable disable
        public Order Order { get; set; }
#nullable restore
    }
}