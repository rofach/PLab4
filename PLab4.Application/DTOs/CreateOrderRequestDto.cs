using System.ComponentModel.DataAnnotations;

namespace RestaurantSystem.Application.DTOs;

public class CreateOrderRequestDto
{
    [Required]
    [MinLength(1)]
    public List<int> ItemIds { get; set; } = new();

    [Required]
    [StringLength(250, MinimumLength = 5)]
    public string DeliveryAddress { get; set; } = string.Empty;
}
