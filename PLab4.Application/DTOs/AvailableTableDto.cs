namespace RestaurantSystem.Application.DTOs;

public class AvailableTableDto
{
    public int TableId { get; set; }
    public string TableName { get; set; } = string.Empty;
    public int Capacity { get; set; }
}
