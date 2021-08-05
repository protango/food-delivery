using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodDelivery.Db
{
    public enum UserRole
    {
        /// <summary>
        /// A regular user, can see all restaurants and place orders from them
        /// </summary>
        CUSTOMER,
        /// <summary>
        /// A restaurant owner, can CRUD their own restaurants and meals
        /// </summary>
        RESTAURANT_OWNER
    }
}