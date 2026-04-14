using Microsoft.EntityFrameworkCore;
using RestaurantSystem.Domain.Entities;

namespace RestaurantSystem.Infrastructure.Persistence;

public class RestaurantDbContext : DbContext
{
    public RestaurantDbContext(DbContextOptions<RestaurantDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<MenuItem> MenuItems => Set<MenuItem>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();
    public DbSet<RestaurantTable> Tables => Set<RestaurantTable>();
    public DbSet<Reservation> Reservations => Set<Reservation>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasIndex(user => user.Email)
            .IsUnique();

        modelBuilder.Entity<OrderItem>()
            .HasOne(orderItem => orderItem.Order)
            .WithMany(order => order.OrderItems)
            .HasForeignKey(orderItem => orderItem.OrderId);

        modelBuilder.Entity<OrderItem>()
            .HasOne(orderItem => orderItem.MenuItem)
            .WithMany()
            .HasForeignKey(orderItem => orderItem.MenuItemId);

        modelBuilder.Entity<Reservation>()
            .HasOne(reservation => reservation.Table)
            .WithMany(table => table.Reservations)
            .HasForeignKey(reservation => reservation.TableId);

        modelBuilder.Entity<MenuItem>().HasData(
            new MenuItem { Id = 1, Name = "Борщ український", Price = 165.00m, IsAvailable = true },
            new MenuItem { Id = 2, Name = "Салат Цезар з куркою", Price = 245.00m, IsAvailable = true },
            new MenuItem { Id = 3, Name = "Крем-суп грибний", Price = 185.00m, IsAvailable = false });

        modelBuilder.Entity<RestaurantTable>().HasData(
            new RestaurantTable { Id = 1, Name = "Стіл 1", Capacity = 2 },
            new RestaurantTable { Id = 2, Name = "Стіл 2", Capacity = 2 },
            new RestaurantTable { Id = 3, Name = "Стіл 3", Capacity = 4 },
            new RestaurantTable { Id = 4, Name = "Стіл 4", Capacity = 4 },
            new RestaurantTable { Id = 5, Name = "Стіл 5", Capacity = 6 });

        modelBuilder.Entity<Order>().HasData(
            new Order
            {
                Id = 1,
                DeliveryAddress = "м. Київ, вул. Хрещатик, 1",
                Status = "Pending",
                TotalPrice = 410.00m,
                CreatedAtUtc = new DateTime(2026, 4, 15, 22, 5, 0, DateTimeKind.Utc)
            });

        modelBuilder.Entity<OrderItem>().HasData(
            new OrderItem { Id = 1, OrderId = 1, MenuItemId = 1, Quantity = 1 },
            new OrderItem { Id = 2, OrderId = 1, MenuItemId = 2, Quantity = 1 });
    }
}
