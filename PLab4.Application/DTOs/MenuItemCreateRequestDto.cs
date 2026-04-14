using System.ComponentModel.DataAnnotations;

namespace RestaurantSystem.Application.DTOs;

public class MenuItemCreateRequestDto
{
    [Required]
    [StringLength(120, MinimumLength = 2)]
    public string Name { get; set; } = string.Empty;

    [Range(0.01, 1000)]
    public decimal Price { get; set; }

    public bool IsAvailable { get; set; }
}
