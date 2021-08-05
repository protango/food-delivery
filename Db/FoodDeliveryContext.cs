#nullable disable
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace FoodDelivery.Db
{
    public class FoodDeliveryContext : DbContext
    {
        public DbSet<Meal> Meals { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderStatusChange> OrderStatusChanges { get; set; }
        public DbSet<Restaurant> Restaurants { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserBlock> UserBlocks { get; set; }

        public FoodDeliveryContext(DbContextOptions options) : base(options) {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            // Setup composite primary keys
            modelBuilder.Entity<OrderStatusChange>().HasKey(x => new { x.OrderId, x.Status });
            modelBuilder.Entity<UserBlock>().HasKey(x => new { x.RestaurantId, x.UserId });
        }
    }
}