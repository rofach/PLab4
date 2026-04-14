namespace RestaurantSystem.Domain.Entities;

public class OrderItem
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public int MenuItemId { get; set; }
    public int Quantity { get; set; } = 1;
    public Order? Order { get; set; }
    public MenuItem? MenuItem { get; set; }
}
