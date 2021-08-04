using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace FoodDelivery.Db
{
    public class FoodDeliveryContext : DbContext
    {
        public DbSet<Restaurant> Restaurants { get; set; }

        public FoodDeliveryContext(DbContextOptions options) : base(options)
        {
        }
    }
}