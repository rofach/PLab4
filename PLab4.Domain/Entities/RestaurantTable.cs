namespace RestaurantSystem.Domain.Entities;

public class RestaurantTable
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Capacity { get; set; }
    public List<Reservation> Reservations { get; set; } = new();
}
