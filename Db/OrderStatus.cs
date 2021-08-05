using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodDelivery.Db
{
    public enum OrderStatus
    {
        PLACED,
        CANCELLED,
        PROCESSING,
        EN_ROUTE,
        DELIVERED,
        RECEIVED
    }
}