using RestaurantSystem.Application.DTOs;

namespace RestaurantSystem.Application.Interfaces.Services;

public interface IReservationService
{
    Task<ReservationResponseDto> ReserveTableAsync(CreateReservationRequestDto request, CancellationToken cancellationToken = default);
    Task DeleteReservationAsync(int reservationId, CancellationToken cancellationToken = default);
    Task<List<ReservedTableDto>> GetReservedTablesAsync(CancellationToken cancellationToken = default);
    Task<List<AvailableTableDto>> GetAvailableTablesAsync(DateTime dateTimeUtc, int guestCount, CancellationToken cancellationToken = default);
}
