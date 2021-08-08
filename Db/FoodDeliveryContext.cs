#nullable disable
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FoodDelivery.Db
{
    public class FoodDeliveryContext : IdentityDbContext<User>  
    {
        public DbSet<Meal> Meals { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderStatusChange> OrderStatusChanges { get; set; }
        public DbSet<Restaurant> Restaurants { get; set; }
        public DbSet<Block> Blocks { get; set; }
        public DbSet<OrderMeal> Ordermeals { get; set; }

        public FoodDeliveryContext(DbContextOptions options) : base(options) {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            // Setup composite primary keys
            modelBuilder.Entity<OrderStatusChange>().HasKey(x => new { x.OrderId, x.Status });
            modelBuilder.Entity<Block>().HasKey(x => new { x.BlockingUserId, x.BlockedUserId });
            modelBuilder.Entity<OrderMeal>().HasKey(x => new { x.OrderId, x.MealId });

            // Setup default values for dates
            modelBuilder.Entity<Order>().Property(x => x.CreatedAt).HasDefaultValueSql("now()");
            modelBuilder.Entity<OrderStatusChange>().Property(x => x.At).HasDefaultValueSql("now()");

            base.OnModelCreating(modelBuilder);
        }
    }
}