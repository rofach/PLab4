namespace RestaurantSystem.Application.DTOs;

public class ReservationResponseDto
{
    public int Id { get; set; }
    public int TableId { get; set; }
    public string TableName { get; set; } = string.Empty;
    public DateTime DateTimeUtc { get; set; }
    public int GuestCount { get; set; }
    public DateTime CreatedAtUtc { get; set; }
}
