using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Reflection.Emit;
using QuickServeAPP.Models;

namespace QuickServeAPP.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext() { }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // User-Order relationship (One-to-many)
            modelBuilder.Entity<Order>()
                .HasOne(o => o.User)
                .WithMany(u => u.Orders)
                .HasForeignKey(o => o.UserID)
                .OnDelete(DeleteBehavior.Restrict);  // Prevent accidental deletion of users when orders are deleted

            // Restaurant-Menu relationship (One-to-many)
            modelBuilder.Entity<Menu>()
                .HasOne(m => m.Restaurant)
                .WithMany(r => r.Menus)
                .HasForeignKey(m => m.RestaurantID)
                .OnDelete(DeleteBehavior.Cascade);  // Cascade delete menus when a restaurant is deleted

            // Order-OrderItem relationship (One-to-many)
            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(oi => oi.OrderID)
                .OnDelete(DeleteBehavior.Cascade);  // Cascade delete order items when an order is deleted

            // Menu-OrderItem relationship (Many-to-one)
            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Menu)
                .WithMany(m => m.OrderItems)
                .HasForeignKey(oi => oi.MenuID)
                .OnDelete(DeleteBehavior.Restrict);  // Prevent accidental deletion of menu items when order items are deleted

            // Cart-CartItem relationship (One-to-many)
            modelBuilder.Entity<CartItem>()
                .HasOne(ci => ci.Cart)
                .WithMany(c => c.CartItems)
                .HasForeignKey(ci => ci.CartID)
                .OnDelete(DeleteBehavior.Cascade);  // Cascade delete cart items when a cart is deleted

            // Menu-CartItem relationship (Many-to-one)
            modelBuilder.Entity<CartItem>()
                .HasOne(ci => ci.Menu)
                .WithMany(m => m.CartItems)
                .HasForeignKey(ci => ci.MenuID)
                .OnDelete(DeleteBehavior.Restrict);  // Prevent accidental deletion of menu items when cart items are deleted

            // User-Cart relationship (One-to-one)
            modelBuilder.Entity<Cart>()
                .HasOne(c => c.User)
                .WithOne(u => u.Cart)
                .HasForeignKey<Cart>(c => c.UserID)
                .OnDelete(DeleteBehavior.Cascade);  // Cascade delete cart when user is deleted

            // Order-Payment relationship (One-to-one)
            modelBuilder.Entity<Payment>()
                .HasOne(p => p.Order)
                .WithOne(o => o.Payment)
                .HasForeignKey<Payment>(p => p.OrderID)
                .OnDelete(DeleteBehavior.Cascade);  // Cascade delete payment when order is deleted

            // User-Rating relationship (One-to-many)
            modelBuilder.Entity<Rating>()
                .HasOne(r => r.User)
                .WithMany(u => u.Ratings)
                .HasForeignKey(r => r.UserID)
                .OnDelete(DeleteBehavior.Restrict);  // Prevent accidental deletion of users when ratings are deleted

            // Restaurant-Rating relationship (One-to-many)
            modelBuilder.Entity<Rating>()
                .HasOne(r => r.Restaurant)
                .WithMany(rest => rest.Ratings)
                .HasForeignKey(r => r.RestaurantID)
                .OnDelete(DeleteBehavior.Restrict);  // Prevent accidental deletion of restaurants when ratings are deleted

            // Payment-User relationship (One-to-many, optional, if you want to track payments by user)
            modelBuilder.Entity<Payment>()
                .HasOne(p => p.User)
                .WithMany(u => u.Payments)
                .HasForeignKey(p => p.UserID)
                .OnDelete(DeleteBehavior.Restrict);  // Prevent accidental deletion of users when payments are deleted

            // Optional: Add index for commonly queried fields
            modelBuilder.Entity<Menu>()
                .HasIndex(m => m.ItemName)
                .IsUnique();  // Ensure menu item names are unique per restaurant (if required)

            // Indexing for better search performance (optional)
            modelBuilder.Entity<Order>()
                .HasIndex(o => o.OrderDate);

            modelBuilder.Entity<Order>(entity =>
            {
                // Map OrderStatus to an integer
                entity.Property(o => o.OrderStatus)
                      .HasConversion<int>();
            });
            base.OnModelCreating(modelBuilder);
        }


        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    base.OnModelCreating(modelBuilder);
        //}

        public DbSet<User> Users { get; set; }
        public DbSet<Restaurant> Restaurants { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Rating> Ratings { get; set; }
    }
}
