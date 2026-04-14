using RestaurantSystem.Domain.Entities;

namespace RestaurantSystem.Application.Interfaces.Repositories;

public interface IReservationRepository
{
    Task<RestaurantTable?> GetTableByIdAsync(int tableId, CancellationToken cancellationToken = default);
    Task<bool> ExistsForTableAndDateTimeAsync(int tableId, DateTime dateTimeUtc, CancellationToken cancellationToken = default);
    Task<Reservation> AddAsync(Reservation reservation, CancellationToken cancellationToken = default);
    Task<Reservation?> GetByIdAsync(int reservationId, CancellationToken cancellationToken = default);
    Task<List<Reservation>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<List<RestaurantTable>> GetAvailableTablesAsync(DateTime dateTimeUtc, int guestCount, CancellationToken cancellationToken = default);
    Task DeleteAsync(Reservation reservation, CancellationToken cancellationToken = default);
}
