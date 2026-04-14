namespace RestaurantSystem.Application.DTOs;

public class ReservedTableDto
{
    public int ReservationId { get; set; }
    public int TableId { get; set; }
    public string TableName { get; set; } = string.Empty;
    public int TableCapacity { get; set; }
    public DateTime DateTimeUtc { get; set; }
    public int GuestCount { get; set; }
}
