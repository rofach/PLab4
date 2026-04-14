using Microsoft.EntityFrameworkCore;
using RestaurantSystem.Application.Interfaces.Repositories;
using RestaurantSystem.Domain.Entities;
using RestaurantSystem.Infrastructure.Persistence;

namespace RestaurantSystem.Infrastructure.Repositories;

public class ReservationRepository : IReservationRepository
{
    private readonly RestaurantDbContext _dbContext;

    public ReservationRepository(RestaurantDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<RestaurantTable?> GetTableByIdAsync(int tableId, CancellationToken cancellationToken = default)
    {
        return _dbContext.Tables
            .AsNoTracking()
            .FirstOrDefaultAsync(table => table.Id == tableId, cancellationToken);
    }

    public Task<bool> ExistsForTableAndDateTimeAsync(int tableId, DateTime dateTimeUtc, CancellationToken cancellationToken = default)
    {
        return _dbContext.Reservations.AnyAsync(
            reservation => reservation.TableId == tableId && reservation.DateTimeUtc == dateTimeUtc,
            cancellationToken);
    }

    public async Task<Reservation> AddAsync(Reservation reservation, CancellationToken cancellationToken = default)
    {
        _dbContext.Reservations.Add(reservation);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return await _dbContext.Reservations
            .Include(savedReservation => savedReservation.Table)
            .FirstAsync(savedReservation => savedReservation.Id == reservation.Id, cancellationToken);
    }

    public Task<Reservation?> GetByIdAsync(int reservationId, CancellationToken cancellationToken = default)
    {
        return _dbContext.Reservations
            .Include(reservation => reservation.Table)
            .FirstOrDefaultAsync(reservation => reservation.Id == reservationId, cancellationToken);
    }

    public Task<List<Reservation>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return _dbContext.Reservations
            .AsNoTracking()
            .Include(reservation => reservation.Table)
            .ToListAsync(cancellationToken);
    }

    public Task<List<RestaurantTable>> GetAvailableTablesAsync(DateTime dateTimeUtc, int guestCount, CancellationToken cancellationToken = default)
    {
        return _dbContext.Tables
            .AsNoTracking()
            .Where(table => table.Capacity >= guestCount)
            .Where(table => !table.Reservations.Any(reservation => reservation.DateTimeUtc == dateTimeUtc))
            .ToListAsync(cancellationToken);
    }

    public async Task DeleteAsync(Reservation reservation, CancellationToken cancellationToken = default)
    {
        _dbContext.Reservations.Remove(reservation);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
