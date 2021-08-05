using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodDelivery.Db
{
    public static class UserRole
    {
        /// <summary>
        /// A regular user, can see all restaurants and place orders from them
        /// </summary>
        public static readonly string CUSTOMER = "CUSTOMER";
        /// <summary>
        /// A restaurant owner, can CRUD their own restaurants and meals
        /// </summary>
        public static readonly string RESTAURANT_OWNER = "RESTAURANT_OWNER";
    }
}