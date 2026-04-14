using System.ComponentModel.DataAnnotations;

namespace RestaurantSystem.Application.DTOs;

public class CreateReservationRequestDto
{
    [Range(1, int.MaxValue)]
    public int TableId { get; set; }

    [Required]
    public DateTime DateTime { get; set; }

    [Range(1, 20)]
    public int GuestCount { get; set; }
}
