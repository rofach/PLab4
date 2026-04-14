namespace RestaurantSystem.Domain.Entities;

public class Reservation
{
    public int Id { get; set; }
    public int TableId { get; set; }
    public DateTime DateTimeUtc { get; set; }
    public int GuestCount { get; set; }
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    public RestaurantTable? Table { get; set; }
}
