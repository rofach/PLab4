using Microsoft.AspNetCore.Mvc;
using RestaurantSystem.Application.DTOs;
using RestaurantSystem.Application.Interfaces.Services;

namespace RestaurantSystem.Api.Controllers;

[ApiController]
[Route("api/reservations")]
public class ReservationsController : ControllerBase
{
    private readonly IReservationService _reservationService;

    public ReservationsController(IReservationService reservationService)
    {
        _reservationService = reservationService;
    }

    [HttpPost]
    public async Task<ActionResult<ReservationResponseDto>> ReserveTable(
        [FromBody] CreateReservationRequestDto request,
        CancellationToken cancellationToken)
    {
        var reservation = await _reservationService.ReserveTableAsync(request, cancellationToken);
        return CreatedAtAction(nameof(ReserveTable), new { id = reservation.Id }, reservation);
    }

    [HttpGet("reserved-tables")]
    public async Task<ActionResult<List<ReservedTableDto>>> GetReservedTables(CancellationToken cancellationToken)
    {
        var reservations = await _reservationService.GetReservedTablesAsync(cancellationToken);
        return Ok(reservations);
    }

    [HttpGet("available-tables")]
    public async Task<ActionResult<List<AvailableTableDto>>> GetAvailableTables(
        [FromQuery] DateTime dateTime,
        [FromQuery] int guestCount,
        CancellationToken cancellationToken)
    {
        var tables = await _reservationService.GetAvailableTablesAsync(dateTime, guestCount, cancellationToken);
        return Ok(tables);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteReservation(int id, CancellationToken cancellationToken)
    {
        await _reservationService.DeleteReservationAsync(id, cancellationToken);
        return NoContent();
    }
}
