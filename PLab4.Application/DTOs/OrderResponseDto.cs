namespace RestaurantSystem.Application.DTOs;

public class OrderResponseDto
{
    public int Id { get; set; }
    public string DeliveryAddress { get; set; } = string.Empty;
    public decimal TotalPrice { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedAtUtc { get; set; }
    public List<OrderLineDto> Items { get; set; } = new();
}

public class OrderLineDto
{
    public int MenuItemId { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Quantity { get; set; }
}
