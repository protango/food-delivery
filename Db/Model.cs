using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace EFGetStarted
{
    public class FoodDeliveryContext : DbContext
    {
        public DbSet<Restaurant> Restaurants { get; set; }

        public FoodDeliveryContext()
        {
        }

        // The following configures EF to create a Sqlite database file in the
        // special "local" folder for your platform.
        protected override void OnConfiguring(DbContextOptionsBuilder options) {
            options.UseNpgsql("Host=localhost;Database=my_db;Username=my_user;Password=my_pw");
        }
    }

    public class Restaurant
    {
        public long Id { get; set; }
    }
}